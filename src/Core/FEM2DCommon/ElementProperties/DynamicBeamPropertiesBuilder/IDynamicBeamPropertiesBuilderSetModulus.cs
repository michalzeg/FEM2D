namespace FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder
{
    public interface IDynamicBeamPropertiesBuilderSetModulus
    {
        IDynamicBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus);
    }
}