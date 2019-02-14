using FEM2D.Nodes;

namespace FEM2D.Results.Nodes
{
    public class NodeResult
    {
        public INode Node { get; internal set; }
        public double UX { get; internal set; }
        public double UY { get; internal set; }
        public double Rz { get; internal set; }
    }
}