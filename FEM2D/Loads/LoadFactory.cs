using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Loads
{
    public class LoadFactory
    {
        private IList<NodalLoad> nodalLoads;

        public LoadFactory()
        {
            this.nodalLoads = new List<NodalLoad>();
        }

        public void Add(Node node, double valueX, double valueY)
        {
            var load = new NodalLoad
            {
                Node = node,
                ValueX = valueX,
                ValueY = valueY
            };
            this.nodalLoads.Add(load);
        }
        

        public IEnumerable<NodalLoad> GetNodalLoads()
        {
            return this.nodalLoads;
        }
    }
}
