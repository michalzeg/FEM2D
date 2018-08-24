using FEM2DCommon.Sections;

namespace FEMCommon.ElementProperties.SectionBuilders.CustomSection
{
    public interface ICustomSectionBuilderAddFirstPerimeter
    {
        ICustomSectionBuilderAddOtherPerimeters WithPerimeter(Perimeter perimeter);
    }
}