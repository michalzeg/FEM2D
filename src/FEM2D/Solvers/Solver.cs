using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;

namespace FEM2D.Solvers
{
    public sealed class Solver
    {
        private IMatrixAggregator matrixAggregator;
        private ILoadAggregator loadAggregator;
        private IMatrixReducer matrixReducer;
        private IMatrixSolver matrixSolver;

        private readonly ElementFactory elementFactory;
        private readonly NodeFactory nodeFactory;
        private readonly LoadFactory loadFactory;

        public ResultFactory Results { get; private set; }

        public Solver(ElementFactory elementFactory, NodeFactory nodeFactory, LoadFactory loadFactory)
        {
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
            this.loadFactory = loadFactory;

            this.matrixAggregator = new MatrixAggregator();
            this.loadAggregator = new LoadAggregator(nodeFactory);
            this.matrixReducer = new MatrixReducer(nodeFactory);
            this.matrixSolver = new CholeskyDescomposition();
        }

        public void Solve()
        {
            var stiffnessMatrix = matrixAggregator.AggregateStiffnessMatrix(this.elementFactory);

            var loads = loadFactory.GetNodalLoads();
            var loadVector = loadAggregator.Aggregate(loads);

            var reducedStiffnessMatrix = this.matrixReducer.ReduceMatrix(stiffnessMatrix);
            var reducedLoadVector = this.matrixReducer.ReduceVector(loadVector);

            var displacements = this.matrixSolver.Solve(reducedStiffnessMatrix, reducedLoadVector);
            this.Results = new ResultFactory(displacements, nodeFactory, elementFactory, loadFactory);
        }
    }
}