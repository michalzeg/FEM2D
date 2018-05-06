using FEM2D.Loads;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;

namespace FEM2D.Solvers
{
    public class LoadAggregator : ILoadAggregator
    {
        public Vector<double> Aggregate(IEnumerable<NodalLoad> loads, int dofNumber)
        {
            var aggregatedLoad = Vector.Build.Sparse(dofNumber, 0d);

            foreach (var load in loads)
            {
                var dofs = load.Node.GetDOF();
                var x = dofs[0];
                var y = dofs[1];
                var r = load.Node.TryGetRotationDOF();
                aggregatedLoad[x] += load.ValueX;
                aggregatedLoad[y] += load.ValueY;
                if (r != -1)
                    aggregatedLoad[r] += load.ValueM;
            }

            return aggregatedLoad;
        }
    }
}