using Common.Extensions;
using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Results.Beams;
using FEM2D.ShapeFunctions;
using FEM2DDynamics.Elements.Beam;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Results.Beam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results
{
    public class DynamicBeamElementResults
    {
        private readonly IDynamicDofDisplacementMap dofDisplacementMap;
        private readonly DynamicLoadFactory loadFactory;

        internal DynamicBeamElementResults(IDynamicDofDisplacementMap dofDisplacementMap,DynamicLoadFactory loadFactory)
        {
            this.dofDisplacementMap = dofDisplacementMap;
            this.loadFactory = loadFactory;
            
        }

        public DynamicBeamElementResult GetResult(IDynamicBeamElement element, double time)
        {
            var result = this.CalculateElementResult(element, time);
            return result;

        }

        private DynamicBeamElementResult CalculateElementResult(IDynamicBeamElement element, double time)
        {
            var dofs = element.GetDOFs();
            var beamLoads = this.loadFactory.GetBeamLoads(element, time);


            var equivalentLoads = beamLoads.GetEquivalentNodalForces().ToVector();

            var displacements = this.dofDisplacementMap.GetDisplacement(dofs,time).ToVector();
            var velociteis = this.dofDisplacementMap.GetVelocity(dofs, time).ToVector();
            var accelerations = this.dofDisplacementMap.GetAcceleration(dofs, time).ToVector();

            var forces = -1 * equivalentLoads 
                + element.GetStiffnessMatrix() * displacements
                +element.GetDampingMatrix()*velociteis
                +element.GetMassMatrix()*accelerations
                ;

            var normalStart = forces[0];
            var shearStart = forces[1];
            var momentStart = forces[2];


            var result = new DynamicBeamElementResult(momentStart, shearStart, beamLoads, element,time);

            return result;
        }

        public double GetDisplacement(IDynamicBeamElement element, double relativePosition, double time)
        {
            var position = relativePosition * element.Length;

            var node0Dofs = element.Nodes[0].GetDOF();
            var node1Dofs = element.Nodes[1].GetDOF();

            var u1 = this.dofDisplacementMap.GetDisplacement(node0Dofs[0],time);
            var u2 = this.dofDisplacementMap.GetDisplacement(node0Dofs[1],time);
            var u3 = this.dofDisplacementMap.GetDisplacement(node0Dofs[2],time);
            var u4 = this.dofDisplacementMap.GetDisplacement(node1Dofs[0],time);
            var u5 = this.dofDisplacementMap.GetDisplacement(node1Dofs[1],time);
            var u6 = this.dofDisplacementMap.GetDisplacement(node1Dofs[2],time);

            var result = u1 * BeamShapeFunctions.N1(position, element.Length)
                + u2 * BeamShapeFunctions.N2(position, element.Length)
                + u3 * BeamShapeFunctions.N3(position, element.Length)
                + u4 * BeamShapeFunctions.N4(position, element.Length)
                + u5 * BeamShapeFunctions.N5(position, element.Length)
                + u6 * BeamShapeFunctions.N6(position, element.Length);
            return result;
        }
    }
}
