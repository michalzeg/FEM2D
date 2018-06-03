using Common.Extensions;
using FEM2DDynamics.Solver;
using FEM2DDynamics.Utils;
using System;

namespace FEM2DDynamics.Time
{
    internal class TimeProvider : ITimeData
    {
        private const double periodToDeltaTimeFactor = 0.01;

        private readonly DynamicSolverSettings settings;
        private readonly INaturalFrequencyCalculator naturalFrequencyCalculator;
        private readonly IProgress<ProgressMessage> progress;

        public double DeltaTime { get; private set; }
        public double CurrentTime { get; private set; }

        public TimeProvider(DynamicSolverSettings settings, INaturalFrequencyCalculator naturalFrequencyCalculator, IProgress<ProgressMessage> progress = null)
        {
            this.settings = settings;
            this.naturalFrequencyCalculator = naturalFrequencyCalculator;
            this.progress = progress;
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
            this.progress?.ReportProgress(this.CurrentTime, this.settings.EndTime);
        }

        public bool IsWorking() => this.CurrentTime.IsApproximatelyLessOrEqualTo(this.settings.EndTime);
    }
}