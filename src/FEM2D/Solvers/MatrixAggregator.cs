using FEM2D.Elements;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;

namespace FEM2D.Solvers
{
    public class MatrixAggregator : IMatrixAggregator
    {
        public virtual Matrix<double> AggregateStiffnessMatrix(ElementFactory elementFactory)
        {
            return this.AggregateStiffnessMatrix(elementFactory.GetAll(), elementFactory.GetDOFsCount());
        }

        public virtual Matrix<double> AggregateStiffnessMatrix(IEnumerable<IElement> elements, int dofNumber)
        {
            var matrix = this.Aggregate(elements, dofNumber, e => e.GetStiffnessMatrix());
            return matrix;
        }

        protected Matrix<double> Aggregate(IEnumerable<IElement> elements, int dofNumber, Func<IElement, Matrix<double>> elementMatrix)
        {
            var aggregatedMatrix = SparseMatrix.Create(dofNumber, dofNumber, 0d);
            foreach (var element in elements)
            {
                var dofs = element.GetDOFs();
                var k = elementMatrix(element);
                for (int i = 0; i < k.ColumnCount; i++)
                {
                    for (int j = 0; j < k.RowCount; j++)
                    {
                        var colIndex = dofs[i];
                        var rowIndex = dofs[j];
                        aggregatedMatrix[rowIndex, colIndex] += k[j, i];
                    }
                }
            }
            return aggregatedMatrix;
        }
    }
}