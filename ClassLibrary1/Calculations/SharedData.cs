using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassLibrary1.Calculations
{
    public class SharedData
    {
        public static double fcu = 30; //[MPa]
        public static double fy_deck = 230; // [MPa]
        public static double fy_rebar = 450; // [MPa]
        public static double steelMass = 7850; // [kg/m^3]
        public static double concreteMass = 2350; // [kg/m^3]
        public static double super_dead = 900; // [N/m^2]
        public static double gamma_r = 1.15; // The partial factr for reinforcing steel
        public static double gamma_c = 1.50; // The partial factor for concrete in flexural compression
        public static double phi_uls_dl = 1.2; // The partial load factor for dead load at Ultimate Limit State
        public static double phi_uls_ll = 1.6; // The partial load factor for live load at Ultimate Limit State
        public static double cover = 25; // [mm] Assumed rebar cover for all calculations
        public static double minConcreteDepth = 50; // [mm] Minimum depth of concrete above the deck profile
        public static double l_over_d_limit = 20; // [-]
        public static double max_bar_size = 20; // [mm]
        public static double Es = 2e5; // [MPa]
        public static double eps_c = 0.0035; // [m/m]
        public static Boolean addSteelForSLS = true;
        public static double extraRebarLimit = 1500;
        public static Boolean texFormat = true;

    }
}