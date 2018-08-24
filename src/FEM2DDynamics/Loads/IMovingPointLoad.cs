using Common.Geometry;

namespace FEM2DDynamics.Loads
{
    public interface IMovingPointLoad
    {
        double GetValueY(double time);

        PointD GetPosition(double time);
    }
}