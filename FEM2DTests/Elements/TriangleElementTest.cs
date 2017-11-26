﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using FEM2D.Elements;
using MathNet.Numerics.LinearAlgebra.Double;
using FEM2D.Nodes;
using Common.Point;
using Common.DTO;

namespace FEM2DTests.Elements
{
    [TestFixture]
    public class TriangleElementTest
    {
        [Test]
        public void TriangleElement_GetArea()
        {
            var nodes = new NodeCollection();
            var node1 = nodes.Create(new PointD(0, 0));
            var node2 = nodes.Create(new PointD(10, 0));
            var node3 = nodes.Create(new PointD(20, 30));

            var membraneProperties = new MembraneProperties
            {
                Thickness = 10,
            };

            var element = new TriangleElement(node1, node2, node3, membraneProperties,1);

            var expectedArea = 0.5 * 10 * 30;
            var actualArea = element.Area;
            Assert.That(expectedArea, Is.EqualTo(actualArea).Within(0.01));
        }
    }
}
