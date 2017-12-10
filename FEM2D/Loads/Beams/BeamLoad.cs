using FEM2D.Elements.Beam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public double[] GetEquivalenNodalForces()
        {
            var result = new[]
                        {
                this.NodalLoads[0].ValueX,
                this.NodalLoads[0].ValueY,
                this.NodalLoads[0].ValueM,
                this.NodalLoads[1].ValueX,
                this.NodalLoads[1].ValueY,
                this.NodalLoads[1].ValueM,
            };
            return result;
        }

        

    }
}
