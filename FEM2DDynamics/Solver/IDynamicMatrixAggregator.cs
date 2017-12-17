using System.Collections.Generic;
using FEM2D.Elements;
using MathNet.Numerics.LinearAlgebra;
using FEM2D.Solvers;

namespace FEM2DDynamics.Solver
{
    public interface IDynamicMatrixAggregator : IMatrixAggregator
    {
        Matrix<double> AggregateMassMatrix(IEnumerable<IElement> elements, int dofNumber);
    }
}