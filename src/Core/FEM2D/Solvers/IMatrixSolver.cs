using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Solvers
{
    public interface IMatrixSolver
    {
        Vector<double> Solve(Matrix<double> K, Vector<double> P);
    }
}