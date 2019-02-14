using FEM2D.Elements.Beam;
using System.Linq;
using FEM2DCommon.Extensions;

namespace FEM2D.Loads.Beams
{
    public abstract class BeamLoad : IBeamLoad
    {
        public NodalLoad[] NodalLoads { get; protected set; }
        public IBeamElement BeamElement { get; private set; }

        protected BeamLoad(IBeamElement beamElement)
        {
            this.BeamElement = beamElement;
        }

        protected NodalLoad[] TransformToGlobal(NodalLoad[] loads)
        {
            var vector = loads.Select(e => new[] { e.ValueX, e.ValueY, e.ValueM }).SelectMany(e => e).ToVector();
            var transformedLoad = this.BeamElement.GetTransformMatrix().Transpose() * vector;

            var result = transformedLoad.Partition(3)
                .Select(e => e.ToArray())
                .Select((e, i) => new NodalLoad(loads[i].Node, e[0], e[1], e[2]))
                .ToArray();
            return result;
        }

        public double[] GetEquivalenNodalForces()
        {
            var vector = new[]
                        {
                this.NodalLoads[0].ValueX,
                this.NodalLoads[0].ValueY,
                this.NodalLoads[0].ValueM,
                this.NodalLoads[1].ValueX,
                this.NodalLoads[1].ValueY,
                this.NodalLoads[1].ValueM,
            }.ToVector();
            var result = this.BeamElement.GetTransformMatrix() * vector;
            return result.AsArray();
        }
    }
}