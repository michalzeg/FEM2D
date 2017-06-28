using Common.Point;
using CuttingEdge.Conditions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge;
using FEM2D.Nodes;
using FEM2D.Materials;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM2D.Elements
{
    public class TriangleElement : ITriangleElement
    {
        private static int counter = 1;

        public int Number { get; private set; }
        public double Area { get; private set; }
        public Node[] Nodes { get; private set; }
        public Material Material { get; private set; }
        public double Thickness { get; private set; }
        public int NumberOfDOFs { get; private set; }

       

        private Matrix<double> B;
        private Matrix<double> D;
        private Matrix<double> K;

        public TriangleElement(Node p1, Node p2, Node p3,Material material,double thickness)
        {
            this.Nodes = new[] { p1, p2, p3 };
            this.Material = material;
            this.Thickness = thickness;
            this.NumberOfDOFs = 6;
            this.Number = counter;
            counter++;

            calculateArea();
        }

        public Matrix<double> GetD()
        {
            if (this.D == null)
            {
                var E = Material.ModulusOfElasticity;
                var v = Material.PoissonsRation;

                this.D = DenseMatrix.OfArray(new double[,]
                {
                {1, v, 0       },
                {v, 1, 0       },
                {0, 0, (1-v)/2 }
                });
                this.D = this.D * (E / 1 - v * v);
            }
            return this.D;
        }
        public Matrix<double> GetB()
        {
            if (this.B == null)
            {
                var nodeCoordinates = this.Nodes.Select(e => e.Coordinates).ToArray();
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

                this.B = DenseMatrix.OfArray(new double[,]
                {
                {y23, 0  , y31, 0  , y12 ,0   },
                {0  , x32, 0  , x13, 0   ,x21 },
                {x32, y23, x13, y31, x21 ,y12 }
                });

                var detJ = x13 * y23 - y13 * x23;

                this.B = this.B * (1 / detJ);
            }
            return this.B;
        }
        public Matrix<double> GetK()
        {
            if (this.K == null)
            {
                this.K = Thickness * Area * B.Transpose() * D * B;
            }
            return this.K;
        }

        public int[] GetDOFs()
        {
            var node1DOFs = this.Nodes[0].GetDOF();
            var node2DOFs = this.Nodes[1].GetDOF();
            var node3DOFs = this.Nodes[2].GetDOF();
            var result = node1DOFs.Concat(node2DOFs).Concat(node3DOFs).ToArray();
            return result;
        }

        private void calculateArea()
        {
            var p1 = this.Nodes[0].Coordinates;
            var p2 = this.Nodes[1].Coordinates;
            var p3 = this.Nodes[2].Coordinates;

            this.Area = 0.5 * Math.Abs(p1.X * p2.Y - p1.X * p3.Y + p2.X * p3.Y - p2.X * p1.Y + p3.X * p1.Y - p3.X * p2.Y);
        }
    }
}
