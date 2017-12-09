using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Matrix
{
    public class BeamMatrix
    {
        public Matrix<double> GetK(double length, double momentOfInertia, double modulusOfElasticity,double area)
        {
            var L = length;
            var I = momentOfInertia;
            var E = modulusOfElasticity;
            var A = area;

            var matrix = DenseMatrix.OfArray(new double[,]
            {
                {A*L*L/I , 0    ,0     , -A*L*L/I, 0   , 0      },
                {0       , 12   , 6*L  , 0       , -12 , 6*L    },
                {0       , 6*L  , 4*L*L, 0       , -6*L, 2*L*L },
                {-A*L*L/I, 0    ,0     , A*L*L/I , 0   , 0      },
                {0       ,-12   , -6*L , 0       , 12  , -6*L   },
                {0       ,6*L   , 2*L*L, 0       , -6*L, 4*L*L  }
            }) * 3 * E * I / (L * L * L);


            return matrix;
        }
    }
}

