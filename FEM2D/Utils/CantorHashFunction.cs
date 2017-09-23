using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Utils
{
    public class CantorHashFunction : IHashFunction
    {
        public Tuple<int, int> DeHash(int hash)
        {
            var w = (int)Math.Floor(Math.Round((Math.Sqrt(8 * hash + 1) - 1) / 2, 5));
            var t = (w * w + w) / 2;
            var j = hash - t;
            var i = w - j;
            return new Tuple<int, int>(i, j);
        }

        public int Hash(int i, int j)
        {
            var result = (i + j) * (i + j + 1)/2 + j;
            return result;
        }
    }
}
