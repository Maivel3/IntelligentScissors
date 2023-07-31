using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace IntelligentScissors
{
    public static class Drawing
    {
        public static void DrawingPath(Graphics gra, Pen p, Point[] point, float[] line)
        {
            p.DashPattern = line;
            gra.DrawCurve(p, point);

        }

            public static void DrawingPath(Graphics gra , Pen p , Point first , Point sec , float [] line)
        {
            Point[] points = new Point[2];
            points[0] = first;
            points[1] = sec;

            DrawingPath(gra, p, points, line);
        }
    }
}
