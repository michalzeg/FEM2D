using FEM2D.Matrix;
using FEM2D.Nodes;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Linq;

namespace FEM2D.Elements.Triangle
{
    public class TriangleElement : ITriangleElement
    {
        private readonly MembraneMatrix matrixCalculator;

        public int Number { get; private set; }
        public double Area { get; private set; }
        public Node[] Nodes { get; private set; }
        public MembraneProperties Properties { get; private set; }
        public object Tag { get; set; }
        private Matrix<double> B;
        private Matrix<double> D;
        private Matrix<double> K;

        internal TriangleElement(Node p1, Node p2, Node p3, MembraneProperties properties, int number)
        {
            this.Nodes = new[] { p1, p2, p3 };
            this.Properties = properties;
            this.Number = number;

            calculateArea();

            this.matrixCalculator = new MembraneMatrix();
        }

        public Matrix<double> GetD()
        {
            if (this.D == null)
            {
                this.D = this.matrixCalculator.GetD(this.Properties);
            }
            return this.D;
        }

        public Matrix<double> GetB()
        {
            if (this.B == null)
            {
                this.B = this.matrixCalculator.GetB(this.Nodes);
            }
            return this.B;
        }

        public Matrix<double> GetStiffnessMatrix()
        {
            if (this.K == null)
            {
                GetB();
                GetD();
                this.K = this.matrixCalculator.GetK(this.Properties.Thickness, this.Area, this.B, this.D);
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