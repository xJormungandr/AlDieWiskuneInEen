using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Calculations
{
    public class Beam
    {
        public int coord;
        public double width;
        public double b_web;
        public double fcu;
        private double mc;
        private double ms;
        public String name;
        public double w;
        public double wc;
        public double wd;
        public double own_weight;
        public double liveLoad;
        public double wd_super;
        public double Mu;
        public double Mr;
        public double As; //Total Tensile Steel area, including the deck steel
        public double As_add;
        public double As_modified;
        public double span;
        public double h;
        public double mod_factor;

        public double UnfactoredLL;
        public double UnfactoredDL;

        public double d;
        public double y; // The centroid of the tension steel
        public double fy; // The weighted yield stress
        public double A_rebar;
        public double neutralAxisDepth;
        public VoidconBeamCalculator c;

        

        public Profile profile;

        public Beam(Profile profile, double span, double webWidth,
                    double thickness, double LL, double super_dead)
        {
            this.profile = profile;
            this.span = span;

            this.width = profile.coverWidth;            
            this.h = thickness;
            this.b_web = webWidth;
            this.liveLoad = LL;
            this.wd_super = super_dead;
            name = profile.toString();
            fcu = SharedData.fcu;
            mc = SharedData.concreteMass;
            ms = SharedData.steelMass;
            

            wc = ((profile.Ac+((h-profile.depth)*width))/1e6)*mc * 9.81; //Weight of Concrete T-Beam [N/m]
            wd = ((profile.w * profile.t) / 1e6) * ms * 9.81; // Weight of steel deck in T-beam in [N/m]
            wd_super = wd_super * (width / 1000); // Superimposed load in [N/m]

            this.UnfactoredLL = LL/1000;
            this.UnfactoredDL = (wc + wd) / width;

            own_weight = wc + wd;
            liveLoad = liveLoad * (width / 1000); // Imposed load in [N/m]      
            
            w = SharedData.phi_uls_dl * (own_weight + wd_super) + SharedData.phi_uls_ll * liveLoad; // [N/m]
            Mu = (w * Math.Pow(span, 2) / 8) * 1000; //[N.mm]
            

            c = new VoidconBeamCalculator(this);

            
            
        }
        public String printFullRecord()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(c.printMessages());
            return sb.ToString();
        }
    }
}
