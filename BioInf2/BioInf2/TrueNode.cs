using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInf2
{
    class TrueNode
    {
        private string value;
        private List<string> outNeighbours;
        private List<string> inNeighbours;
        private int visits;

        public TrueNode(string value)
        {
            this.value = value;
            this.outNeighbours = new List<string>();
            this.inNeighbours = new List<string>();
            this.visits = 0;
        }

        public string GetValue()
        {
            return this.value;
        }

        public void AddOutNeighbour(string subSeq)
        {
            this.outNeighbours.Add(subSeq);
        }

        public int CountOutNeighbours()
        {
            return this.outNeighbours.Count();
        }

        public bool IsInOutNeighbours(string subSeq)
        {
            return this.outNeighbours.Contains(subSeq);
        }

        public void AddInNeighbour(string subSeq)
        {
            this.inNeighbours.Add(subSeq);
        }

        public int CountInNeighbours()
        {
            return this.inNeighbours.Count();
        }

        public bool IsInInNeighbours(string subSeq)
        {
            return this.inNeighbours.Contains(subSeq);
        }

        public int InAndOutNeighboursValue()
        {
            return this.inNeighbours.Count() + this.outNeighbours.Count();
        }

        public void DeleteOneInNeighbour(string s)
        {
            this.inNeighbours.Remove(s);
        }

        public void DeleteOneOutNeighbour(string s)
        {
            this.outNeighbours.Remove(s);
        }

        public string GetOneFromInNeighbours(int i)
        {
            return this.inNeighbours[i];
        }

        public string GetOneFromOutNeighbours(int i)
        {
            return this.outNeighbours[i];
        }

        public int GetVisits()
        {
            return this.visits;
        }

        public void IncVisits()
        {
            this.visits++;
        }

    }
}
