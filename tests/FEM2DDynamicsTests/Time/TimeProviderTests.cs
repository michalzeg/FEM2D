using FEM2DDynamics.Solver;
using FEM2DDynamics.Time;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DDynamicsTests.Time
{
    [TestFixture]
    public class TimeProviderTests
    {
        [Test]
        public void TimeProvider_DeltaTimeIsNotChangedByNaturalFrequencyCalculator_HasProperValue()
        {
            var deltaTime = 10;

            var settings = new DynamicSolverSettings
            {
                DeltaTime = deltaTime,
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(1000);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);

            Assert.That(timeProvider.DeltaTime, Is.EqualTo(deltaTime).Within(0.00001));
        }

        [Test]
        public void TimeProvider_DeltaTimeIsChangedByNaturalFrequencyCalculator_HasProperValue()
        {
            var deltaTime = 10;

            var settings = new DynamicSolverSettings
            {
                DeltaTime = deltaTime,
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(100);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);

            Assert.That(timeProvider.DeltaTime, Is.EqualTo(1).Within(0.00001));
        }

        [Test]
        public void TimeProvider_CurrentTimeAtStart_HasProperValue()
        {
            var time = 10;

            var settings = new DynamicSolverSettings
            {
                StartTime = time,
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(1000);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);

            Assert.That(timeProvider.CurrentTime, Is.EqualTo(time).Within(0.00001));
        }

        [Test]
        public void TimeProvider_CurrentTimeAfterTick_HasProperValue()
        {
            var time = 10;
            var deltaTime = 1;
            var settings = new DynamicSolverSettings
            {
                StartTime = time,
                DeltaTime = deltaTime
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(1000);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);
            timeProvider.Tick();
            Assert.That(timeProvider.CurrentTime, Is.EqualTo(11).Within(0.00001));
        }

        [Test]
        public void TimeProvider_IsWorking_BeforeEnd_ReturnsTrue()
        {
            var time = 10;
            var deltaTime = 1;
            var endTime = 12;
            var settings = new DynamicSolverSettings
            {
                StartTime = time,
                DeltaTime = deltaTime,
                EndTime = endTime,
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(1000);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);
            timeProvider.Tick();
            Assert.That(timeProvider.IsWorking(), Is.True);
        }

        [Test]
        public void TimeProvider_IsWorking_AtEnd_ReturnsTrue()
        {
            var time = 11;
            var deltaTime = 1;
            var endTime = 12;
            var settings = new DynamicSolverSettings
            {
                StartTime = time,
                DeltaTime = deltaTime,
                EndTime = endTime,
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(1000);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);
            timeProvider.Tick();
            Assert.That(timeProvider.IsWorking(), Is.True);
        }

        [Test]
        public void TimeProvider_IsWorking_AfterEnd_ReturnsFalse()
        {
            var time = 10;
            var deltaTime = 1;
            var endTime = 12;
            var settings = new DynamicSolverSettings
            {
                StartTime = time,
                DeltaTime = deltaTime,
                EndTime = endTime,
            };
            var naturalFrecuencyCalculator = Substitute.For<INaturalFrequencyCalculator>();
            naturalFrecuencyCalculator.GetPeriod().Returns(1000);

            var timeProvider = new TimeProvider(settings, naturalFrecuencyCalculator);
            timeProvider.Tick();
            timeProvider.Tick();
            timeProvider.Tick();
            Assert.That(timeProvider.IsWorking(), Is.False);
        }
    }
}