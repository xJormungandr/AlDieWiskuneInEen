using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary1.Calculations
{
    public class Rebar
    {

        String type;
        int size;
        //double cover;

        public Rebar(String type, int size)
        {
            this.type = type;
            this.size = size;
        }

        public double getArea()
        {
            double area = Math.PI * Math.Pow(size / 2, 2);
            return area;
        }

        public String toString()
        {
            return type;
        }

    }
}