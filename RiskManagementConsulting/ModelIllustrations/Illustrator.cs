using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using MathFinanceLib;

namespace ModelIllustrations
{
    public partial class Illustrator : Form
    {
        const double DEFAULT_SPOT = 100.0;
        const double DEFAULT_STRIKE = 100.0;
        const double DEFAULT_VOL = 0.2;
        const double DEFAULT_RATE = 0.01;
        const double DEFAULT_DIVIDEND = 0.02;
        const double DEFAULT_T = 0.5;

        const double DEFAULT_SPOT_MIN = 50.0;
        const double DEFAULT_SPOT_MAX = 150.0;
        const double DEFAULT_STRIKE_MIN = 50.0;
        const double DEFAULT_STRIKE_MAX = 150.0;
        const double DEFAULT_VOL_MIN = 0.1;
        const double DEFAULT_VOL_MAX = 0.5;
        const double DEFAULT_RATE_MIN = 0.0;
        const double DEFAULT_RATE_MAX = 0.05;
        const double DEFAULT_DIVIDEND_MIN = 0.0;
        const double DEFAULT_DIVIDEND_MAX = 0.05;
        const double DEFAULT_T_MIN = 1/252;
        const double DEFAULT_T_MAX = 1;

        private double[] SPOT_SHOCK_LIST = new double[17] { -0.3, -0.2, -0.1, -0.05, -0.04, -0.03, -0.02, -0.01, 0.0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.1, 0.2, 0.3 };

        private BSBasic.BSOption ValuedOption;
        private BSBasic.BSOption DefaultOption;
        private Dictionary<double, double> DefaultOptionCurveData;
        private Dictionary<double, double> UserDefinedOptionCurveData;
        private double PrevPrice;

        public Illustrator()
        {
            //TAB: Black Schole's illustration
            CreateDefaultOption();
            InitializeComponent();
            SetDefaults();

            //TAB: Delta Hedge's illustration
            deltaTab.Select();
            SetDefaultHedgedOption();
        }

        #region BSIllustration
        private void AutoRefresh()
        {
            RefreshOption();
            RefreshGraph();
            RefreshGreekData();
        }

        private void RefreshOption()
        {
            ValuedOption = new BSBasic.BSOption();
            ValuedOption.Type = (this.putRadioButton.Checked ? BSBasic.OptionType.EuroPut : BSBasic.OptionType.EuroCall);
            ValuedOption.Spot = (double)spotBar.Value;
            ValuedOption.Strike = (double)strikeBar.Value;
            ValuedOption.Volatility = volBar.Value / 100.0;
            ValuedOption.Rate = rateBar.Value / 1000.0;
            ValuedOption.Dividend = divBar.Value / 1000.0;
            ValuedOption.T = timeBar.Value / 252.0;

            if (ValuedOption.Type == BSBasic.OptionType.EuroPut)
            {
                ValuedOption.Price = BlackScholeLib.VanillaPutPrice(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Delta = BlackScholeLib.VanillaPutDelta(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Gamma = BlackScholeLib.VanillaPutGamma(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Theta = BlackScholeLib.VanillaPutTheta(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Vega = BlackScholeLib.VanillaPutVega(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
            }
            else
            {
                ValuedOption.Price = BlackScholeLib.VanillaCallPrice(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Delta = BlackScholeLib.VanillaCallDelta(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Gamma = BlackScholeLib.VanillaCallGamma(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Theta = BlackScholeLib.VanillaCallTheta(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
                ValuedOption.Vega = BlackScholeLib.VanillaCallVega(ValuedOption.Spot, ValuedOption.Strike, ValuedOption.Volatility, ValuedOption.Rate, ValuedOption.Dividend, ValuedOption.T);
            }
            UserDefinedOptionCurveData = GenerateOptionCurve(ValuedOption);
        }

        private Dictionary<double, double> GenerateOptionCurve(BSBasic.BSOption valuedOpt)
        {
            var pvCurve = new Dictionary<double,double>();
            var centeredSpot = valuedOpt.Spot;
            var opt = valuedOpt;
            for (int i = 0; i < SPOT_SHOCK_LIST.Length; i++)
            {
                var spotShocked = centeredSpot * (1 + SPOT_SHOCK_LIST[i]);
                if (opt.Type == BSBasic.OptionType.EuroPut)
                {
                    var priceShocked = BlackScholeLib.VanillaPutPrice(spotShocked, opt.Strike, opt.Volatility, opt.Rate, opt.Dividend, opt.T);
                    pvCurve.Add(spotShocked, priceShocked);
                }
                else
                {
                    var priceShocked = BlackScholeLib.VanillaCallPrice(spotShocked, opt.Strike, opt.Volatility, opt.Rate, opt.Dividend, opt.T);
                    pvCurve.Add(spotShocked, priceShocked);
                }
            }
            return pvCurve;
        }

        private void RefreshGraph()
        {
            bsGraphControl.GraphPane.CurveList.Clear();
            bsGraphControl.GraphPane.YAxis.Scale.Max = 50;
            bsGraphControl.GraphPane.YAxis.Scale.Min = 0;
            bsGraphControl.GraphPane.XAxis.Scale.Min = DEFAULT_SPOT_MIN;
            bsGraphControl.GraphPane.XAxis.Scale.Max = DEFAULT_SPOT_MAX;

            var centerPoint = (double)spotBar.Value;
            var points = new PointPairList(UserDefinedOptionCurveData.Keys.ToArray(), UserDefinedOptionCurveData.Values.ToArray());
            var defaultPoints = new PointPairList(DefaultOptionCurveData.Keys.ToArray(), DefaultOptionCurveData.Values.ToArray());
            var price = UserDefinedOptionCurveData[centerPoint];
            var pricePoint = new PointPairList(new double[1] { centerPoint }, new double[1] { price });

            LineItem curve2A = bsGraphControl.GraphPane.AddCurve("", pricePoint, Color.Red, SymbolType.Circle);
            LineItem curve1A = bsGraphControl.GraphPane.AddCurve("User Defined Option PV Curve", points, Color.Blue, SymbolType.Diamond);
            LineItem curve3A = bsGraphControl.GraphPane.AddCurve("Default Option PV Curve", defaultPoints, Color.Gray, SymbolType.HDash);

            curve2A.Symbol.Size = 12F;
            curve2A.Symbol.Fill = new Fill(Color.Red);
            curve2A.Tag = 1;

            curve1A.Symbol.Size = 5F;
            curve1A.Symbol.Fill = new Fill(Color.Blue);
            curve1A.Line.Width = 2F;
            curve1A.Tag = 2;

            curve3A.Symbol.Size = 5F;
            curve3A.Symbol.Fill = new Fill(Color.Gray);
            curve3A.Line.Width = 2F;
            curve3A.Tag = 3;

            bsGraphControl.Refresh();
            bsGraphControl.AxisChange();
        }

        private void RefreshGreekData()
        {
            bsDataGridView.Rows[0].Cells[0].Value = ValuedOption.Price.ToString("0.##");
            bsDataGridView.Rows[0].Cells[1].Value = ValuedOption.Delta.ToString("p2");
            bsDataGridView.Rows[0].Cells[2].Value = ValuedOption.Gamma.ToString("0.##");
            bsDataGridView.Rows[0].Cells[3].Value = ValuedOption.Theta.ToString("0.##");
            bsDataGridView.Rows[0].Cells[4].Value = (ValuedOption.Vega/100).ToString("p2");

            //price
            if (Math.Abs(ValuedOption.Price) > Math.Abs(PrevPrice))
            {
                bsDataGridView[0, 0].Style.BackColor = Color.Green;
                bsDataGridView[0, 0].Style.ForeColor = Color.White;
            }
            else
            {
                bsDataGridView[0, 0].Style.BackColor = Color.DarkRed;
                bsDataGridView[0, 0].Style.ForeColor = Color.White;
            }

            PrevPrice = ValuedOption.Price;
            bsDataGridView.Refresh();
        }

        private void CreateDefaultOption()
        {
            //fill default option curve data
            DefaultOption = new BSBasic.BSOption();
            DefaultOption.Type = BSBasic.OptionType.EuroPut;
            DefaultOption.Spot = DEFAULT_SPOT;
            DefaultOption.Strike = DEFAULT_STRIKE;
            DefaultOption.Volatility = DEFAULT_VOL;
            DefaultOption.Rate = DEFAULT_RATE;
            DefaultOption.Dividend = DEFAULT_DIVIDEND;
            DefaultOption.T = DEFAULT_T;
            DefaultOptionCurveData = GenerateOptionCurve(DefaultOption);

            if (DefaultOption.Type == BSBasic.OptionType.EuroPut)
            {
                DefaultOption.Price = BlackScholeLib.VanillaPutPrice(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Delta = BlackScholeLib.VanillaPutDelta(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Gamma = BlackScholeLib.VanillaPutGamma(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Theta = BlackScholeLib.VanillaPutTheta(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Vega = BlackScholeLib.VanillaPutVega(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
            }
            else
            {
                DefaultOption.Price = BlackScholeLib.VanillaCallPrice(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Delta = BlackScholeLib.VanillaCallDelta(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Gamma = BlackScholeLib.VanillaCallGamma(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Theta = BlackScholeLib.VanillaCallTheta(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
                DefaultOption.Vega = BlackScholeLib.VanillaCallVega(DefaultOption.Spot, DefaultOption.Strike, DefaultOption.Volatility, DefaultOption.Rate, DefaultOption.Dividend, DefaultOption.T);
            }

            PrevPrice = DefaultOption.Price;
        }

        private void SetDefaults()
        {
            //set default for GUI controls
            //track bar
            spotMinLabel.Text = DEFAULT_SPOT_MIN.ToString();
            spotMaxLabel.Text = DEFAULT_SPOT_MAX.ToString();
            strikeMinLabel.Text = DEFAULT_STRIKE_MIN.ToString();
            strikeMaxLabel.Text = DEFAULT_STRIKE_MAX.ToString();
            volMinLabel.Text = DEFAULT_VOL_MIN.ToString("p2");
            volMaxLabel.Text = DEFAULT_VOL_MAX.ToString("p2");
            rateMinLabel.Text = DEFAULT_RATE_MIN.ToString("p2");
            rateMaxLabel.Text = DEFAULT_RATE_MAX.ToString("p2");
            divMinLabel.Text = DEFAULT_DIVIDEND_MIN.ToString("p2");
            divMaxLabel.Text = DEFAULT_DIVIDEND_MAX.ToString("p2");
            timeMinLabel.Text = DEFAULT_T_MIN.ToString();
            timeMaxLabel.Text = DEFAULT_T_MAX.ToString();

            spotBar.Minimum = (int)DEFAULT_SPOT_MIN;
            spotBar.Maximum = (int)DEFAULT_SPOT_MAX;
            spotBar.Value = (int)DEFAULT_SPOT;
            //spotLabel.Text = "Spot = " + spotBar.Value.ToString();

            strikeBar.Minimum = (int)DEFAULT_STRIKE_MIN;
            strikeBar.Maximum = (int)DEFAULT_STRIKE_MAX;
            strikeBar.Value = (int)DEFAULT_STRIKE;
            //strikeLabel.Text = "Strike = " + strikeBar.Value.ToString();

            volBar.Minimum = (int)(DEFAULT_VOL_MIN * 100);
            volBar.Maximum = (int)(DEFAULT_VOL_MAX * 100);
            volBar.Value = (int)(DEFAULT_VOL * 100);
            volLabel.Text = "Volatility = " + (volBar.Value / 100.0).ToString("p2");

            rateBar.Minimum = (int)(DEFAULT_RATE_MIN * 1000);
            rateBar.Maximum = (int)(DEFAULT_RATE_MAX * 1000);
            rateBar.Value = (int)(DEFAULT_RATE * 1000);
            //rateLabel.Text = "Rate = " + (rateBar.Value / 1000.0).ToString("p2");

            divBar.Minimum = (int)(DEFAULT_DIVIDEND_MIN * 1000);
            divBar.Maximum = (int)(DEFAULT_DIVIDEND_MAX * 1000);
            divBar.Value = (int)(DEFAULT_DIVIDEND * 1000);
            //divLabel.Text = "Dividend = " + (divBar.Value / 1000.0).ToString("p2");

            timeBar.Minimum = (int)(DEFAULT_T_MIN * 252);
            timeBar.Maximum = (int)(DEFAULT_T_MAX * 252);
            timeBar.Value = (int)(DEFAULT_T * 252);
            //timeLabel.Text = "Time = " + (timeBar.Value / 252.0).ToString();

            //graph layout
            bsGraphControl.GraphPane.Title.Text = "Option Curve";
            bsGraphControl.GraphPane.YAxis.Title.Text = "Price";
            bsGraphControl.GraphPane.XAxis.Title.Text = "Spot";

            
        }

        private void spotBar_Scroll(object sender, EventArgs e)
        {
            spotLabel.Text = "Spot = " + spotBar.Value.ToString();
            AutoRefresh();
        }

        private void strikeBar_Scroll(object sender, EventArgs e)
        {
            strikeLabel.Text = "Strike = " + strikeBar.Value.ToString();
            AutoRefresh();
        }

        private void volBar_Scroll(object sender, EventArgs e)
        {
            volLabel.Text = "Volatility = " + (volBar.Value / 100.0).ToString("p2");
            AutoRefresh();
        }

        private void rateBar_Scroll(object sender, EventArgs e)
        {
            rateLabel.Text = "Rate = " + (rateBar.Value / 1000.0).ToString("p2");
            AutoRefresh();
        }

        private void divBar_Scroll(object sender, EventArgs e)
        {
            divLabel.Text = "Dividend = " + (divBar.Value / 1000.0).ToString("p2");
            AutoRefresh();
        }

        private void timeBar_Scroll(object sender, EventArgs e)
        {
            timeLabel.Text = "Time = " + (timeBar.Value / 252.0).ToString("0.##");
            AutoRefresh();
        }

        private void spotBarReset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                spotBar.Value = (int)DEFAULT_SPOT;
            }
        }

        private void strikeBarReset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                strikeBar.Value = (int)DEFAULT_STRIKE;
            }
        }

        private void volBarReset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                volBar.Value = (int)(DEFAULT_VOL * 100);
            }
        }

        private void rateBarReset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rateBar.Value = (int)(DEFAULT_RATE * 1000);
            }
        }

        private void divBarReset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                divBar.Value = (int)(DEFAULT_DIVIDEND * 1000);
            }
        }

        private void timeBarReset(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                timeBar.Value = (int)(DEFAULT_T * 252);
            }
        }

        private void putRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            DefaultOption.Type = (this.putRadioButton.Checked ? BSBasic.OptionType.EuroPut : BSBasic.OptionType.EuroCall);
            DefaultOptionCurveData = GenerateOptionCurve(DefaultOption);
            AutoRefresh();
        }

        private void callRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            DefaultOption.Type = (this.putRadioButton.Checked ? BSBasic.OptionType.EuroPut : BSBasic.OptionType.EuroCall);
            DefaultOptionCurveData = GenerateOptionCurve(DefaultOption);
            AutoRefresh();
        }
        #endregion

        private void SetDefaultHedgedOption()
        {
            //var dataList = new List<string>() { "PUT", "CALL" };
            //OptionType.DataSource = dataList;
            //OptionType.Items[0] = dataList[0];
        }
    }
}
