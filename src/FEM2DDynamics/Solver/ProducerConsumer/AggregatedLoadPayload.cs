using MathNet.Numerics.LinearAlgebra;

namespace FEM2DDynamics.Solver.ProducerConsumer
{
    internal class AggregatedLoadPayload
    {
        public Vector<double> AggregatedLoad { get; set; }
        public double Time { get; set; }
    }
}