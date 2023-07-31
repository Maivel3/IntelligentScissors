using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntelligentScissors
{
    public struct Boundary
    {
        public int Xminimum, Xmaximum, Yminimum, Ymaximum;
    }
    public static class Flood
    {
        private static RGBPixel[,] img;

        public static RGBPixel[,] fill(List<Point> selected_points, RGBPixel[,] ImageMatrix)
        {
            Boundary bondry = GET_Boundary(selected_points); 
            img = Functions.CropedImg(ImageMatrix, bondry);
            for (int i = 0; i < selected_points.Count; i++)
            {
                int Xtmp = selected_points[i].X - bondry.Xminimum;
                int Ytmp = selected_points[i].Y - bondry.Yminimum;
                img[Ytmp, Xtmp].block = true;
            }
           
            int Width = ImageOperations.GetWidth(img) - 1;
            int Hight = ImageOperations.GetHeight(img) - 1;
            for (int i = 0; i <= Width; i++)
            {
                if (!img[0, i].block)
                {
                    DFS(new Vector2D(i, 0));
                }
            }

            for (int i = 0; i <= Width; i++)
            {
                if (!img[Hight, i].block)
                {
                    DFS(new Vector2D(i, Hight)); 
                }
            }

            for (int i = 0; i <= Hight; i++)
            {
                if (!img[i, 0].block)
                { 
                    DFS(new Vector2D(0, i)); 
                }
            }
            for (int i = 0; i <= Hight; i++)
            {
                if (!img[i, Width].block)
                {
                    DFS(new Vector2D(Width, i));
                }
            }
            return img;
        }

        private static void DFS(Vector2D node)
        {
            List<Vector2D> lis = new List<Vector2D>();
            lis.Add(node);
            while (lis.Count > 0)
            {
                int x = lis.Count;
                Vector2D current_node = lis[x-1];
                lis.RemoveAt(x - 1);
                if (Functions.checkCoordinatesPixel((int)current_node.X, (int)current_node.Y, img))
                {
                    if (!img[(int)current_node.Y, (int)current_node.X].block)
                    {
                        img[(int)current_node.Y, (int)current_node.X].block = true;
                        
                        img[(int)current_node.Y, (int)current_node.X].blue = 255; 
                        img[(int)current_node.Y, (int)current_node.X].green = 255;
                        img[(int)current_node.Y, (int)current_node.X].red = 255;

                        lis.Add(new Vector2D(current_node.X, current_node.Y + 1));
                        lis.Add(new Vector2D(current_node.X, current_node.Y - 1));
                        lis.Add(new Vector2D(current_node.X + 1, current_node.Y));
                        lis.Add(new Vector2D(current_node.X - 1, current_node.Y));
                    }
                }
            }
        }
        private static Boundary GET_Boundary(List<Point> selected_points)  
        {
            Boundary b;
           
            b.Xminimum = b.Yminimum = 1000000000;
            b.Xmaximum = b.Ymaximum = -1000000000;
            for (int i = 0; i < selected_points.Count; i++)
            {
                int x = selected_points[i].X;
                int y = selected_points[i].Y;
                if (x > b.Xmaximum)
                {
                    b.Xmaximum = x;
                }
                if (x < b.Xminimum)
                {
                    b.Xminimum = x; 
                }
                if (y > b.Ymaximum)
                {
                    b.Ymaximum = y;
                }
                if (y < b.Yminimum)
                { 
                    b.Yminimum = y;
                }
            }
            return b;
        }
    }
}