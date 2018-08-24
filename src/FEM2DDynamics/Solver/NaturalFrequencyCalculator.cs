using Common.Extensions;
using FEM2DDynamics.Matrix;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FEM2DDynamics.Solver
{
    internal class NaturalFrequencyCalculator : INaturalFrequencyCalculator
    {
        private readonly Matrix<double> mass;
        private readonly Matrix<double> stiffness;

        private IList<double> naturalFrequency;

        public NaturalFrequencyCalculator(MatrixData matrixData)
            : this(matrixData.MassMatrix, matrixData.StiffnessMatrix)
        {
        }

        public NaturalFrequencyCalculator(Matrix<double> mass, Matrix<double> stiffness)
        {
            this.mass = mass;
            this.stiffness = stiffness;

            this.Calculate();
        }

        private void Calculate()
        {
            var eigen = (this.mass.Inverse() * this.stiffness).Evd();
            this.naturalFrequency = eigen.EigenValues
                                         .Where(e => !e.Real.IsApproximatelyEqualTo(1d))
                                         .OrderBy(e => e.Real)
                                         .Select(e => Math.Sqrt(e.Real))
                                         .ToList();
        }

        public double GetFirstMode()
        {
            return this.naturalFrequency[0];
        }

        public double GetSecondMode()
        {
            return this.naturalFrequency[1];
        }

        public double GetPeriod()
        {
            var result = 1 / (this.GetFirstMode() / 2 / Math.PI);
            return result;
        }
    }
}