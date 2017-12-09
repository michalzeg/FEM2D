using FEM2D.Elements.Beam;
using FEM2D.Loads;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var displacements = this.dofDisplacementMap.GetValue(dofs).ToArray();

            var displacementVector = Vector.Build.DenseOfArray(displacements);

            var forces = element.GetStiffnessMatrix() * displacementVector;

            var normalStart = forces[0];
            var shearStart = forces[1];
            var momentStart = forces[2];

            var loads = this.loads.Where(e => e.BeamElement.Equals(element));

            var result = new BeamElementResult(momentStart, shearStart, loads,element);
            
            return result;
        }

        public BeamElementResult GetResult(IBeamElement element)
        {
            return this.elementResultMap[element];
        }
    }
}
