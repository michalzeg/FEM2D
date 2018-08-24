using System;

namespace FEM2D.Utils
{
    internal interface IHashFunction
    {
        int Hash(int i, int j);

        Tuple<int, int> DeHash(int hash);
    }
}