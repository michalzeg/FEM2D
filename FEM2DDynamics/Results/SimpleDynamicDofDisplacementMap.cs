using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results
{

    /// <summary>
    /// Displacement calculator. It uses simple interpolation to calculate displacement values over time.
    /// </summary>
    internal class SimpleDynamicDofDisplacementMap : IDynamicDofDisplacementMap
    {
        private readonly DynamicDisplacements dynamicDisplacements;

        internal SimpleDynamicDofDisplacementMap(DynamicDisplacements dynamicDisplacements)
        {
            this.dynamicDisplacements = dynamicDisplacements;
        }

        public double GetValue(int dofIndex, double time)
        {


            var leftValue = this.dynamicDisplacements.GetClosestLeft(time);
            var rightValue = this.dynamicDisplacements.GetClosesRight(time);

            var average = leftValue + (rightValue - leftValue) / 2;

            return average[dofIndex];
        }
        public IEnumerable<double> GetValue(IEnumerable<int> dofIndices, double time)
        {

            var result = dofIndices.Select(e => this.GetValue(e, time)).ToList();

            return result;
        }
    }
}
