using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelIllustrations
{
    public class BSBasic
    {
        public enum OptionType
        {
            EuroPut,
            EuroCall,
            AmericanPut,
            AmericanCall,
            AsianPut,
            AisanCall,
            BinaryPut,
            BinaryCall,
            Cliquet
        }
        public class BSOption
        {
            public OptionType Type;
            public double Spot;
            public double Strike;
            public double Volatility;
            public double T;
            public double Rate;
            public double Dividend;

            public double Price;
            public double Delta;
            public double Gamma;
            public double Theta;
            public double Vega;
        }

        public class Future
        {
            public double Spot;
            public double T;
            public double Rate;
            public double Dividend;
        }

        public BSOption CurrOption;

        public static BSOption InitializeOption(OptionType type, double spot, double strike, double vol, double rate, double div, double t)
        {
            //set default values for option
            var opt = new BSOption();
            opt.Type = type;
            opt.Spot = spot;
            opt.Strike = strike;
            opt.Volatility = vol;
            opt.T = t;
            opt.Rate = rate;
            opt.Dividend = div;
            return opt;
        }
    }
}
