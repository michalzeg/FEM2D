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

        public Matrix<double> AggregateDampingMatrix(DynamicElementFactory elementFactory)
        {
            return this.AggregateDampingMatrix(elementFactory.GetAll(), elementFactory.GetDOFsCount());
        }

        public Matrix<double> AggregateMassMatrix(IEnumerable<IElement> elements, int dofNumber)
        {
            var dynamicElements = elements.OfType<IDynamicElement>();

            var matrix = this.Aggregate(dynamicElements, dofNumber, e => ((IDynamicElement)e).GetMassMatrix());
            return matrix;
        }

        public Matrix<double> AggregateMassMatrix(DynamicElementFactory elementFactory)
        {
            return this.AggregateMassMatrix(elementFactory.GetAll(), elementFactory.GetDOFsCount());
        }

        public Matrix<double> AggregateStiffnessMatrix(DynamicElementFactory elementFactory)
        {
            return base.AggregateStiffnessMatrix(elementFactory.GetAll(), elementFactory.GetDOFsCount());
        }
    }
}