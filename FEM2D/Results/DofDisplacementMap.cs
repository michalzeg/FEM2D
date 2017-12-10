using CuttingEdge.Conditions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results
{
    internal class DofDisplacementMap
    {
        private IDictionary<int, double> dofDisplacementMap;

        public DofDisplacementMap(Vector<double> displacements)
        {
            this.dofDisplacementMap = displacements
                .Select((e, i) => new { index = i, value = e })
                .ToDictionary(i => i.index, v => v.value);
        }

        public double GetValue(int dofIndex)
        {
            double result = 0d;

            this.dofDisplacementMap.TryGetValue(dofIndex, out result);

            return result;
        }
        public IEnumerable<double> GetValue(IEnumerable<int> dofIndices)
        {
            Condition.Requires(dofIndices).IsNotNull().IsNotEmpty();

            var displacements = dofIndices.Select(e => this.dofDisplacementMap[e]).ToArray();
            return displacements;
        }
    }
}
