using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;
using VoidConFormwork.Models;

namespace VoidConFormwork.Controllers
{
    public class VoidConCalculationsController : Controller
    {
        // GET: VoidConCalculations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _RebarRequirement(RebarInfo Info)
        {

            var items = new List<string>();

            if (!string.IsNullOrEmpty(Info.liveload))
                items = Info.liveload.Split('|').ToList();

            var liveLoads = new List<double>();

            foreach (var item in items)
            {
                if(item != "")
                    liveLoads.Add(double.Parse(item));
            }

            
            //{ 1500, 2000, 2500, 3000, 4000, 5000, 7500 }
            if (liveLoads.Count < 1)
                liveLoads = new List<double> { 1500, 2000, 2500, 3000, 4000, 5000, 7500 };

           

            enum_Profiles profile = new enum_Profiles();

            switch (Info.Profile)
            {
                case "VP50":
                    profile = enum_Profiles.vp50;
                    break;
                case "VP115":
                    profile = enum_Profiles.vp115;
                    break;
                case "VP200":
                    profile = enum_Profiles.vp200;
                    break;
                default:
                    break;
            }

            var Result = ClassLibrary1.Calculations.Session.DoLogic(profile, liveLoads.ToArray());
            
            return PartialView("~/Views/VoidConCalculations/_RebarRequirement.cshtml", Result);
        }
    }
}