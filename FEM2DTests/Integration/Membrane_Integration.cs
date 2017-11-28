using Common.DTO;
using FEM2D.Results;
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
    public class MembraneIntegrationTests
    {
        private ResultProvider result;
        private MembraneInputData membraneData;
        private Structure structure;

        [OneTimeSetUp]
        public void Setup()
        {
            this.membraneData = GetMembraneData();

            this.structure = new Structure();
            this.structure.AddMembraneGeometry(membraneData);
            this.structure.Solve();

            this.result = structure.Results;
        }

        [Test]
        public void Membrane_Integration_MinMaxStresses_Passed()
        {

            var expectedMaxSigmaX =  250.63 ;
            var expectedMaxSigmaY =  320.67 ;
            var expectedMaxTauXY  =  330.89 ;
                                      
            var expectedMinSigmaX =  -330.24 ;
            var expectedMinSigmaY =  -597.20 ;
            var expectedMinTauXY =   -321.56; 


            var actualMaxSigmaX = result.TriangleResult.Max(e => e.SigmaX);
            var actualMaxSigmaY = result.TriangleResult.Max(e => e.SigmaY);
            var actualMaxTauXY = result.TriangleResult.Max(e => e.TauXY);

            var actualMinSigmaX = result.TriangleResult.Min(e => e.SigmaX);
            var actualMinSigmaY = result.TriangleResult.Min(e => e.SigmaY);
            var actualMinTauXY = result.TriangleResult.Min(e => e.TauXY);

            var tolerance = 1;
            Assert.Multiple(() =>
            {
                Assert.That(expectedMaxSigmaX, Is.EqualTo(actualMaxSigmaX).Within(tolerance).Percent);
                Assert.That(expectedMaxSigmaY, Is.EqualTo(actualMaxSigmaY).Within(tolerance).Percent);
                Assert.That(expectedMaxTauXY, Is.EqualTo(actualMaxTauXY).Within(tolerance).Percent);

                Assert.That(expectedMinSigmaX, Is.EqualTo(actualMinSigmaX).Within(tolerance).Percent);
                Assert.That(expectedMinSigmaY, Is.EqualTo(actualMinSigmaY).Within(tolerance).Percent);
                Assert.That(expectedMinTauXY, Is.EqualTo (actualMinTauXY).Within(tolerance).Percent);
            });

        }

        [Test]
        public void Membrane_Integration_NumberOfNodesAndElements_Passed()
        {
            var expectedNodeCount = 26;
            var expectedElementCount = 27;

            var actualNodeCount = this.structure.NodeFactory.GetNodeCount();
            var actualElementCount = this.structure.ElementFactory.GetAll().Count();

            Assert.Multiple(() =>
            {
                Assert.That(expectedElementCount, Is.EqualTo(actualElementCount));
                Assert.That(expectedNodeCount, Is.EqualTo(actualNodeCount));
            });
        }



        private static MembraneInputData GetMembraneData()
        {
            var vertex1 = new VertexInput
            {
                Number = 1,
                X = 0,
                Y = 0,
                SupportX = true,
                SupportY = true,
            };
            var vertex2 = new VertexInput
            {
                Number = 2,
                X = 20,
                Y = 30,
                LoadY = -100,
            };
            var vertex3 = new VertexInput
            {
                Number = 3,
                X = 0,
                Y = 10,
                SupportX = true,
                SupportY = true,
            };

            var edge1 = new Edge
            {
                Number = 1,
                Start = vertex1,
                End = vertex2,
            };
            var edge2 = new Edge
            {
                Number = 2,
                Start = vertex2,
                End = vertex3,
            };
            var edge3 = new Edge
            {
                Number = 3,
                Start = vertex3,
                End = vertex1,

            };

            var membraneData = new MembraneInputData
            {
                Vertices = new[] { vertex1, vertex2, vertex3 },
                Edges = new[] { edge1, edge2, edge3 },
                Properties = new MembraneProperties
                {
                    ModulusOfElasticity = 200000000,
                    PoissonsRation = 0.25,
                    Thickness = 0.5
                },
            };
            return membraneData;
        }
    }
}
