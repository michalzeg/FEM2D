using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Results.Beams.ForceDistributionCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Beams
{
    public class BeamElementResult
    {
        private readonly IEnumerable<IBeamLoad> beamLoads;
        private readonly IBeamElement element;
        private readonly IBeamForceDistributionCalculator distributionCalculator;
        private readonly double momentAtStart;
        private readonly double shearAtStart;
       

        internal BeamElementResult(double momentAtStart, double shearAtStart, IEnumerable<IBeamLoad> loads,IBeamElement element)
        {
            this.momentAtStart = momentAtStart;
            this.shearAtStart = shearAtStart;
            this.beamLoads = loads;
            this.element = element;
            this.distributionCalculator = new BeamForceDistributionCalculator(loads);
        }

        public double Moment(double relativePosition)
        {

            var result = this.momentAtStart
                       - this.distributionCalculator.Moment(relativePosition)
                       - this.MomentFromShearAtStart(relativePosition);
            return result;
        }

        public double Shear(double relativePosition)
        {

            var result = shearAtStart
                         + this.distributionCalculator.Shear(relativePosition);
            return result;             
        }

        
        private double MomentFromShearAtStart(double relativePosition)
        {
            return relativePosition * this.shearAtStart * this.element.Length;
        }

        

        
    }
}
