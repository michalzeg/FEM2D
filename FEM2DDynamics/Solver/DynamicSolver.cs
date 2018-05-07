using FEM2D.Nodes;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using FEM2DDynamics.Time;
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
        private readonly TimeProvider timeProvider;
        private readonly INaturalFrequencyCalculator naturalFrequencyCalculator;

        private readonly MatrixData matrixData;

        public DynamicResultFactory Results { get; private set; }

        public DynamicSolver(DynamicSolverSettings settings, DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory)
        {
            this.settings = settings;
            this.elementFactory = elementFactory;
            this.nodeFactory = nodeFactory;
            this.loadFactory = loadFactory;

            
            this.matrixAggregator = new DynamicMatrixAggregator();
            this.matrixReducer = new MatrixReducer(nodeFactory);
            

            this.matrixData = new MatrixData(this.matrixReducer, this.matrixAggregator, this.elementFactory, this.nodeFactory.GetDOFsCount());
            this.naturalFrequencyCalculator = new NaturalFrequencyCalculator(this.matrixData);
            this.timeProvider = new TimeProvider(settings, naturalFrequencyCalculator);
            this.equationSolver = new DifferentialEquationMatrixSolver(this.timeProvider);
        }


        public void Solve()
        {
            
            var dampingFactors = new RayleightDamping(naturalFrequencyCalculator, settings.DampingRatio);

            elementFactory.UpdateDampingFactor(dampingFactors);

            var displacements = this.equationSolver.Solve(matrixData, loadFactory, this.nodeFactory.GetDOFsCount(), matrixReducer);
            this.Results = new DynamicResultFactory(displacements, loadFactory);
        }

        

        
    }
}