using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties.Builder
{
    public interface IDynamicBeamPropertiesBuilderSetSection
    {
        IDynamicBeamPropertiesBuilderFinish SetRectangularSection(double width, double height);

        IDynamicBeamPropertiesBuilderFinish SetSection(Section section);
    }
}