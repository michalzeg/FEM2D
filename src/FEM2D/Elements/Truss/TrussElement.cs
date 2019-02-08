using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2D.Matrix;
using FEM2D.Nodes;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements.Truss
{
    internal class TrussElement : ITrussElement
    {
        private readonly TrussMatrix trussMatrix;
        private Matrix<double> stiffnessMatrix;
        private Matrix<double> transformMatrix;
        private Matrix<double> localStiffnessMatrix;

        public Node[] Nodes { get; }
        public int Number { get; }
        public BarProperties BarProperties { get; }
        public double Length { get; }
        public object Tag { get; set; }

        protected internal TrussElement(Node node1, Node node2, BarProperties beamProperties, int number)
        {
            this.Nodes = new[] { node1, node2 };
            this.BarProperties = beamProperties;
            this.Number = number;
            this.Length = node1.DistanceTo(node2);

            this.trussMatrix = new TrussMatrix();
        }

        public int[] GetDOFs()
        {
            var node1DOFs = this.Nodes[0].GetDOF();
            var node2DOFs = this.Nodes[1].GetDOF();
            var result = node1DOFs.Concat(node2DOFs).ToArray();
            return result;
        }

        public Matrix<double> GetStiffnessMatrix()
        {
            if (this.stiffnessMatrix == null)
                this.stiffnessMatrix = this.GetTransformMatrix().Transpose() * this.GetLocalStiffnessMatrix() * this.GetTransformMatrix();
            return this.stiffnessMatrix;
        }

        private Matrix<double> GetLocalStiffnessMatrix()
        {
            if (this.localStiffnessMatrix == null)
                this.localStiffnessMatrix = this.trussMatrix.GetK(this.Length, this.BarProperties.ModulusOfElasticity, this.BarProperties.Area);
            return this.localStiffnessMatrix;
        }

        public Matrix<double> GetTransformMatrix()
        {
            if (this.transformMatrix == null)
                this.transformMatrix = this.trussMatrix.GetT(this.Nodes[0].Coordinates, this.Nodes[1].Coordinates);

            return this.transformMatrix;
        }
    }
}