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
        private readonly IMatrixAggregator matrixAggregator;
        private readonly ILoadAggregator loadAggregator;
        private readonly IBoundaryProvider boundaryProvider;
        private readonly IMatrixSolver matrixSolver;

        public ResultFactory Results { get; private set; }

        public Solver()
        {
            this.matrixAggregator = new MatrixAggregator();
            this.loadAggregator = new LoadAggregator();
            this.boundaryProvider = new BoundaryProvider();
            this.matrixSolver = new CholeskyDescomposition();
        }


        public void Solve(ElementFactory elementFactory, NodeFactory nodeFactory, IEnumerable<NodalLoad> loads)
        {
            var nodes = nodeFactory.GetAll();
            var elements = elementFactory.GetAll();

            var dofNumber = nodeFactory.GetDOFsCount();
            var stiffnessMatrix = matrixAggregator.Aggregate(elements, dofNumber);
            var loadVector = loadAggregator.Aggregate(loads, dofNumber);

            var reducedStiffnessMatrix = stiffnessMatrix.Clone();
            var reducedLoadVector = loadVector.Clone();
            this.boundaryProvider.CreateVector(nodes, dofNumber);
            this.boundaryProvider.Reduce(reducedStiffnessMatrix, reducedLoadVector);

            var displacements = this.matrixSolver.Solve(reducedStiffnessMatrix, reducedLoadVector);
            this.Results = new ResultFactory(displacements, nodes, elements);
        }
    }
}
