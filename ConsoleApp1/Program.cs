using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using ClassLibrary1.Calculations;
using ClassLibrary1;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
           Session.DoLogic(enum_Profiles.vp50, new double[] { 1500, 2000, 2500, 3000, 4000, 5000, 7500 });            
        } 
    }
}
