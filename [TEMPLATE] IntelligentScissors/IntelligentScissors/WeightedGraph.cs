using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    class WeightedGraph
    {
        public static List<EdgesOfGraph> NeighboursOfPixels(int Node_Index, RGBPixel[,] ImageMatrix)
        {

            List<EdgesOfGraph> neighbours = new List<EdgesOfGraph>();
            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);
            //get x , y indices of the node
            var unflat = Functions.Unflatten(Node_Index, Width);
            int X = (int)unflat.X, Y = (int)unflat.Y;
            // calculate the gradient with right and bottom neighbour
            var Gradient = ImageOperations.CalculatePixelEnergies(X, Y, ImageMatrix);
            if (X < Width - 1) // have a right neighbour ?  
            {
                //add to neighbours list with cost 1/G
                if (Gradient.X == 0)
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X + 1, Y, Width), 10000000000000000));
                else
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X + 1, Y, Width), 1 / (Gradient.X)));
            }
            if (Y < Height - 1) // have a Bottom neighbour ?
            {
                //add to neighbours list with cost 1/G
                if (Gradient.Y == 0)
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X, Y + 1, Width), 10000000000000000));
                else
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X, Y + 1, Width), 1 / (Gradient.Y)));
            }
            if (Y > 0) // have a Top neighbour ?
            {
                // calculate the gradient with top neighbour
                Gradient = ImageOperations.CalculatePixelEnergies(X, Y - 1, ImageMatrix);
                //add to neighbours list with cost 1/G
                if (Gradient.Y == 0)
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X, Y - 1, Width), 10000000000000000));
                else
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X, Y - 1, Width), 1 / (Gradient.Y)));
            }
            if (X > 0) // have a Left neighbour ?
            {
                // calculate the gradient with left neighbour
                Gradient = ImageOperations.CalculatePixelEnergies(X - 1, Y, ImageMatrix);

                //add to neighbours list with cost 1/G 
                if (Gradient.X == 0)
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X - 1, Y, Width), 10000000000000000));
                else
                    neighbours.Add(new EdgesOfGraph(Node_Index, Functions.Flatten(X - 1, Y, Width), 1 / (Gradient.X)));
            }
            return neighbours; // return neighbours
        }
    }
}
