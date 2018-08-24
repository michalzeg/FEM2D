using System.Collections.Generic;

namespace FEM2DDynamics.Results
{
    internal interface IDynamicDofDisplacementMap
    {
        TimeDisplacementPair GetDisplacements(IEnumerable<int> dofIndices, double time);
    }
}