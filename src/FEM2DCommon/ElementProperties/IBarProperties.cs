namespace FEM2DCommon.ElementProperties
{
    public interface IBarProperties
    {
        double Area { get; }
        double ModulusOfElasticity { get; }
        double MomentOfInertia { get; }
    }
}