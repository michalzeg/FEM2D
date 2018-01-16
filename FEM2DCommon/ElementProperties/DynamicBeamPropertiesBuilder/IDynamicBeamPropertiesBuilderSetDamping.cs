namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public interface IDynamicBeamPropertiesBuilderSetDamping
    {
        IDynamicBeamPropertiesBuilderSetModulus SetDamping(double damping);
    }
}