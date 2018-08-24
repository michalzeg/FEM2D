using FEM2D.Elements;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace FEM2DDynamics.Solver
{
    public interface IDynamicMatrixAggregator : IMatrixAggregator
    {
        Matrix<double> AggregateMassMatrix(IEnumerable<IElement> elements, int dofNumber);

        Matrix<double> AggregateMassMatrix(DynamicElementFactory elementFactory);

        Matrix<double> AggregateDampingMatrix(IEnumerable<IElement> elements, int dofNumber);

        Matrix<double> AggregateDampingMatrix(DynamicElementFactory elementFactory);

        Matrix<double> AggregateStiffnessMatrix(DynamicElementFactory elementFactory);
    }
}