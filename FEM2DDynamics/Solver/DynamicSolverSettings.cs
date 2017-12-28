using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Solver
{
    public class DynamicSolverSettings
    {
        public double DeltaTime { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public static DynamicSolverSettings Default
        {
            get
            {
                return new DynamicSolverSettings
                {
                    DeltaTime = 0.01,
                    StartTime = 0,
                    EndTime = 100
                };
            }
        }
    }
}
