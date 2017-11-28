using FEM2D.Nodes;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DTests.Nodes.Dofs
{
    [TestFixture]
    public class DofTests
    {
        private IDofNumberCalculator subDofNumberCalculator;

        [SetUp]
        public void SetUp()
        {
            this.subDofNumberCalculator = Substitute.For<IDofNumberCalculator>();
        }

        [Test]
        [Ignore("To be impelemnted")]
        public void DofTests_()
        {
            // Arrange


            // Act
            Dof dof = this.CreateDof();


            // Assert

        }

        private Dof CreateDof()
        {
            return new Dof(
                this.subDofNumberCalculator);
        }
    }
}
