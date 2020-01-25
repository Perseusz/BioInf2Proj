using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BioInf2
{
    public class Node
    {
        private string value;
        private bool isError;
        private int id;
        private Dictionary<Node, int> neighbourConnections;
        public Node(string value)
        {
            this.isError = false;
            this.neighbourConnections = new Dictionary<Node, int>();
            this.value = value;
        }

        public void SetError(bool error)
        {
            this.isError = error;
        }

        public bool GetError()
        {
            return this.isError;
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public int GetId()
        {
            return this.id;
        }

        public void AddNeighbour(Node neighbour)
        {
            int coverage = this.GetCoverage(neighbour.GetValue());
            this.neighbourConnections.Add(neighbour, coverage);
        }

        public int GetCoverage(string neighbourValue)
        {
            int coverage = 0;
            //for (int i = 1; i < value.Length; i++)
            //{

                if (this.value.Substring(1).Equals(neighbourValue.Substring(0, neighbourValue.Length - 1)))
                {
                    coverage = neighbourValue.Length - 1;
                    return coverage;
                }


            //}
            return coverage;
        }

        public Dictionary<Node, int> GetNeighbours()
        {
            return this.neighbourConnections;
        }

        public int GetNeighbourConnection(Node index)
        {
            return this.neighbourConnections[index];
        }

        public string GetValue()
        {
            return this.value;
        }

        public void SortNeighbours()
        {
            Dictionary<Node, int> sortedNeighbours = new Dictionary<Node, int>();
            foreach (KeyValuePair<Node, int> pair in this.neighbourConnections.OrderByDescending(key => key.Value))
            {
                sortedNeighbours.Add(pair.Key, pair.Value);
            }

            this.neighbourConnections.Clear();
            this.neighbourConnections = sortedNeighbours;
        }

        public void PrintNeighbours()
        {
            this.SortNeighbours();
            foreach (KeyValuePair<Node, int> n in neighbourConnections)
            {
                Console.Write("Neighbour = {0}, Coverage = {1}", n.Key.GetValue(), n.Value);
                Console.Write("\n");
            }
        }
    }
}
