using FEM2D.Elements.Beam;
using FEM2D.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Beams
{
    public class BeamElementResult
    {
        private readonly IEnumerable<IBeamLoad> beamLoads;
        private readonly IBeamElement element;
        private readonly IEnumerable<BeamPointLoad> pointLoads;
        private readonly double momentAtStart;
        private readonly double shearAtStart;
       

        internal BeamElementResult(double momentAtStart, double shearAtStart, IEnumerable<IBeamLoad> loads,IBeamElement element)
        {
            this.momentAtStart = momentAtStart;
            this.shearAtStart = shearAtStart;
            this.beamLoads = loads;
            this.element = element;
            this.pointLoads = loads.OfType<BeamPointLoad>().Cast<BeamPointLoad>().ToList();
        }

        public double Moment(double relativePosition)
        {

            var result = this.momentAtStart
                       - this.momentFromPointLoads(relativePosition)
                       - this.momentFromShearAtStart(relativePosition);
            return result;
        }

        public double Shear(double relativePosition)
        {

            var result = shearAtStart
                         + this.shearFromPointLoads(relativePosition);
            return result;             
        }

        private double momentFromPointLoads(double relativePosition)
        {
            var result = this.pointLoads
                             .Where(e => e.RelativePosition < relativePosition)
                             .Sum(e => (relativePosition - e.RelativePosition) * e.BeamElement.Length * e.ValueY);
            return result;
        }
        private double momentFromShearAtStart(double relativePosition)
        {
            return relativePosition * this.shearAtStart * this.element.Length;
        }

        private double shearFromPointLoads(double relativePosition)
        {
            var result = this.pointLoads
                            .Where(e => e.RelativePosition < relativePosition)
                            .Sum(e => e.ValueY);
            return result;
        }

        
    }
}
