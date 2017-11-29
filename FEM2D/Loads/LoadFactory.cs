using FEM2D.Elements.Beam;
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

        public void AddNodalLoad(Node node, double valueX, double valueY)
        {
            var load = new NodalLoad(node, valueX, valueY);

            this.nodalLoads.Add(load);
        }
        public void AddBeamPointLoad(IBeamElement beamElement, double valueY)
        {

        }

        public IEnumerable<NodalLoad> GetNodalLoads()
        {
            return this.nodalLoads;
        }
    }
}
