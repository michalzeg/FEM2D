using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Solvers
{
    public class Solver
    {
        private readonly MatrixAggregator matrixAggregator;
        private readonly LoadAggregator loadAggregator;
        private readonly BoundaryProvider boundaryProvider;

        public Solver()
        {
            this.matrixAggregator = new MatrixAggregator();
            this.loadAggregator = new LoadAggregator();
            this.boundaryProvider = new BoundaryProvider();
        }

        public void Solve(IEnumerable<ITriangleElement> elements, IEnumerable<Node> nodes, IEnumerable<NodalLoad> loads)
        {
            var dofNumber = nodes.Count() * 2;
            var stiffnessMatrix = matrixAggregator.Aggregate(elements, dofNumber);
            var loadVector = loadAggregator.Aggregate(loads, dofNumber);
            this.boundaryProvider.Cross(stiffnessMatrix, loadVector, nodes);

            var displacements = stiffnessMatrix.Inverse() * loadVector;
        }
    }
}
