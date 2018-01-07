using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamicTestApplication
{
    public class ResultData
    {
        double MaxStress { get; set; }
        double MinStress { get; set; }
        double MaxAbsoluteDisplacement { get; set; }
        IEnumerable<TimeResult> TimeResults { get; set; }
    }

    public class TimeResult
    {
        double Time { get; set; }
        IEnumerable<PositionResult> PositionResults { get; set; }
    }

    public class PositionResult
    {
        double Position { get; set; }
        double TopStress { get; set; }
        double BottomStress { get; set; }
        double Displacement { get; set; }
    }
}
