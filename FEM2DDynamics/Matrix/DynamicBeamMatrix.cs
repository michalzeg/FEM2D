using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Matrix
{
    internal class DynamicBeamMatrix
    {
        public static Matrix<double> GetMassMatrix(double length, double density)
        {
            var L = length;
            var u = density;


            var matrix = DenseMatrix.OfArray(new double[,]
            {
                {140 , 0      , 0     , 70     , 0      , 0       },
                {0   , 156    , 22*L  , 0      , 54     , -13*L   },
                {0   , 22*L   , 4*L*L , 0      , 13*L   , -3*L*L  },
                {70  , 0      , 0     , 140    , 0      , 0       },
                {0   , 54     , 13*L  , 0      , 156    , -22*L   },
                {0   ,-13*L   , -3*L*L, 0      , -22*L  , 4*L*L   }
            }) * u * L / 420;


            return matrix;
        }
    }
}
