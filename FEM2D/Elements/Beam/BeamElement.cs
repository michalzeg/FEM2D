using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using FEM2D.Matrix;
using FEM2DCommon.DTO;

namespace FEM2D.Elements.Beam
{
    public class BeamElement : IBeamElement
    {
        private readonly BeamMatrix beamMatrix;

        public Node[] Nodes { get; private set; }
        public int Number { get; private set; }
        public BeamProperties BeamProperties { get; set; }
        public double Length { get; private set; }


        protected internal BeamElement(Node node1, Node node2, BeamProperties beamProperties, int number)
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
            return this.beamMatrix.GetK(this.Length, this.BeamProperties.MomentOfInertia, this.BeamProperties.ModulusOfElasticity,this.BeamProperties.Area);
        }
    }
}
