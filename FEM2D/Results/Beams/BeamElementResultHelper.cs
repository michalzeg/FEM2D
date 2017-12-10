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
        internal static IEnumerable<BeamPointLoad> GetRelativePosition(this IEnumerable<BeamPointLoad> loads, double relativePosition)
        {
            return loads.Where(e => e.RelativePosition < relativePosition);
        }

        internal static double[] GetEquivalentNodalForces(this IEnumerable<IBeamLoad> loads)
        {
            var equivalentLoads = loads.Select(e => e.GetEquivalenNodalForces());

            double[] result = new double[6];
            foreach (var load in equivalentLoads)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] += load[i];
                }
            }
            return result;
        }
    }
}
