using Common.DTO;
using Common.Point;
using FEM2D.Elements;
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
        public void Node_ReturnsMembraneDOFs()
        {
            var nodes = new NodeFactory();
            var elements = new ElementFactory();
            var properties = MembraneProperties.Default;

            var p1 = new PointD(10, 20);
            var p2 = new PointD(20, 20);
            var p3 = new PointD(20, 30);

            var node1 = nodes.Create(p1);
            var node2 = nodes.Create(p2);
            var node3 = nodes.Create(p3);

            var element = elements.CreateTriangle(node1, node2, node3, properties);

            var expectedDOFs1 = new[] { 0, 1 };
            var expectedDOFs2 = new[] { 2, 3 };
            var expectedDOFs3 = new[] { 4, 5 };

            var actualDOFs1 = node1.GetDOF();
            var actualDOFs2 = node2.GetDOF();
            var actualDOFs3 = node3.GetDOF();

            Assert.Multiple(() =>
            {
                Assert.That(actualDOFs1, Is.EquivalentTo(expectedDOFs1));
                Assert.That(actualDOFs2, Is.EquivalentTo(expectedDOFs2));
                Assert.That(actualDOFs3, Is.EquivalentTo(expectedDOFs3));
            });

        }

        [Test]
        [Ignore("To be implemented")]
        public void Node_ReturnsBeamDOFs()
        {
        }
    }
}
