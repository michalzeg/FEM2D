using FEM2D.Loads;
using FEM2D.Loads.Beams;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Results.Beams.ForceDistributionCalculators
{
    internal class UniformLoadDistributionCalculator : IBeamForceDistributionCalculator
    {
        private readonly IEnumerable<BeamUniformLoad> uniformLoads;

        public UniformLoadDistributionCalculator(IEnumerable<BeamUniformLoad> uniformLoads)
        {
            this.uniformLoads = uniformLoads;
        }

        public double Moment(double relativePosition)
        {
            var result = this.uniformLoads
                             .Sum(e => this.MomentFromUniformLoad(e, relativePosition));
            return result;
        }

        private double MomentFromUniformLoad(BeamUniformLoad load, double relativePosition)
        {
            var distance = relativePosition * load.BeamElement.Length;

            var moment = distance * load.ValueY * distance / 2;
            return moment;
        }

        public double Shear(double relativePosition)
        {
            var result = this.uniformLoads
                            .Sum(e => this.ShearFromUniformLoad(e, relativePosition));
            return result;
        }

        private double ShearFromUniformLoad(BeamUniformLoad load, double relativePosition)
        {
            var distance = relativePosition * load.BeamElement.Length;
            var shear = distance * load.ValueY;
            return shear;
        }
    }
}