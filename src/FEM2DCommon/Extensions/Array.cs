using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;

namespace FEM2DCommon.Extensions
{
    public static class ArrayExtensions
    {
        public static Vector<double> ToVector(this IEnumerable<double> collection)
        {
            return Vector.Build.DenseOfArray(collection.ToArray());
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> items, int partitionSize)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / partitionSize)
                        .Select(g => g.Select(x => x.item));
        }
    }
}