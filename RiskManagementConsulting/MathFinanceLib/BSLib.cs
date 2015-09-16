using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFinanceLib
{
    public class BlackScholeLib
    {
        static public double CalcD1(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = (Math.Log(s / k) + (r - q + 0.5 * Math.Pow(vol, 2)) * t) / (vol * Math.Sqrt(t));
            return d1;
        }

        static public double CalcD2(double s, double k, double vol, double r, double q, double t)
        {
            double d2 = CalcD1(s, k, vol, r, q, t);
            d2 -= vol * Math.Sqrt(t);
            return d2;
        }

        static public double N(double d)
        {
            return CDFNormal(d);
        }

        static public double NPrime(double d)
        {
            return Math.Exp(-0.5 * Math.Pow(d, 2)) / Math.Sqrt(2 * Math.PI);
        }

        static public double CDFNormal(double x)
        {
            const double a = 0.2316419;
            const double a1 = 0.31938153;
            const double a2 = -0.356563782;
            const double a3 = 1.781477937;
            const double a4 = -1.821255978;
            const double a5 = 1.330274429;

            double d = 1 / (1 + a * Math.Abs(x));
            double polynomial = a1 * d + a2 * Math.Pow(d, 2) + a3 * Math.Pow(d, 3) + a4 * Math.Pow(d, 4) + a5 * Math.Pow(d, 5);
            double cdf = 1 - Math.Exp(-0.5 * Math.Pow(x, 2)) * polynomial / Math.Sqrt(2 * Math.PI);
            if (x < 0)
            {
                cdf = 1 - cdf;
            }
            return cdf;
        }

        //option payoff
        static public double VanillaCallPayoff(double s, double k)
        {
            return Math.Max(s - k, 0);
        }

        static public double VanillaPutPayoff(double s, double k)
        {
            return Math.Max(k - s, 0);
        }

        //option price
        static public double VanillaCallPrice(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            double d2 = CalcD2(s, k, vol, r, q, t);

            return s * Math.Exp(-q * t) * N(d1) - k * Math.Exp(-r * t) * N(d2);
        }

        static public double VanillaPutPrice(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            double d2 = CalcD2(s, k, vol, r, q, t);

            return k * Math.Exp(-r * t) * N(-d2) - s * N(-d1) * Math.Exp(-q * t);
        }

        //Greeks calculation for vanilla call
        static public double VanillaCallDelta(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            return Math.Exp(-q * t) * N(d1);
        }

        static public double VanillaCallGamma(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            return Math.Exp(-q * t) * NPrime(d1) / (s * vol * Math.Sqrt(t));
        }

        static public double VanillaCallTheta(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            double d2 = CalcD2(s, k, vol, r, q, t);
            double theta = -s * Math.Exp(-q * t) * NPrime(d1) * vol / (2 * Math.Sqrt(t));
            theta += q * s * N(d1) * Math.Exp(-q * t);
            theta += r * k * Math.Exp(-r * t) * N(d2);
            return theta;
        }

        static public double VanillaCallVega(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            double vega = s * Math.Sqrt(t) * Math.Exp(-q * t) * NPrime(d1);
            return vega;
        }

        //Greeks calculation for vanilla put
        static public double VanillaPutDelta(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            return Math.Exp(-q * t) * (N(d1) - 1);
        }

        static public double VanillaPutGamma(double s, double k, double vol, double r, double q, double t)
        {
            return VanillaCallGamma(s, k, vol, r, q, t);
        }

        static public double VanillaPutTheta(double s, double k, double vol, double r, double q, double t)
        {
            double d1 = CalcD1(s, k, vol, r, q, t);
            double d2 = CalcD2(s, k, vol, r, q, t);
            double theta = -s * Math.Exp(-q * t) * NPrime(d1) * vol / (2 * Math.Sqrt(t));
            theta -= q * s * N(-d1) * Math.Exp(-q * t);
            theta += r * k * Math.Exp(-r * t) * N(-d2);
            return theta;
        }

        static public double VanillaPutVega(double s, double k, double vol, double r, double q, double t)
        {
            return VanillaCallVega(s, k, vol, r, q, t);
        }
    }

}
