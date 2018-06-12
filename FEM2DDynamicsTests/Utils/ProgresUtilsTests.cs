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
            var expectedResult = new ProgressMessage(1, 5);
            ProgressMessage actualResult = null;
            Action<ProgressMessage> progress = m => actualResult = m;

            ProgresUtils.ReportProgress(progress, 1, 5);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test()]
        public void ReportProgress_DoubleValueProvided_ReportHasNOTBeenCalled()
        {
            var expectedResult = new ProgressMessage(0.1, 5);
            ProgressMessage actualResult = null;
            Action<ProgressMessage> progress = m => actualResult = m;

            ProgresUtils.ReportProgress(progress, 0.1, 5);

            Assert.That(actualResult, Is.EqualTo(null));
        }
    }
}