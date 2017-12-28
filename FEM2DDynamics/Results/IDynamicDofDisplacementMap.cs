using System.Collections.Generic;

namespace FEM2DDynamics.Results
{
    internal interface IDynamicDofDisplacementMap
    {
        IEnumerable<double> GetValue(IEnumerable<int> dofIndices, double time);
        double GetValue(int dofIndex, double time);
    }
}