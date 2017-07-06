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
using FEM2D.Matrix;

namespace FEM2D.Elements
{
    public class TriangleElement : ITriangleElement
    {
        private static int counter = 1;

        private readonly MatrixCalculator matrixCalculator;

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

            this.matrixCalculator = new MatrixCalculator();
        }

        public Matrix<double> GetD()
        {
            if (this.D == null)
            {
                this.D = this.matrixCalculator.GetD(this.Material);
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

        public Matrix<double> GetK()
        {
            if (this.K == null)
            {
                GetB();
                GetD();
                this.K = this.matrixCalculator.GetK(this.Thickness, this.Area, this.B, this.D);
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
