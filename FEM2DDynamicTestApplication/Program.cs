using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using Common.Extensions;
using FEM2DCommon.Sections;
using FEM2D.Structures;
using FEM2DDynamics.Solver;
using FEM2DDynamics.Structure;
using FEM2DStressCalculator.Beams;
using netDxf;
using netDxf.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEMCommon.ElementProperties.SectionBuilders;
using FEMCommon.ElementProperties.SectionBuilders.CustomSection;
using Common.Geometry;
using FEMCommon.ElementProperties.DynamicBeamPropertiesBuilder;

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
            var dynamicProperties = NewMethod();

            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.0001,
                EndTime = 40,
                StartTime = 0,
                DampingRatio = 0.003,
            };



            var structure = new DynamicStructure(settings);
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();
            var node4 = structure.NodeFactory.Create(30, 0);
            var node5 = structure.NodeFactory.Create(40, 0);
            node5.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);
            var beam3 = structure.ElementFactory.CreateBeam(node3, node4, dynamicProperties);
            var beam4 = structure.ElementFactory.CreateBeam(node4, node5, dynamicProperties);
            structure.LoadFactory.AddPointMovingLoad(-1000, 0, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -4, 1);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1, 1);
            var beam2Result = results.GetResult(beam2, 1);


            //Json
            SaveToJson(settings, beam1, beam2, beam3, beam4, results);


            //dxf
            SaveToDxf(settings, beam1, results);

        }

        private static DynamicBeamProperties NewMethod()
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

        private static void SaveToJson(DynamicSolverSettings settings, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam1, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam2, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam3, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam4, FEM2DDynamics.Results.DynamicBeamElementResults results)
        {
            var section = Section.FromRectangle(1, 0.3);

            var stressCalculator = new BeamStressCalculator(section.SectionProperties);


            var deltaT = 1;
            var time = 0;

            var beams = new[] { beam1, beam2, beam3, beam4 };
            var relativePositions = Enumerable.Range(0, 11)
                                    .Select(e => e / 10d)
                                    .ToList();
            var timeResults = new List<TimeResult>();
            while (time <= settings.EndTime)
            {
                var positionResults = new List<PositionResult>();
                var stresses = new List<double>();
                foreach (var beam in beams)
                {
                    var beamResult = results.GetResult(beam, time);

                    foreach (var relativePosition in relativePositions)
                    {
                        var globalPosition = relativePosition * beam.Length + beam.Nodes[0].Coordinates.X;
                        var displ = beamResult.GetDisplacement(relativePosition);
                        var forces = beamResult.GetBeamForces(relativePosition);

                        var topStress = stressCalculator.TopNormalStress(forces);
                        var bottomStress = stressCalculator.BottomNormalStress(forces);

                        stresses.Add(topStress);
                        stresses.Add(bottomStress);

                        var positionResult = new PositionResult
                        {
                            GlobalPosition = globalPosition,
                            TopStress = topStress,
                            BottomStress = bottomStress,
                            Displacement = displ,
                        };
                        positionResults.Add(positionResult);
                    }

                }

                var timeResult = new TimeResult();
                timeResult.Time = time;
                timeResult.MaxStress = stresses.Max();
                timeResult.MinStress = stresses.Min();
                timeResult.PositionResults = positionResults;
                timeResults.Add(timeResult);
                time += deltaT;
            }

            var resultData = new ResultData();
            resultData.TimeResults = timeResults;

            var extremes = timeResults.SelectMany(e => e.PositionResults)
                .Select(e => Math.Abs(e.Displacement).Round(3))
                .ToList();

            resultData.MaxAbsoluteDisplacement = extremes.Select(e => e).Max();

            var obj = JsonConvert.SerializeObject(resultData);
            File.WriteAllText(@"e:\disp.json", obj);

        }

        private static void SaveToDxf(DynamicSolverSettings settings, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam1, FEM2DDynamics.Results.DynamicBeamElementResults results)
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var time = 0d;

            while (time <= settings.EndTime)
            {
                var result = results.GetResult(beam1, time);
                var displ = result.GetDisplacement(1);
                var moment = result.Moment(1);


                document.AddEntity(new Point(time * 100, moment, 0));

                time += settings.DeltaTime;
            }
            document.Save(filePath);
        }
    }
}
