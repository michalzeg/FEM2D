using FEM2D.Elements;

namespace FEM2D.Results
{
    public class MembraneElementResult
    {
        public ITriangleElement Element { get; internal set; }
        public double SigmaX { get; internal set; }
        public double SigmaY { get; internal set; }
        public double TauXY { get; internal set; }
    }
}