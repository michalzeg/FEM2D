using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class ArrayExtensions
    {
        public static Vector<double> ToVector(this IEnumerable<double> collection)
        {
            return Vector.Build.DenseOfArray(collection.ToArray());
        }

    }
}
