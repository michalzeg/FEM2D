using System.Collections.Generic;
using FEM2D.Loads;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Solvers
{
    public interface ILoadAggregator
    {
        Vector<double> Aggregate(IEnumerable<NodalLoad> loads, int dofNumber);
    }
}