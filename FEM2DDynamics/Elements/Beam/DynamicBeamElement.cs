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
        private readonly IDampingMatrixCalculator dampingCalculator;

        public DynamicBeamProperties DynamicBeamProperties { get; private set; }

        internal DynamicBeamElement(Node node1, Node node2, DynamicBeamProperties dynamicBeamProperties, int number)
            :base(node1,node2,dynamicBeamProperties.BeamProperties,number)
        {
            this.DynamicBeamProperties = dynamicBeamProperties;
            this.dampingCalculator = new SimpleDampingMatrixCalculator();
        }
        internal DynamicBeamElement(IBeamElement beamElement,DynamicBeamProperties dynamicBeamProeprties)
            :base(beamElement.Nodes[0],beamElement.Nodes[1],beamElement.BeamProperties,beamElement.Number)
        {
            this.DynamicBeamProperties = dynamicBeamProeprties;
            this.dampingCalculator = new SimpleDampingMatrixCalculator();
        }

        public Matrix<double> GetMassMatrix()
        {
            var matrix = DynamicBeamMatrix.GetMassMatrix(this.Length, this.DynamicBeamProperties.Density);
            return matrix;
        }

        public bool IsBetweenEnds(PointD point)
        {
            return Geometry.IsInsideSegmentWithoutEnds(this.Nodes[0].Coordinates, this.Nodes[1].Coordinates, point);
        }

        public Matrix<double> GetDampingMatrix()
        {
            var matrix =  dampingCalculator.GetDampingMatrix(this.GetStiffnessMatrix(), this.GetMassMatrix());
            return matrix;
        }
    }
}
