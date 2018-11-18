namespace FEM2DCommon.ElementProperties
{
    public interface IBarElementProperties
    {
        double Area { get; }
        double ModulusOfElasticity { get; }
        double MomentOfInertia { get; }
    }
}