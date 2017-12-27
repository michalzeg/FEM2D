using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Matrix
{
    internal class MatrixData
    {
        public Matrix<double> MassMatrix { get; set; }
        public Matrix<double> StiffnessMatrix { get; set; }
        public Matrix<double> DampingMatrix { get; set; }

    }
}
