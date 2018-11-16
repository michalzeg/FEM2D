using Common.Geometry;
using FEM2D.Elements.Truss;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Matrix
{
    internal class TrussMatrix
    {
        public Matrix<double> GetK(double length, double modulusOfElasticity, double area)
        {
            var L = length;
            var E = modulusOfElasticity;
            var A = area;

            var matrix = DenseMatrix.OfArray(new double[,]
            {
                { 1, 0,-1, 0},
                { 0, 0, 0, 0},
                {-1, 0, 1, 0},
                { 0, 0, 0, 0}
            }) * A * E / L;

            return matrix;
        }

        public Matrix<double> GetT(PointD startPoint, PointD endPoint)
        {
            var length = startPoint.DistanceTo(endPoint);
            var sina = (endPoint.Y - startPoint.Y) / length;
            var cosa = (endPoint.X - startPoint.X) / length;
            var matrix = DenseMatrix.OfArray(new double[,]
            {
                {  cosa, sina,     0,    0 },
                { -sina, cosa,     0,    0 },
                {     0,    0,  cosa, sina },
                {     0,    0, -sina, cosa }
            });

            return matrix;
        }
    }
}