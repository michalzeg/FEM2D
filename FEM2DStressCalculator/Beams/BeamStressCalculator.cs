using FEM2DCommon.Forces;
using FEM2DCommon.Sections;

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

        public double NormalStressAt(BeamForces forces, double y)
        {
            var distance = y - this.sectionProperties.Y0;
            var result = (forces.Axial / this.sectionProperties.A)
                         + forces.Moment / this.sectionProperties.Ix0 * distance;

            return result;
        }
    }
}