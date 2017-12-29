using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Results.Beams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Results.Beam
{
    public class DynamicBeamElementResult : BeamElementResult
    {
        public double Time { get; }

        public DynamicBeamElementResult(double momentAtStart, double shearAtStart, IEnumerable<IBeamLoad> loads, IBeamElement element,IList<double> displacements, double time) 
            :base(momentAtStart,shearAtStart,loads,element,displacements)
        {
            this.Time = time;
        }
    }
}
