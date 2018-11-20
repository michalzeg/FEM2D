using FEM2D.Restraints;
using FEM2D.Structures;
using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using FEM2DCommon.Sections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DTests.Integration
{
    [TestFixture]
    public class Truss_Integration
    {
        [Test]
        public void Truss_Integration_Case1()
        {
            var L = 3;
            var P = 5;
            var E = 205;

            var properties = new BarProperties();
            properties.SectionProperties = Section.FromRectangle(1, 1).SectionProperties;
            properties.ModulusOfElasticity = E;

            var A = properties.Area;

            var structure = new Structure();

            var node1 = structure.NodeFactory.Create(0, 0);
            var node2 = structure.NodeFactory.Create(L, 0);
            var node3 = structure.NodeFactory.Create(L, L);
            node1.SetPinnedSupport();
            node2.SetPinnedSupport();

            var element1 = structure.ElementFactory.CreateTruss(node1, node2, properties);
            var element2 = structure.ElementFactory.CreateTruss(node2, node3, properties);
            var element3 = structure.ElementFactory.CreateTruss(node1, node3, properties);

            structure.LoadFactory.AddNodalLoad(node3, 2 * P, -P);

            structure.Solve();

            var nodeResults = structure.Results.NodeResults;
            var elementResults = structure.Results.TrussResults;

            Assert.Multiple(() =>
            {
                Assert.That(nodeResults.GetNodeResult(node1).UX, Is.EqualTo(0).Within(0.001));
                Assert.That(nodeResults.GetNodeResult(node1).UY, Is.EqualTo(0).Within(0.001));
                Assert.That(nodeResults.GetNodeResult(node1).Rz, Is.EqualTo(0).Within(0.001));

                Assert.That(nodeResults.GetNodeResult(node2).UX, Is.EqualTo(0).Within(0.001));
                Assert.That(nodeResults.GetNodeResult(node2).UY, Is.EqualTo(0).Within(0.001));
                Assert.That(nodeResults.GetNodeResult(node2).Rz, Is.EqualTo(0).Within(0.001));

                Assert.That(nodeResults.GetNodeResult(node3).UX, Is.EqualTo(8.656 * P * L / E / A).Within(0.001));
                Assert.That(nodeResults.GetNodeResult(node3).UY, Is.EqualTo(-3.0 * P * L / E / A).Within(0.001));
                Assert.That(nodeResults.GetNodeResult(node3).Rz, Is.EqualTo(0).Within(0.001));

                Assert.That(elementResults.GetElementResult(element1).NormalForce, Is.EqualTo(0).Within(0.1));
                Assert.That(elementResults.GetElementResult(element2).NormalForce, Is.EqualTo(3 * P).Within(0.1));
                Assert.That(elementResults.GetElementResult(element3).NormalForce, Is.EqualTo(-2.828 * P).Within(0.1));
            });
        }

        [Test]
        public void Truss_Integration_Case2()
        {
            var L = 1;
            var P = 1;
            var E = 1;

            var properties = new BarProperties
            {
                SectionProperties = Section.FromRectangle(1, 1).SectionProperties,
                ModulusOfElasticity = E
            };

            var A = properties.Area;

            var structure = new Structure();

            var node1 = structure.NodeFactory.Create(0, 0);
            var node2 = structure.NodeFactory.Create(0, 2 * L);
            var node3 = structure.NodeFactory.Create(L, L);
            var node4 = structure.NodeFactory.Create(2 * L, 2 * L);
            var node5 = structure.NodeFactory.Create(2 * L, L);
            node1.SetPinnedSupport();
            node4.SetRestraint(Restraint.FixedX);
            node5.SetPinnedSupport();

            var element1 = structure.ElementFactory.CreateTruss(node1, node2, properties);
            var element2 = structure.ElementFactory.CreateTruss(node2, node4, properties);
            var element3 = structure.ElementFactory.CreateTruss(node1, node3, properties);
            var element4 = structure.ElementFactory.CreateTruss(node3, node4, properties);
            var element5 = structure.ElementFactory.CreateTruss(node2, node3, properties);
            var element6 = structure.ElementFactory.CreateTruss(node3, node5, properties);

            structure.LoadFactory.AddNodalLoad(node2, 0, -P);
            structure.LoadFactory.AddNodalLoad(node4, 0, -P);

            structure.Solve();

            var nodeResults = structure.Results.NodeResults;
            var elementResults = structure.Results.TrussResults;

            Assert.Multiple(() =>
            {
                Assert.That(elementResults.GetElementResult(element1).NormalForce, Is.EqualTo(1.06066).Within(0.0001));
                Assert.That(elementResults.GetElementResult(element2).NormalForce, Is.EqualTo(0.06066).Within(0.0001));
                Assert.That(elementResults.GetElementResult(element3).NormalForce, Is.EqualTo(1.32842).Within(0.0001));
                Assert.That(elementResults.GetElementResult(element4).NormalForce, Is.EqualTo(1.41421).Within(0.00001));
                Assert.That(elementResults.GetElementResult(element5).NormalForce, Is.EqualTo(-0.08578).Within(0.0001));
                Assert.That(elementResults.GetElementResult(element6).NormalForce, Is.EqualTo(-0.12132).Within(0.0001));
            });
        }
    }
}