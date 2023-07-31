using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
   public class EdgesOfGraph
    {
        public int fromPixel;
        public int toPixel;
        public double weightOfEdge;
        public EdgesOfGraph(int fromPixel , int toPixel ,double weightOfEdge)
        {
            this.fromPixel = fromPixel;
            this.toPixel = toPixel;
            this.weightOfEdge = weightOfEdge;
        }
    }
}
