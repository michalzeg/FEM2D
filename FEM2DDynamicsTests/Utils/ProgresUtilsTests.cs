using NUnit.Framework;
using FEM2DDynamics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace FEM2DDynamics.Utils.Tests
{
    [TestFixture()]
    public class ProgresUtilsTests
    {
        [Test()]
        public void ReportProgress_IntegerValueProvided_ReportHasBeenCalled()
        {
            var msg = new ProgressMessage(1, 5);
            var progress = Substitute.For<IProgress<ProgressMessage>>();

            ProgresUtils.ReportProgress(progress, 1, 5);

            progress.Received().Report(msg);
        }

        [Test()]
        public void ReportProgress_DoubleValueProvided_ReportHasNOTBeenCalled()
        {
            var msg = new ProgressMessage(0.1, 5);
            var progress = Substitute.For<IProgress<ProgressMessage>>();

            ProgresUtils.ReportProgress(progress, 0.1, 5);

            progress.DidNotReceive().Report(msg);
        }
    }
}