using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFinanceLib
{
    public class DateUtility
    {
        public static int IntDate(DateTime d)
        {
            return d.Year * 10000 + d.Month * 100 + d.Day;
        }
        public static DateTime FromIntDate(int yyyymmdd)
        {
            int yyyy = yyyymmdd / 10000;
            int mmdd = yyyymmdd - yyyy * 10000;
            int mm = mmdd / 100;
            int dd = mmdd - mm * 100;
            return new DateTime(yyyy, mm, dd);
        }

        public static DateTime LastBusinessDayOfMonth(int month, int year, Holiday.Calendar cal)
        {
            var firstDayOfNextMonth = new DateTime(year, month, 1).AddMonths(1);
            var lastDay = firstDayOfNextMonth.AddDays(-1);
            return Holiday.DateAdd(Holiday.StepInterval.Day, 0, lastDay, Holiday.Direction.backward, cal);
        }

        public static DateTime PreviousBusinessDay(DateTime date, Holiday.Calendar cal)
        {
            // if date is NOT a trade day, then adding 0 trade days will return the previous trade day, otherwise we add -1 trade days
            int daysBack = Holiday.IsTradeDate(date, cal) ? -1 : 0;

            return Holiday.AddTradeDays(date, daysBack, cal);
        }

        public static DateTime NetDate(int dt)
        {
            return new DateTime(Year(dt), Month(dt), Day(dt));
        }

        public static DateTime Get3rdFriday(int year, int month)
        {
            var d = new DateTime(year, month, 1);
            switch (d.DayOfWeek)
            {
                case (DayOfWeek.Sunday):
                    return new DateTime(year, month, 6 + 14);
                case (DayOfWeek.Monday):
                    return new DateTime(year, month, 5 + 14);
                case (DayOfWeek.Tuesday):
                    return new DateTime(year, month, 4 + 14);
                case (DayOfWeek.Wednesday):
                    return new DateTime(year, month, 3 + 14);
                case (DayOfWeek.Thursday):
                    return new DateTime(year, month, 2 + 14);
                case (DayOfWeek.Friday):
                    return new DateTime(year, month, 1 + 14);
                case (DayOfWeek.Saturday):
                    return new DateTime(year, month, 7 + 14);
                default:
                    throw new Exception();
            }
        }

        public static int IntTime(DateTime d)
        {
            return d.Hour * 10000 + d.Minute * 100 + d.Second;
        }

        public static int Day(int d)
        {
            return d % 100;
        }

        public static int Month(int d)
        {
            return d / 100 % 100;
        }

        public static int Year(int d)
        {
            return d / 10000;
        }
    }
    
    [Serializable]
    public class HolidayCalendarSerializable
    {
        public readonly string CalendarCode;
        public readonly string CalendarName;
        public readonly string Description;
    }

    [Serializable]
    public class HolidayDateSerializable
    {
        public readonly string CalendarCode;
        public readonly DateTime ObservedDate;
        public readonly DateTime? StartDate;
        public readonly DateTime? EndDate;
        public readonly string Comments;
    }


    public static class Holiday
    {
        public enum Direction
        {
            backward,
            forward,
            modifiedForward
        }

        public enum StepInterval
        {
            Day = 1,
            Month = 2,
            Year = 3,
            Week = 4,
        }

        // If you change this enum, you MUST make corresponding changes to the HolidayCalendar table (and vice versa)!
        public enum Calendar
        {
            USA = 0,
            KOR,
            EU,
            UK,
            USABOND,
            USDLIBOR,
            BLND,
            JP,
            BUDBI,
            LUX,
            NUM_Calendar
        }

        // configurations...
        public static DateTime HolidayCalendarAsOfDate = DateTime.Now.Date;  // date to apply when filtering the holiday lists
        public static string SqlConnString = null;  // load holidays from this server/db

        // cache from the db (must be serializable in order to pass it to the grid)...
        public static List<HolidayCalendarSerializable> HolidayCalendars;  // cache of HolidayCalendar table
        public static List<HolidayDateSerializable> HolidayDates;  // cache of HolidayDate table

        // the calendar data...
        static readonly Dictionary<DateTime, int>[] HolidayMaps = new Dictionary<DateTime, int>[(int)Calendar.NUM_Calendar];
        static readonly List<int>[] TradeDates = new List<int>[(int)Calendar.NUM_Calendar];


        // This initializer is intended to be called by DataSynapse.RunTasks(), passing in the cached lists that
        // DataSynapseClient.RunTasks() squirrelled away.
        //
        public static void Initialize(DateTime? holidayCalendarAsOfDate,
                                      List<HolidayCalendarSerializable> holidayCalendars, 
                                      List<HolidayDateSerializable> holidayDates)
        {
            HolidayCalendarAsOfDate = holidayCalendarAsOfDate ?? DateTime.Now.Date;
            SqlConnString = null;
            HolidayCalendars = holidayCalendars;
            HolidayDates = holidayDates;
        }


        // Changes the as-of date on an already-initialized class (avoids having to reload from the database).
        // 
        public static void RestoreCalendarAsOfDate(DateTime? date)
        {
            HolidayCalendarAsOfDate = date ?? DateTime.Now.Date;
        }

        // Initializes HolidayMaps[] and TradeDates[], filtered as of HolidayCalendarAsOfDate.
        //
        public static int TradingDays(DateTime start, DateTime end, Calendar cal)
        {
            List<int> dts = GetTradeDates(cal);

            int mult = 1;

            if (start == end)
                return 0;

            //If the user inputs the dates in the wrong order, this will flip them
            // but the function will return a negative number of days
            if (start > end)
            {
                DateTime holdStart = start;
                start = end;
                end = holdStart;
                mult = -1;
            }

            int i1 = dts.BinarySearch(DateUtility.IntDate(start));
            if (i1 < 0)
                i1 = ~i1 - 1;

            int i2 = dts.BinarySearch(DateUtility.IntDate(end));
            if (i2 < 0)
                i2 = ~i2 - 1;

            return mult * (i2 - i1);
        }


        static public bool IsHoliday(DateTime d, Calendar cal)
        {
            var hm = GetHolidayMap(cal);

            if (hm.ContainsKey(d))
                return true;
            return false;
        }


        static public bool IsTradeDate(DateTime d, Calendar cal)
        {
            List<int> dts = GetTradeDates(cal);

            int i = dts.BinarySearch(DateUtility.IntDate(d));

            if (i > 0)
                return true;
            return false;
        }


        /// <summary>
        /// This function will add the specified number of trading days to the specified date.
        /// </summary>
        /// <param name="dt">The starting date</param>
        /// <param name="add">The number of trading days you wish to add</param>
        /// <param name="cal">Holiday calendar used for calculation</param>
        /// <returns></returns>
        static public DateTime AddTradeDays(DateTime dt, int add, Calendar cal)
        {
            List<int> dts = GetTradeDates(cal);

            int i = dts.BinarySearch(DateUtility.IntDate(dt));
            if (i < 0)
                i = ~i - 1;

            return DateUtility.NetDate(dts[i + add]);
        }


        // step back
        //
        // Note: SubtractTradeDays(dt, N, cal) != AddTradeDays(dt, -N, cal)
        //  When dt is a weekend or holiday, SubtractTradeDays(dt, 1, cal) returns the prior trade date
        //  where AddTradeDays(dt, -1, cal) returns the (trade) day before the prior trade date.  
        // Concrete example:
        //  SubtractTradeDays(1/1/2010, 1, USA) == 12/31/2010
        //  AddTradeDays(1/1/2010, -1, USA) == 12/30/2009
        // The AddTradeDays behavior is consistent with T+N == (T+0)+N for all N, where T+0 is the
        //  AddTradeDays(dt, 0, cal) behavior of backing up to the prior trade date when dt is not a trading day.
        //
        // TODO: Fix callers to SubtractTradeDays that depend on this inconsistent behavior and change SubtractTradeDays to return AddTradeDays(dt, -nDays, cal)
        //
        static public DateTime SubtractTradeDays(DateTime dt, int nDays, Calendar cal)
        {
            List<int> dts = GetTradeDates(cal);

            int add = -nDays;

            int i = dts.BinarySearch(DateUtility.IntDate(dt));
            if (i < 0)
            {
                i = ~i - 1;
                if (add < 0)
                    add++;
            }

            return DateUtility.NetDate(dts[i + add]);
        }


        /// <summary>
        /// This function will adjust the startDate by the number of intervals specified. 
        /// If the new date is not a trade date, the date will be corrected to the nearest trading date in the direction specified.
        /// </summary>
        /// <param name="interval">The steps size</param>
        /// <param name="steps">The signed number of steps to move</param>
        /// <param name="startDate">The initial starting date</param>
        /// <param name="correction">The direction to move if the result is not a trading date</param>
        /// <param name="cal"></param>
        /// <returns></returns>
        //
        // Modified Following:                A Business Day Convention whereby payment days that fall on a Holiday or Saturday or a Sunday roll forward to the next TARGET Business Day, 
        //  unless that day falls in the next calendar month, in which case the payment day rolls backward to the immediately preceding TARGET Business Day.
        //
        static public DateTime DateAdd(StepInterval interval, int steps, DateTime startDate, Direction correction, Calendar cal)
        {
            List<int> dts = GetTradeDates(cal);

            DateTime newDate = startDate;
            switch (interval)
            {
                case StepInterval.Day:
                    newDate = startDate.AddDays(steps);
                    break;
                case StepInterval.Week:
                    newDate = startDate.AddDays(steps * 7);
                    break;
                case StepInterval.Month:
                    newDate = startDate.AddMonths(steps);
                    break;
                case StepInterval.Year:
                    newDate = startDate.AddYears(steps);
                    break;
                // ReSharper disable RedundantEmptyDefaultSwitchBranch
                default:
                    break;
                // ReSharper restore RedundantEmptyDefaultSwitchBranch
            }

            if (correction == Direction.modifiedForward)
            {
                var adjDate = DateAdd(interval, steps, startDate, Direction.forward, cal);
                if (adjDate.Month == newDate.Month)
                    return adjDate;
                return DateAdd(interval, steps, startDate, Direction.backward, cal);
            }

            int i = dts.BinarySearch(DateUtility.IntDate(newDate));
            if (i < 0)
            {
                i = ~i - 1;
                if (correction == Direction.forward)
                    i++;
            }

            return DateUtility.NetDate(dts[i]);
        }

        static public DateTime ThisOrNextTDay(DateTime d, Calendar cal)
        {
            while (!IsTradeDate(d, cal)) d = d.AddDays(1);
            return d;
        }


        /// <summary>
        /// Returns the nearest trade day in either direction that is also the specified day of week.
        /// </summary>
        static public DateTime NearestTDay(DateTime d, DayOfWeek day, Calendar cal)
        {
            int i = 0;
            while (true)
            {
                DateTime dateForward = d.AddDays(i);
                if (dateForward.DayOfWeek == day && IsTradeDate(d, cal)) return dateForward;
                DateTime dateBackward = d.AddDays(-i);
                if (dateBackward.DayOfWeek == day && IsTradeDate(d, cal)) return dateBackward;
                i++;
            }
        }


        public static Dictionary<DateTime, int> GetHolidayMap(Calendar cal)
        {
            return HolidayMaps[(int)cal];
        }


        public static List<DateTime> GetHolidayList(Calendar cal)
        {
            return HolidayMaps[(int)cal].Keys.OrderBy(dt => dt).ToList();
        }


        private static List<int> GetTradeDates(Calendar cal)
        {
            return TradeDates[(int)cal];
        }


        public static DateTime[] GetTradeDates(Calendar cal, DateTime date_from, DateTime date_to)
        {
            var tds = GetTradeDates(cal);
            int from = DateUtility.IntDate(date_from);
            int to = DateUtility.IntDate(date_to);

            return tds.Where(x => x >= from && x <= to).Select(x => new DateTime(x / 10000, x / 100 % 100, x % 100)).ToArray();
        }
    }
}
