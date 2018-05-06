using Common.Geometry;
using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;
using FEM2DDynamics.Solver;
using FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FEM2DDynamics.Structure.Tests
{
    [TestFixture()]
    public class DynamicStructureTests
    {
        private const double tolerance = 0.0000001;

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
                Assert.That(beam1Result.Moment(0), Is.EqualTo(-3.86535248253495E-12).Within(tolerance));
                Assert.That(beam1Result.Moment(0.5), Is.EqualTo(321.918536638808).Within(tolerance));
                Assert.That(beam1Result.Moment(1), Is.EqualTo(643.83707327762).Within(tolerance));
                Assert.That(beam1Result.Shear(0), Is.EqualTo(-64.3837073277624).Within(tolerance));
                Assert.That(beam1Result.Shear(0.5), Is.EqualTo(-64.3837073277624).Within(tolerance));
                Assert.That(beam1Result.Shear(1), Is.EqualTo(-64.3837073277624).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo(-4.54487376181266E-06).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo(-7.47330685858709E-06).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo(-0.00754867717045442).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo(-0.0129776063023049).Within(tolerance));

                Assert.That(beam2Result.Moment(0), Is.EqualTo(-1215.34451702688).Within(tolerance));
                Assert.That(beam2Result.Moment(0.5), Is.EqualTo(1284.65548297335).Within(tolerance));
                Assert.That(beam2Result.Moment(1), Is.EqualTo(3784.65548297358).Within(tolerance));
                Assert.That(beam2Result.Shear(0), Is.EqualTo(-500.000000000047).Within(tolerance));
                Assert.That(beam2Result.Shear(0.5), Is.EqualTo(-500.000000000047).Within(tolerance));
                Assert.That(beam2Result.Shear(1), Is.EqualTo(-500.000000000047).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo(-7.47330685858709E-06).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo(-4.5448737618126E-06).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo(-0.0129776063023049).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo(-0.00754867717045455).Within(tolerance));
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
                Assert.That(beam1Result.Moment(0), Is.EqualTo(-0.789274773684904).Within(tolerance));
                Assert.That(beam1Result.Moment(0.5), Is.EqualTo(-1512.424696048).Within(tolerance));
                Assert.That(beam1Result.Moment(1), Is.EqualTo(-1024.06011732232).Within(tolerance));
                Assert.That(beam1Result.Shear(0), Is.EqualTo(902.327084254863).Within(tolerance));
                Assert.That(beam1Result.Shear(0.5), Is.EqualTo(-97.6729157451368).Within(tolerance));
                Assert.That(beam1Result.Shear(1), Is.EqualTo(-97.6729157451368).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo(-2.07039700044352E-05).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo(-2.49709549837756E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo(5.78872023505154E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo(3.22969521823748E-05).Within(tolerance));

                Assert.That(beam2Result.Moment(0), Is.EqualTo(-1008.9675776956).Within(tolerance));
                Assert.That(beam2Result.Moment(0.5), Is.EqualTo(-502.185524326943).Within(tolerance));
                Assert.That(beam2Result.Moment(1), Is.EqualTo(4.59652904171094).Within(tolerance));
                Assert.That(beam2Result.Shear(0), Is.EqualTo(-101.356410673731).Within(tolerance));
                Assert.That(beam2Result.Shear(0.5), Is.EqualTo(-101.356410673731).Within(tolerance));
                Assert.That(beam2Result.Shear(1), Is.EqualTo(-101.356410673731).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo(-2.49709549837756E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo(-1.56437335066609E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo(3.22969521823748E-05).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo(-3.64660805682782E-05).Within(tolerance));
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
                Assert.That(beam1Result.Moment(0), Is.EqualTo(-3.89967410101511).Within(tolerance));
                Assert.That(beam1Result.Moment(0.5), Is.EqualTo(-3040.7597264858).Within(tolerance));
                Assert.That(beam1Result.Moment(1), Is.EqualTo(-2077.61977887058).Within(tolerance));
                Assert.That(beam1Result.Shear(0), Is.EqualTo(2807.37201047696).Within(tolerance));
                Assert.That(beam1Result.Shear(0.5), Is.EqualTo(-192.627989523044).Within(tolerance));
                Assert.That(beam1Result.Shear(1), Is.EqualTo(-192.627989523044).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(0.5), Is.EqualTo(-4.21997173824629E-05).Within(tolerance));
                Assert.That(beam1Result.GetDisplacement(1), Is.EqualTo(-5.01977699240744E-05).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(0.5), Is.EqualTo(0.000219816565555834).Within(tolerance));
                Assert.That(beam1Result.GetAcceleration(1), Is.EqualTo(2.10145024489881E-05).Within(tolerance));

                Assert.That(beam2Result.Moment(0), Is.EqualTo(-2016.53901026798).Within(tolerance));
                Assert.That(beam2Result.Moment(0.5), Is.EqualTo(-992.04146450219).Within(tolerance));
                Assert.That(beam2Result.Moment(1), Is.EqualTo(32.4560812635975).Within(tolerance));
                Assert.That(beam2Result.Shear(0), Is.EqualTo(-204.899509153158).Within(tolerance));
                Assert.That(beam2Result.Shear(0.5), Is.EqualTo(-204.899509153158).Within(tolerance));
                Assert.That(beam2Result.Shear(1), Is.EqualTo(-204.899509153158).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0), Is.EqualTo(-5.01977699240744E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(0.5), Is.EqualTo(-3.13898962718562E-05).Within(tolerance));
                Assert.That(beam2Result.GetDisplacement(1), Is.EqualTo(0).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0), Is.EqualTo(2.10145024489881E-05).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(0.5), Is.EqualTo(-0.000131293493274999).Within(tolerance));
                Assert.That(beam2Result.GetAcceleration(1), Is.EqualTo(0).Within(tolerance));

            });
        }
    }
}