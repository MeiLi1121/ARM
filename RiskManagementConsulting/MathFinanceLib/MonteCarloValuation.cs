using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFinanceLib
{
    public class MonteCarloValuation
    {
        public double[][] ScenarioObject { set; get; }
        public double Volatility { set; get; }
        public double InitialSpot { set; get; }
        public int NumTimeStep;
        public double NumYears;
        public double TimeStepSize;
        public int NumScenario;
        public double RiskFreeRate;
        public double Dividend;

        public double[][] GenerateGBMScenario()
        {
            var scn = new double[NumScenario][];

            for (int i = 0; i < NumScenario; i++)
            {
                scn[i] = new double[NumTimeStep];
                scn[i][0] = 1.0;
            }

            for (int i = 0; i < NumScenario; i++)
            {
                for (int j = 1; j < NumTimeStep; j++)
                {
                    var rnd = GetNormalRnd();
                    scn[i][j] = Math.Exp((RiskFreeRate - Dividend - 0.5 * Volatility * Volatility) * TimeStepSize + Volatility * Math.Sqrt(TimeStepSize) * rnd) * scn[i][j-1];
                }
            }
            return scn;
        }

        public double GetNormalRnd()
        {
            double rnd;
            SimpleRNG.SetSeed(12345);
            rnd = SimpleRNG.GetNormal();
            return rnd;
        }

        public double[][] GetAssetPriceScenarios()
        {
            var scn = new double[NumScenario][];
            var tmpScn = ScenarioObject;
            var s = InitialSpot;
            for (int i = 0; i < NumScenario; i++)
            {
                scn[i] = new double[NumTimeStep];
                for (int j = 0; j < NumTimeStep; j++)
                {
                    scn[i][j] = tmpScn[i][j] * s;
                }
            }
            return scn;
        }
    }
}
