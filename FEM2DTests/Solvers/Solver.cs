using Common.DTO;
using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Restraints;
using FEM2D.Solvers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DTests.Solvers
{
    [TestFixture]
    public class SolverTest
    {
        [Test]
        public void SolverTest_Passed()
        {
            var material = new MembraneProperties
            {
                ModulusOfElasticity = 30 * Math.Pow(10, 6),// 210000000,
                PoissonsRation = 0.25,// 0.3
                Thickness = 0.5
            };

            var node1 = new Node(3, 0,1,Restraint.FixedY);
            var node2 = new Node(3, 2,2);
            var node3 = new Node(0, 2,3, Restraint.Fixed);
            var node4 = new Node(0, 0,4, Restraint.Fixed);
            var nodes = new[] { node1, node2, node3, node4 };

            var nodeLoad = new NodalLoad
            {
                Node = node2,
                ValueY = -1000,// -100000
            };
            var loads = new[] { nodeLoad };

            var element1 = new TriangleElement(node1, node2, node4, material,1);
            var element2 = new TriangleElement(node3, node4, node2, material,2);
            var elements = new[] { element1, element2 };


            var solver = new Solver();
            solver.Solve(elements, nodes, loads);
            var results = solver.Results;

            Assert.Multiple(() =>
            {
                Assert.That(results.TriangleResult[0].SigmaX, Is.EqualTo(-93d).Within(1d));
                Assert.That(results.TriangleResult[0].SigmaY, Is.EqualTo(-1136d).Within(1d));
                Assert.That(results.TriangleResult[0].TauXY, Is.EqualTo(-62d).Within(1d));

                Assert.That(results.TriangleResult[1].SigmaX, Is.EqualTo(93d).Within(1d));
                Assert.That(results.TriangleResult[1].SigmaY, Is.EqualTo(23d).Within(1d));
                Assert.That(results.TriangleResult[1].TauXY, Is.EqualTo(-297d).Within(1d));
            });
        }
    }
}
