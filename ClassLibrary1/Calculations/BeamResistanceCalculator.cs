using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Calculations
{
    public class BeamResistanceCalculator
    {
        public double As;
        public double fy = SharedData.fy_rebar;
        public double fcu;
        public double b;
        public double d;
        public double k1;
        public double k2;
        public double gamma_r;
        public double Es = 2e5;
        public double eps_c = 0.0035;
        public double eps_s;
        public double Mr;

        public BeamResistanceCalculator(double fcu,double b)
        {
            this.fcu = fcu;
            this.b = b;
            k1 = 0.45 * (1 - (Math.Sqrt(fcu) / 52.5));
            k2 = (Math.Pow(2 - Math.Sqrt(fcu) / 17.5, 2) + 2) / (4 * (3 - (Math.Sqrt(fcu) / 17.5)));
            gamma_r = 1 / SharedData.gamma_r;
            eps_s = fy / Es;
        }
        
        public double getBalancedRebarRatio(double k1,double fy, double fcu)
        {
            double rho_b = (k1 * fcu / (gamma_r * fy)) * (eps_c / (eps_s + eps_c));
            return rho_b;
        }

        public double getUltimateMoment()
        {
            double rho_b = getBalancedRebarRatio(k1, fy, fcu);
            double rho = As / (b * d);
            if(rho<rho_b)
            {
                Mr = As * (gamma_r * fy) * (1 - (rho * (k2 * gamma_r * fy) / (k1 * fcu))) * d;
            }
            else
            {
                Mr = double.NaN;
                Console.WriteLine("As/bd balanced = %.2f\n", rho_b);
                Console.WriteLine("As/bd required = %.2f\n", rho);
                Console.WriteLine("The beam is over reinforced");
            }
            return Mr;
        }

        public void setYieldStress(double new_fy)
        {
            fy = new_fy;
        }

        public void setRebarArea(double rebar)
        {
            As = rebar;
        }

        public void setEffectiveDepth(double d)
        {
            this.d = d;
        }

        public static void main()
        {
            double PI = Math.PI;
            int n = 4;
            double dia = 12;
            double As = 500;
            Console.WriteLine("As = %.2f [mm^2]\n: "+ As);
            double h = 600;
            double cover = 25;
            double d = 500;
            Console.Write("d = " + d);
            double Mu = 100e6;
            BeamResistanceCalculator r = new BeamResistanceCalculator(30, 300);
            r.setEffectiveDepth(d);
            r.setRebarArea(As);
            r.getUltimateMoment();
            Console.WriteLine("Resistance moment = %.2f [Nmm]\n: " + r.getUltimateMoment());
            Console.WriteLine("Difference Between Applied and Resistance Moment: " + (Mu - r.getUltimateMoment()));

            BeamReinforcingCalculator rebarCalculator = new BeamReinforcingCalculator(Mu, 30, 300);
            double finalX = 0.0;
            double finalAs = 0.0;

            rebarCalculator.setEffectiveDepth(d);
            rebarCalculator.setYieldStress(450);
            rebarCalculator.calculate(ref finalX,ref finalAs);
            rebarCalculator.getSpanLimit(As, 450);

            Console.WriteLine("Depth of Neutral Axis = %.2f [mm]\n: "+ finalX);
            Console.WriteLine("Rebar Required = %.2f [mm^2]\n :"+ finalAs);







        }

    }


}
