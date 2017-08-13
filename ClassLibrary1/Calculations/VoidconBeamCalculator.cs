using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Calculations;


namespace ClassLibrary1.Calculations
{
    public class VoidconBeamCalculator
    {
        private ArrayList messages;
        Profile p;
        Beam beam;
        double fcu;
        double fy;
        double fy_more;
        double fyr;
        double fyd;
        double fservice;
        double M_over_bd_squared;
        double mod_factor;
        double b;
        double b_web;
        double d;
        double y;
        double d_new;
        double Mu;
        double Mr;
        double Mr_add;
        double As;
        double As_record;
        double k1;
        double k2;
        double gamma_r; // Rebar partial factor
        double gamma_c;
        double x;
        double new_x;
        double z;
        double Fc;
        double Fs;
        double A_rebar;
        double extraRebar;
        double y_deck;
        double A_deck;
        double y_rebar;
        double h;
        double allowed_span;
        double deck_depth;
        public Boolean overReinforced = false;
        public Boolean SLS_limited = false;
        public Boolean ULS_limited = false;
        public Boolean verbose = true;
        public Boolean addRebarForSLS = SharedData.addSteelForSLS;
        public Boolean noConvergence = false;
        //UnivariatePointValuePair pair;
        double span;
        private double inc_limit = 100000;

        public VoidconBeamCalculator(Beam theBeam)
        {
            this.span = theBeam.span;
            this.Mu = theBeam.Mu;
            this.fcu = theBeam.fcu;
            this.b = theBeam.width;
            this.b_web = theBeam.b_web;
            this.deck_depth = theBeam.profile.depth;
            this.p = theBeam.profile;
            this.beam = theBeam;
            messages = new ArrayList();
            fyr = SharedData.fy_rebar;
            fyd = SharedData.fy_deck;
            h = theBeam.h;
            y_deck = theBeam.profile.tensionSteelCentroid;
            A_deck = theBeam.profile.At;
            y_rebar = SharedData.cover + (SharedData.max_bar_size / 2);
            k1 = 0.45 * (1 - (Math.Sqrt(fcu) / 52.5));
            k2 = (Math.Pow(2 - Math.Sqrt(fcu) / 17.5, 2) + 2) / (4 * (3 - (Math.Sqrt(fcu) / 17.5)));
            gamma_r = 1 / SharedData.gamma_r;
            gamma_c = 1 / SharedData.gamma_c;

            calculateRebarRequirement();
            addMessage(recordAnalysisStatistics("Data"));
            addMessage(recordAnalysisStatistics("Rebar"));
            if (Mr < Mu)
            {
                ULS_limited = true;
                if (verbose)
                {
                    addMessage(Environment.NewLine);
                    addMessage("The design is ULS controlled.\n");
                    addMessage(recordAnalysisStatistics("Forces"));
                }
            }
            if (x / d > 0.5)
            {
                overReinforced = true;
            }
            As_record = As;
            if (span > getSpanLimit() & !overReinforced & !ULS_limited)
            {
                SLS_limited = true;
                addMessage("The design is SLS controlled.\n");
                if (addRebarForSLS & !ULS_limited)
                {
                    addMessage(Environment.NewLine);
                    addMessage("Data at START of remedial step: " + new string(Session.Space,1));
                    addMessage(recordAnalysisStatistics("Deflections"));

                    calculateUpdatedRebarRequirement();

                    addMessage(Environment.NewLine);
                    addMessage("Data at END of remedial step: " + new string(Session.Space,1));
                    addMessage(recordAnalysisStatistics("Deflections"));
                    addMessage(recordAnalysisStatistics("Rebar"));
                }
            }
            if (x / d > 0.5)
            {
                overReinforced = true;
            }
            if (Mr < Mu)
            {
                ULS_limited = true;
            }
            if (ULS_limited | overReinforced)
            {
                if (verbose)
                {
                    addMessage(Environment.NewLine);
                    addMessage("The design has become ULS controlled while adding steel to control deflections" + Environment.NewLine);
                    addMessage(recordAnalysisStatistics("Forces"));
                }
            }
            if (!SLS_limited)
            {
                addMessage(Environment.NewLine);
                addMessage("Force record: " + Environment.NewLine );
                addMessage(recordAnalysisStatistics("Forces"));
                getSpanLimit();
                addMessage(Environment.NewLine);
                addMessage(recordAnalysisStatistics("Deflections"));
            }
        }

        private void calculateRebarRequirement()
        {
            int count = 0;
            A_rebar = 0;
            do
            {
                count++;
                y = (y_deck * A_deck + y_rebar * A_rebar) / (A_deck + A_rebar);
                d = h - y;
                fy = (A_deck * fyd + A_rebar * fyr) / (A_deck + A_rebar);
                As = A_deck + A_rebar;
                Fs = gamma_r * fy * As;
                x = 0;
                int count2 = 0;
                do
                {
                    count2++;
                    z = d - k2 * x;
                    if (z > 0.95 * d)
                    {
                        z = 0.95 * d;
                    }
                    if (x > (h - deck_depth))
                    {
                        double aSlab = b * (h - deck_depth);
                        double aWeb = b_web * (x - (h - deck_depth));
                        double FcSlab = k1 * fcu * aSlab;
                        double FcWeb = k1 * fcu * aWeb;
                        Fc = FcSlab + FcWeb;
                    }
                    else
                    {
                        Fc = k1 * fcu * b * x;
                    }
                    if (count2 > 1 & count2 < inc_limit)
                    {
                        x = x + 0.1;
                    }
                } while (Fc < Fs & x < d / 2 & count2 < inc_limit);
                Mr = Math.Min(Fs, Fc) * z;
                if (count > 1 & count < inc_limit)
                {
                    A_rebar = A_rebar + 0.1;
                }

            } while (Mu > Mr & A_rebar < 2500 & count < inc_limit);
        }

        private void calculateUpdatedRebarRequirement()
        {

            int count = 0;
            extraRebar = 0;
            do
            {
                count++;
                y = (y_deck * A_deck + y_rebar * (A_rebar + extraRebar)) / ((A_rebar + extraRebar) + A_deck);
                d = h - y;
                fy = (A_deck * fyd + (A_rebar + extraRebar) * fyr) / (A_deck + A_rebar + extraRebar);
                As = A_deck + A_rebar + extraRebar;
                Fs = gamma_r * fy * As;
                x = 0;
                int count2 = 0;
                do
                {
                    count2++;
                    z = d - k2 * x;
                    if (z > 0.95 * d)
                    {
                        z = 0.95 * d;
                    }
                    if (x > (h - deck_depth))
                    {
                        double aSlab = b * (h - deck_depth);
                        double aWeb = b_web * (x - (h - deck_depth));
                        double FcSlab = k1 * fcu * aSlab;
                        double FcWeb = k1 * fcu * aWeb;
                        Fc = FcSlab + FcWeb;
                    }
                    else
                    {
                        Fc = k1 * fcu * b * x;
                    }
                    if (count2 > 1 & count2 < inc_limit)
                    {
                        x = x + 0.1;
                    }
                } while (Fc < Fs & x < (d / 2) & count2 < inc_limit);
                Mr = Math.Min(Fs, Fc) * z;
                if (count > 1 & count < inc_limit)
                {
                    extraRebar = extraRebar + 0.1;
                }

            } while (span > getSpanLimit() & extraRebar < SharedData.extraRebarLimit & count < inc_limit);

            if (span > getSpanLimit())
            {
                noConvergence = true;
                SLS_limited = true;
                //            extraRebar = 0;
            }
        }

        private double getSpanLimit()
        {
            double beta_b = 1.0; // The amount of moment redistribution 
            double As_req = As_record;
            fservice = (fy / SharedData.gamma_r) * ((1.1 + 1.0) / (SharedData.phi_uls_dl + SharedData.phi_uls_ll)) * (As_req / As) * (1 / beta_b);
            M_over_bd_squared = (Mu / (b * Math.Pow(d, 2)));
            mod_factor = 0.55 + (477 - fservice) / (120 * (0.9 + (M_over_bd_squared)));
            if (mod_factor > 2.0)
            {
                mod_factor = 2.0;
            }
            allowed_span = (SharedData.l_over_d_limit * mod_factor) * (d / 1000);
            //        System.out.printf("Span/Allowed span = %.3f\n",span/allowed_span);
            return allowed_span;
        }

        private void addMessage(String message)
        {
            messages.Add(message);
        }

        public String printMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach (String s in messages)
            {
                sb.Append(s);
                sb.Append(new String(Session.Space, 1));
                               
            };
            return sb.ToString();

        }



        public String printRebarRequirement()
        {
            String rebarRequirement;
            if (!SLS_limited & !overReinforced & !ULS_limited)
            {
                if (SharedData.texFormat)
                {
                    rebarRequirement = (Math.Round(A_rebar / 10) * 10).ToString("F0") + new string(Session.Space, 1);
                }
                else
                {
                    rebarRequirement = (Math.Round(A_rebar / 10) * 10).ToString("F0") + new string(Session.Space, 1);
                }
            }
            else if (SLS_limited & !overReinforced & !ULS_limited & !noConvergence)
            {
                if (SharedData.texFormat)
                {
                    String nr = (Math.Round((A_rebar + extraRebar) / 10) * 10).ToString("F0");

                    rebarRequirement = nr  +  new string(Session.Space, 1);
                }
                else
                {
                    rebarRequirement = (Math.Round((A_rebar + extraRebar) / 10) * 10).ToString("F0")+new string(Session.Space,1);
                }
            }
            else
            {
                if (SharedData.texFormat)
                {
                    rebarRequirement = new string(Session.Dash,1);

                }
                else
                {
                    rebarRequirement = new string(Session.Dash, 1);
                }
            }
            return rebarRequirement;
        }

        private void checkULSLimited()
        {
            if (Mr < Mu | x / d > 0.5)
            {
                ULS_limited = true;
                overReinforced = true;
                if (verbose & SLS_limited)
                {
                    addMessage(String.Format(" ", "\n"));
                    addMessage("The design has become ULS controlled while adding rebar to control long term deflections\n");
                    
                }
                else if (verbose)
                {
                    addMessage(String.Format(" ", "\n"));
                    addMessage("The design is ULS controlled.\n");
                }
            }
        }

        private String recordAnalysisStatistics(String type)
        {
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case "Data":
                    {
                        
                        sb.Append(String.Format("Profile used in the beam = %s\n", p.name)).
                                Append(String.Format("Depth of the profile (dp) = %.0f [mm]\n", p.depth)).
                                Append(String.Format("Beam span (L) = %.2f [m]\n", span)).
                                Append(String.Format("Total depth of the beam (h) = %.0f [mm]\n", h)).
                                Append(String.Format("Beam spacing (b) = %.0f [mm]\n", b)).
                                Append("Analysis uses the BS8110 parabolic compression block\n").
                                Append(String.Format("Compression block width (k1) = %.3f [-]\n", k1)).
                                Append(String.Format("Compression block centroid (k2) = %.3f [-]\n", k2)).
                                Append(String.Format("Distance to the steel deck's centroid from bottom (y_deck) = %.2f [mm]\n", p.tensionSteelCentroid)).
                                Append(String.Format("Un-factored weight of concrete (wc) = %.2f [N/m]\n", beam.wc)).
                                Append(String.Format("Un-factored weight deck steel (wd) = %.3f [N/m]\n", beam.wd)).
                                Append(String.Format("Un-factored superimposed dead load (wd_super) = %.3f [N/m]\n", beam.wd_super)).
                                Append(String.Format("Total un-factored dead load (Gn) = %.3f [N/m]\n", beam.own_weight + beam.wd_super)).
                                Append(String.Format("Un-factored imposed load (live load) (LL) = %.3f [N/m]\n", beam.liveLoad));
                        break;                
                    }
                case "Forces":
                    {
                        sb.Append(String.Format("Distance to effective steel centroid from bottom (y) = %.2f [mm]\n", y)).
                                Append(String.Format("Depth of the compression block (x) = %.2f [mm]\n", x)).
                                Append(String.Format("Effective depth (d) = %.2f [mm]\n", d)).
                                Append(String.Format("x/d = %.3f\n", x / d)).
                                Append(String.Format("Lever arm (z) = %.2f [mm]\n", z)).
                                Append(String.Format("Concrete compression force (Fc) = %.2f [kN]\n", Fc / 1000)).
                                Append(String.Format("Steel tension force (Fs) = %.2f [kN]\n", Fs / 1000)).
                                Append(String.Format("Moment of resistance (Mr) = %.2f [kNm]\n", Mr / 1e6)).
                                Append(String.Format("Ultimate factored moment (Mu) = %.2f [kNm]\n", Mu / 1e6));
                        if (Fc < Fs)
                        {
                            sb.Append("The compression block depth limits the capacity of the beam:\n");
                            sb.Append(String.Format("x/d = %.3f\n", x / d));
                        }
                        else if (Mr < Mu)
                        {
                            sb.Append(String.Format("The beam is not suitable because Mu > Mr\n"));
                            if (Fc > Fs - 1 & Fc < Fs + 1)
                            {
                                sb.Append("Steel and concrete forces are balanced:\n");
                                sb.Append("The moment of resistance is determined from the balanced forces at maximum compression block depth\n");
                            }
                            else
                            {
                                sb.Append("Concrete and steel forces could not be balanced.\n");
                            }
                        }
                        break;
                    }

                case "Rebar":
                    {
                        sb.Append(String.Format("Deck steel area acting as rebar (A_deck) = %.2f [mm^2]\n", p.At)).
                                Append(String.Format("Extra rebar required for ULS (Ar_ULS) = %.2f [mm^2]\n", A_rebar)).
                                Append(String.Format("Extra rebar required for SLS control (Ar_SLS) = %.2f [mm^2]\n", extraRebar)).
                                Append(String.Format("Total effective tension steel area (As) = %.2f [mm^2]\n", As)).
                                Append(String.Format("Weighted average UN-FACTORED steel stress (f_avg_un_f) = %.2f [MPa]\n", fy)).
                                Append(String.Format("Weighted average FACTORED (gamma = 1.15) steel stress (f_avg) = %.2f [MPa]\n", fy * gamma_r));
                        break;
                    }
                case "Deflections":
                    {
                        sb.Append("Calculation of deflection limits according to SANS 0100-1:200 Table 15\n").
                                Append(String.Format("Beam span (L) = %.0f [mm]\n", span * 1000)).
                                Append(String.Format("Depth of the compression block (x) = %.2f [mm]\n", x)).
                                Append(String.Format("Effective depth (d) = %.2f [mm]\n", d)).
                                Append(String.Format("Steel service stress (fs) = %.2f [MPa]\n", fservice)).
                                Append(String.Format("M/(bd^2) = %.3f [-]\n", M_over_bd_squared)).
                                Append(String.Format("x/d = %.3f\n", x / d)).
                                Append(String.Format("Span to depth ratio (L/d) = %.2f [-]\n", span * 1000 / d)).
                                Append(String.Format("Basic (L/d) limit for nominally restrained simply supported floors  = %.2f [-]\n", SharedData.l_over_d_limit)).
                                Append(String.Format("Modified l/d ratio = %.2f [-]\n", SharedData.l_over_d_limit * mod_factor));
                        break;
                    }
            }
            return sb.ToString();
        }

    }
}
