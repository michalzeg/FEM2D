using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Materials;
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
            var material = new Material
            {
                ModulusOfElasticity = 210000000,
                PoissonsRation = 0.3
            };
            var thickness = 0.5;

            var node1 = new Node(3, 0);
            var node2 = new Node(3, 2);
            var node3 = new Node(0, 2, Restraint.Fixed);
            var node4 = new Node(0, 0, Restraint.Fixed);
            var nodes = new[] { node1, node2, node3, node4 };

            var nodeLoad = new NodalLoad
            {
                Node = node2,
                ValueY = -100000
            };
            var loads = new[] { nodeLoad };

            var element1 = new TriangleElement(node1, node2, node4, material, thickness);
            var element2 = new TriangleElement(node3, node4, node2, material, thickness);
            var elements = new[] { element1, element2 };


            var solver = new Solver();
            solver.Solve(elements, nodes, loads);
        }
    }
}
