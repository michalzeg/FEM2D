using System.Collections.Generic;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Solvers
{
    public interface IMatrixReducer
    {
        void Initialize(IEnumerable<Node> nodes, int dofCount);
        Matrix<double> ReduceMatrix(Matrix<double> matrix);
        Vector<double> ReduceVector(Vector<double> vector);
    }
}