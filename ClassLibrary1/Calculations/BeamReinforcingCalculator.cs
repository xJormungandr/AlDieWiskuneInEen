using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet;




namespace ClassLibrary1.Calculations
{
    public class BeamReinforcingCalculator
    {
        double As;
        double fy;
        double fcu;
        double b;
        double d;
        double k1;
        double k2;
        double gamma_r;
        double Es = 2e5;
        double eps_c = 0.0035;
        double eps_s;
        double Mu;
        double x;

        public BeamReinforcingCalculator(double Mu, double fcu,double b)
        {
            this.Mu = Mu;
            this.fcu = fcu;
            this.b = b;

            k1 = 0.45 * (1 - (Math.Sqrt(fcu) / 52.5));
            k2 = (Math.Pow(2 - Math.Sqrt(fcu) / 17.5, 2) + 2) / (4 * (3 - (Math.Sqrt(fcu) / 17.5)));
            gamma_r = 1 / SharedData.gamma_r;
        }

        public void setYieldStress(double new_fy)
        {
            fy = new_fy;
        }

        public void setEffectiveDepth(double new_d)
        {
            d = new_d;
        }

        public void calculate(ref double _x, ref double _As)
        {
            double o = -k1 * fcu * b * -k2;
            double p = -k1 * fcu * b * d;
            double q = Mu;

            double x1 = (-p + Math.Sqrt(p * p - 4 * o * q)) / (2 * o);
            double x2 = (-p - Math.Sqrt(p * p - 4 * o * q)) / (2 * o);

            double x = Math.Min(x1, x2);
            double As = Mu / (gamma_r * fy * (d - k2 * x));

            //Console.WriteLine(x1.ToString());
            //Console.WriteLine(x2.ToString());
            //Console.ReadLine();

            _x = x;
            _As = As;
        }

        public double getSpanLimit(double As_supplied, double fy_avg)
        {
            BeamResistanceCalculator MrCalc = new BeamResistanceCalculator(fcu, b);
            MrCalc.setRebarArea(As_supplied);
            MrCalc.setEffectiveDepth(d);
            MrCalc.setYieldStress(fy_avg);
            double Mr = MrCalc.getUltimateMoment();
                       
            double beta_b = 1.0;

            double As_req = this.As;
            double fservice = (fy / SharedData.gamma_r) * ((1.1 + 1.0) / (SharedData.phi_uls_dl + SharedData.phi_uls_ll)) * (As_req / As_supplied) * (1 / beta_b);

            double M_over_bd_squared = (Mu / (b * Math.Pow(d, 2)));
            double mod_factor = 0.55 + (477 - fservice) / (120 * (0.9 + (M_over_bd_squared)));

            if(mod_factor > 2.0)
            {
                mod_factor = 2.0;
            }

            double allowed_span = (SharedData.l_over_d_limit * mod_factor) * (d / 1000);
            Console.WriteLine("Difference  Applied and Resistance Moment = %.10f\n", Mu - Mr);
            return allowed_span;
        }
    }
}
