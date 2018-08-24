namespace FEM2D.Results.Beams
{
    internal interface IBeamForceDistributionCalculator
    {
        double Moment(double relativePosition);

        double Shear(double relativePosition);
    }
}