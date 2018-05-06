using NUnit.Framework;
using FEM2DDynamics.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;
using FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder;
using Common.Geometry;
using FEM2DDynamics.Solver;

namespace FEM2DDynamics.Structure.Tests
{
    [TestFixture()]
    public class DynamicStructureTests
    {
        const double tolerance = 0.0000001;
        private static DynamicBeamProperties GetDynamicProperties()
        {
            var perimeters = new List<Perimeter>
            {
                new Perimeter(new List<PointD>{
                    new PointD(-0.5,0),
                    new PointD(-0.5,0.1),
                    new PointD(0.5,1),
                    new PointD(0.5,0),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.05,0.1),
                    new PointD(-0.05,1),
                    new PointD(0.05,1),
                    new PointD(0.05,0.1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.05,1),
                    new PointD(-0.05,2),
                    new PointD(0.05,2),
                    new PointD(0.05,1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.05,2),
                    new PointD(-0.05,3.9),
                    new PointD(0.05,3.9),
                    new PointD(0.05,2),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(-0.5,3.9),
                    new PointD(-0.5,4),
                    new PointD(0.5,4),
                    new PointD(0.5,3.9),
                }),


                new Perimeter(new List<PointD>{
                    new PointD(2,0),
                    new PointD(2,0.1),
                    new PointD(3,1),
                    new PointD(3,0),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2.45,0.1),
                    new PointD(2.45,1),
                    new PointD(2.55,1),
                    new PointD(2.55,0.1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2.45,1),
                    new PointD(2.45,2),
                    new PointD(2.55,2),
                    new PointD(2.55,1),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2.45,2),
                    new PointD(2.45,3.9),
                    new PointD(2.55,3.9),
                    new PointD(2.55,2),
                }),
                new Perimeter(new List<PointD>{
                    new PointD(2,3.9),
                    new PointD(2,4),
                    new PointD(3,4),
                    new PointD(3,3.9),
                }),
            };

            var section = new Section(perimeters);

            var dynamicProperties = DynamicBeamPropertiesBuilder.Create()
                .SetSteel()
                .SetSection(section)
                .Build();
            return dynamicProperties;
        }

        [Test()]
        public void DynamicStructure_IntegrationTest_StaticForceInTheMiddle()
        {
            var dynamicProperties = GetDynamicProperties();

            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.01,
                EndTime = 4,
                StartTime = 0,
                DampingRatio = 0.003,
            };

            var structure = new DynamicStructure(settings);
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);

            structure.LoadFactory.AddPointMovingLoad(-1000, 10, 0);

            structure.Solve();
            var results = structure.Results.BeamResults;
            var time = 2;
            var beam1Result = results.GetResult(beam1, time);
            var beam2Result = results.GetResult(beam2, time);

            var generatedTest =
                $"Assert.That(beam1Result.Time, Is.EqualTo({beam1Result.Time}).Within(tolerance));" + Environment.NewLine +

                $"Assert.That(beam1Result.Moment(0), Is.EqualTo({beam1Result.Moment(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.Moment(0.5), Is.EqualTo({beam1Result.Moment(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.Moment(1), Is.EqualTo({beam1Result.Moment(1)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.Shear(0), Is.EqualTo({beam1Result.Shear(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.Shear(0.5), Is.EqualTo({beam1Result.Shear(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.Shear(1), Is.EqualTo({beam1Result.Shear(1)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo({beam1Result.GetDisplacement(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo({beam1Result.GetDisplacement(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo({beam1Result.GetDisplacement(1)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo({beam1Result.GetAcceleration(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo({beam1Result.GetAcceleration(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo({beam1Result.GetAcceleration(1)}).Within(tolerance));" + Environment.NewLine +
                $"" + Environment.NewLine +
                $"Assert.That(beam2Result.Moment(0), Is.EqualTo({beam2Result.Moment(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.Moment(0.5), Is.EqualTo({beam2Result.Moment(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.Moment(1), Is.EqualTo({beam2Result.Moment(1)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.Shear(0), Is.EqualTo({beam2Result.Shear(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.Shear(0.5), Is.EqualTo({beam2Result.Shear(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.Shear(1), Is.EqualTo({beam2Result.Shear(1)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo({beam2Result.GetDisplacement(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo({beam2Result.GetDisplacement(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo({beam2Result.GetDisplacement(1)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo({beam2Result.GetAcceleration(0)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo({beam2Result.GetAcceleration(0.5)}).Within(tolerance));" + Environment.NewLine +
                $"Assert.That(beam2Result.GetAcceleration(1), Is.EqualTo({beam2Result.GetAcceleration(1)}).Within(tolerance));" + Environment.NewLine +
                $""

                ;

            Assert.Multiple(() =>
            {
                Assert.That(beam1Result.Time, Is.EqualTo(2).Within(tolerance));
                Assert.That(beam1Result.Moment(0), Is.EqualTo(2.8421709430404E-13).Within(tolerance));
                Assert.That(beam1Result.Moment(0.5), Is.EqualTo(-1232.88200884092).Within(tolerance));
                Assert.That(beam1Result.Moment(1), Is.EqualTo(-2465.76401768184).Within(tolerance));
                Assert.That(beam1Result.Shear(0), Is.EqualTo(246.576401768184).Within(tolerance));
                Assert.That(beam1Result.Shear(0.5), Is.EqualTo(246.576401768184).Within(tolerance));
                Assert.That(beam1Result.Shear(1), Is.EqualTo(246.576401768184).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo(-3.43324254623854E-05).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo(-5.03470422153992E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo(-0.00352982839444143).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo(-0.0054744875917376).Within(tolerance));

                Assert.That(beam2Result.Moment(0), Is.EqualTo(-3352.77181593721).Within(tolerance));
                Assert.That(beam2Result.Moment(0.5), Is.EqualTo(-852.77181593712).Within(tolerance));
                Assert.That(beam2Result.Moment(1), Is.EqualTo(1647.22818406297).Within(tolerance));
                Assert.That(beam2Result.Shear(0), Is.EqualTo(-500.000000000017).Within(tolerance));
                Assert.That(beam2Result.Shear(0.5), Is.EqualTo(-500.000000000017).Within(tolerance));
                Assert.That(beam2Result.Shear(1), Is.EqualTo(-500.000000000017).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo(-5.03470422153992E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo(-3.43324254623866E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo(-0.0054744875917376).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo(-0.0035298283944376).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(1), Is.EqualTo(0).Within(tolerance));

            });
        }

        [Test()]
        public void DynamicStructure_IntegrationTest_OneMovingLoad()
        {
            var dynamicProperties = GetDynamicProperties();

            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.01,
                EndTime = 4,
                StartTime = 0,
                DampingRatio = 0.003,
            };

            var structure = new DynamicStructure(settings);
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);

            structure.LoadFactory.AddPointMovingLoad(-1000, 0, 1);

            structure.Solve();
            var results = structure.Results.BeamResults;
            var time = 2;
            var beam1Result = results.GetResult(beam1, time);
            var beam2Result = results.GetResult(beam2, time);
            

            Assert.Multiple(() =>
            {
                Assert.That(beam1Result.Time, Is.EqualTo(2).Within(tolerance));
                Assert.That(beam1Result.Moment(0), Is.EqualTo(-0.43022147436985).Within(tolerance));
                Assert.That(beam1Result.Moment(0.5), Is.EqualTo(-1507.9097659485).Within(tolerance));
                Assert.That(beam1Result.Moment(1), Is.EqualTo(-1015.38931042263).Within(tolerance));
                Assert.That(beam1Result.Shear(0), Is.EqualTo(901.495908894826).Within(tolerance));
                Assert.That(beam1Result.Shear(0.5), Is.EqualTo(-98.5040911051742).Within(tolerance));
                Assert.That(beam1Result.Shear(1), Is.EqualTo(-98.5040911051742).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo(-2.06108693498933E-05).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo(-2.48475644581292E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo(4.88672969906445E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo(-1.49518662896552E-05).Within(tolerance));

                Assert.That(beam2Result.Moment(0), Is.EqualTo(-1001.81197862627).Within(tolerance));
                Assert.That(beam2Result.Moment(0.5), Is.EqualTo(-498.704192275804).Within(tolerance));
                Assert.That(beam2Result.Moment(1), Is.EqualTo(4.40359407465792).Within(tolerance));
                Assert.That(beam2Result.Shear(0), Is.EqualTo(-100.621557270092).Within(tolerance));
                Assert.That(beam2Result.Shear(0.5), Is.EqualTo(-100.621557270092).Within(tolerance));
                Assert.That(beam2Result.Shear(1), Is.EqualTo(-100.621557270092).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo(-2.48475644581292E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo(-1.55697782426499E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo(-1.49518662896552E-05).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo(4.83508435064712E-06).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(1), Is.EqualTo(0).Within(tolerance));

            });
        }

        [Test()]
        public void DynamicStructure_IntegrationTest_TwoMovingLoad()
        {
            var dynamicProperties = GetDynamicProperties();

            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.01,
                EndTime = 4,
                StartTime = 0,
                DampingRatio = 0.003,
            };

            var structure = new DynamicStructure(settings);
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);

            structure.LoadFactory.AddPointMovingLoad(-1000, 0, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -1, 1);

            structure.Solve();
            var results = structure.Results.BeamResults;
            var time = 2;
            var beam1Result = results.GetResult(beam1, time);
            var beam2Result = results.GetResult(beam2, time);



            Assert.Multiple(() =>
            {
                Assert.That(beam1Result.Time, Is.EqualTo(2).Within(tolerance));
                Assert.That(beam1Result.Moment(0), Is.EqualTo(-2.12546304533053).Within(tolerance));
                Assert.That(beam1Result.Moment(0.5), Is.EqualTo(-3016.51254249551).Within(tolerance));
                Assert.That(beam1Result.Moment(1), Is.EqualTo(-2030.89962194568).Within(tolerance));
                Assert.That(beam1Result.Shear(0), Is.EqualTo(2802.87741589004).Within(tolerance));
                Assert.That(beam1Result.Shear(0.5), Is.EqualTo(-197.122584109965).Within(tolerance));
                Assert.That(beam1Result.Shear(1), Is.EqualTo(-197.122584109965).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo(-4.2030285162733E-05).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo(-4.99906660747212E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo(0.000131490051853702).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo(5.48462242802293E-05).Within(tolerance));

                Assert.That(beam2Result.Moment(0), Is.EqualTo(-2008.25974815882).Within(tolerance));
                Assert.That(beam2Result.Moment(0.5), Is.EqualTo(-987.59966190666).Within(tolerance));
                Assert.That(beam2Result.Moment(1), Is.EqualTo(33.0604243454966).Within(tolerance));
                Assert.That(beam2Result.Shear(0), Is.EqualTo(-204.132017250431).Within(tolerance));
                Assert.That(beam2Result.Shear(0.5), Is.EqualTo(-204.132017250431).Within(tolerance));
                Assert.That(beam2Result.Shear(1), Is.EqualTo(-204.132017250431).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo(-4.99906660747212E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo(-3.12625277882741E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo(5.48462242802293E-05).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo(-0.000119157322591677).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(1), Is.EqualTo(0).Within(tolerance));

            });
        }
    }
}