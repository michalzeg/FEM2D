using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

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

        public static Matrix<double> GetDampingMatrix(Matrix<double> stiffness, Matrix<double> mass, double stiffnessFactor, double massFactor)
        {
            var matrix = stiffnessFactor * stiffness + massFactor * mass;

            return matrix;
        }
    }
}