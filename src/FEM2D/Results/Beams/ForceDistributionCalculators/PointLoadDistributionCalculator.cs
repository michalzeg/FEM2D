using FEM2D.Loads;
using FEM2D.Loads.Beams;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Results.Beams.ForceDistributionCalculators
{
    internal class PointLoadDistributionCalculator : IBeamForceDistributionCalculator
    {
        private readonly IEnumerable<BeamPointLoad> pointLoads;

        public PointLoadDistributionCalculator(IEnumerable<BeamPointLoad> pointLoads)
        {
            this.pointLoads = pointLoads;
        }

        public double Moment(double relativePosition)
        {
            var result = this.pointLoads
                             .GetRelativePosition(relativePosition)
                             .Sum(e => (relativePosition - e.RelativePosition) * e.BeamElement.Length * e.ValueY);
            return result;
        }

        public double Shear(double relativePosition)
        {
            var result = this.pointLoads
                            .GetRelativePosition(relativePosition)
                            .Sum(e => e.ValueY);
            return result;
        }
    }
}