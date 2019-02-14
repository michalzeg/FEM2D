using FEM2D.Structures;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;
using NUnit.Framework;
using System;

namespace FEM2DTests.Integration
{
    [TestFixture]
    public class Frame_Integration
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public void Frame_Integration_1()
        {
            var properties = new BarProperties
            {
                ModulusOfElasticity = 210000000,
                SectionProperties = new SectionProperties
                {
                    A = 160000d / 1000000d,
                    Ix0 = 0.0021333333333333333,
                }
            };

            var structure = new Structure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetFixedSupport();
            var node2 = structure.NodeFactory.Create(0, 5);
            var node3 = structure.NodeFactory.Create(5, 5);
            node3.SetFixedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, properties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, properties);
            structure.LoadFactory.AddBeamUniformLoad(beam1, -3);
            structure.LoadFactory.AddBeamPointLoad(beam2, -15, 0.5);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1);
            var beam2Result = results.GetResult(beam2);

            Assert.Multiple(() =>
            {
                Assert.That(0, Is.EqualTo(beam1Result.Moment(0)).Within(0.1));
                Assert.That(-3.555, Is.EqualTo(beam1Result.Moment(0.5)).Within(0.1));
                Assert.That(11.64, Is.EqualTo(beam1Result.Moment(1)).Within(0.1));

                Assert.That(11.64, Is.EqualTo(beam2Result.Moment(0)).Within(0.1));
                Assert.That(-12.93, Is.EqualTo(beam2Result.Moment(0.5)).Within(0.1));
                Assert.That(0, Is.EqualTo(beam2Result.Moment(1)).Within(0.1));

                Assert.That(5.1719, Is.EqualTo(beam1Result.Shear(0)).Within(0.1));
                Assert.That(-2.328, Is.EqualTo(beam1Result.Shear(0.5)).Within(0.1));
                Assert.That(-9.828, Is.EqualTo(beam1Result.Shear(1)).Within(0.1));

                Assert.That(9.828, Is.EqualTo(beam2Result.Shear(0)).Within(0.1));
                Assert.That(9.828, Is.EqualTo(beam2Result.Shear(0.5)).Within(0.1));
                Assert.That(-5.172, Is.EqualTo(beam2Result.Shear(1)).Within(0.1));

                Assert.That(9.828, Is.EqualTo(beam1Result.Axial()).Within(0.1));
                Assert.That(9.828, Is.EqualTo(beam2Result.Axial()).Within(0.1));
            });
        }

        [Test]
        public void Frame_Integration_CAD_CAM_Example()
        {
            var properties = new BarProperties
            {
                ModulusOfElasticity = 2 * Math.Pow(10, 10),
                SectionProperties = new SectionProperties
                {
                    A = 0.15,
                    Ix0 = 3.125 * Math.Pow(10, -3),
                }
            };

            var structure = new Structure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetFixedWithRotationSupport();
            var node2 = structure.NodeFactory.Create(0, 5);
            var node3 = structure.NodeFactory.Create(5, 5);
            node3.SetFixedWithRotationSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, properties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, properties);
            structure.LoadFactory.AddBeamUniformLoad(beam1, -3000);
            structure.LoadFactory.AddBeamPointLoad(beam2, -15000, 0.5);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1);
            var beam2Result = results.GetResult(beam2);

            Assert.Multiple(() =>
            {
                Assert.That(5660.2, Is.EqualTo(beam1Result.Moment(0)).Within(0.1));
                Assert.That(-2731.45, Is.EqualTo(beam1Result.Moment(0.5)).Within(0.1));
                Assert.That(7626.9, Is.EqualTo(beam1Result.Moment(1)).Within(0.1));

                Assert.That(7626.9, Is.EqualTo(beam2Result.Moment(0)).Within(0.1));
                Assert.That(-9768.6, Is.EqualTo(beam2Result.Moment(0.5)).Within(0.1));
                Assert.That(10336.0, Is.EqualTo(beam2Result.Moment(1)).Within(0.1));

                Assert.That(7106.7, Is.EqualTo(beam1Result.Shear(0)).Within(0.1));
                Assert.That(-393.32, Is.EqualTo(beam1Result.Shear(0.5)).Within(0.1));
                Assert.That(-7893.3, Is.EqualTo(beam1Result.Shear(1)).Within(0.1));

                Assert.That(6958.2, Is.EqualTo(beam2Result.Shear(0)).Within(0.1));
                Assert.That(6958.2, Is.EqualTo(beam2Result.Shear(0.5)).Within(0.1));
                Assert.That(-8041.8, Is.EqualTo(beam2Result.Shear(1)).Within(0.1));

                Assert.That(6958.2, Is.EqualTo(beam1Result.Axial()).Within(0.1));
                Assert.That(7893.3, Is.EqualTo(beam2Result.Axial()).Within(0.1));
            });
        }

        [Test]
        public void Frame_Integration_InclinedBars()
        {
            var properties = new BarProperties
            {
                ModulusOfElasticity = 210000000,
                SectionProperties = new SectionProperties
                {
                    A = 160000d / 1000000d,
                    Ix0 = 0.0021333333333333333,
                }
            };

            var structure = new Structure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetFixedSupport();
            var node2 = structure.NodeFactory.Create(-5, 10);
            var node3 = structure.NodeFactory.Create(5, 5);
            node3.SetFixedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, properties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, properties);
            structure.LoadFactory.AddNodalLoad(node2, 0, -100000);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1);
            var beam2Result = results.GetResult(beam2);

            Assert.Multiple(() =>
            {
                Assert.That(0, Is.EqualTo(beam1Result.Moment(0)).Within(0.1));
                Assert.That(-22.2, Is.EqualTo(beam1Result.Moment(0.5)).Within(0.1));
                Assert.That(-44.5, Is.EqualTo(beam1Result.Moment(1)).Within(0.1));

                Assert.That(-44.5, Is.EqualTo(beam2Result.Moment(0)).Within(0.1));
                Assert.That(-22.2, Is.EqualTo(beam2Result.Moment(0.5)).Within(0.1));
                Assert.That(0, Is.EqualTo(beam2Result.Moment(1)).Within(0.1));

                Assert.That(4, Is.EqualTo(beam1Result.Shear(0)).Within(0.1));
                Assert.That(4, Is.EqualTo(beam1Result.Shear(0.5)).Within(0.1));
                Assert.That(4, Is.EqualTo(beam1Result.Shear(1)).Within(0.1));

                Assert.That(-4, Is.EqualTo(beam2Result.Shear(0)).Within(0.1));
                Assert.That(-4, Is.EqualTo(beam2Result.Shear(0.5)).Within(0.1));
                Assert.That(-4, Is.EqualTo(beam2Result.Shear(1)).Within(0.1));

                Assert.That(149069.9, Is.EqualTo(beam1Result.Axial()).Within(0.1));
                Assert.That(-74536.9, Is.EqualTo(beam2Result.Axial()).Within(0.1));
            });
        }
    }
}