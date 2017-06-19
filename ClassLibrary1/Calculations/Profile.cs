using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary1.Calculations
{
    public class Profile
    {

       public double w, t, At, tensionSteelCentroid, Ac, depth, coverWidth, webWidth;
        public String name;

        public Profile(String name,double deckDepth, double webWidth, double coverWidth,
                        double unrolled_with, double plate_thickness, double tension_area,
                        double tensionSteelCentroid, double concrete_area)
        {
            this.name = name;
            this.depth = deckDepth;
            this.webWidth = webWidth;
            this.coverWidth = coverWidth;
            this.w = unrolled_with;
            this.t = plate_thickness;
            this.At = tension_area;
            this.tensionSteelCentroid = tensionSteelCentroid;
            this.Ac = concrete_area;

        }

        public String toString()
        {
            return name;
        }


    }
}