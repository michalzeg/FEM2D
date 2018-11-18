using FEM2D.Matrix;
using FEM2D.Nodes;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using MathNet.Numerics.LinearAlgebra;
using System.Linq;

namespace FEM2D.Elements.Beam
{
    public class BeamElement : IBeamElement
    {
        private readonly BeamMatrix beamMatrix;
        private Matrix<double> stiffnessMatrix;

        public Node[] Nodes { get; private set; }
        public int Number { get; private set; }
        public IBarElementProperties BeamProperties { get; set; }
        public double Length { get; private set; }

        protected internal BeamElement(Node node1, Node node2, IBarElementProperties beamProperties, int number)
        {
            this.Nodes = new[] { node1, node2 };
            this.BeamProperties = beamProperties;
            this.Number = number;
            this.Length = node1.DistanceTo(node2);

            this.beamMatrix = new BeamMatrix();
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
                this.stiffnessMatrix = this.beamMatrix.GetK(this.Length, this.BeamProperties.MomentOfInertia, this.BeamProperties.ModulusOfElasticity, this.BeamProperties.Area);
            return this.stiffnessMatrix;
        }
    }
}