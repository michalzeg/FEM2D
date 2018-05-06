using FEM2D.Elements;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace FEM2D.Solvers
{
    public interface IMatrixAggregator
    {
        Matrix<double> AggregateStiffnessMatrix(IEnumerable<IElement> elements, int dofNumber);
    }
}