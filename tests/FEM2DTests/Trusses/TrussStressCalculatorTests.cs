using FEM2D.Results.Trusses;
using FEM2DCommon.Sections;
using FEM2DStressCalculator.Trusses;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DTests.Trusses
{
    [TestFixture]
    public class TrussStressCalculatorTests
    {
        [Test]
        public void NormalStress_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var properties = new SectionProperties
            {
                A = 2
            };
            var unitUnderTest = new TrussStressCalculator(properties);
            var forces = new TrussElementResult
            {
                NormalForce = 1000
            };

            // Act
            var result = unitUnderTest.NormalStress(forces);

            // Assert
            Assert.That(result, Is.EqualTo(500).Within(0.1));
        }
    }
}