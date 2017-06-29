using Common.Point;
using FEM2D.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DTests.Nodes
{
    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void Node_ReturnsDOFs()
        {
            var p = new PointD(10, 20);
            var node = new Node(p);
            var number = node.Number;

            var expectedDOFs = new[] { number * 2, number * 2 +1};
            var actualDOFs = node.GetDOF();

            Assert.That(actualDOFs, Is.EquivalentTo(expectedDOFs));
        }

    }
}
