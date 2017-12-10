using FEM2D.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Beams.ForceDistributionCalculators
{
    internal class BeamForceDistributionCalculator : IBeamForceDistributionCalculator
    {
        private readonly PointLoadDistributionCalculator pointLoadDistributionCalculator;

        public BeamForceDistributionCalculator(IEnumerable<IBeamLoad> loads)
        {
            this.pointLoadDistributionCalculator = new PointLoadDistributionCalculator(loads.OfType<BeamPointLoad>().ToList());


        }

        public double Moment(double relativePosition)
        {
            var result = this.pointLoadDistributionCalculator.Moment(relativePosition);

            return result;
        }

        public double Shear(double relativePosition)
        {
            var result = this.pointLoadDistributionCalculator.Shear(relativePosition);

            return result;
        }
    }
}
