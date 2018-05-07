using FEM2D.Nodes;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

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

        private readonly MatrixData matrixData;

        public DynamicResultFactory Results { get; private set; }

        public DynamicSolver(DynamicSolverSettings settings, DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory)
        {
            this.matrixAggregator = new DynamicMatrixAggregator();
            this.matrixReducer = new MatrixReducer();
            this.equationSolver = new DifferentialEquationMatrixSolver(settings);
            this.settings = settings;
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
            this.loadFactory = loadFactory;

            this.matrixReducer.Initialize(nodeFactory);
            this.matrixData = new MatrixData(this.matrixReducer, this.matrixAggregator, this.elementFactory, this.nodeFactory.GetDOFsCount());
        }


        public void Solve()
        {
            var naturalFrequencies = new NaturalFrequencyCalculator(this.matrixData.MassMatrix, this.matrixData.StiffnessMatrix);
            var dampingFactors = new RayleightDamping(naturalFrequencies, settings.DampingRatio);
            this.CheckDeltaTime(naturalFrequencies);

            elementFactory.UpdateDampingFactor(dampingFactors);

            var displacements = this.equationSolver.Solve(matrixData, loadFactory, this.nodeFactory.GetDOFsCount(), matrixReducer);
            this.Results = new DynamicResultFactory(displacements, loadFactory);
        }

        private void CheckDeltaTime(NaturalFrequencyCalculator naturalFrequency)
        {
            var period = naturalFrequency.GetPeriod();
            if (this.settings.DeltaTime >= 0.01 * period)
                this.settings.DeltaTime = 0.01 * period;
        }

        
    }
}