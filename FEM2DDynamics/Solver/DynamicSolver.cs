﻿using FEM2D.Nodes;
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
        private readonly IDynamicMatrixAggregator matrixAggregator;

        private readonly IMatrixReducer matrixReducer;
        private readonly IEquationOfMotionSolver equationSolver;
        private readonly DynamicSolverSettings settings;
        private readonly DynamicElementFactory elementFactory;
        private readonly NodeFactory nodeFactory;
        private readonly DynamicLoadFactory loadFactory;
        private readonly TimeProvider timeProvider;
        private readonly INaturalFrequencyCalculator naturalFrequencyCalculator;
        private readonly IDampingFactorCalculator dampingCalulator;
        private readonly MatrixData matrixData;
        private readonly ILoadAggregator loadAggregator;
        public DynamicResultFactory Results { get; private set; }

        public DynamicSolver(DynamicSolverSettings settings, DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory)
        {
            this.settings = settings;
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
            this.loadFactory = loadFactory;

            this.matrixAggregator = new DynamicMatrixAggregator();
            this.matrixReducer = new MatrixReducer(nodeFactory);
            this.loadAggregator = new LoadAggregator(nodeFactory);

            this.matrixData = new MatrixData(this.matrixReducer, this.matrixAggregator, this.elementFactory);
            this.naturalFrequencyCalculator = new NaturalFrequencyCalculator(this.matrixData);
            this.timeProvider = new TimeProvider(settings, naturalFrequencyCalculator);
            this.equationSolver = new DifferentialEquationMatrixSolver(this.timeProvider,loadAggregator,matrixReducer);
            this.dampingCalulator = new RayleightDamping(naturalFrequencyCalculator, settings.DampingRatio);
        }

        public void Solve()
        {
            elementFactory.UpdateDampingFactor(this.dampingCalulator);

            var displacements = this.equationSolver.Solve(matrixData, loadFactory);
            this.Results = new DynamicResultFactory(displacements, loadFactory);
        }
    }
}