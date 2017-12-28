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

        public double GetDisplacement(int dofIndex, double time)
        {

            var leftValue = this.dynamicDisplacements.GetClosestDisplacementLeft(time);
            var rightValue = this.dynamicDisplacements.GetClosesDisplacementRight(time);

            var average = leftValue + (rightValue - leftValue) / 2;

            return average[dofIndex];
        }
        public IEnumerable<double> GetDisplacement(IEnumerable<int> dofIndices, double time)
        {

            var result = dofIndices.Select(e => this.GetDisplacement(e, time)).ToList();

            return result;
        }

        public double GetVelocity(int dofIndex, double time)
        {

            var leftValue = this.dynamicDisplacements.GetClosestVelocitiesLeft(time);
            var rightValue = this.dynamicDisplacements.GetClosesVelocitiesRight(time);

            var average = leftValue + (rightValue - leftValue) / 2;

            return average[dofIndex];
        }
        public IEnumerable<double> GetVelocity(IEnumerable<int> dofIndices, double time)
        {

            var result = dofIndices.Select(e => this.GetVelocity(e, time)).ToList();

            return result;
        }

        public double GetAcceleration(int dofIndex, double time)
        {

            var leftValue = this.dynamicDisplacements.GetClosestAccelerationsLeft(time);
            var rightValue = this.dynamicDisplacements.GetClosesAccelerationsRight(time);

            var average = leftValue + (rightValue - leftValue) / 2;

            return average[dofIndex];
        }
        public IEnumerable<double> GetAcceleration(IEnumerable<int> dofIndices, double time)
        {

            var result = dofIndices.Select(e => this.GetAcceleration(e, time)).ToList();

            return result;
        }
    }
}
