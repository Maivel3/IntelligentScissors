using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace IntelligentScissors
{
    class Functions
    {
       
        public static List<T> Insert_Des_To_Source<T>(List<T> destination, List<T> source)
        {
            List<T> desTemporary = destination;
            if (destination == null || source == null)
            {
                throw new ArgumentNullException();
            }
            for (int k = 0; k < source.Count; k++)
            {
                desTemporary.Add(source[k]);
            }
            return desTemporary;
        }
        public static int Flatten(int X, int Y, int width)
        {
            return (X) + (Y * width);
        }
        public static Vector2D Unflatten(int Index, int width)
        {
            // y -> row ,  x -> column  
            return new Vector2D((int)Index % (int)width, (int)Index / width);
        }
        public static List<T> Insert_Des_To_Source<T>(List<T> destination, T[] source)
        {
            List<T> desTemporary = destination;
            if (destination == null || source == null)
            {
                return null;
                throw new ArgumentNullException();
            }
            for (int k = 0; k < source.Length; k++)
            {
                desTemporary.Add(source[k]);
            }
            return desTemporary;
        }

        public static double distanceOfTwoPoint(int p1, int p2, int w)
        {
            int x1, x2;
            int y1, y2;
            double dis;

            x1 = p1 % w;
            y1 = p1 / w;

            x2 = p2 % w;
            y2 = p2 / w;

            dis = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            return dis;

        }

        public static bool checkBoundary(int newCoordinates, Boundary bound, int w)
        {
            int x, y;
            bool checkX, checkY;
            x = newCoordinates % w;
            y = newCoordinates / w;

            if (x > bound.Xminimum && x < bound.Xmaximum)
            {
                checkX = true;
            }
            else
                checkX = false;

            if (y > bound.Yminimum && y < bound.Ymaximum)
            {
                checkY = true;
            }
            else
                checkY = false;

            return (checkY && checkX);
        }

        public static bool checkCoordinatesPixel(int x_new, int y_new, RGBPixel[,] img)
        {
            bool xCheck = false,
                yCheck = false;
            int hImg = ImageOperations.GetHeight(img);
            int wImg = ImageOperations.GetWidth(img);

            if (x_new < wImg && x_new >= 0)
                xCheck = true;
            if (y_new < hImg && y_new >= 0)
                yCheck = true;

            return (yCheck && xCheck);
        }

        public static RGBPixel[,] CropedImg(RGBPixel[,] img, Boundary bound)
        {
            int newH, newW;
            RGBPixel[,] newImg;

            newH =(int)( bound.Ymaximum - bound.Yminimum);
            newW =(int)( bound.Xmaximum - bound.Xminimum);
            newImg = new RGBPixel[newH + 1, newW + 1];

            for (int i = 0; i <= newH; i++)
            {
                for (int j = 0; j <= newW; j++)
                {
                    newImg[i, j] = img[bound.Yminimum + i, bound.Xminimum + j];
                }
            }

            return newImg;

        }

        public static RGBPixel[,] CopyImg(RGBPixel[,] img)
        {
            int CopyH, CopyW;
            RGBPixel[,] CopyImg;

            CopyH = ImageOperations.GetHeight(img);
            CopyW = ImageOperations.GetWidth(img);
            CopyImg = new RGBPixel[CopyH , CopyW ];

            for (int i = 0; i < CopyH; i++)
            {
                for (int j = 0; j < CopyW; j++)
                {
                    CopyImg[i, j] = img[i, j];
                }
            }

            return CopyImg;

        }


        public static Point Cr_P (Point point , Boundary B)
        {
            point.X = point.X + (int) B.Xminimum ;
            point.Y = point.Y + (int)B.Yminimum;

            return point;
        }
        public static List<Point> Cr_P(List<Point> Path, Boundary B)
        {
            for (int i = 0; i < Path.Count; i++)
                Path[i] = Cr_P(Path[i], B);
            return Path;
        }
        public static int Cr_P(int N, Boundary B, int WidthM, int WS)
        {
            int x, y , Node;
            x = (N % WidthM )- (int) B.Xminimum ;
            y = (N / WidthM ) - (int) B.Yminimum;

            Node = x + (y * WS);
               
            return Node;
        }
    }
}
