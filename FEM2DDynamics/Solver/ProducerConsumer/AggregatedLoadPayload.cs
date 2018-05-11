using MathNet.Numerics.LinearAlgebra;

namespace FEM2DDynamics.Solver
{
    internal class AggregatedLoadPayload
    {
        public Vector<double> AggregatedLoad { get; set; }
        public double Time { get; set; }
    }
}