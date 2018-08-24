using FEM2DCommon.Extensions;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

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

        public TimeDisplacementPair GetDisplacements(IEnumerable<int> dofIndices, double time)
        {
            var leftValue = this.dynamicDisplacements.GetClosestLeft(time);
            var rightValue = this.dynamicDisplacements.GetClosesRight(time);

            var avgDisplacement = this.CalculateAverage(leftValue.Displacements, rightValue.Displacements);
            var avgVelocity = this.CalculateAverage(leftValue.Velocities, rightValue.Velocities);
            var avgAccelerations = this.CalculateAverage(leftValue.Accelerations, rightValue.Accelerations);

            var displacementResult = this.GetAppropriateValues(avgDisplacement, dofIndices);
            var velocityResult = this.GetAppropriateValues(avgVelocity, dofIndices);
            var accelerationResult = this.GetAppropriateValues(avgAccelerations, dofIndices);

            return new TimeDisplacementPair
            {
                Time = time,
                Displacements = displacementResult,
                Velocities = velocityResult,
                Accelerations = accelerationResult,
            };
        }

        private Vector<double> GetAppropriateValues(Vector<double> values, IEnumerable<int> dofIndices)
        {
            var result = values.Select((e, i) => new { Value = e, Index = i })
                                                    .Where(e => dofIndices.Contains(e.Index))
                                                    .Select(e => e.Value)
                                                    .ToVector();
            return result;
        }

        private Vector<double> CalculateAverage(Vector<double> leftValue, Vector<double> rightValue)
        {
            var average = leftValue + (rightValue - leftValue) / 2;

            return average;
        }
    }
}