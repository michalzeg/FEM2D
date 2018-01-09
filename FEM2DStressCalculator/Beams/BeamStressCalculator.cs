using Common.Forces;
using Common.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DStressCalculator.Beams
{
    public class BeamStressCalculator
    {
        private readonly SectionProperties sectionProperties;

        public BeamStressCalculator(SectionProperties sectionProperties)
        {
            this.sectionProperties = sectionProperties;
        }

        public double TopNormalStress(BeamForces forces)
        {
            var result = (forces.Axial / this.sectionProperties.A)
                         + forces.Moment / this.sectionProperties.Ix0 * this.sectionProperties.DY0_max;

            return result;
        }

        public double BottomNormalStress(BeamForces forces)
        {
            var result = (forces.Axial / this.sectionProperties.A) 
                - (forces.Moment / this.sectionProperties.Ix0 * this.sectionProperties.DY0_min);

            return result;
        }
    }
}
