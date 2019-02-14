using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace FEM2D.Solvers
{
    public interface IMatrixReducer
    {
        void Initialize(NodeFactory nodeFactory);

        void Initialize(IEnumerable<INode> nodes, int dofCount);

        Matrix<double> ReduceMatrix(Matrix<double> matrix);

        Vector<double> ReduceVector(Vector<double> vector);
    }
}