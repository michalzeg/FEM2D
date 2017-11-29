using FEM2D.Loads;
using NSubstitute;
using NUnit.Framework;

namespace FEM2DTests.Loads
{
    [TestFixture]
    public class LoadFactoryTests
    {


        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        [Ignore("Not impelemted yet")]
        public void TestMethod1()
        {
            // Arrange


            // Act
            LoadFactory factory = this.CreateFactory();


            // Assert

        }

        private LoadFactory CreateFactory()
        {
            return new LoadFactory();
        }
    }
}
