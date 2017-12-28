using Common.Extensions;
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

            var forces = -1*equivalentLoads + element.GetStiffnessMatrix() * displacements;

            var normalStart = forces[0];
            var shearStart = forces[1];
            var momentStart = forces[2];

            

            var result = new BeamElementResult(momentStart, shearStart, beamLoads,element);
            
            return result;
        }

        public BeamElementResult GetResult(IBeamElement element)
        {
            return this.elementResultMap[element];
        }

        public double GetDisplacement(IBeamElement element, double relativePosition)
        {
            var position = relativePosition * element.Length;

            var node0Dofs = element.Nodes[0].GetDOF();
            var node1Dofs = element.Nodes[1].GetDOF();

            var u1 = this.dofDisplacementMap.GetValue(node0Dofs[0]);
            var u2 = this.dofDisplacementMap.GetValue(node0Dofs[1]);
            var u3 = this.dofDisplacementMap.GetValue(node0Dofs[2]);
            var u4 = this.dofDisplacementMap.GetValue(node1Dofs[0]);
            var u5 = this.dofDisplacementMap.GetValue(node1Dofs[1]);
            var u6 = this.dofDisplacementMap.GetValue(node1Dofs[2]);

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
