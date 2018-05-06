using FEM2D.Elements.Beam;

namespace FEM2D.Loads
{
    public interface IBeamLoad
    {
        IBeamElement BeamElement { get; }
        NodalLoad[] NodalLoads { get; }

        double[] GetEquivalenNodalForces();
    }
}