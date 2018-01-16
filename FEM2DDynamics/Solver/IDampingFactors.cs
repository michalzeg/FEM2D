namespace FEM2DDynamics.Solver
{
    public interface IDampingFactors
    {
        double MassDampingFactor { get; }
        double StiffnessDampingFactor { get; }
    }
}