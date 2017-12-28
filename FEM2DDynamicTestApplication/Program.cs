﻿using Common.DTO;
using Common.ElementProperties;
using FEM2D.Structures;
using FEM2DDynamics.Structure;
using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamicTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicLoadInCentre();

        }

        private static void DynamicLoadInCentre()
        {
            var properties = new BeamProperties
            {
                Area = 0.2,
                ModulusOfElasticity = 200000,
                MomentOfInertia = 1,
            };
            var dynamicProperties = new DynamicBeamProperties
            {
                BeamProperties = properties,
                Density = 2000,
            };

            var structure = new DynamicStructure();
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);
            structure.LoadFactory.AddPointMovingLoad(-1000, 10);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1,1);
            var beam2Result = results.GetResult(beam2,1);


            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var time = 0d;
            while (time<=10)
            {
                var displ = results.GetDisplacement(beam1, 1, time);
                var moment = beam1Result.Moment(1);

                beam1Result = results.GetResult(beam1, time);
                document.AddEntity(new Point(time*100,moment , 0));

                time += 0.01;
            }
            document.Save(filePath);
        }
    }
}
