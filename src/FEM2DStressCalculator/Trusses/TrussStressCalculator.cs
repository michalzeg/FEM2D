using FEM2D.Results.Trusses;
using FEM2DCommon.Forces;
using FEM2DCommon.Sections;

namespace FEM2DStressCalculator.Trusses
{
    public class TrussStressCalculator
    {
        private readonly SectionProperties sectionProperties;

        public TrussStressCalculator(SectionProperties sectionProperties)
        {
            this.sectionProperties = sectionProperties;
        }

        public double NormalStress(TrussElementResult forces) => forces.NormalForce / this.sectionProperties.A;
    }
}