namespace FEM2D.Results.Beams.ForceDistributionCalculators
{
    internal interface IBeamForceDistributionCalculator
    {
        double Moment(double relativePosition);

        double Shear(double relativePosition);
    }
}