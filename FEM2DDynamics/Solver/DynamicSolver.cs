﻿using FEM2D.Nodes;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;
using System.Collections;
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
            
            this.equationSolver = new DifferentialEquationMatrixSolver(timeProvider, matrixData,aggregatedLoadPayloads);
            elementFactory.UpdateDampingFactor(dampingCalculator);
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
}