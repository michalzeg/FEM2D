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
        private IList<INodalLoad> nodalLoads;

        public LoadFactory()
        {
            this.nodalLoads = new List<INodalLoad>();
        }

        public void AddNodalLoad(Node node, double valueX, double valueY)
        {
            var load = new NodalLoad(node, valueX, valueY);

            this.nodalLoads.Add(load);
        }
        public void AddBeamPointLoad(IBeamElement beamElement, double valueY, double relativePosition)
        {
            var load = new BeamPointLoad(beamElement, valueY, relativePosition);
            this.nodalLoads.Add(load);
        }

        public IEnumerable<NodalLoad> GetNodalLoads()
        {
            return this.nodalLoads.SelectMany(e => e.NodalLoads);
        }
    }
}
