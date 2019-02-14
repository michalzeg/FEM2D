using FEM2D.Elements.Beam;
using FEM2D.Loads.Beams;

namespace FEM2D.Loads.Beams
{
    public class BeamUniformLoad : BeamLoad, INodalLoad
    {
        public double ValueY { get; private set; }

        public BeamUniformLoad(IBeamElement beamElement, double valueY)
            : base(beamElement)
        {
            this.ValueY = valueY;

            var node1Load = this.GenerateNode1Load(0, 1);
            var node2Load = this.GenerateNode1Load(1, -1);

            this.NodalLoads = this.TransformToGlobal(new[] { node1Load, node2Load });
        }

        private NodalLoad GenerateNode1Load(int nodeIndex, int multiplier)
        {
            var valueY = this.ValueY * this.BeamElement.Length / 2;
            var valueM = this.ValueY * multiplier * this.BeamElement.Length * this.BeamElement.Length / 12;
            var node = this.BeamElement.Nodes[nodeIndex];
            var result = new NodalLoad(node, 0, valueY, valueM);

            return result;
        }
    }
}