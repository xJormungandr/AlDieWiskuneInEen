using ClassLibrary1.Calculations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ClassLibrary1.Calculations
{
    class Session
    {
        public static List<Beam> beamList = new List<Beam>();
        public static List<double> spanList = new List<double>();
        public static List<string> dataLine = new List<string>();
        public static List<Profile> profileList = new List<Profile>();
        static double[] liveLoad;
        static double[] slabThickness;
        static double[] spanLengths;
        
        public static void main(String [] args)
        {

            Profile vp50 = new Profile("VP50", 50, 115, 415, 625, 0.8, 292, 20.55, 6450);
            Profile vp115 = new Profile("VP115", 115, 150, 600, 1250, 0.8, 320, 18.75, 19625);
            Profile vp200 = new Profile("VP200", 200, 230, 760, 1250, 0.8, 384, 15.20, 52500);

            profileList.Add(vp50);
            profileList.Add(vp115);
            profileList.Add(vp200);

            liveLoad = new double[] { 1500, 2000, 2500, 3000, 4000, 5000, 7500 };

            foreach (Profile profile in profileList){
                spanList.Clear();
                if (profile.name == "VP50")
                {
                    slabThickness = new double[] { 120, 135, 150, 160 };
                    spanLengths = Util.increment(4.0, 0.50, 11);
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

                doCalculations(profile,spanList,slabThickness);
            }
            


        }

        public static void doCalculations(Profile p, List<Double> spanList, double[] slabThickness)
        {

            Console.WriteLine("%33s", " & & & &");
            foreach (Double s in spanList)
            {
                if (spanList.IndexOf(s) == spanList.Count() - 1)
                {
                    Console.WriteLine("%6.2f \\\\", s);
                }
                else
                {
                    Console.WriteLine("%6.2f &", s);

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
                            dataLine.Add(String.Format("%6.2f &", LL / 1000));
                            dataLine.Add(String.Format("%7.3f &", temp.own_weight / (temp.width / 1000) / 1000));
                            dataLine.Add(String.Format("%7.2f &", temp.w / (temp.width / 1000) / 1000));
                            dataLine.Add(String.Format("%5.0f & ", t));
                            //printIni = false;
                        }

                        dataLine.Add(temp.c.printRebarRequirement());
                    }

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
                Console.Write("\\midrule");
            }

            Console.WriteLine();


            foreach(var b in beamList)
            {
                String path = "./Output/" + b.name + "_" + b.span + "m" + "_"
                + String.Format("%.2f", b.liveLoad / (b.width / 1000) / 1000)
                + "_" + String.Format("%.3f", b.w / (b.width / 1000) / 1000)
                + ".txt";
                String out123 = b.printFullRecord();
                //Util.writeToFile(out123, path);
            }         
        }
    }
}





    

