using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamicTestApplication
{
    public class ResultData
    {
        public double MaxAbsoluteDisplacement { get; set; }
        public IEnumerable<TimeResult> TimeResults { get; set; }
    }

    public class TimeResult
    {
        public double Time { get; set; }
        public double MaxStress { get; set; }
        public double MinStress { get; set; }
        public IEnumerable<PositionResult> PositionResults { get; set; }
    }

    public class PositionResult
    {
        public double GlobalPosition { get; set; }
        public double TopStress { get; set; }
        public double BottomStress { get; set; }
        public double Displacement { get; set; }
    }
}
