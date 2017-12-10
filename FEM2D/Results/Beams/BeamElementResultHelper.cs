using FEM2D.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results.Beams
{
    internal static class BeamElementResultHelper
    {
        public static IEnumerable<BeamPointLoad> GetRelativePosition(this IEnumerable<BeamPointLoad> loads, double relativePosition)
        {
            return loads.Where(e => e.RelativePosition < relativePosition);
        }
    }
}
