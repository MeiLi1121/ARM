using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFinanceLib
{
    public class CubicSplineSet
    {
        //This structure stores the (x,y) ordered pairs we will fit the splines to.
        //It implements IComparable so that we can easily sort an array of these in
        //ascending order, by x-coordinate.
        struct DataPoint : IComparable {
            public double X;
            public double Y;
            public int CompareTo(object obj) {
                DataPoint DataPointForComparison;
                DataPointForComparison = (DataPoint)obj;
                if (this.X == DataPointForComparison.X)
                    return 0;
                else if (this.X > DataPointForComparison.X) {
                    return 1;
                }
                else {
                    return -1;
                }
            }
        }        

        //This structure stores the coefficients and x0 for each cubic spline in the series:
        public struct CubicSpline {
            public double a; //Constant term
            public double b; //Coefficient of the first degree term
            public double c; //Coefficient of the second degree term
            public double d; //Coefficient of the third degree term
            public double x0; //'Offset for this spline (i.e. the x-value of the left endpoint)
        }

        //The following enums/structs allow the user to specify certain parameters for fitting the model.
        //For example, the user can select to use clamped or natural splines. If the user selects clamped
        //splines, then the user can specify which slopes to use at the endpoints.

        public enum CubicSplineType {
            Natural = 0, //Assume zero second derivative at the endpoints
            Clamped = 1  //Use given values for the first derivative at both endpoints
        }
        public enum ClampedSplineMode {
            UseDataSlopes = 0,  //Use the slope of the last two points on either end
            UseUserDefSlopes = 1 //User-specified slope on either end
        }
        public struct ClampedSplineSettings {
            public ClampedSplineMode EndpointSlopeMode;
            public double UserDefSlopeLeft; //If UseUserDefSlopes is selected, this is the slope at the left endpoint
            public double UserDefSlopeRight; //If UseUserDefSlopes is selected, this is the slope at the right endpoint
        }
        public struct CubicSplineSettings {
            public CubicSplineType Type;
            public ExtrapMode Extrap;
            public ClampedSplineSettings Clamped;
        }
        public enum ExtrapMode {
            UseDataPointSlope = 0,  //Use the slopes calculated from the first two and last two data points
            UseSplineSlope = 1,     //Use the slopes of the cubic splines at either endpoint
            UseNearestDataPoint = 2 //Use the nearest available data point with no "trend" adjustment
        }

        /// <summary>  
        /// <para>The various cubic spline settings are stored in this struct.</para>
        /// <para>.Clamped contains settings specified to clamped splines only.</para>
        /// <para>.Type allows the user to select natural or clamped splines.</para>
        /// <para>.Extrap allows the user to select the extrapolation mode for points
        /// outside of the original data point x-coordinate range.</para>
        /// </summary>
        public CubicSplineSettings Settings;

        private int NumPoints = 0; //number of x,y coordinates
        private DataPoint[] DataPointArray; //for storing the x,y coordinates the user feeds into this class
        private CubicSpline[] CubicSplineArray; //for storing all of the individual splines
        private double EndpointLeft = 0.0; //the x-coordinate of the left endpoint
        private double EndpointRight = 0.0; //the x-coordinate of the right endpoint

        private bool ModelFitted = false;

        public CubicSplineSet() {
            //Default settings:
            Settings.Clamped.EndpointSlopeMode = ClampedSplineMode.UseDataSlopes;
            Settings.Extrap = ExtrapMode.UseDataPointSlope;
            Settings.Type = CubicSplineType.Natural;
            Reset();
        }

        public CubicSplineSet(ExtrapMode extraMode)
        {
            Settings.Clamped.EndpointSlopeMode = ClampedSplineMode.UseDataSlopes;
            Settings.Extrap = extraMode;
            Settings.Type = CubicSplineType.Natural;
            Reset();   
        }

        /// <summary>  
        ///This function clears all data points, allowing the user to start over. The
        ///public member Settings is not reset to its default state, however.
        ///</summary>
        public void Reset() {
            NumPoints = 0;
            DataPointArray = null;
            CubicSplineArray = null;
            EndpointLeft = 0.0;
            EndpointRight = 0.0;
            ModelFitted = false;
        }

        /// <summary>  
        ///Adds a new x,y coordinate. If the user has already added a point with the same x-coordinate
        ///as specified by the input paramter x, the function will not add the point and return False.
        /// </summary>
        public bool AddPoint(double x, double y) {
            DataPoint[] tempDataPointArray;
            DataPoint newDataPoint = new DataPoint();

            //Check to make sure we don't already have a data point for this x-coord:
            for (int pointIndex = 0; pointIndex < NumPoints; pointIndex++) {
                if (DataPointArray[pointIndex].X == x) {
                    return false;
                }
            }

            //If we are adding a new point, the current spline array
            //is no longer valid:
            if (CubicSplineArray != null) {
                CubicSplineArray = null;
            }
            ModelFitted = false;

            //Increment the number of points, save the contents
            //of the original array and add an additional element:            
            NumPoints++;
            if (NumPoints > 1) {
                tempDataPointArray = (DataPoint[])DataPointArray.Clone();
                DataPointArray = new DataPoint[NumPoints];
                //Fill the resized array with all of the old data points...
                for (int i = 0; i < NumPoints - 1; i++) {
                    DataPointArray[i] = tempDataPointArray[i];
                }                
            }
            else {
                DataPointArray = new DataPoint[NumPoints];
            }
            newDataPoint.X = x;
            newDataPoint.Y = y;
            DataPointArray[NumPoints - 1] = newDataPoint;
            return true;
        }

        public double XAxisLeftEndpoint {
            get {
                if (ModelFitted == false)
                    return 0.0;
                else
                    return EndpointLeft;
            }            
        }

        public double XAxisRightEndpoint {
            get {
                if (ModelFitted == false)
                    return 0.0;
                else
                    return EndpointRight;
            }
        }

        /// <summary>  
        ///Fits a series of cubic splines to the x,y data points, using the parameters specified in
        ///the public class member Settings. The method returns "false" if the user has added less than three 
        /// points (it is not possible to fit cubic splines to less than three points).  
        /// </summary>  
        public bool FitSplines() {
            double xDiff = 0.0;
            double xDiff0 = 0.0;
            double xDiff1 = 0.0;
            int numRows = -1;
            int numCols = -1;
            double[,] augMatrix = null;
            double[] solution = null;
            double clampLeftSlope = 0.0;
            double clampRightSlope = 0.0;

            if (NumPoints < 3) {
                return false;
            }

            //Set up a new CubicSpline array:
            CubicSplineArray = new CubicSpline[NumPoints - 1];

            //Sort the array in ascending order, by x-coord, before starting:
            Array.Sort(DataPointArray);
            augMatrix = new double[NumPoints, NumPoints + 1];

            //The dimensions of our augmented matrix (system of equations):
            numRows = NumPoints;
            numCols = NumPoints + 1;

            //Get the left and right endpoints (for extrapolation)
            EndpointLeft = DataPointArray[0].X;
            EndpointRight = DataPointArray[NumPoints - 1].X;

            //Set each element of the matrix equal to zero at first:
            for (int row = 0; row < numRows; row++) {
                for (int col = 0; col < numCols; col++) {
                    augMatrix[row, col] = 0.0;
                }
            }

            //The following code creates and solves a tridiagonal matrix, to find the cubic spline coefficients:

            //The first and last rows of the matrix differ according to the spline type:
            switch (Settings.Type) {
                case CubicSplineType.Natural:
                    //This is the pattern needed for a natural spline:
                    augMatrix[0, 0] = 1.0;
                    augMatrix[0, 1] = 0.0;
                    augMatrix[0, numCols - 1] = 0.0;

                    augMatrix[numRows - 1, numCols - 3] = 0.0;
                    augMatrix[numRows - 1, numCols - 2] = 1.0;
                    augMatrix[numRows - 1, numCols - 1] = 0.0;
                    break;
                case CubicSplineType.Clamped: 
                    switch (Settings.Clamped.EndpointSlopeMode) {
                        case ClampedSplineMode.UseUserDefSlopes:
                            clampLeftSlope = Settings.Clamped.UserDefSlopeLeft;
                            clampRightSlope = Settings.Clamped.UserDefSlopeRight;
                            break;
                        case ClampedSplineMode.UseDataSlopes:
                            clampLeftSlope = (DataPointArray[1].Y - DataPointArray[0].Y) / (DataPointArray[1].X - DataPointArray[0].X);
                            clampRightSlope = (DataPointArray[NumPoints - 1].Y - DataPointArray[NumPoints - 2].Y) / (DataPointArray[NumPoints - 1].X - DataPointArray[NumPoints - 2].X);
                            break;
                    }

                    //These formulas were derived algebraically... there is no point in trying to explain them here:
                    xDiff = DataPointArray[1].X - DataPointArray[0].X;
                    augMatrix[0, 0] = 2.0 * xDiff;
                    augMatrix[0, 1] = xDiff;
                    augMatrix[0, numCols - 1] = (3.0 / xDiff) * (DataPointArray[1].Y - DataPointArray[0].Y) - 3.0 * clampLeftSlope;

                    xDiff = DataPointArray[NumPoints - 1].X - DataPointArray[NumPoints - 2].X;
                    augMatrix[numRows - 1, numCols - 3] = xDiff;
                    augMatrix[numRows - 1, numCols - 2] = 2.0 * xDiff;
                    augMatrix[numRows - 1, numCols - 1] = 3.0 * clampRightSlope - (3.0 / xDiff) * (DataPointArray[NumPoints - 1].Y - DataPointArray[NumPoints - 2].Y);
                    break;
            }

            //The remainder of the tridiagonal matrix is the same for all spline types:            
            for (int row = 1; row < NumPoints - 1; row++) {
                //These formulas were derived algebraically--once again, no point in trying to explain them:
                xDiff0 = DataPointArray[row].X - DataPointArray[row - 1].X;
                xDiff1 = DataPointArray[row + 1].X - DataPointArray[row].X;
                augMatrix[row, row - 1] = xDiff0; //h(ixRow - 1);
                augMatrix[row, row] = 2.0 * (xDiff0 + xDiff1);
                augMatrix[row, row + 1] = xDiff1;
                augMatrix[row, numCols - 1] = (3.0 / xDiff1) * (DataPointArray[row + 1].Y - DataPointArray[row].Y) - (3.0 / xDiff0) * (DataPointArray[row].Y - DataPointArray[row - 1].Y);
            }

            SolveTridiagonalMatrix(ref augMatrix, numRows, numCols, ref solution);

            //Final calculations for getting the coefficients:
            for (int i = 0; i < NumPoints - 1; i++) {
                xDiff = DataPointArray[i + 1].X - DataPointArray[i].X;
                CubicSplineArray[i].a = DataPointArray[i].Y;
                CubicSplineArray[i].b = (DataPointArray[i + 1].Y - DataPointArray[i].Y) / xDiff - (xDiff / 3.0) * (2.0 * solution[i] + solution[i + 1]);
                CubicSplineArray[i].c = solution[i];
                CubicSplineArray[i].d = (solution[i + 1] - solution[i]) / (3.0 * xDiff);
                CubicSplineArray[i].x0 = DataPointArray[i].X;
            }

            ModelFitted = true;
            return true;
        }


        //To find the cubic spline parameters, we need to solve a tridiagonal matrix. This
        //function is designed purely for that task.
        private void SolveTridiagonalMatrix(ref double[,] augMatrix, int numRows, int numCols, ref double[] solution) {
            double[,] interMatrix = null; //a matrix for storing intermediate calculations
            double factor = 0.0;

            //A tridiagonal matrix has this form:
            // Row
            //  1  [ a1 d1  0  0  0  0 ]
            //  2  [ c1 a2 d2  0  0  0 ]
            //  3  [  0 c2 a3 d3  0  0 ]
            //  4  [  0  0 c3 a4 d3  0 ]
            //  5  [  0  0  0 c4 a5 d4 ]
            //  6  [  0  0  0  0 c5 a6 ]

            //The tridiagonal matrix for set of cubic splines cannot have zero entries for a, c, or d in rows 2 through n-1,
            //where n is the number of rows. This makes the solving algorithm much simpler that it would be otherwise. We can
            //use a simple Gaussian elimination technique, and it works nicely.

            solution = new double[numRows];
            interMatrix = new double[numRows, numCols];

            //Copy the first row of the system of equations
            for (int col = 0; col < numCols; col++) {
                interMatrix[0, col] = augMatrix[0, col];
            }

            //Gaussian elimination algorithm:
            for (int i = 1; i < numRows; i++) {
                //The variable "factor" is the amount by which we multiply an entire row so that it can be canceled.
                //We use "i" for both the row and column iteration because we are moving diagonally
                //through the matrix.
                if (augMatrix[i, i - 1] != 0) {
                    //we want to eliminate one number at a time:
                    factor = interMatrix[i - 1, i - 1] / augMatrix[i, i - 1];
                }
                else {
                    //avoiding a divide by zero:
                    factor = 0.0;
                }

                //elimination:
                for (int col = 0; col < numCols; col++) {
                    interMatrix[i, col] = interMatrix[i - 1, col] - factor * augMatrix[i, col];
                }
            }

            //Solving for the value at the endpoint (this depends on the spline type):
            switch (Settings.Type) {
                case CubicSplineType.Natural:
                    solution[numRows - 1] = 0.0;
                    break;
                case CubicSplineType.Clamped:
                    solution[numRows - 1] = interMatrix[numRows - 1, numCols - 1] / interMatrix[numRows - 1, numCols - 2];
                    break;
            }

            //A simple iterative procedure; note that we loop backwards here,
            //moving diagonally in the matrix from the lower right to the upper left
            for (int i = numRows - 2; i > -1; i--) {
                solution[i] = (interMatrix[i, numCols - 1] - solution[i + 1] * interMatrix[i, i + 1]) / interMatrix[i, i];
            }
        }

        /// <summary>  
        /// <para>After the splines have been fit to the data points, this function returns the
        /// spline value corresponding to any x-coordinate. If the x-coordinate is beyond
        /// the range of the original data points, then an extrapolated value is returned 
        /// according to whatever the user has specified in Settings.Extrap.</para>
        /// <para>If IsReady reutrns "false," then this function returns zero.</para>
        /// </summary>
        public double GetSplineValue(double x) {
            int pt = 0;
            CubicSpline[] cs = CubicSplineArray; //shorthand for the cubic spline array
            DataPoint[] dp = DataPointArray; //shorthand for the data point array

            if (ModelFitted == false) {
                return 0.0;
            }

            //If we are to the left of the leftmost data point:
            if (x < EndpointLeft) {
                switch (Settings.Extrap) {
                    case ExtrapMode.UseDataPointSlope:
                        return (x - EndpointLeft) * ((dp[1].Y - dp[0].Y) / (dp[1].X - dp[0].X)) + dp[0].Y;
                    case ExtrapMode.UseSplineSlope:
                        return (x - EndpointLeft) * cs[0].b + dp[0].Y;
                    case ExtrapMode.UseNearestDataPoint:
                        return dp[0].Y;
                }
            }

            //If we are to the right of the rightmost data point:
            if (x > EndpointRight) {
                switch (Settings.Extrap) {
                    case ExtrapMode.UseDataPointSlope:
                        return (x - EndpointRight) * ((dp[NumPoints - 1].Y - dp[NumPoints - 2].Y) / (dp[NumPoints - 1].X - dp[NumPoints - 2].X)) + dp[NumPoints - 1].Y;
                    case ExtrapMode.UseSplineSlope:
                        return (x - EndpointRight) * (cs[NumPoints - 2].b + 2.0 * cs[NumPoints - 2].c * (EndpointRight - cs[NumPoints - 2].x0) + 3.0 * cs[NumPoints - 2].d * Math.Pow(EndpointRight - cs[NumPoints - 2].x0, 2.0)) + dp[NumPoints - 1].Y;
                    case ExtrapMode.UseNearestDataPoint:
                        return dp[NumPoints - 1].Y;
                }
            }

            //Find the first data point with an x-coordinate greater than the point we're trying to get,
            //so that we use the correct cubic spline:
            while (pt < (NumPoints - 1) && cs[pt].x0 <= x) {
                pt++;
            } pt--;

            return cs[pt].a + cs[pt].b * (x - cs[pt].x0) + cs[pt].c * Math.Pow(x - cs[pt].x0, 2.0) + cs[pt].d * Math.Pow(x - cs[pt].x0, 3.0);
        }

        /// <summary>  
        /// <para>After the splines have been fit to the data points, this function returns the
        /// first derivative of the splines corresponding to any x-coordinate. If the x-coordinate 
        /// is beyond the range of the original data points, the derivative at the minimum or maximum
        /// x-coordinate is returned.</para>
        /// <para>If IsReady reutrns "false," then this function returns zero.</para>
        /// </summary>
        // TODO: better extrapolation modes could be implemented here, since this extrapolation method
        // is not consistent with all of the spline value extrapolation methods.
        public double GetSplineSlope(double x) {            
            double xp = x; // "x prime", i.e. x adjusted for the x-coordinate bounds
            CubicSpline[] cs = CubicSplineArray; //shorthand for the cubic spline array
            DataPoint[] dp = DataPointArray; //shorthand for the data point array

            if (ModelFitted == false)
                return 0.0;

            if (xp < EndpointLeft)
                xp = EndpointLeft;
            else if (xp > EndpointRight)
                xp = EndpointRight;

            //Find the first data point with an x-coordinate greater than the point we're trying to get:
            int pt = 0;
            while (pt < (NumPoints - 1) && cs[pt].x0 <= x) {
                pt++;
            } pt--;

            // Return the first derivative of the spline curve:
            return cs[pt].b + 2.0 * cs[pt].c * (x - cs[pt].x0) + 3.0 * cs[pt].d * Math.Pow(x - cs[pt].x0, 2.0);
        }

        /// <summary> 
        /// Indicates whether the splines have been fit to the data points. If this property returns
        /// "false," then no spline data will be available from the class.
        /// </summary>
        public bool IsReady {
            get { 
                return ModelFitted; 
            }
        }

        /// <summary> 
        /// Returns the number of splines which were fit to the data points
        /// </summary>
        public int NumSplines {
            get {
                if (ModelFitted == true) {
                    return NumPoints - 1;
                }
                else {
                    return 0;
                }
            }
        }


        /// <summary> 
        /// Returns a copy of the spline array. This copy is not affected by any subsequent changes
        /// to the instance of the CubicSplineSet class.
        /// </summary>
        public CubicSpline[] GetSplineArrayCopy() {
            if (ModelFitted == true) {
                CubicSpline[] cubicSplineArrayToReturn = (CubicSpline[])CubicSplineArray.Clone();
                return cubicSplineArrayToReturn;
            }
            else {
                return null;
            }
        }
    }
    public class InterpolationMethods
    {
        public double LinearInterpolation(Dictionary<double, double> values, double x)
        {
            double y = 0;
            //var inputs = values.OrderBy(v => v.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            double x1 = values.Keys.Where(k => k <= x).Max();
            double x2 = values.Keys.Where(k => k > x).Max();

            double y1 = values[x1];
            double y2 = values[x2];

            y = y1 + (x - x1) * (y2 - y1) / (x2 - x1);
            return y;
        }
    }
}
