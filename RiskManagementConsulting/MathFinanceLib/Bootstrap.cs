using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFinanceLib
{
    class Bootstrap
    {
        public Dictionary<double, double> getParYields(double[] termYrs, double[] parRates)
        {
            var parYieldsDic = new Dictionary<double, double>();
            for (int i = 0; i < termYrs.Length; i++)
            {
                parYieldsDic.Add(termYrs[i], parRates[i]);
            }
            return parYieldsDic;
        }

        //public Dictionary<double, double> Bootstrapping(Dictionary<double, double> parYields)
        //{
            
        //    return;
        //}

        public double bootstrapNext(double coupon, Dictionary<double, double> discountFactor)
        {
            double nextValue;
            double numerator = 1.0;
            for (double i = 0.0; i < discountFactor.Keys.Last(); i += 0.5)
            {
                numerator -= coupon * discountFactor[i];
            }
            nextValue = numerator / (1.0 + coupon);
            return nextValue;
        }
    }
}
