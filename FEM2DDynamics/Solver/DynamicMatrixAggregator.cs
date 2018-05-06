using FEM2D.Elements;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace FEM2DDynamics.Solver
{
    public class DynamicMatrixAggregator : MatrixAggregator, IDynamicMatrixAggregator
    {
        public Matrix<double> AggregateDampingMatrix(IEnumerable<IElement> elements, int dofNumber)
        {
            var dynamicElements = elements.OfType<IDynamicElement>();

            var matrix = this.Aggregate(dynamicElements, dofNumber, e => ((IDynamicElement)e).GetDampingMatrix());
            return matrix;
        }

        public Matrix<double> AggregateMassMatrix(IEnumerable<IElement> elements, int dofNumber)
        {
            var dynamicElements = elements.OfType<IDynamicElement>();

            var matrix = this.Aggregate(dynamicElements, dofNumber, e => ((IDynamicElement)e).GetMassMatrix());
            return matrix;
        }
    }
}