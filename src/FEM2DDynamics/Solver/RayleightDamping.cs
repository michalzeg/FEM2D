﻿using MathNet.Numerics.LinearAlgebra;

namespace FEM2DDynamics.Solver
{
    internal class RayleightDamping : IDampingFactorCalculator
    {
        private readonly INaturalFrequencyCalculator naturalFrequencyCalculator;
        private readonly double dampingRatio;

        public double StiffnessDampingFactor { get; private set; }
        public double MassDampingFactor { get; private set; }

        public RayleightDamping(INaturalFrequencyCalculator naturalFrequencyCalculator, double dampingRatio)
        {
            this.naturalFrequencyCalculator = naturalFrequencyCalculator;
            this.dampingRatio = dampingRatio;
            this.Initialize();
        }

        private void Initialize()
        {
            var ksiVector = GetKsiVector();
            var omegaMatrix = GetOmegaMatrix();

            var dampingFactors = 2 * omegaMatrix.Inverse() * ksiVector;
            this.MassDampingFactor = dampingFactors[0];
            this.StiffnessDampingFactor = dampingFactors[1];
        }

        private Vector<double> GetKsiVector()
        {
            var ksi = this.dampingRatio;
            var ksiVector = Vector<double>.Build.DenseOfArray(new[] { ksi, ksi });
            return ksiVector;
        }

        private Matrix<double> GetOmegaMatrix()
        {
            var omega1 = this.naturalFrequencyCalculator.GetFirstMode();
            var omega2 = this.naturalFrequencyCalculator.GetSecondMode();
            var omegaMatrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {1/omega1,omega1 },
                {1/omega2, omega2 }
            });
            return omegaMatrix;
        }
    }
}