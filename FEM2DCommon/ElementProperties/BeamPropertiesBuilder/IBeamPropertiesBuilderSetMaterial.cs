namespace Common.ElementProperties
{
    public interface IBeamPropertiesBuilderSetMaterial
    {
        IBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus);
        IBeamPropertiesBuilderSetSection SetSteel();
    }
}