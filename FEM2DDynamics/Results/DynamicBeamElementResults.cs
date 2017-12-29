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

            var displacements = this.dofDisplacementMap.GetDisplacements(dofs, time);
            

            var forces = -1 * equivalentLoads 
                + element.GetStiffnessMatrix() * displacements.Displacements
                +element.GetDampingMatrix()*displacements.Velocities
                +element.GetMassMatrix()*displacements.Accelerations
                ;

            var normalStart = forces[0];
            var shearStart = forces[1];
            var momentStart = forces[2];


            var result = new DynamicBeamElementResult(momentStart, shearStart, beamLoads, element,displacements.Displacements.ToList(),time);

            return result;
        }

       
    }
}
