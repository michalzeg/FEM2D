using FEM2D.Elements.Truss;
using FEM2DCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Trusses
{
    public class TrussElementResults
    {
        private readonly DofDisplacementMap dofDisplacementMap;
        private readonly IEnumerable<ITrussElement> elements;
        private readonly Dictionary<ITrussElement, TrussElementResult> elementResultMap;

        internal TrussElementResults(DofDisplacementMap dofDisplacementMap, IEnumerable<ITrussElement> elements)
        {
            this.elements = elements;
            this.dofDisplacementMap = dofDisplacementMap;
            this.elementResultMap = CalculateResults(elements);
        }

        private Dictionary<ITrussElement, TrussElementResult> CalculateResults(IEnumerable<ITrussElement> elements) => elements.ToDictionary(e => e, f => this.CalculateElementResult(f));

        private TrussElementResult CalculateElementResult(ITrussElement element)
        {
            var dofs = element.GetDOFs();
            var displacements = this.dofDisplacementMap.GetValue(dofs).ToVector();

            var globalForces = element.GetStiffnessMatrix() * displacements;
            var localForces = element.GetTransformMatrix() * globalForces;
            var result = new TrussElementResult()
            {
                NormalForce = localForces[0],
            };
            return result;
        }

        public TrussElementResult GetElementResult(ITrussElement element) => this.elementResultMap[element];

        public IEnumerable<TrussElementResult> GetElementResult(IEnumerable<ITrussElement> elements)
            => this.elementResultMap.Keys
                   .Intersect(elements)
                   .Select(e => this.elementResultMap[e])
                   .ToList();

        public IEnumerable<TrussElementResult> GetElementResult() => this.elementResultMap.Values;
    }
}