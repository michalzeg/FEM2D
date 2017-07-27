using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;
using FEM2D.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Solvers
{
    public class Solver
    {
        private readonly MatrixAggregator matrixAggregator;
        private readonly LoadAggregator loadAggregator;
        private readonly BoundaryVector boundaryProvider;

        public ResultProvider Results { get; private set; }

        public Solver()
        {
            this.matrixAggregator = new MatrixAggregator();
            this.loadAggregator = new LoadAggregator();
            this.boundaryProvider = new BoundaryVector();
        }

        public void Solve(MembraneGeometry membraneGeometry)
        {
            this.Solve(membraneGeometry.Elements, membraneGeometry.Nodes, membraneGeometry.NodalLoads);
        }

        public void Solve(IEnumerable<ITriangleElement> elements, IEnumerable<Node> nodes, IEnumerable<NodalLoad> loads)
        {
            var dofNumber = nodes.Count() * 2;
            var stiffnessMatrix = matrixAggregator.Aggregate(elements, dofNumber);
            var loadVector = loadAggregator.Aggregate(loads, dofNumber);

            var reducedStiffnessMatrix = stiffnessMatrix.Clone();
            var reducedLoadVector = loadVector.Clone();
            this.boundaryProvider.CreateVector(nodes, dofNumber);
            this.boundaryProvider.Reduce(reducedStiffnessMatrix, reducedLoadVector);

            var cholesky = new CholeskyDescomposition();
            //var displacements2 = reducedStiffnessMatrix.Inverse() *reducedLoadVector;
            var displacements = cholesky.Solve(reducedStiffnessMatrix, reducedLoadVector);
            this.Results = new ResultProvider(displacements, nodes, elements);
        }
    }
}
