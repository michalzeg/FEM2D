using CuttingEdge.Conditions;
using FEM2D.Elements.Beam;
using FEM2D.Loads.Beams;
using FEM2D.ShapeFunctions;
using System;

namespace FEM2D.Loads.Beams
{
    public class BeamPointLoad : BeamLoad, INodalLoad
    {
        public double ValueY { get; private set; }
        public double RelativePosition { get; private set; }

        public BeamPointLoad(IBeamElement beamElement, double valueY, double relativePosition)
            : base(beamElement)
        {
            Condition.Requires(relativePosition).IsGreaterOrEqual(0).IsLessOrEqual(1);

            this.ValueY = valueY;
            this.RelativePosition = relativePosition;

            var node1Load = this.GenerateNode1Load(0, relativePosition, BeamShapeFunctions.N2, BeamShapeFunctions.N3);
            var node2Load = this.GenerateNode1Load(1, relativePosition, BeamShapeFunctions.N5, BeamShapeFunctions.N6);

            this.NodalLoads = new[] { node1Load, node2Load };
        }

        private NodalLoad GenerateNode1Load(int nodeIndex, double relativePosition, Func<double, double, double> shapeFunctionY, Func<double, double, double> shapeFunctionM)
        {
            var position = relativePosition * this.BeamElement.Length;

            var valueY = this.ValueY * shapeFunctionY(position, this.BeamElement.Length);
            var valueM = this.ValueY * shapeFunctionM(position, this.BeamElement.Length);
            var node = this.BeamElement.Nodes[nodeIndex];
            var result = new NodalLoad(node, 0, valueY, valueM);

            return result;
        }

        public static BeamPointLoad FromGlobalPosition(IBeamElement beamElement, double valueY, double globalPosition)
        {
            var absolutePosition = globalPosition - beamElement.Nodes[0].Coordinates.X;
            var relativePosition = absolutePosition / beamElement.Length;

            var result = new BeamPointLoad(beamElement, valueY, relativePosition);
            return result;
        }
    }
}