﻿using Common.DTO;
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
        public void Beam_Integration_MinMaxStresses_Passed()
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
            var results = structure.Results;
            Assert.Fail();
        }

    }
}
