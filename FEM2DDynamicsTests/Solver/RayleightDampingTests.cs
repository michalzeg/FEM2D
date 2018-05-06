using FEM2DDynamics.Solver;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DDynamicsTests.Solver
{
    [TestFixture]
    public class RayleightDampingTests
    {
        const double damping = 0.2;

        private INaturalFrequencyCalculator subNaturalFrequencyCalculator;

        [SetUp]
        public void SetUp()
        {
            this.subNaturalFrequencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            this.subNaturalFrequencyCalculator.GetFirstMode().Returns(5);
            this.subNaturalFrequencyCalculator.GetSecondMode().Returns(15);
        }

        [Test]
        public void RayleightDamping_CalculateMassAndStiffnesFacors_ReturnsProperValues()
        {
            var rayleightDamping = this.CreateRayleightDamping();

            var expectedStiffnessFactor = 0.02;
            var expectedMassFactor = 1.5;

            Assert.Multiple(() =>
            {
                Assert.That(rayleightDamping.StiffnessDampingFactor, Is.EqualTo(expectedStiffnessFactor).Within(0.001));
                Assert.That(rayleightDamping.MassDampingFactor, Is.EqualTo(expectedMassFactor).Within(0.001));

            });

        }

        private RayleightDamping CreateRayleightDamping()
        {
            return new RayleightDamping(
                this.subNaturalFrequencyCalculator,
                damping);
        }
    }
}
