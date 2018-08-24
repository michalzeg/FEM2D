namespace FEM2DDynamics.Solver
{
    public interface IDampingFactorCalculator
    {
        double MassDampingFactor { get; }
        double StiffnessDampingFactor { get; }
    }
}