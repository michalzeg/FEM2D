using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Results.Beams.ForceDistributionCalculators;
using FEM2D.ShapeFunctions;
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
        private readonly IList<double> displacements;
        private readonly IBeamForceDistributionCalculator distributionCalculator;
        private readonly double momentAtStart;
        private readonly double shearAtStart;
       

        internal BeamElementResult(double momentAtStart, double shearAtStart, IEnumerable<IBeamLoad> loads,IBeamElement element, IList<double> displacements)
        {
            this.momentAtStart = momentAtStart;
            this.shearAtStart = shearAtStart;
            this.beamLoads = loads;
            this.element = element;
            this.displacements = displacements;
            this.distributionCalculator = new BeamForceDistributionCalculator(loads);
        }

        public double Moment(double relativePosition)
        {

            var result = this.momentAtStart
                       - this.distributionCalculator.Moment(relativePosition)
                       - this.MomentFromShearAtStart(relativePosition);
            return result;
        }

        public double Shear(double relativePosition)
        {

            var result = shearAtStart
                         + this.distributionCalculator.Shear(relativePosition);
            return result;             
        }

        public double GetDisplacement(double relativePosition)
        {
            var position = relativePosition * this.element.Length;


            var u1 = this.displacements[0];
            var u2 = this.displacements[1];
            var u3 = this.displacements[2];
            var u4 = this.displacements[3];
            var u5 = this.displacements[4];
            var u6 = this.displacements[5]; 

            var result = u1 * BeamShapeFunctions.N1(position, element.Length)
                + u2 * BeamShapeFunctions.N2(position, element.Length)
                + u3 * BeamShapeFunctions.N3(position, element.Length)
                + u4 * BeamShapeFunctions.N4(position, element.Length)
                + u5 * BeamShapeFunctions.N5(position, element.Length)
                + u6 * BeamShapeFunctions.N6(position, element.Length);
            return result;
        }

        private double MomentFromShearAtStart(double relativePosition)
        {
            return relativePosition * this.shearAtStart * this.element.Length;
        }

        

        
    }
}
