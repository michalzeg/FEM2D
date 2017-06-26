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
using FEM2D.TriangleMatrces;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM2D.Elements
{
    internal class TriangleElement
    {
        private static int counter = 1;

        public int Number { get; private set; }
        public double Area { get; private set; }
        public Node[] Nodes { get; private set; }
        public Material Material { get; private set; }
        public double Thickness { get; private set; }

        private Matrix<double> B;
        private Matrix<double> D;
        private Matrix<double> K;

        public TriangleElement(Node p1, Node p2, Node p3,Material material,double thickness)
        {
            Condition.Requires(p1).IsNotNull().Evaluate(p => p != p2 && p != p3);
            Condition.Requires(p2).IsNotNull().Evaluate(p => p != p1 && p != p3);
            Condition.Requires(p3).IsNotNull().Evaluate(p => p != p1 && p != p2);
            this.Nodes = new[] { p1, p2, p3 };

            Condition.Requires(material).IsNotNull();
            this.Material = material;

            Condition.Requires(thickness).IsGreaterThan(0);
            this.Thickness = thickness;

            this.Number = counter;
            counter++;
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

    }
}
