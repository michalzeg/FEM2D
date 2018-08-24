namespace FEM2DCommon.ElementProperties
{
    public interface IBeamPropertiesBuilderSetMaterial
    {
        IBeamPropertiesBuilderSetSection SetModulusOfElasticity(double modulus);

        IBeamPropertiesBuilderSetSection SetSteel();
    }
}