namespace FEM2DCommon.DTO
{
    public interface IBarElementProperties
    {
        double Area { get; }
        double ModulusOfElasticity { get; }
        double MomentOfInertia { get; }
    }
}