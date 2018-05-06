namespace FEM2DDynamics.Solver
{
    public class DynamicSolverSettings
    {
        public double DeltaTime { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double DampingRatio { get; set; }

        public static DynamicSolverSettings Default
        {
            get
            {
                return new DynamicSolverSettings
                {
                    DeltaTime = 0.01,
                    StartTime = 0,
                    EndTime = 100,
                    DampingRatio = 0.03,
                };
            }
        }
    }
}