using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Utils
{
    public static class ProgresUtils
    {
        public static void ReportProgress(this Action<ProgressMessage> @this, double currentValue, double maxValue)
        {
            if (Math.Abs(currentValue % 1).IsApproximatelyEqualToZero())
                @this.Invoke(new ProgressMessage(currentValue, maxValue));
        }
    }
}