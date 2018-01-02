using Common.Forces;
using Common.Sections;
using FEM2DStressCalculator.Beams;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DTests.Beams
{
    [TestFixture]
    public class BeamStressCalculatorTests
    {
        private SectionProperties subSectionProperties;

        [SetUp]
        public void SetUp()
        {
            this.subSectionProperties = new SectionProperties()
            {
                A = 2d,
                Ix0 = 100d,
                DY0_max = 2d,
                DY0_min = 0.5,
            };
            
        }

        [Test]
        public void BeamStressCalculator_PositiveMoment()
        {
            // Arrange
            var forces = new BeamForces
            {
                Axial = 1000,
                Moment = 2000,
            };

            // Act
            BeamStressCalculator beamStressCalculator = this.CreateBeamStressCalculator();

            var expectedTopStress = 540;
            var expectedBottomStress = 490;

            // Assert
            var actualTopStress = beamStressCalculator.TopNormalStress(forces);
            var actualBottomStress = beamStressCalculator.BottomNormalStress(forces);

            Assert.Multiple(() => {
                Assert.That(expectedTopStress, Is.EqualTo(actualTopStress).Within(1).Percent);
                Assert.That(expectedBottomStress, Is.EqualTo(actualBottomStress).Within(1).Percent);
            });

        }

        [Test]
        public void BeamStressCalculator_NegativeMoment()
        {
            // Arrange
            var forces = new BeamForces
            {
                Axial = 1000,
                Moment = -2000,
            };

            // Act
            BeamStressCalculator beamStressCalculator = this.CreateBeamStressCalculator();

            var expectedTopStress = 460;
            var expectedBottomStress = 510;

            // Assert
            var actualTopStress = beamStressCalculator.TopNormalStress(forces);
            var actualBottomStress = beamStressCalculator.BottomNormalStress(forces);

            Assert.Multiple(() => {
                Assert.That(expectedTopStress, Is.EqualTo(actualTopStress).Within(1).Percent);
                Assert.That(expectedBottomStress, Is.EqualTo(actualBottomStress).Within(1).Percent);
            });

        }

        private BeamStressCalculator CreateBeamStressCalculator()
        {
            return new BeamStressCalculator(
                this.subSectionProperties);
        }
    }
}
