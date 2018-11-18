using FEM2D.Loads;
using System.Collections.Generic;

namespace FEM2DDynamics.Solver.ProducerConsumer
{
    internal class NodalForcePayload
    {
        public IEnumerable<NodalLoad> NodalLoads { get; set; }
        public double Time { get; set; }
    }
}