using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties
{
    public class BarProperties : IBarProperties
    {
        public double ModulusOfElasticity { get; set; }
        public Section Section { get; set; }

        public double Area => this.Section.SectionProperties.A;
        public double MomentOfInertia => this.Section.SectionProperties.Iy0;

        public static BarProperties Default => new BarProperties
        {
            ModulusOfElasticity = 200000000,
            Section = Section.FromRectangle(1, 0.3),
        };
    }
}