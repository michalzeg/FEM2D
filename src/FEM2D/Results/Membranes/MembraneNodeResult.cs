using FEM2D.Results.Nodes;

namespace FEM2D.Results.Membranes
{
    public class MembraneNodeResult : NodeResult
    {
        public double AverageSigmaXX { get; internal set; }
        public double AverageSigmaYY { get; internal set; }
        public double AverageTauXY { get; internal set; }
    }
}