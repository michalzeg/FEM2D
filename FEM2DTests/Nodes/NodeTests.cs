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
        NodeFactory nodeFactory;
        ElementFactory elementFactory;
        MembraneProperties membraneProperties;
        BeamProperties beamProperties;

        PointD p1;
        PointD p2;
        PointD p3;

        [SetUp]
        public void Setup()
        {
            nodeFactory = new NodeFactory();
            elementFactory = new ElementFactory();
            membraneProperties = MembraneProperties.Default;
            beamProperties = BeamProperties.Default;

            p1 = new PointD(10, 20);
            p2 = new PointD(20, 20);
            p3 = new PointD(20, 30);
        }

        [Test]
        public void Node_ReturnsMembraneDOFs()
        {
            
            var node1 = nodeFactory.Create(p1);
            var node2 = nodeFactory.Create(p2);
            var node3 = nodeFactory.Create(p3);

            var element = elementFactory.CreateTriangle(node1, node2, node3, membraneProperties);

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

        [Test]
        [Ignore("To be implemented")]
        public void Node_ReturnsBeamAndMembraneDOFs()
        {
        }

        [Test]
        public void Node_ReturnsAppropriateNumbers()
        {
            var node1 = nodeFactory.Create(p1);
            var node2 = nodeFactory.Create(p2);
            var node3 = nodeFactory.Create(p3);

            Assert.Multiple(() =>
            {
                Assert.That(node1.Number, Is.EqualTo(1));
                Assert.That(node2.Number, Is.EqualTo(2));
                Assert.That(node3.Number, Is.EqualTo(3));
            });

            

        }

        [Test]
        public void Node_ReturnsDistanceToOtherNode()
        {
            var node1 = nodeFactory.Create(p1);
            var node3 = nodeFactory.Create(p3);

            var expectedDistance = 10d * Math.Sqrt(2);
            var actualDistance = node1.DistanceTo(node3);

            Assert.That(expectedDistance, Is.EqualTo(actualDistance).Within(1).Percent);
            
        }
    }
}
