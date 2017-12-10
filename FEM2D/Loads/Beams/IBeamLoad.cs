using FEM2D.Elements.Beam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Loads
{

    public interface IBeamLoad
    {
        IBeamElement BeamElement { get; }
        NodalLoad[] NodalLoads { get; }
        double[] GetEquivalenNodalForces();
    }
}
