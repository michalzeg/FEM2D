using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties.SectionBuilders.CustomSection
{
    public interface ICustomSectionBuilderAddOtherPerimeters
    {
        ICustomSectionBuilderFinish WithNoMorePerimeters();

        ICustomSectionBuilderAddOtherPerimeters WithPerimeter(Perimeter perimeter);
    }
}