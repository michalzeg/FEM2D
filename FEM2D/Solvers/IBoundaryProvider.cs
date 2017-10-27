using System.Collections.Generic;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Solvers
{
    public interface IBoundaryProvider
    {
        Vector<double> BundaryVector { get; }

        void CreateVector(IEnumerable<Node> nodes, int dofNumber);
        void Reduce(Matrix<double> matrix, Vector<double> vector);
    }
}