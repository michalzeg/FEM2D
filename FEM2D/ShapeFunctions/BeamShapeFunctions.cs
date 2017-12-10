using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.ShapeFunctions
{
    public class BeamShapeFunctions
    {

        public double N1(double relativePosition, double length)
        {
            var x = relativePosition;
            var L = length;
            return 1 - x / L;
        }

        public double N2(double relativePosition, double length)
        {
            var x = relativePosition;
            var L = length;

            return 1 - 3 * x * x / (L * L) + 2 * x * x * x / (L * L * L);
        }

        public double N3(double relativePosition, double length)
        {
            var x = relativePosition;
            var L = length;

            return x * (1 - x / L) * (1 - x / L);
        }

        public double N4(double relativePosition, double length)
        {
            var x = relativePosition;
            var L = length;

            return x / L;
        }

        public double N5(double relativePosition, double length)
        {
            var x = relativePosition;
            var L = length;

            return 3 * x * x / (L * L) - 2 * x * x * x / (L * L * L);
        }

        public double N6(double relativePosition, double length)
        {
            var x = relativePosition;
            var L = length;

            return x * x / L * (x / L - 1);
        }
    }
}
