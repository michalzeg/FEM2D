using MathNet.Numerics.LinearAlgebra;

namespace FEM2DDynamics.Solver
{
    public interface IDampingMatrixCalculator
    {
        Matrix<double> GetDampingMatrix(Matrix<double> stiffness, Matrix<double> mass);
    }
}