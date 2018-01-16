using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using FEM2D.Elements.Beam;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using FEM2DDynamics.Matrix;
using Common.Geometry;
using FEM2DCommon.Geometry;
using FEM2DDynamics.Solver;

namespace FEM2DDynamics.Elements.Beam
{
    public class DynamicBeamElement : BeamElement, IDynamicBeamElement
    {

        private double stiffnessDampingFactor = 1;
        private double massDampingFactor = 1;
        private Matrix<double> massMatrix;
        private Matrix<double> dampingMatrix;


        public DynamicBeamProperties DynamicBeamProperties { get; private set; }

        internal DynamicBeamElement(Node node1, Node node2, DynamicBeamProperties dynamicBeamProperties, int number)
            :base(node1,node2,dynamicBeamProperties.BeamProperties,number)
        {
            this.DynamicBeamProperties = dynamicBeamProperties;
        }
        internal DynamicBeamElement(IBeamElement beamElement,DynamicBeamProperties dynamicBeamProeprties)
            :base(beamElement.Nodes[0],beamElement.Nodes[1],beamElement.BeamProperties,beamElement.Number)
        {
            this.DynamicBeamProperties = dynamicBeamProeprties;
        }

        public Matrix<double> GetMassMatrix()
        {
            if (this.massMatrix == null)
                this.massMatrix = DynamicBeamMatrix.GetMassMatrix(this.Length, this.DynamicBeamProperties.Density);
            return this.massMatrix;
        }

        public bool IsBetweenEnds(PointD point)
        {
            return Geometry.IsInsideSegmentWithoutEnds(this.Nodes[0].Coordinates, this.Nodes[1].Coordinates, point);
        }

        public Matrix<double> GetDampingMatrix()
        {
            if (this.dampingMatrix == null)
                this.dampingMatrix = DynamicBeamMatrix.GetDampingMatrix(this.GetStiffnessMatrix(), this.GetMassMatrix(), this.stiffnessDampingFactor, this.massDampingFactor);
            return this.dampingMatrix;
        }

        public void UpdateDampingFactors(IDampingFactors dampingFactors)
        {
            this.massDampingFactor = dampingFactors.MassDampingFactor;
            this.stiffnessDampingFactor = dampingFactors.StiffnessDampingFactor;
            this.dampingMatrix = DynamicBeamMatrix.GetDampingMatrix(this.GetStiffnessMatrix(), this.GetMassMatrix(), this.stiffnessDampingFactor, this.massDampingFactor);
        }
    }
}
