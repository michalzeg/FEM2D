using Common.Extensions;
using FEM2DCommon.Forces;
using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.ShapeFunctions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2DCommon.Extensions;

namespace FEM2D.Results.Beams
{
    public class BeamElementResults
    {
        private readonly DofDisplacementMap dofDisplacementMap;
        private readonly IEnumerable<IBeamElement> elements;
        private readonly IEnumerable<IBeamLoad> loads;

        private IDictionary<IBeamElement, BeamElementResult> elementResultMap;

        internal BeamElementResults(DofDisplacementMap dofDisplacementMap, IEnumerable<IBeamElement> elements, IEnumerable<IBeamLoad> loads)
        {
            this.loads = loads;
            this.dofDisplacementMap = dofDisplacementMap;
            this.elements = elements;

            this.CreateElementResultMap();

        }

        private void CreateElementResultMap()
        {
            this.elementResultMap = this.elements.ToDictionary(e => e, f => this.CalculateElementResult(f));

        }
        private BeamElementResult CalculateElementResult(IBeamElement element)
        {
            var dofs = element.GetDOFs();
            var beamLoads = this.loads.Where(e => e.BeamElement.Equals(element)).ToList();

            var equivalentLoads = beamLoads.GetEquivalentNodalForces().ToVector();

            var displacements = this.dofDisplacementMap.GetValue(dofs).ToVector();

            var forces = -1 * equivalentLoads + element.GetStiffnessMatrix() * displacements;
            var forcesAtStart = BeamForces.FromFEMResult(forces);

            var result = new BeamElementResult(forcesAtStart, beamLoads, element, displacements.ToList());

            return result;
        }

        

        public BeamElementResult GetResult(IBeamElement element)
        {
            return this.elementResultMap[element];
        }

        
    }
}
