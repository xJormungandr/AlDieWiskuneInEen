﻿using ClassLibrary1.Calculations;
using ClassLibrary1.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ClassLibrary1.Calculations
{
    public class Session
    {
        public static List<Beam> beamList = new List<Beam>();
        //public static List<double> spanList = new List<double>();
        public static List<string> dataLine = new List<string>();
        public static List<Profile> profileList = new List<Profile>();
        static double[] liveLoad;
        static double[] slabThickness;
        //static double[] spanLengths;
        public const char Space = ' ';
        public const char Dash = '-';
        public static Boolean Bool50 = true;
        public static Boolean Bool115 = true;
        public static Boolean Bool200 = true;
        

        public static Results DoLogic(enum_Profiles inputProfile, double [] _liveLoad)
        {

            liveLoad = _liveLoad;

            var ReturnResults = new Results();
            ReturnResults.profileLengths = new List<double>();
            

            Profile CurrentProfile = null;
            switch (inputProfile)
            {
                case enum_Profiles.vp50:
                    CurrentProfile = new Profile("VP50", 50, 115, 415, 625, 0.8, 292, 20.55, 6450);
                    ReturnResults.profileName = "VP50";
                    break;
                case enum_Profiles.vp115:
                    CurrentProfile = new Profile("VP115", 115, 150, 600, 1250, 0.8, 320, 18.75, 19625);
                    ReturnResults.profileName = "VP115";
                    break;
                case enum_Profiles.vp200:
                    CurrentProfile = new Profile("VP200", 200, 230, 760, 1250, 0.8, 384, 15.20, 52500);
                    ReturnResults.profileName = "VP200";
                    break;
                default:
                    break;
            }

            profileList.Add(CurrentProfile);


            //liveLoad = new double[] { 1500, 2000, 2500, 3000, 4000, 5000, 7500 };

            foreach (Profile profile in profileList)
            {
                //spanList.Clear();
                if (profile.name == "VP50")
                {
                    slabThickness = new double[] { 120, 135, 150, 160 };
                    ReturnResults.profileLengths = Util.increment(2.5, 0.25, 9);
                }
                else if (profile.name == "VP115")
                {
                    slabThickness = new double[] { 170, 190, 200, 225, 250 };
                    ReturnResults.profileLengths = Util.increment(4.5, 0.25, 12);

                }
                else if (profile.name == "VP200")
                {
                    slabThickness = new double[] { 255, 275, 300, 320, 340 };
                    ReturnResults.profileLengths = Util.increment(4.0, 0.50, 11);

                }


                doCalculations(profile, ReturnResults.profileLengths, slabThickness, ref ReturnResults);

            }

            return ReturnResults;
        }

        public static void doCalculations(Profile p, List<Double> spanList, double[] slabThickness, ref Results results)
        {
            results.values = new List<ResultsList>();




            foreach (Double t in slabThickness)
            {
                foreach (Double LL in liveLoad)
                {
                    Boolean printIni = true;

                    var _ResultList = new ResultsList();
                    _ResultList.extraRebar = new List<string>();
                    _ResultList.fireRating = new List<string>();

                    foreach (Double L in spanList)
                    {

                        beamList.Add(new Beam(p, L, p.webWidth, t, LL, SharedData.super_dead));
                        Beam temp = beamList[(beamList.Count() - 1)];
                        if (printIni)
                        {
                            if (p.name == "VP50" && Bool50)
                            {
                                //Console.WriteLine(p.name);
                                Bool50 = false;
                            }
                            else if (p.name == "VP115" && Bool115)
                            {
                                //Console.WriteLine(p.name);
                                Bool115 = false;
                            }
                            else if (p.name == "VP200" && Bool200)
                            {
                                //Console.WriteLine(p.name);
                                Bool200 = false;
                            }
                            dataLine.Add((LL / 1000).ToString("F2"));
                            _ResultList.liveLoad = (LL / 1000);
                            //dataLine.Add(new string(Space, 2));
                            dataLine.Add((temp.own_weight / (temp.width / 1000) / 1000).ToString("F3"));
                            _ResultList.deadLoad = (temp.own_weight / (temp.width / 1000) / 1000);
                           // dataLine.Add(new string(Space, 2));
                            dataLine.Add((temp.w / (temp.width / 1000) / 1000).ToString("F2"));
                            _ResultList.factoredLoad = (temp.w / (temp.width / 1000) / 1000);
                            //dataLine.Add(new string(Space, 2));
                            dataLine.Add((t).ToString("F0"));
                            _ResultList.slabThickness = t;
                            //dataLine.Add(new string(Space, 2));
                            printIni = false;
                        }


                        dataLine.Add(temp.c.printRebarRequirement());
                        _ResultList.extraRebar.Add(temp.c.printRebarRequirement());

                        double extraRebarULS;
                        double extraRebarFLS;
                        string extraRebarFLS_Dash = "-";

                        if (temp.c.printRebarRequirement() == "-")
                        {
                            extraRebarULS = 0.0;                            
                            extraRebarFLS = Convert.ToDouble(temp.c.FireRatingCalculator());
                            
                        }
                        else
                        {

                            extraRebarULS = Convert.ToDouble(temp.c.printRebarRequirement());
                            extraRebarFLS = Convert.ToDouble(temp.c.FireRatingCalculator());
                        }
                                    
                        
                        if(extraRebarFLS > extraRebarULS & temp.c.printRebarRequirement() != "-")
                        {
                            _ResultList.fireRating.Add(extraRebarFLS.ToString("f0"));
                        }
                        else
                        {
                            _ResultList.fireRating.Add(extraRebarFLS_Dash);
                        }

                    }

                    results.values.Add(_ResultList);
                    
                    String temp2 = dataLine[(dataLine.Count() - 1)];
                    String newtemp = temp2.Replace("&", "\\\\"); 
                    dataLine.Add(newtemp);
                    foreach (var line in dataLine)
                    {
                        Console.Write(line);
                    }
                    Console.WriteLine();
                    dataLine = new List<string>();
                    
                }

                

                if (t != slabThickness[slabThickness.Count() - 1])
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Next Slab Thickness for " + p.name);
                }
                else
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Next Voidcon Formwork");
                    Console.WriteLine(Environment.NewLine);
                }


            }

            Console.WriteLine();

            foreach (var b in beamList)
            {
                String path = "./Output/" + b.name + "_" + b.span + "m" + "_"
                + String.Format("F2", b.liveLoad / (b.width / 1000) / 1000)
                + "_" + String.Format("F3", b.w / (b.width / 1000) / 1000)
                + ".txt";
                String out123 = b.printFullRecord();
                //Util.writeToFile(out123, path);
            }

        }

    }
}







