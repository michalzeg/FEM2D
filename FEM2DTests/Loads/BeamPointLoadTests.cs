using FEM2D.Elements.Beam;
using FEM2D.Loads;
using FEM2D.Nodes;
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
        }

        [Test]
        public void BeamPointLoad_ThrowsErrorIfRelativePositionLessThanZero_Passed()
        {

            Action beamPointLoadCreator = ()=> new BeamPointLoad(this.beamElement,-1000,-1);

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

            var expectedNode1Load = -600d;
            var expectedNode2Load = -400d;

            var actualNode1Load = beamPointLoad.NodalLoads[0].ValueY;
            var actualNode2Load = beamPointLoad.NodalLoads[1].ValueY;
            Assert.Multiple(() =>
            {
                Assert.That(expectedNode1Load, Is.EqualTo(actualNode1Load).Within(1).Percent);
                Assert.That(expectedNode2Load, Is.EqualTo(actualNode2Load).Within(1).Percent);
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
