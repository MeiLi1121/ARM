using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFinanceLib
{
    public class IRStruct
    {
        public double[] Terms, Coupon;
        public string[] Type;

        // Class constructor, none
        public IRStruct()
        { }

        // Class constructor, terms and coupon rates
        public IRStruct(double[] terms, double[] coupon)
        {
            this.Terms = terms;
            this.Coupon = coupon;
        }

        // Class constructor, terms, coupon rates and type
        public IRStruct(double[] terms, double[] coupon, string[] type)
        {
            this.Terms = terms;
            this.Coupon = coupon;
            this.Type = type;
        }

        public Dictionary<double, double> setYields(double[] termYrs, double[] parRates)
        {
            var yields = new Dictionary<double, double>();
            for (int i = 0; i < termYrs.Length; i++)
            {
                yields.Add(termYrs[i], parRates[i]);
            }
            return yields;
        }

        public Dictionary<double, double> Bootstrap(Dictionary<double, double> yields)
        {
            var discountFactor = new Dictionary<double, double>();

            // Set the 0-year discount factor
            discountFactor.Add(0.0, 1.0);

            // Calculate the discount factor at 0.5-year
            try
            {
                discountFactor.Add(0.5, 1/(1+yields[0.5]/200));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can't find 0.5-year rate", e);
            }

            // Calculate the discount factor after 0.5-year
            for (double tYear = 1.0; tYear <= yields.Keys.Last(); tYear += 0.5)
            {
                //try
                //{
                    double couponT = yields[tYear]/100;
                    double RHS = 1.0;
                    for (double i = 0.0; i < tYear; i += 0.5)
                    {
                        RHS -= couponT * discountFactor[i];
                    }
                    discountFactor.Add(tYear, RHS / (1 + couponT));
                //}
                //catch (Exception e)
                //{
                //    throw new ApplicationException(("Can't find " + tYear + "-year rate"), e);
                //}
            }

            return discountFactor;
        }

        public Dictionary<double, double> discountDF2ZeroRate(Dictionary<double, double> discountFactor)
        {
            var zeroRates = new Dictionary<double, double>();

            for (double tYear = 0.0; tYear <= discountFactor.Keys.Last(); tYear += 0.5)
            {
                double zero = -System.Math.Log(discountFactor[tYear]) / tYear;
                zeroRates.Add(tYear, zero);
            }

            return zeroRates;
        }
    }
}
