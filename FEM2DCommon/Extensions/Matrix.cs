using Common.Extensions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.Extensions
{
    public static class MatrixExtensions
    {
        public static bool IsApproximatelyEqualTo(this Matrix<double> matrix, Matrix<double> other)
        {
            if (matrix == null && other == null)
                return true;
            if ((matrix == null && other != null)
                || (matrix != null && other == null))
                return false;

            if (matrix.ColumnCount != other.ColumnCount
                || matrix.RowCount != other.RowCount)
                return false;


            for (int colIndex = 0; colIndex < matrix.ColumnCount; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < matrix.RowCount; rowIndex++)
                {
                    if (!matrix[rowIndex, colIndex].IsApproximatelyEqualTo(other[rowIndex, colIndex]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
