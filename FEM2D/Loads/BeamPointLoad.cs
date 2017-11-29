using CuttingEdge.Conditions;
using FEM2D.Elements.Beam;
using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Loads
{
    public class BeamPointLoad:INodalLoad
    {
        public NodalLoad[] NodalLoads { get; }
        private readonly IBeamElement beamElement;
        private readonly double valueY;
        private readonly double relativePosition;

        public BeamPointLoad(IBeamElement beamElement, double valueY, double relativePosition)
        {
            Condition.Requires(relativePosition).IsGreaterOrEqual(0).IsLessOrEqual(1);

            this.beamElement = beamElement;
            this.valueY = valueY;
            this.relativePosition = relativePosition;


            var node1Load = this.GenerateNodalLoad(0, 1 - relativePosition);
            var node2Load = this.GenerateNodalLoad(1, relativePosition);

            this.NodalLoads = new[] { node1Load, node2Load };
        }

        private NodalLoad GenerateNodalLoad(int nodeIndex,double relativePosition)
        {
            var loadValue = this.valueY * relativePosition;
            var node = this.beamElement.Nodes[nodeIndex];
            var result = new NodalLoad(node, 0, loadValue);
            
            return result;
        }



    }
}
