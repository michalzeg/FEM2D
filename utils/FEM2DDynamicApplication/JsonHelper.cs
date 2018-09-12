using Common.Extensions;
using FEM2DCommon.Sections;
using FEM2DDynamics.Solver;
using FEM2DStressCalculator.Beams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FEM2DDynamicTestApplication
{
    public class JsonHelper
    {
        public static void SaveToJson(DynamicSolverSettings settings, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam1, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam2, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam3, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam4, FEM2DDynamics.Results.DynamicBeamElementResults results)
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
                time = ProcessTime(results, stressCalculator, deltaT, time, beams, relativePositions, timeResults);
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

        private static int ProcessTime(FEM2DDynamics.Results.DynamicBeamElementResults results, BeamStressCalculator stressCalculator, int deltaT, int time, FEM2DDynamics.Elements.Beam.IDynamicBeamElement[] beams, List<double> relativePositions, List<TimeResult> timeResults)
        {
            var positionResults = new List<PositionResult>();
            var stresses = new List<double>();
            foreach (var beam in beams)
            {
                var beamResult = results.GetResult(beam, time);
                ApplyRelativePosition(stressCalculator, relativePositions, positionResults, stresses, beam, beamResult);
            }

            var timeResult = new TimeResult();
            timeResult.Time = time;
            timeResult.MaxStress = stresses.Max();
            timeResult.MinStress = stresses.Min();
            timeResult.PositionResults = positionResults;
            timeResults.Add(timeResult);
            time += deltaT;
            return time;
        }

        private static void ApplyRelativePosition(BeamStressCalculator stressCalculator, List<double> relativePositions, List<PositionResult> positionResults, List<double> stresses, FEM2DDynamics.Elements.Beam.IDynamicBeamElement beam, FEM2DDynamics.Results.Beam.DynamicBeamElementResult beamResult)
        {
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
    }
}