using Common.Extensions;
using FEM2DDynamics.Solver;
using FEM2DDynamics.Utils;
using System;

namespace FEM2DDynamics.Time
{
    internal class ReportTimeProvider : ITimeProvider, ITimeData
    {
        private readonly ITimeProvider timeProvider;
        private readonly IProgress<ProgressMessage> progress;

        public ReportTimeProvider(ITimeProvider timeProvider, IProgress<ProgressMessage> progress = null)
        {
            this.timeProvider = timeProvider;
            this.progress = progress;
        }

        public double CurrentTime => this.timeProvider.CurrentTime;

        public double DeltaTime => this.timeProvider.DeltaTime;

        public double EndTime => this.timeProvider.EndTime;

        public bool IsWorking() => this.timeProvider.IsWorking();

        public void Tick()
        {
            this.timeProvider.Tick();
            this.progress?.ReportProgress(this.CurrentTime, this.timeProvider.EndTime);
        }
    }

    internal class TimeProvider : ITimeData, ITimeProvider
    {
        private const double periodToDeltaTimeFactor = 0.01;

        private readonly DynamicSolverSettings settings;
        private readonly INaturalFrequencyCalculator naturalFrequencyCalculator;
        private readonly IProgress<ProgressMessage> progress;

        public double DeltaTime { get; private set; }
        public double CurrentTime { get; private set; }
        public double EndTime => this.settings.EndTime;

        public TimeProvider(DynamicSolverSettings settings, INaturalFrequencyCalculator naturalFrequencyCalculator)
        {
            this.settings = settings;
            this.naturalFrequencyCalculator = naturalFrequencyCalculator;
            this.CurrentTime = settings.StartTime;
            this.CheckDeltaTime();
        }

        private void CheckDeltaTime()
        {
            var period = naturalFrequencyCalculator.GetPeriod();
            this.DeltaTime = Math.Min(periodToDeltaTimeFactor * period, this.settings.DeltaTime);
        }

        public void Tick()
        {
            this.CurrentTime += this.DeltaTime;
        }

        public bool IsWorking() => this.CurrentTime.IsApproximatelyLessOrEqualTo(this.settings.EndTime);
    }
}