using FEM2D.Elements;
using FEM2D.Elements.Triangle;

namespace FEM2D.Results.Membranes
{
    public class MembraneElementResult
    {
        public ITriangleElement Element { get; internal set; }
        public double SigmaX { get; internal set; }
        public double SigmaY { get; internal set; }
        public double TauXY { get; internal set; }
    }
}