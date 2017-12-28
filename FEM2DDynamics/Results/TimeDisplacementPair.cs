using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results
{
    internal class TimeDisplacementPair
    {
        public double Time { get; set; }
        public Vector<double> Displacements { get; set; }
        public Vector<double> Velocities { get; set; }
        public Vector<double> Accelerations { get; set; }
    }
}
