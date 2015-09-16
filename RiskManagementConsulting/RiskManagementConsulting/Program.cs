using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathFinanceLib;

namespace RiskManagementConsulting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var a = new MathFinanceLib.MonteCarloValuation();
            a.NumScenario = 100;
            a.NumTimeStep = 252;
            a.TimeStepSize = 1 / 252.0;
            a.Volatility = 0.2;
            a.RiskFreeRate = 0.05;
            a.Dividend = 0.0;
            a.ScenarioObject = a.GenerateGBMScenario();
            Console.Read();
            //a.ScenarioObject = "";
            //a.ValueAsset();

        }
    }
}
