using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntelligentScissors
{
    class AnchorPoint
    {
        public static List<Point> WireLiveForPixel;
        public static List<int> RedrawnPath;
        public static List<double> TimeTaken;
        public static List<Point> PathOfAnchor()
        {
            int fCount = 0;
            List<Point> oldPath = new List<Point>();
            for (int k = 0; k < WireLiveForPixel.Count; k++)
            {
                if (RedrawnPath[k] >= 10 && TimeTaken[k] > 1)
                {
                    fCount = k;
                }
            }
            for (int k = 0; k < fCount; k++)
            {
                oldPath.Add(WireLiveForPixel[k]);
            }

            return oldPath;
        }

        public static void ClearPath()
        {
            RedrawnPath = new List<int>();
            TimeTaken = new List<double>();
            WireLiveForPixel = new List<Point>();

        }

        public static void PathUpdate(List<Point> p, double T)
        {
            int countOfPath = 0,
                countOfWire = 0;

            int SizeOfPath = p.Count,
                SizeOfWireLive = WireLiveForPixel.Count;


            while (countOfPath < SizeOfPath && countOfWire < SizeOfWireLive)
            {
                if (p[countOfPath] == WireLiveForPixel[countOfWire])
                {
                    TimeTaken[countOfWire] += T;
                    RedrawnPath[countOfWire] += 1;
                }
                else
                {
                    WireLiveForPixel[countOfWire] = p[countOfPath];
                    TimeTaken[countOfWire] = 0;
                    RedrawnPath[countOfWire] = 0;
                }
                countOfWire++;
                countOfPath++;
            }

            while (countOfPath < SizeOfPath)
            {
                WireLiveForPixel.Add(p[countOfPath]);
                RedrawnPath.Add(0);
                TimeTaken.Add(0);
                countOfPath++;
            }
            while (countOfWire < SizeOfWireLive)
            {
                WireLiveForPixel[countOfWire] = new Point(-1, -1);
                RedrawnPath[countOfWire] = 0;
                TimeTaken[countOfWire] = 0;
                countOfWire++;
            }
        }
    }
}
