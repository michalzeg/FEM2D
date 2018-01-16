namespace FEM2DDynamics.Solver
{
    internal interface INaturalFrequencyCalculator
    {
        double GetFirstMode();
        double GetSecondMode();
    }
}