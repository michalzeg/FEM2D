using FEM2D.Nodes;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;

namespace FEM2DDynamics.Solver
{
    internal class DynamicSolver
    {
        private readonly DynamicSolverSettings settings;
        private readonly DynamicElementFactory elementFactory;
        private readonly NodeFactory nodeFactory;
        private readonly DynamicLoadFactory loadFactory;

        private IEquationOfMotionSolver equationSolver;

        public DynamicSolver(DynamicSolverSettings settings, DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory)
        {
            this.settings = settings;
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
            this.loadFactory = loadFactory;
        }

        public void Initialize()
        {
            var matrixAggregator = new DynamicMatrixAggregator();
            var matrixReducer = new MatrixReducer(this.nodeFactory);
            var loadAggregator = new LoadAggregator(this.nodeFactory);
            var matrixData = new MatrixData(matrixReducer, matrixAggregator, this.elementFactory);
            var naturalFrequencyCalculator = new NaturalFrequencyCalculator(matrixData);
            var timeProvider = new TimeProvider(this.settings, naturalFrequencyCalculator);
            var dampingCalculator = new RayleightDamping(naturalFrequencyCalculator, settings.DampingRatio);

            elementFactory.UpdateDampingFactor(dampingCalculator);
            this.equationSolver = new DifferentialEquationMatrixSolver(timeProvider, loadAggregator, matrixReducer, matrixData, loadFactory);
        }

        public DynamicResultFactory Solve()
        {
            var displacements = this.equationSolver.Solve();
            return new DynamicResultFactory(displacements, loadFactory);
        }
    }
}