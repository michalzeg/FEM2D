using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class VectorExtensions
    {
        public static bool IsApproximatelyEqualTo(this Vector<double> vector, Vector<double> other)
        {
            if (vector == null && other == null)
                return true;
            if ((vector == null && other != null)
                || (vector != null && other == null))
                return false;

            if (vector.Count != other.Count)
                return false;


            for (int rowIndex = 0; rowIndex < vector.Count; rowIndex++)
            {
                    if (!vector[rowIndex].IsApproximatelyEqualTo(other[rowIndex]))
                    {
                        return false;
                    }
            }
            return true;
        }
    }
}
