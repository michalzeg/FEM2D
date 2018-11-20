using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties
{
    public class BarProperties
    {
        public double ModulusOfElasticity { get; set; }

        public SectionProperties SectionProperties { get; set; } = new SectionProperties();

        public double Area => this.SectionProperties.A;
        public double MomentOfInertia => this.SectionProperties.Ix0;

        public static BarProperties Default => new BarProperties
        {
            ModulusOfElasticity = 200000000,
            SectionProperties = Section.FromRectangle(0.3, 1).SectionProperties,
        };
    }
}