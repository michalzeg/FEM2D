using CuttingEdge.Conditions;
using FEM2D.Elements.Beam;
using FEM2D.Nodes;
using FEM2D.ShapeFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Loads
{
    public class BeamPointLoad:INodalLoad, IBeamLoad
    {
        public NodalLoad[] NodalLoads { get; }
        public IBeamElement BeamElement { get; private set; }
        public double ValueY { get; private set; }
        public double RelativePosition { get; private set; }

        internal double[] EquivalentNodalForces { get; private set; }

        public BeamPointLoad(IBeamElement beamElement, double valueY, double relativePosition)
        {
            Condition.Requires(relativePosition).IsGreaterOrEqual(0).IsLessOrEqual(1);

            this.BeamElement = beamElement;
            this.ValueY = valueY;
            this.RelativePosition = relativePosition;


            var node1Load = this.GenerateNode1Load(0, relativePosition, BeamShapeFunctions.N2, BeamShapeFunctions.N3);
            var node2Load = this.GenerateNode1Load(1, relativePosition, BeamShapeFunctions.N5, BeamShapeFunctions.N6);

            this.NodalLoads = new[] { node1Load, node2Load };
        }

        public double[] GetEquivalenNodalForces()
        {
            var result = new[]
                        {
                this.NodalLoads[0].ValueX,
                this.NodalLoads[0].ValueY,
                this.NodalLoads[0].ValueM,
                this.NodalLoads[1].ValueX,
                this.NodalLoads[1].ValueY,
                this.NodalLoads[1].ValueM,
            };
            return result;
        }

        private NodalLoad GenerateNode1Load(int nodeIndex,double relativePosition, Func<double,double,double> shapeFunctionY,Func<double,double,double> shapeFunctionM)
        {
            var position = relativePosition * this.BeamElement.Length;

            var valueY = this.ValueY * shapeFunctionY(position, this.BeamElement.Length);
            var valueM = this.ValueY * shapeFunctionM(position, this.BeamElement.Length);
            var node = this.BeamElement.Nodes[nodeIndex];
            var result = new NodalLoad(node, 0, valueY,valueM);
            
            return result;
        }

        
    }
}
