using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    class periortyqueue
    {
        private List<EdgesOfGraph> PHeap = new List<EdgesOfGraph>(); //array of edges
     
        private void Swapedges(EdgesOfGraph x, EdgesOfGraph y)
        {
            EdgesOfGraph temp = x;
            EdgesOfGraph a = y;
            x = a;
            y = temp;

        }

        private int NodeDirec(int n, int node)
        {
            if (n == 1) //back to parent
            {
                node = (node - 1) / 2;
                return node;
            }
            else if (n == 2) //left
            {
                node = (node * 2) + 1;
                return node;
            }
            else //right
            {
                node = node * 2 + 2;
                return node;
            }

        }

       

        private void modifytree(int n, int Node)
        {
            if (n == 1)
            {
                if (PHeap[Node].weightOfEdge >= PHeap[NodeDirec(1, Node)].weightOfEdge || Node == 0)
                    return;

                EdgesOfGraph temp = PHeap[NodeDirec(1, Node)];
                PHeap[NodeDirec(1, Node)] = PHeap[Node];
                PHeap[Node] = temp;
                
                modifytree(1,(NodeDirec(1, Node)));
            }
            else
            {
                if ((NodeDirec(2, Node) < PHeap.Count && NodeDirec(3, Node) >= PHeap.Count && PHeap[NodeDirec(2, Node)].weightOfEdge >= PHeap[Node].weightOfEdge) ||
                NodeDirec(2, Node) >= PHeap.Count
                || (NodeDirec(2, Node) < PHeap.Count && NodeDirec(3, Node) < PHeap.Count && PHeap[NodeDirec(2, Node)].weightOfEdge >= PHeap[Node].weightOfEdge &&
                PHeap[NodeDirec(3, Node)].weightOfEdge >= PHeap[Node].weightOfEdge))
                    return;

                if (NodeDirec(3, Node) < PHeap.Count && PHeap[NodeDirec(3, Node)].weightOfEdge <= PHeap[NodeDirec(2, Node)].weightOfEdge)
                {
                    EdgesOfGraph temp = PHeap[NodeDirec(3, Node)];
                    PHeap[NodeDirec(3, Node)] = PHeap[Node];
                    PHeap[Node] = temp;
                    modifytree(2,(NodeDirec(3, Node)));
                }

                else
                {
                    EdgesOfGraph temp = PHeap[NodeDirec(2, Node)];
                    PHeap[NodeDirec(2, Node)] = PHeap[Node];
                    PHeap[Node] = temp;
                    modifytree(2,(NodeDirec(2, Node)));
                }
            }

        }
        public void Push(EdgesOfGraph Node)
        {
            PHeap.Add(Node);
            modifytree(1,(PHeap.Count - 1));
        }

        public EdgesOfGraph Pop()
        {
            EdgesOfGraph temp = PHeap[0];
            PHeap[0] = PHeap[PHeap.Count - 1];
            PHeap.RemoveAt(PHeap.Count - 1);
            modifytree(2,0);
            return temp;
        }

        public bool IsEmpty()
        {
            if (PHeap.Count == 0)
                return true;
            return false;
        }


    }
}
