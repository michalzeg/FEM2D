using Common.DTO;
using Common.ElementProperties;
using FEM2D.Structures;
using FEM2DDynamics.Solver;
using FEM2DDynamics.Structure;
using netDxf;
using netDxf.Entities;
using Newtonsoft.Json;
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
            var properties = BeamProperties.Default;
            var dynamicProperties = new DynamicBeamProperties
            {
                BeamProperties = properties,
                Density = 2000,
            };
            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.01,
                EndTime = 400,
                StartTime = 0
            };

            var structure = new DynamicStructure(settings);
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);
            structure.LoadFactory.AddPointMovingLoad(-1000, 0,1);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1,1);
            var beam2Result = results.GetResult(beam2,1);


            //dxf
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var time = 0d;

            while (time<=settings.EndTime)
            {
                //var displ = beam1Result.GetDisplacement(1);
                var moment = beam1Result.Moment(1);

                beam1Result = results.GetResult(beam1, time);
                document.AddEntity(new Point(time,moment , 0));

                time += 0.01;
            }
            document.Save(filePath);

           
        }
    }
}
