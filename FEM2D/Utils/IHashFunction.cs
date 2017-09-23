using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Utils
{
    interface IHashFunction
    {
        int Hash(int i, int j);
        Tuple<int, int> DeHash(int hash);
    }
}
