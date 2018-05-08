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

        public ResultFactory Results { get; private set; }

        public Solver()
        {
            
        }

        public void Solve(ElementFactory elementFactory, NodeFactory nodeFactory, LoadFactory loadFactory)
        {
            this.matrixAggregator = new MatrixAggregator();
            this.loadAggregator = new LoadAggregator(nodeFactory);
            this.matrixReducer = new MatrixReducer();
            this.matrixSolver = new CholeskyDescomposition();

            var nodes = nodeFactory.GetAll();
            var elements = elementFactory.GetAll();
            var loads = loadFactory.GetNodalLoads();

            var dofNumber = nodeFactory.GetDOFsCount();
            var stiffnessMatrix = matrixAggregator.AggregateStiffnessMatrix(elements, dofNumber);
            var loadVector = loadAggregator.Aggregate(loads);

            this.matrixReducer.Initialize(nodes, dofNumber);
            var reducedStiffnessMatrix = this.matrixReducer.ReduceMatrix(stiffnessMatrix);
            var reducedLoadVector = this.matrixReducer.ReduceVector(loadVector);

            var displacements = this.matrixSolver.Solve(reducedStiffnessMatrix, reducedLoadVector);
            this.Results = new ResultFactory(displacements, nodeFactory, elementFactory, loadFactory);
        }
    }
}