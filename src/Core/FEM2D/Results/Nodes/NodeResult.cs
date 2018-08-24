using FEM2D.Nodes;

namespace FEM2D.Results
{
    public class NodeResult
    {
        public Node Node { get; internal set; }
        public double UX { get; internal set; }
        public double UY { get; internal set; }
        public double Rz { get; internal set; }
    }
}