using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Calculations
{
    class Util
    {
        public static List<double> increment(double start, double increment, int numberOfExtra)
        {
            var d = new List<double>();
            d.Add(start);
            for (int i = 1; i <= numberOfExtra; i++)
            {
                d[i] = d[i - 1] + increment;
            }
            return d;
        }
    }
}
