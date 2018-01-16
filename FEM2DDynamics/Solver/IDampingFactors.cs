namespace FEM2DDynamics.Solver
{
    internal interface IDampingFactors
    {
        double MassDampingFActor { get; }
        double StiffnessDampingFactor { get; }
    }
}