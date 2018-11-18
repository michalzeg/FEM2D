using FEM2D.Elements;
using FEM2D.Nodes;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using NUnit.Framework;

namespace FEM2DTests.Integration
{
    [TestFixture]
    public class GetDOFCount_Integration
    {
        [Test]
        public void GetDofCount_ElementAndNodeFactoryReturnsTheSameNumber()
        {
            var nodeFactory = new NodeFactory();
            var node1 = nodeFactory.Create(0, 0);
            var node2 = nodeFactory.Create(10, 0);
            var node3 = nodeFactory.Create(20, 0);

            var elementFactory = new ElementFactory();
            var element1 = elementFactory.CreateBeam(node1, node2, BeamProperties.Default);
            var element2 = elementFactory.CreateTriangle(node1, node2, node3, MembraneProperties.Default);
            var element3 = elementFactory.CreateBeam(node2, node3, BeamProperties.Default);

            var nodeDofCount = nodeFactory.GetDOFsCount();
            var elementDofCount = elementFactory.GetDOFsCount();

            Assert.That(nodeDofCount, Is.EqualTo(elementDofCount));
        }
    }
}