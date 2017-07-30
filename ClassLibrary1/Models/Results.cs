using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Models
{
    class Results
    {
        public string profileName { get; set; }
        public static List<string> profileLengths { get; set; }



    }

    public class ResultsList
    {
        public double imposedLoad;
        public double deadLoad;
        public double factoredLoad;
        public double slabThickness;
    }

}
