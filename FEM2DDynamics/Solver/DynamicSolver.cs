using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;
using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
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
        private readonly IDampingMatrixCalculator dampingCalculator;
        private readonly IEquationOfMotionSolver equationSolver;

        public DynamicSolver()
        {
            this.matrixAggregator = new DynamicMatrixAggregator();
            this.matrixReducer = new MatrixReducer();
            this.matrixSolver = new CholeskyDescomposition();
            this.dampingCalculator = new SimpleDampingMatrixCalculator();
            this.equationSolver = new DifferentialEquationMatrixSolver();
        }


        public void Solve(DynamicElementFactory elementFactory, NodeFactory nodeFactory, DynamicLoadFactory loadFactory)
        {
            var nodes = nodeFactory.GetAll();
            var elements = elementFactory.GetAll();

            var dofNumber = nodeFactory.GetDOFsCount();
            var stiffnessMatrix = matrixAggregator.AggregateStiffnessMatrix(elements, dofNumber);
            var massMatrix = matrixAggregator.AggregateMassMatrix(elements, dofNumber);
            var dampingMatrix = dampingCalculator.GetDampingMatrix(stiffnessMatrix, massMatrix);

            this.matrixReducer.Initialize(nodes, dofNumber);

            var matrixData = this.GetMatrixData(stiffnessMatrix, massMatrix, dampingMatrix);


            var displacements = this.equationSolver.Solve(matrixData, loadFactory, dofNumber,matrixReducer);
            //this.Results = new ResultFactory(displacements, nodeFactory, elementFactory, loadFactory);
        }

        private MatrixData GetMatrixData(Matrix<double> stiffnessMatrix, Matrix<double> massMatrix, Matrix<double> dampingMatrix)
        {
            var reducedStiffnessMatrix = this.matrixReducer.ReduceMatrix(stiffnessMatrix);
            var reducedMassMatrix = this.matrixReducer.ReduceMatrix(massMatrix);
            var reducedDampingMatrix = this.matrixReducer.ReduceMatrix(dampingMatrix);
            var matrixData = new MatrixData
            {
                DampingMatrix = reducedDampingMatrix,
                MassMatrix = reducedMassMatrix,
                StiffnessMatrix = reducedStiffnessMatrix
            };
            return matrixData;
        }
    }
}
