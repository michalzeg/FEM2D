using FEM2D.Loads;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace FEM2D.Solvers
{
    public interface ILoadAggregator
    {
        Vector<double> Aggregate(IEnumerable<NodalLoad> loads);
    }
}