using Common.DTO;
using FEM2D.Structures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DTests.Integration
{

    [TestFixture]
    public class Beam_Integration
    {
        

        [OneTimeSetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Beam_Integration_PointForceInMiddle()
        {

            var properties = new BeamProperties
            {
                Area = 0.2,
                ModulusOfElasticity = 200000,
                MomentOfInertia = 1,
            };

            var structure = new Structure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2,properties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, properties);
            structure.LoadFactory.AddNodalLoad(node2, 0, -1000);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1);
            var beam2Result = results.GetResult(beam2);


            Assert.Multiple(() =>
            {
                Assert.That(0,    Is.EqualTo(beam1Result.Moment(0)).Within(0.1));
                Assert.That(-2500, Is.EqualTo(beam1Result.Moment(0.5)).Within(0.1));
                Assert.That(-5000, Is.EqualTo(beam1Result.Moment(1)).Within(0.1));

                Assert.That(-5000, Is.EqualTo(beam2Result.Moment(0)).Within(0.1));
                Assert.That(-2500, Is.EqualTo(beam2Result.Moment(0.5)).Within(0.1));
                Assert.That(0, Is.EqualTo(beam2Result.Moment(1)).Within(0.1));

                Assert.That(500, Is.EqualTo(beam1Result.Shear(0)).Within(0.1));
                Assert.That(500, Is.EqualTo(beam1Result.Shear(0.5)).Within(0.1));
                Assert.That(500, Is.EqualTo(beam1Result.Shear(1)).Within(0.1));

                Assert.That(-500, Is.EqualTo(beam2Result.Shear(0)).Within(0.1));
                Assert.That(-500, Is.EqualTo(beam2Result.Shear(0.5)).Within(0.1));
                Assert.That(-500, Is.EqualTo(beam2Result.Shear(1)).Within(0.1));
            });
        }

        [Test]
        public void Beam_Integration_PointMomentInMiddle()
        {

            var properties = new BeamProperties
            {
                Area = 0.2,
                ModulusOfElasticity = 200000,
                MomentOfInertia = 1,
            };

            var structure = new Structure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, properties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, properties);
            structure.LoadFactory.AddNodalLoad(node2, 0, -1000);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1);
            var beam2Result = results.GetResult(beam2);

            Assert.Fail();

            Assert.Multiple(() =>
            {
                Assert.That(0, Is.EqualTo(beam1Result.Moment(0)).Within(0.1));
                Assert.That(-2500, Is.EqualTo(beam1Result.Moment(0.5)).Within(0.1));
                Assert.That(-5000, Is.EqualTo(beam1Result.Moment(1)).Within(0.1));

                Assert.That(-5000, Is.EqualTo(beam2Result.Moment(0)).Within(0.1));
                Assert.That(-2500, Is.EqualTo(beam2Result.Moment(0.5)).Within(0.1));
                Assert.That(0, Is.EqualTo(beam2Result.Moment(1)).Within(0.1));

                Assert.That(500, Is.EqualTo(beam1Result.Shear(0)).Within(0.1));
                Assert.That(500, Is.EqualTo(beam1Result.Shear(0.5)).Within(0.1));
                Assert.That(500, Is.EqualTo(beam1Result.Shear(1)).Within(0.1));

                Assert.That(-500, Is.EqualTo(beam2Result.Shear(0)).Within(0.1));
                Assert.That(-500, Is.EqualTo(beam2Result.Shear(0.5)).Within(0.1));
                Assert.That(-500, Is.EqualTo(beam2Result.Shear(1)).Within(0.1));
            });
        }


        [Test]
        public void Beam_Integration_PointForceInQuarter()
        {

            var properties = new BeamProperties
            {
                Area = 0.2,
                ModulusOfElasticity = 200000,
                MomentOfInertia = 1,
            };

            var structure = new Structure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, properties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, properties);
            structure.LoadFactory.AddBeamPointLoad(beam1, -1000, 0.5);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1);
            var beam2Result = results.GetResult(beam2);


            Assert.Multiple(() =>
            {
                //Assert.That(0, Is.EqualTo(beam1Result.Moment(0)).Within(0.1));
                Assert.That(-3750, Is.EqualTo(beam1Result.Moment(0.5)).Within(0.1));
                //Assert.That(-2500, Is.EqualTo(beam1Result.Moment(1)).Within(0.1));
                //
                //Assert.That(-2500, Is.EqualTo(beam2Result.Moment(0)).Within(0.1));
                //Assert.That(-1250, Is.EqualTo(beam2Result.Moment(0.5)).Within(0.1));
                //Assert.That(0, Is.EqualTo(beam2Result.Moment(1)).Within(0.1));
                //
                //Assert.That(750, Is.EqualTo(beam1Result.Shear(0)).Within(0.1));
                //Assert.That(750, Is.EqualTo(beam1Result.Shear(0.49)).Within(0.1));
                //Assert.That(-250, Is.EqualTo(beam1Result.Shear(0.51)).Within(0.1));
                //Assert.That(-250, Is.EqualTo(beam1Result.Shear(1)).Within(0.1));
                //
                //Assert.That(-250, Is.EqualTo(beam2Result.Shear(0)).Within(0.1));
                //Assert.That(-250, Is.EqualTo(beam2Result.Shear(0.5)).Within(0.1));
                //Assert.That(-250, Is.EqualTo(beam2Result.Shear(1)).Within(0.1));
            });
        }
    }
}
