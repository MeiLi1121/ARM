using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathFinanceLib;

namespace RZTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] swap_terms = {0.5, 1.0};
            double[] swap_rates = {0.5343, 0.5318};

            IRStruct interest_rate = new IRStruct(swap_terms, swap_rates);
            Dictionary<double, double> rates_dictionary = interest_rate.setYields(swap_terms, swap_rates);
            
            //foreach (var pair in rates_dictionary)
            //{
            //    Console.WriteLine("{0},{1}", pair.Key.ToString(), pair.Value.ToString());
            //}

            Dictionary<double, double> discount_factor = interest_rate.Bootstrap(rates_dictionary);

            foreach (var pair in discount_factor)
            {
                Console.WriteLine("{0},{1}", pair.Key.ToString(), pair.Value.ToString());
            }
            Console.Read();
        }
    }
}
