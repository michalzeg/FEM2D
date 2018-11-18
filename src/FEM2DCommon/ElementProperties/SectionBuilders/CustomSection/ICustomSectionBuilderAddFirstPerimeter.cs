using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties.SectionBuilders.CustomSection
{
    public interface ICustomSectionBuilderAddFirstPerimeter
    {
        ICustomSectionBuilderAddOtherPerimeters WithPerimeter(Perimeter perimeter);
    }
}