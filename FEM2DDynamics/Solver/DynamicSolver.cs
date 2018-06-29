using FEM2D.Nodes;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;
using FEM2DDynamics.Utils;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    internal class DynamicSolver
    {
        private readonly DynamicSolverSettings settings;
        private readonly DynamicElementFactory elementFactory;
        private readonly NodeFactory nodeFactory;
        private readonly DynamicLoadFactory loadFactory;
        private readonly Action<ProgressMessage> progress;
        private BlockingCollection<NodalForcePayload> nodalLoadPayloads;
        private BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads;
        private NodalForceProducer nodaLoadProducer;
        private AggregatedLoadProducer aggregatedLoadProducer;

        private IEquationOfMotionSolver equationSolver;

        public DynamicSolver(DynamicSolverSettings settings, DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory, Action<ProgressMessage> progress)
        {
            this.settings = settings;
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
            this.loadFactory = loadFactory;
            this.progress = progress;
        }

        public void Initialize()
        {
            var matrixAggregator = new DynamicMatrixAggregator();
            var matrixReducer = new MatrixReducer(this.nodeFactory);
            var loadAggregator = new LoadAggregator(this.nodeFactory);
            var matrixData = new MatrixData(matrixReducer, matrixAggregator, this.elementFactory);
            var naturalFrequencyCalculator = new NaturalFrequencyCalculator(matrixData);
            var timeProvider = new ReportTimeProvider(new TimeProvider(this.settings, naturalFrequencyCalculator), this.progress);
            var dampingCalculator = new RayleightDamping(naturalFrequencyCalculator, settings.DampingRatio);

            this.nodalLoadPayloads = new BlockingCollection<NodalForcePayload>();
            this.aggregatedLoadPayloads = new BlockingCollection<AggregatedLoadPayload>();
            this.nodaLoadProducer = new NodalForceProducer(this.loadFactory, timeProvider, nodalLoadPayloads);
            this.aggregatedLoadProducer = new AggregatedLoadProducer(this.aggregatedLoadPayloads, this.nodalLoadPayloads, loadAggregator, matrixReducer);

            this.equationSolver = new DifferentialEquationMatrixSolver(timeProvider, matrixData, aggregatedLoadPayloads);
            elementFactory.UpdateDampingFactor(dampingCalculator);
        }

        public DynamicResultFactory Solve()
        {
            var nodalLoadProducerTask = Task.Run(() => this.nodaLoadProducer.Execute());
            var aggregatedLoadProducerTask = Task.Run(() => this.aggregatedLoadProducer.Execute());
            var solverTask = Task.Run(() => this.equationSolver.Solve());

            Task.WaitAll(new[] { nodalLoadProducerTask, aggregatedLoadProducerTask, solverTask });

            var displacements = this.equationSolver.Result;
            this.Dispose();
            return new DynamicResultFactory(displacements, loadFactory);
        }

        private void Dispose()
        {
            try
            {
                this.aggregatedLoadPayloads.Dispose();
                this.nodalLoadPayloads.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}