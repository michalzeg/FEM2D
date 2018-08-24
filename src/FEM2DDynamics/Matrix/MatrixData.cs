using FEM2D.Solvers;
using FEM2DDynamics.Elements;
using FEM2DDynamics.Solver;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace FEM2DDynamics.Matrix
{
    internal class MatrixData
    {
        private readonly Lazy<Matrix<double>> massMatrix;
        private readonly Lazy<Matrix<double>> stiffnessMatrix;
        private readonly Lazy<Matrix<double>> dampingMatrix;
        private readonly IMatrixReducer matrixReducer;
        private readonly IDynamicMatrixAggregator matrixAggregator;
        private readonly DynamicElementFactory elementFactory;

        public Matrix<double> MassMatrix => this.massMatrix.Value;
        public Matrix<double> StiffnessMatrix => this.stiffnessMatrix.Value;
        public Matrix<double> DampingMatrix => this.dampingMatrix.Value;

        public MatrixData(IMatrixReducer matrixReducer, IDynamicMatrixAggregator matrixAggregator, DynamicElementFactory elementFactory)
        {
            this.massMatrix = new Lazy<Matrix<double>>(this.GetMassMatrix);
            this.stiffnessMatrix = new Lazy<Matrix<double>>(this.GetStiffnessMatrix);
            this.dampingMatrix = new Lazy<Matrix<double>>(this.GetDampingMatrix);

            this.matrixReducer = matrixReducer;
            this.matrixAggregator = matrixAggregator;
            this.elementFactory = elementFactory;
        }

        private Matrix<double> GetDampingMatrix()
        {
            var dampingMatrix = matrixAggregator.AggregateDampingMatrix(this.elementFactory);
            var reducedDampingMatrix = matrixReducer.ReduceMatrix(dampingMatrix);
            return reducedDampingMatrix;
        }

        private Matrix<double> GetMassMatrix()
        {
            var massMatrix = matrixAggregator.AggregateMassMatrix(this.elementFactory);
            var reducedMassMatrix = this.matrixReducer.ReduceMatrix(massMatrix);
            return reducedMassMatrix;
        }

        private Matrix<double> GetStiffnessMatrix()
        {
            var stiffnessMatrix = matrixAggregator.AggregateStiffnessMatrix(this.elementFactory);
            var reducedStiffnessMatrix = this.matrixReducer.ReduceMatrix(stiffnessMatrix);
            return reducedStiffnessMatrix;
        }
    }
}