using FEM2DCommon.Sections;

namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public interface IDynamicBeamPropertiesBuilderSetSection
    {
        IDynamicBeamPropertiesBuilderFinish SetRectangularSection(double width, double height);
        IDynamicBeamPropertiesBuilderFinish SetSection(Section section);
    }
}