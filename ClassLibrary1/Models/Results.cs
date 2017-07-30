using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Models
{
    public class Results
    {
        public string profileName { get; set; }
        public List<double> profileLengths { get; set; }
        public List<ResultsList> values { get; set; }
        
    }

    public class ResultsList
    {
        public double imposedLoad { get; set; }
        public double deadLoad { get; set; }
        public double factoredLoad { get; set; }
        public double slabThickness { get; set; }
        public List<string> extraRebar { get; set; }
    }

}
