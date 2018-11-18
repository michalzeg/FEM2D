namespace FEM2DCommon.ElementProperties.Builder
{
    public interface IBeamPropertiesBuilderSetMaterial
    {
        IBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus);

        IBeamPropertiesBuilderSetSection SetSteel();
    }
}