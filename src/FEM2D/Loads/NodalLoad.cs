using FEM2D.Nodes;

namespace FEM2D.Loads
{
    public class NodalLoad : INodalLoad
    {
        public NodalLoad[] NodalLoads => new[] { this };

        public INode Node { get; }
        public double ValueX { get; }
        public double ValueY { get; }
        public double ValueM { get; }

        public NodalLoad(INode node, double valueX, double valueY, double valueM = 0)
        {
            this.Node = node;
            this.ValueX = valueX;
            this.ValueY = valueY;
            this.ValueM = valueM;
        }
    }
}