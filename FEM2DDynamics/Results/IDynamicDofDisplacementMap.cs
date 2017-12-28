using System.Collections.Generic;

namespace FEM2DDynamics.Results
{
    internal interface IDynamicDofDisplacementMap
    {
        IEnumerable<double> GetAcceleration(IEnumerable<int> dofIndices, double time);
        double GetAcceleration(int dofIndex, double time);
        IEnumerable<double> GetDisplacement(IEnumerable<int> dofIndices, double time);
        double GetDisplacement(int dofIndex, double time);
        IEnumerable<double> GetVelocity(IEnumerable<int> dofIndices, double time);
        double GetVelocity(int dofIndex, double time);
    }
}