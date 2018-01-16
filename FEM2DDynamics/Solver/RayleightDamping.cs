using FEM2DCommon.ElementProperties;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    internal class RayleightDamping : IDampingFactors
    {
        private readonly NaturalFrequencyCalculator naturalFrequencyCalculator;
        private readonly DynamicBeamProperties dynamicBeamProperties;

        public double StiffnessDampingFactor { get; private set; }
        public double MassDampingFActor { get; private set; }

        public RayleightDamping(NaturalFrequencyCalculator naturalFrequencyCalculator, DynamicBeamProperties dynamicBeamProperties)
        {
            this.naturalFrequencyCalculator = naturalFrequencyCalculator;
            this.dynamicBeamProperties = dynamicBeamProperties;

            this.Initialize();
        }

        private void Initialize()
        {
            var ksiVector = GetKsiVector();
            var omegaMatrix = GetOmegaMatrix();

            var dampingFactors = 2 * omegaMatrix.Inverse() * ksiVector;
        }

        private Vector<double> GetKsiVector()
        {
            var ksi = this.dynamicBeamProperties.Damping;
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
