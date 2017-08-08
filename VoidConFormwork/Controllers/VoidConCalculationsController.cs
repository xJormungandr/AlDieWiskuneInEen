using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;

namespace VoidConFormwork.Controllers
{
    public class VoidConCalculationsController : Controller
    {
        // GET: VoidConCalculations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _RebarRequirement(string Profile, double[] liveload = null  )
        {
            //{ 1500, 2000, 2500, 3000, 4000, 5000, 7500 }
            if (liveload == null)
                liveload = new double[] { 1500, 2000, 2500, 3000, 4000, 5000, 7500 };

            enum_Profiles profile = new enum_Profiles();

            switch (Profile)
            {
                case "VP50":
                    profile = enum_Profiles.vp50;
                    break;
                case "VP150":
                    profile = enum_Profiles.vp115;
                    break;
                case "VP200":
                    profile = enum_Profiles.vp200;
                    break;
                default:
                    break;
            }

            var Result = ClassLibrary1.Calculations.Session.DoLogic(profile, liveload);

            return PartialView("~/Views/VoidConCalculations/_RebarRequirement.cshtml", Result);
        }
    }
}