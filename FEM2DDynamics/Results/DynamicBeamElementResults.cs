using FEM2D.Results.Beams;
using FEM2DCommon.Extensions;
using FEM2DCommon.Forces;
using FEM2DDynamics.Elements.Beam;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Results.Beam;
using System.Linq;

namespace FEM2DDynamics.Results
{
    public class DynamicBeamElementResults
    {
        private readonly IDynamicDofDisplacementMap dofDisplacementMap;
        private readonly DynamicLoadFactory loadFactory;

        internal DynamicBeamElementResults(IDynamicDofDisplacementMap dofDisplacementMap, DynamicLoadFactory loadFactory)
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
                + element.GetDampingMatrix() * displacements.Velocities
                + element.GetMassMatrix() * displacements.Accelerations
                ;

            var forcesAtStart = BeamForces.FromFEMResult(forces);

            var result = new DynamicBeamElementResult(forcesAtStart, beamLoads, element, displacements.Displacements.ToList(), displacements.Accelerations.ToList(), time);

            return result;
        }
    }
}