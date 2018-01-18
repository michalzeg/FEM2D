using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    internal class DynamicSolver
    {
        private readonly IDynamicMatrixAggregator matrixAggregator;
        
        private readonly IMatrixReducer matrixReducer;
        private readonly IMatrixSolver matrixSolver;
        private readonly IEquationOfMotionSolver equationSolver;
        private readonly DynamicSolverSettings settings;


        public DynamicResultFactory Results { get; private set; }

        public DynamicSolver(DynamicSolverSettings settings)
        {
            this.matrixAggregator = new DynamicMatrixAggregator();
            this.matrixReducer = new MatrixReducer();
            this.matrixSolver = new CholeskyDescomposition();
            this.equationSolver = new DifferentialEquationMatrixSolver(settings);
            this.settings = settings;
        }


        public void Solve(DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory)
        {
            var nodes = nodeFactory.GetAll();
            var elements = elementFactory.GetAll();

            var dofNumber = nodeFactory.GetDOFsCount();
            this.matrixReducer.Initialize(nodes, dofNumber);
            var reducedStiffnessMatrix = GetStiffnessMatrix(elements, dofNumber);
            var reducedMassMatrix = GetMassMatrix(elements, dofNumber);

            var naturalFrequencies = new NaturalFrequencyCalculator(reducedMassMatrix, reducedStiffnessMatrix);
            var dampingFactors = new RayleightDamping(naturalFrequencies, settings.DampingRatio);
            this.CheckDeltaTime(naturalFrequencies);

            elementFactory.UpdateDampingFactor(dampingFactors);
            var reducedDampingMatrix = GetDampingMatrix(elements, dofNumber);

            var matrixData = this.GetMatrixData(reducedStiffnessMatrix, reducedMassMatrix, reducedDampingMatrix);


            var displacements = this.equationSolver.Solve(matrixData, loadFactory, dofNumber, matrixReducer);
            this.Results = new DynamicResultFactory(displacements, loadFactory);

        }

        private void CheckDeltaTime(NaturalFrequencyCalculator naturalFrequency)
        {
            var period = naturalFrequency.GetPeriod();
            if (this.settings.DeltaTime >= 0.1 * period)
                this.settings.DeltaTime = 0.1 * period;
        }

        private Matrix<double> GetDampingMatrix(IEnumerable<IDynamicElement> elements, int dofNumber)
        {
            var dampingMatrix = matrixAggregator.AggregateDampingMatrix(elements, dofNumber);
            var reducedDampingMatrix = matrixReducer.ReduceMatrix(dampingMatrix);
            return reducedDampingMatrix;
        }

        

        private Matrix<double> GetMassMatrix(IEnumerable<IDynamicElement> elements, int dofNumber)
        {
            var massMatrix = matrixAggregator.AggregateMassMatrix(elements, dofNumber);
            var reducedMassMatrix = this.matrixReducer.ReduceMatrix(massMatrix);
            return reducedMassMatrix;
        }

        private Matrix<double> GetStiffnessMatrix(IEnumerable<IDynamicElement> elements, int dofNumber)
        {
            var stiffnessMatrix = matrixAggregator.AggregateStiffnessMatrix(elements, dofNumber);
            var reducedStiffnessMatrix = this.matrixReducer.ReduceMatrix(stiffnessMatrix);
            return reducedStiffnessMatrix;
        }

        private MatrixData GetMatrixData(Matrix<double> stiffnessMatrix, Matrix<double> massMatrix, Matrix<double> dampingMatrix)
        {
            
            var matrixData = new MatrixData
            {
                DampingMatrix = dampingMatrix,
                MassMatrix = massMatrix,
                StiffnessMatrix = stiffnessMatrix
            };
            return matrixData;
        }
    }
}
