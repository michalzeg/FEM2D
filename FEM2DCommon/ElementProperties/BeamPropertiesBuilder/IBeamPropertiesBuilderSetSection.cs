

using FEM2DCommon.Sections;

namespace FEM2DCommon.ElementProperties
{
    public interface IBeamPropertiesBuilderSetSection
    {
        IBeamPropertiesBuilderFinish SetSection(Section section);
        IBeamPropertiesBuilderFinish SetRectangularSection(double width, double height);
    }
}