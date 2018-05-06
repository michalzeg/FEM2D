using FEM2DDynamics.Solver;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;

namespace FEM2DDynamicsTests.Solver
{
    [TestFixture]
    public class NaturalFrequencyCalculatorTests
    {
        const double tolerance = 0.0001;
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void NaturalFrequencyCalculator_GetFirstMode_ReturnsProperValue()
        {
            var matrices = GetMatrices();

            var naturalFrequencyCalculator = new NaturalFrequencyCalculator(matrices.massMatrix, matrices.stiffnessMatrix);

            var actualResult = naturalFrequencyCalculator.GetFirstMode()/2/Math.PI;

            var expectedResult = 5.7423;

            Assert.That(actualResult, Is.EqualTo(expectedResult).Within(tolerance));

        }

        [Test]
        public void NaturalFrequencyCalculator_GetSecondMode_ReturnsProperValue()
        {
            var matrices = GetMatrices();

            var naturalFrequencyCalculator = new NaturalFrequencyCalculator(matrices.massMatrix, matrices.stiffnessMatrix);

            var actualResult = naturalFrequencyCalculator.GetSecondMode() / 2 / Math.PI;

            var expectedResult = 13.8631;

            Assert.That(actualResult, Is.EqualTo(expectedResult).Within(tolerance));

        }

        [Test]
        public void NaturalFrequencyCalculator_GetPeriod_ReturnsProperValue()
        {
            var matrices = GetMatrices();

            var naturalFrequencyCalculator = new NaturalFrequencyCalculator(matrices.massMatrix, matrices.stiffnessMatrix);

            var actualResult = naturalFrequencyCalculator.GetPeriod();

            var expectedResult = 1/(5.7423);

            Assert.That(actualResult, Is.EqualTo(expectedResult).Within(tolerance));

        }

        private static (Matrix<double> massMatrix, Matrix<double> stiffnessMatrix) GetMatrices()
        {
            var E = 200 * Math.Pow(10, 9);
            var h = 3;
            var I = 2500 * Math.Pow(10, -8);
            var m1 = 2000;
            var m2 = 1000;
            var k = 24 * E * I / (h * h * h);

            var stiffnessMatrix = DenseMatrix.OfArray(new double[,]
            {
                {2*k, -k },
                {-k, k }
            });

            var massMatrix = DenseMatrix.OfArray(new double[,]
            {
                {m1, 0 },
                {0, m2 },
            });

            return (massMatrix, stiffnessMatrix);
        }
    }
}