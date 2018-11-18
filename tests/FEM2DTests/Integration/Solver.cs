using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Restraints;
using FEM2D.Solvers;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using NUnit.Framework;
using System;

namespace FEM2DTests.Integration
{
    [TestFixture]
    public class SolverTest
    {
        [Test]
        public void Solver_IntegrationTest_Passed()
        {
            var material = new MembraneProperties
            {
                ModulusOfElasticity = 30 * Math.Pow(10, 6),// 210000000,
                PoissonsRation = 0.25,// 0.3
                Thickness = 0.5
            };

            var nodes = new NodeFactory();

            var node1 = nodes.Create(3, 0, Restraint.FixedY);
            var node2 = nodes.Create(3, 2);
            var node3 = nodes.Create(0, 2, Restraint.Fixed);
            var node4 = nodes.Create(0, 0, Restraint.Fixed);

            var elements = new ElementFactory();
            var element1 = elements.CreateTriangle(node1, node2, node4, material);
            var element2 = elements.CreateTriangle(node3, node4, node2, material);

            var loads = new LoadFactory();
            loads.AddNodalLoad(node2, 0, -1000);

            var solver = new Solver(elements, nodes, loads);
            solver.Solve();
            var results = solver.Results;

            var result1 = results.MembraneResults.GetElementResult(element1);
            var result2 = results.MembraneResults.GetElementResult(element2);

            Assert.Multiple(() =>
            {
                Assert.That(result1.SigmaX, Is.EqualTo(-93d).Within(1d));
                Assert.That(result1.SigmaY, Is.EqualTo(-1136d).Within(1d));
                Assert.That(result1.TauXY, Is.EqualTo(-62d).Within(1d));

                Assert.That(result2.SigmaX, Is.EqualTo(93d).Within(1d));
                Assert.That(result2.SigmaY, Is.EqualTo(23d).Within(1d));
                Assert.That(result2.TauXY, Is.EqualTo(-297d).Within(1d));
            });
        }
    }
}