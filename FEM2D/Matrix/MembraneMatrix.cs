using FEM2D.Nodes;
using FEM2DCommon.DTO;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Matrix
{
    public class MembraneMatrix
    {
        public Matrix<double> GetD(MembraneProperties properties)
        {
            var E = properties.ModulusOfElasticity;
            var v = properties.PoissonsRation;

            var D = DenseMatrix.OfArray(new double[,]
            {
                {1, v, 0       },
                {v, 1, 0       },
                {0, 0, (1-v)/2 }
            }) * (E / (1 - v * v));

            return D;
        }

        public Matrix<double> GetB(IEnumerable<Node> nodes)
        {
            var nodeCoordinates = nodes.Select(e => e.Coordinates).ToArray();
            var p1 = nodeCoordinates[0];
            var p2 = nodeCoordinates[1];
            var p3 = nodeCoordinates[2];

            var y23 = p2.Y - p3.Y;
            var y31 = p3.Y - p1.Y;
            var y12 = p1.Y - p2.Y;
            var y13 = p1.Y - p3.Y;

            var x32 = p3.X - p2.X;
            var x13 = p1.X - p3.X;
            var x21 = p2.X - p1.X;
            var x23 = p2.X - p3.X;
            var detJ = x13 * y23 - y13 * x23;
            var B = DenseMatrix.OfArray(new double[,]
            {
                {y23, 0  , y31, 0  , y12 ,0   },
                {0  , x32, 0  , x13, 0   ,x21 },
                {x32, y23, x13, y31, x21 ,y12 }
            }) * (1 / detJ);

            return B;
        }

        public Matrix<double> GetK(double thickness, double area, Matrix<double> B, Matrix<double> D)
        {
            var K = thickness * area * B.Transpose() * D * B;
            return K;
        }
    }
}