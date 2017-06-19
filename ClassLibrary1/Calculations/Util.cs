using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Calculations
{
    class Util
    {
        public static double[] increment(double start, double increment, int numberOfExtra)
        {
            double[] d = new double[numberOfExtra + 1];
            d[0] = start;
            for (int i = 1; i <= numberOfExtra; i++)
            {
                d[i] = d[i - 1] + increment;
            }
            return d;
        }
    }
}
