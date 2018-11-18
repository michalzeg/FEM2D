using FEM2D.Elements.Beam;

namespace FEM2D.Loads.Beams
{
    public interface IBeamLoad
    {
        IBeamElement BeamElement { get; }
        NodalLoad[] NodalLoads { get; }

        double[] GetEquivalenNodalForces();
    }
}