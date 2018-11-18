namespace FEM2DCommon.ElementProperties.Builder
{
    public interface IDynamicBeamPropertiesBuilderSetMaterial
    {
        IDynamicBeamPropertiesBuilderSetDensity SetCustomMaterial();

        IDynamicBeamPropertiesBuilderSetSection SetSteel();
    }
}