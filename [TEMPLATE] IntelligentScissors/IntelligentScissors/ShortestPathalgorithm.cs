using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace IntelligentScissors
{
    class ShortestPathalgorithm
    {

        public static List<int> Dijkstra(int Source, int destination, RGBPixel[,] ImageMatrix)
        {
            int matrix_Width = ImageOperations.GetWidth(ImageMatrix);
            int matrix_HIght = ImageOperations.GetHeight(ImageMatrix);
            int matrix_nodes = matrix_HIght * matrix_Width;
            const double infi = 1000000000000000000;
            List<double> path = Enumerable.Repeat(infi, matrix_nodes).ToList();
            List<int> sp_nodes;
            sp_nodes = Enumerable.Repeat(-1, matrix_nodes).ToList();
            EdgesOfGraph edge = new EdgesOfGraph(-1, Source, 0);
            periortyqueue p = new periortyqueue();
            p.Push(edge);
            while (p.IsEmpty() == false) 
            {
               
                EdgesOfGraph current_Edge = p.Pop();
                if (path[current_Edge.toPixel] <= current_Edge.weightOfEdge)
                {
                    continue;
                }
                path[current_Edge.toPixel] = current_Edge.weightOfEdge;

                sp_nodes[current_Edge.toPixel] = current_Edge.fromPixel;
                if (current_Edge.toPixel == destination)
                {
                    break;
                }
                List<EdgesOfGraph> neighbours = WeightedGraph.NeighboursOfPixels(current_Edge.toPixel, ImageMatrix);

                int x = neighbours.Count;
                int i = 0;
                while (i<neighbours.Count-1)
                {

                    EdgesOfGraph n = neighbours[i];
                    if ((path[n.fromPixel] + n.weightOfEdge) < path[n.toPixel])
                    {
                        n.weightOfEdge = path[n.fromPixel] + n.weightOfEdge;
                        p.Push(n);
                    }
                    i++;
                    x--;
                }

            }
            return sp_nodes;
        }

        public static List<int> Dijkstra(int Source, RGBPixel[,] ImageMatrix)
        {
            int matrix_Width = ImageOperations.GetWidth(ImageMatrix);
            int matrix_HIght = ImageOperations.GetHeight(ImageMatrix);
            int matrix_area = matrix_HIght * matrix_Width;
            const double infi = 1000000000000000000;
            List<double> path = Enumerable.Repeat(infi, matrix_area).ToList();
            List<int> sp_nodes;
            sp_nodes = Enumerable.Repeat(-1, matrix_area).ToList();
            EdgesOfGraph edge = new EdgesOfGraph(-1, Source, 0);
            periortyqueue p = new periortyqueue();
            p.Push(edge);
            while (p.IsEmpty() == false) 
            {
               
                EdgesOfGraph current_Edge = p.Pop();
                if (path[current_Edge.toPixel] <= current_Edge.weightOfEdge)
                {
                    continue;
                }
                path[current_Edge.toPixel] = current_Edge.weightOfEdge;

                sp_nodes[current_Edge.toPixel] = current_Edge.fromPixel;

                List<EdgesOfGraph> neighbours = WeightedGraph.NeighboursOfPixels(current_Edge.toPixel, ImageMatrix);

                int x = neighbours.Count;
                int i = 0;
                while (i<neighbours.Count)
                {

                    EdgesOfGraph n = neighbours[i];
                    if ((path[n.fromPixel] + n.weightOfEdge) < path[n.toPixel])
                    {
                        n.weightOfEdge = path[n.fromPixel] + n.weightOfEdge;
                        p.Push(n);
                    }
                    i++;
                    x--;
                }

            } 
            return sp_nodes;
        }
        public static List<Point> Backtracking(List<int> nodes, int Dest, int matrix_width)
        {
            List<int> path = new List<int>();
            path.Add(Dest);
            int CurrentNode = nodes[Dest];
            while (CurrentNode != -1)
            {



                path.Add(CurrentNode);
                CurrentNode = nodes[CurrentNode];
            }
            int x = path.Count-1;
            List<Point> dsp = new List<Point>();
            while (path.Count != 0) 
            {

                Point point = new Point((int)path[x] % matrix_width, (int)path[x] / matrix_width);
                path.RemoveAt(x);
                x--;
                dsp.Add(point);

            }
            
            return dsp;
        }


        public static List<Point> GenerateShortestPath(int Source, int Dest, RGBPixel[,] ImageMatrix)
        {

            List<int> Previous_list = Dijkstra(Source, Dest, ImageMatrix);
            return Backtracking(Previous_list, Dest, ImageOperations.GetWidth(ImageMatrix));
        }


        public static Boundary Square_Boundary(int Src, int Width, int Height)
        {
          long a = Src % Width;
          long c = Src / Width;

            Vector2D source = new Vector2D(a,c);
            Boundary b = new Boundary();
            int max_dist = 200;
            if (source.X > max_dist)
            {
                b.Xminimum =(int) source.X - max_dist;
            }
            else if (source.X <= max_dist)
            {
                b.Xminimum = 0;
            }
            if (Width - source.X > max_dist)
            {
                b.Xmaximum = (int)source.X + max_dist;
            }
            else if (Width - source.X <= max_dist)
            {
                b.Xmaximum = Width;
            }
            if (source.Y > max_dist)
            {
                b.Yminimum = (int)source.Y - max_dist;
            }
            else if (source.Y <= max_dist)
            {
                b.Yminimum = 0;
            }
            if (Height - source.Y > max_dist)
            {
                b.Ymaximum = (int)source.Y + max_dist;
            }
            else
            {
                b.Ymaximum = Height;
            }
            return b;
        }

        
    }
}
