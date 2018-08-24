using FEM2DCommon.Sections;

namespace FEMCommon.ElementProperties.SectionBuilders.CustomSection
{
    public interface ICustomSectionBuilderAddOtherPerimeters
    {
        ICustomSectionBuilderFinish WithNoMorePerimeters();

        ICustomSectionBuilderAddOtherPerimeters WithPerimeter(Perimeter perimeter);
    }
}