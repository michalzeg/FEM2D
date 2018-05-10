using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;
using MathNet.Numerics.LinearAlgebra;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    internal class DynamicSolver
    {
        private readonly DynamicSolverSettings settings;
        private readonly DynamicElementFactory elementFactory;
        private readonly NodeFactory nodeFactory;
        private readonly DynamicLoadFactory loadFactory;

        private BlockingCollection<NodalForcePayload> nodalLoadPayloads;
        private BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads;
        private NodalForceProducer nodaLoadProducer;
        private AggregatedLoadProducer aggregatedLoadProducer;

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

            this.nodalLoadPayloads = new BlockingCollection<NodalForcePayload>();
            this.aggregatedLoadPayloads = new BlockingCollection<AggregatedLoadPayload>();
            this.nodaLoadProducer = new NodalForceProducer(this.loadFactory, timeProvider, nodalLoadPayloads);
            this.aggregatedLoadProducer = new AggregatedLoadProducer(this.aggregatedLoadPayloads, this.nodalLoadPayloads, loadAggregator, matrixReducer);
            elementFactory.UpdateDampingFactor(dampingCalculator);
            this.equationSolver = new DifferentialEquationMatrixSolver(timeProvider, matrixData,nodaLoadProducer,aggregatedLoadPayloads);
        }

        public DynamicResultFactory Solve()
        {
            var nodalLoadProducerTask = Task.Run(() => this.nodaLoadProducer.Execute());
            var aggregatedLoadProducerTask = Task.Run(() => this.aggregatedLoadProducer.Execute());
            var solverTask = Task.Run(() => this.equationSolver.Solve());
            
            Task.WaitAll(new[] { nodalLoadProducerTask, aggregatedLoadProducerTask, solverTask });

            var displacements = this.equationSolver.Result;
            return new DynamicResultFactory(displacements, loadFactory);
        }
    }
    internal class NodalForcePayload
    {
        public IEnumerable<NodalLoad> NodalLoads { get; set; }
        public double Time { get; set; }
    }

    internal class AggregatedLoadPayload
    {
        public Vector<double> AggregatedLoad { get; set; }
        public double Time { get; set; }
    }

    internal class AggregatedLoadProducer
    {
        private readonly ILoadAggregator loadAggregator;
        private readonly IMatrixReducer matrixReducer;

        public AggregatedLoadProducer(BlockingCollection<AggregatedLoadPayload> aggregatedLoadPayloads, BlockingCollection<NodalForcePayload> nodalForcePayloads, ILoadAggregator matrixAggregator, IMatrixReducer matrixReducer)
        {
            AggregatedLoadPayloads = aggregatedLoadPayloads;
            NodalLoadPayloads = nodalForcePayloads;
            this.loadAggregator = matrixAggregator;
            this.matrixReducer = matrixReducer;
        }

        public BlockingCollection<AggregatedLoadPayload> AggregatedLoadPayloads { get; }
        public BlockingCollection<NodalForcePayload> NodalLoadPayloads { get; }

        public void Execute()
        {
            do
            {
                var payload = this.NodalLoadPayloads.Take();
                var loads = payload.NodalLoads;

                var aggregatedLoad = this.loadAggregator.Aggregate(loads);
                var reducedLoad = this.matrixReducer.ReduceVector(aggregatedLoad);

                var result = new AggregatedLoadPayload
                {
                    AggregatedLoad = reducedLoad,
                    Time = payload.Time
                };
                this.AggregatedLoadPayloads.Add(result);
            }
            while (!this.NodalLoadPayloads.IsCompleted);
            this.AggregatedLoadPayloads.CompleteAdding();
        }

    }

    internal class NodalForceProducer
    {
        private readonly DynamicLoadFactory loadFactory;
        private readonly TimeProvider timeProvider;

        public NodalForceProducer(DynamicLoadFactory loadFactory, TimeProvider timeProvider, BlockingCollection<NodalForcePayload> nodalLoadsPayloads)
        {
            this.loadFactory = loadFactory;
            this.timeProvider = timeProvider;
            NodalLoadsPayloads = nodalLoadsPayloads;

        }

        public BlockingCollection<NodalForcePayload> NodalLoadsPayloads { get; }

        public void Execute()
        {
            do
            {
                var loads = this.loadFactory.GetNodalLoads(this.timeProvider.CurrentTime).ToList();

                var result = new NodalForcePayload
                {
                    NodalLoads = loads,
                    Time = this.timeProvider.CurrentTime
                };
                this.NodalLoadsPayloads.Add(result);
                this.timeProvider.Tick();
               
            } while (this.timeProvider.IsWorking());
            this.NodalLoadsPayloads.CompleteAdding();
        }

    }
}