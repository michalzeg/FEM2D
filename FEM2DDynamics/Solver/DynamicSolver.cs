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

        private BlockingCollection<Payload> payloads;
        private LoadPositionProducer producer;

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

            this.payloads = new BlockingCollection<Payload>();
            this.producer = new LoadPositionProducer(this.loadFactory, timeProvider, payloads,loadAggregator,matrixReducer);

            elementFactory.UpdateDampingFactor(dampingCalculator);
            this.equationSolver = new DifferentialEquationMatrixSolver(timeProvider, loadAggregator, matrixReducer, matrixData,producer,payloads);
        }

        public DynamicResultFactory Solve()
        {
            var producerTask = Task.Run(() => this.producer.Execute());
            var solverTask = Task.Run(() => this.equationSolver.Solve());

            Task.WaitAll(new[] { producerTask, solverTask });

            var displacements = this.equationSolver.Result;
            return new DynamicResultFactory(displacements, loadFactory);
        }
    }
    internal class Payload
    {
        public Vector<double> AggregatedLoad { get; set; }
        public double Time { get; set; }
    }

    internal class LoadPositionProducer
    {
        private readonly DynamicLoadFactory loadFactory;
        private readonly TimeProvider timeProvider;
        private readonly ILoadAggregator loadAggregator;
        private readonly IMatrixReducer matrixReducer;

        public LoadPositionProducer(DynamicLoadFactory loadFactory, TimeProvider timeProvider, BlockingCollection<Payload> payloads,ILoadAggregator matrixAggregator, IMatrixReducer matrixReducer)
        {
            this.loadFactory = loadFactory;
            this.timeProvider = timeProvider;
            Payloads = payloads;
            this.loadAggregator = matrixAggregator;
            this.matrixReducer = matrixReducer;
        }

        public BlockingCollection<Payload> Payloads { get; }

        public void Execute()
        {
            do
            {
                var loads = this.loadFactory.GetNodalLoads(this.timeProvider.CurrentTime).ToList();

                var aggregatedLoad = this.loadAggregator.Aggregate(loads);
                var reducedLoad = this.matrixReducer.ReduceVector(aggregatedLoad);

                var result = new Payload
                {
                    AggregatedLoad = reducedLoad,
                    Time = this.timeProvider.CurrentTime
            };
                this.Payloads.Add(result);
                this.timeProvider.Tick();
               
            } while (this.timeProvider.IsWorking());
            this.Payloads.CompleteAdding();
        }

    }
}