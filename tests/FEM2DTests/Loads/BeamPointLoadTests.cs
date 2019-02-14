using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Loads.Beams;
using FEM2D.Nodes;
using FEM2D.Nodes.Dofs;
using MathNet.Numerics.LinearAlgebra.Double;
using NSubstitute;
using NUnit.Framework;
using System;

namespace FEM2DTests.Loads
{
    [TestFixture]
    public class BeamPointLoadTests
    {
        private IBeamElement beamElement;

        [SetUp]
        public void SetUp()
        {
            var dofCalculator = Substitute.For<IDofNumberCalculator>();

            this.beamElement = Substitute.For<IBeamElement>();
            this.beamElement.Length.Returns(10);
            this.beamElement.Nodes.Returns(new[]
            {
                new Node(0,0,1,dofCalculator),
                new Node(1,1,1,dofCalculator),
            });
            this.beamElement.GetTransformMatrix().Returns(DenseMatrix.CreateIdentity(6));
        }

        [Test]
        public void BeamPointLoad_ThrowsErrorIfRelativePositionLessThanZero_Passed()
        {
            Action beamPointLoadCreator = () => new BeamPointLoad(this.beamElement, -1000, -1);

            Assert.That(new TestDelegate(beamPointLoadCreator), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void BeamPointLoad_ThrowsErrorIfRelativePositionGreaterThanOne_Passed()
        {
            Action beamPointLoadCreator = () => new BeamPointLoad(this.beamElement, -1000, 1.01);

            Assert.That(new TestDelegate(beamPointLoadCreator), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void BeamPointLoad_ReturnsProperLoadAtNodes_Passed()
        {
            var beamPointLoad = this.CreateBeamPointLoad(0.4);

            var expectedNode1LoadY = -648d;
            var expectedNode2LoadY = -352d;

            var expectedNode1LoadM = -1440d;
            var expectedNode2LoadM = 960d;

            var actualNode1LoadY = beamPointLoad.NodalLoads[0].ValueY;
            var actualNode2LoadY = beamPointLoad.NodalLoads[1].ValueY;

            var actualNode1LoadM = beamPointLoad.NodalLoads[0].ValueM;
            var actualNode2LoadM = beamPointLoad.NodalLoads[1].ValueM;
            Assert.Multiple(() =>
            {
                Assert.That(expectedNode1LoadY, Is.EqualTo(actualNode1LoadY).Within(1).Percent);
                Assert.That(expectedNode2LoadY, Is.EqualTo(actualNode2LoadY).Within(1).Percent);

                Assert.That(expectedNode1LoadM, Is.EqualTo(actualNode1LoadM).Within(1).Percent);
                Assert.That(expectedNode2LoadM, Is.EqualTo(actualNode2LoadM).Within(1).Percent);
            });
        }

        private BeamPointLoad CreateBeamPointLoad(double relativePosition)
        {
            return new BeamPointLoad(
                this.beamElement,
                -1000,
                relativePosition);
        }
    }
}