namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public interface IDynamicBeamPropertiesBuilderSetMaterial
    {
        IDynamicBeamPropertiesBuilderSetDensity SetCustomMaterial();
        IDynamicBeamPropertiesBuilderSetSection SetSteel();
    }
}