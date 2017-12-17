using System.Collections.Generic;
using FEM2D.Elements;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Solvers
{
    public interface IMatrixAggregator
    {
        Matrix<double> AggregateStiffnessMatrix(IEnumerable<IElement> elements, int dofNumber);
    }
}