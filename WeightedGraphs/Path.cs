using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedGraphs
{
    public class Path
    {
        private List<string> nodes = new();

        public void Add(string node) => nodes.Add(node);

        public override string ToString()
        {
            return nodes.ToString()!;
        }
    }
}
