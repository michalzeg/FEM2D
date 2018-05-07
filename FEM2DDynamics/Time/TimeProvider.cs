using FEM2DDynamics.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Time
{
    internal class TimeProvider
    {
        const double periodToDeltaTimeFactor = 0.01;

        private readonly DynamicSolverSettings settings;
        private readonly INaturalFrequencyCalculator naturalFrequencyCalculator;

        public double DeltaTime { get; private set; }
        public double CurrentTime { get; private set; }

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

        public bool IsWorking() => this.CurrentTime <= this.settings.EndTime;

    }
}
