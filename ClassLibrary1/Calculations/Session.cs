﻿using ClassLibrary1.Calculations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ClassLibrary1.Calculations
{
    public class Session
    {
        public static List<Beam> beamList = new List<Beam>();
        public static List<double> spanList = new List<double>();
        public static List<string> dataLine = new List<string>();
        public static List<Profile> profileList = new List<Profile>();
        static double[] liveLoad;
        static double[] slabThickness;
        static double[] spanLengths;
        public const char Space = ' ';
        public const char Dash = '-';
        public static Boolean Bool50 = true;
        public static Boolean Bool115 = true;
        public static Boolean Bool200 = true;
        public static FileStream ostrm;
        public static StreamWriter writer;
        public static TextWriter oldOut = Console.Out;
        public static String path = "C:/Users/DenJormungandr/Desktop/VoidconOutput.txt";





public static void DoLogic(enum_Profiles inputProfile)
        {

            try
            {
                ostrm = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open VoidconOutput.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }

            Profile CurrentProfile = null;
            switch (inputProfile)
            {
                case enum_Profiles.vp50:
                    CurrentProfile = new Profile("VP50", 50, 115, 415, 625, 0.8, 292, 20.55, 6450);
                    break;
                case enum_Profiles.vp115:
                    CurrentProfile = new Profile("VP115", 115, 150, 600, 1250, 0.8, 320, 18.75, 19625);
                    break;
                case enum_Profiles.vp200:
                    CurrentProfile = new Profile("VP200", 200, 230, 760, 1250, 0.8, 384, 15.20, 52500);
                    break;
                default:
                    break;
            }

            profileList.Add(CurrentProfile);


            liveLoad = new double[] { 1500, 2000, 2500, 3000, 4000, 5000, 7500 };

            foreach (Profile profile in profileList){
                spanList.Clear();
                if (profile.name == "VP50")
                {
                    slabThickness = new double[] { 120, 135, 150, 160 };
                    spanLengths = Util.increment(2.5, 0.25, 9);
                }
                else if (profile.name == "VP115")
                {
                    slabThickness = new double[] { 170, 190, 200, 225,250 };
                    spanLengths = Util.increment(4.5, 0.25, 12);

                }
                else if (profile.name == "200")
                {
                    slabThickness = new double[] { 255, 275, 300, 320,340 };
                    spanLengths = Util.increment(4.0, 0.50, 11);

                }
                foreach(Double s in spanLengths)
                {
                    spanList.Add(s);
                }
                Console.SetOut(writer);
                doCalculations(profile,spanList,slabThickness);
 
            }
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
        }

        public static void doCalculations(Profile p, List<Double> spanList, double[] slabThickness)
        {

            Console.Write(" & & & & ");


            foreach (Double s in spanList)
            {
                if (spanList.IndexOf(s) == spanList.Count() - 1)
                {
                    Console.Write(s.ToString("F2") + new string(Space, 1));
                    
                }
                else
                {
                    Console.Write(s.ToString("F2") + new string(Space, 1));
                    
                }
            };
            Console.WriteLine("");
            Console.WriteLine("");


            foreach (Double t in slabThickness)
            {
                foreach (Double LL in  liveLoad)
                {
                    Boolean printIni = true;
                    foreach (Double L in spanLengths)
                    {
                        beamList.Add(new Beam(p, L, p.webWidth, t, LL, SharedData.super_dead));
                        Beam temp = beamList[(beamList.Count() - 1)];
                        if (printIni)
                        {
                                if (p.name == "VP50" && Bool50) 
                                {
                                    Console.WriteLine(p.name);
                                    Bool50 = false;
                                }
                                else if(p.name == "VP115" && Bool115)
                                {
                                    Console.WriteLine(p.name);
                                    Bool115 = false;
                                }
                                else if(p.name == "VP200" && Bool200)
                                {
                                    Console.WriteLine(p.name);
                                    Bool200 = false;
                                }
                            dataLine.Add((LL / 1000).ToString("F2"));
                            dataLine.Add(new string(Space,2));
                            dataLine.Add(( temp.own_weight / (temp.width / 1000) / 1000).ToString("F3"));
                            dataLine.Add(new string(Space, 2));
                            dataLine.Add(( temp.w / (temp.width / 1000) / 1000).ToString("F2"));
                            dataLine.Add(new string(Space, 2));
                            dataLine.Add((t).ToString("F0"));
                            dataLine.Add(new string(Space, 2));
                            printIni = false;
                        }

                        dataLine.Add(temp.c.printRebarRequirement());
                        
                    }

                    String temp2 = dataLine[(dataLine.Count() - 1)];                    
                    String newtemp = temp2.Replace("&", "\\\\"); //Geen Fokken Idee Wat Hierdie Doen Nie
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
                        
            foreach(var b in beamList)
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





    

