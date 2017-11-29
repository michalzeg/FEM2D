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
        public Matrix<double> GetK(double length, double momentOfInertia, double modulusOfElasticity)
        {
            var L = length;
            var I = momentOfInertia;
            var E = modulusOfElasticity;

            var matrix = DenseMatrix.OfArray(new double[,]
            {
                {12 , 6*L  , -12 , 6L    },
                {6*L, 4*L*L, -6*L, 2*L*L },
                {-12, 6L   , 12  , -6*L  },
                {6L , 2*L*L, -6*L, 4*L*L }
            }) * E * I / (L * L * L);


            return matrix;
        }
    }
}

