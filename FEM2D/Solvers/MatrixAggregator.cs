using FEM2D.Elements;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Solvers
{
    public class MatrixAggregator : IMatrixAggregator
    {
        public Matrix<double> Aggregate(IEnumerable<ITriangleElement> elements, int dofNumber)
        {
            var aggregatedMatrix = SparseMatrix.Create(dofNumber, dofNumber, 0d);

            foreach (var element in elements)
            {
                var dofs = element.GetDOFs();
                var k = element.GetK();
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
