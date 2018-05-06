using MathNet.Numerics.LinearAlgebra;

namespace FEM2DDynamics.Matrix
{
    internal class MatrixData
    {
        public Matrix<double> MassMatrix { get; set; }
        public Matrix<double> StiffnessMatrix { get; set; }
        public Matrix<double> DampingMatrix { get; set; }
    }
}