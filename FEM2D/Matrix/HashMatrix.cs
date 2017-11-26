using FEM2D.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
namespace FEM2D.Matrix
{
    public class HashMatrix<T>  where T :struct,IEquatable<T>,IFormattable
    {
        private readonly IHashFunction hashFunction;
        private readonly IDictionary<int, T> container;
        private readonly int rows;
        private readonly int columns;

        public HashMatrix(int rows, int columns)
        {
            this.hashFunction = new CantorHashFunction();
            this.container = new Dictionary<int, T>();
            this.rows = rows;
            this.columns = columns;
        }

        public T this[int i, int j]
        {
            get
            {
                var hash = this.hashFunction.Hash(i, j);
                return this.container[hash];
            }
            set
            {
                var hash = this.hashFunction.Hash(i, j);
                this.container[hash] = value;
            }
        }

        public Matrix<T> GetMatrix()
        {
            var result = CreateMatrix.Sparse<T>(this.rows, this.columns);

            foreach (var value in this.container)
            {
                var indexes = this.hashFunction.DeHash(value.Key);
                var i = indexes.Item1;
                var j = indexes.Item2;

                result[i, j] = value.Value;
            }
            return result;
        }
    }
}
