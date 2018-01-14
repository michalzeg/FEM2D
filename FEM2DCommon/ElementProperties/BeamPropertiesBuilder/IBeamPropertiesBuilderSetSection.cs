namespace FEM2DCommon.ElementProperties
{
    public interface IBeamPropertiesBuilderSetSection
    {
        IBeamPropertiesBuilderFinish SetRectangularSection(double width, double height);
    }
}