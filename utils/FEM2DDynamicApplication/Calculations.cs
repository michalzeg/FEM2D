using FEM2DDynamics.Solver;
using FEM2DDynamics.Structure;
using FEM2DDynamics.Utils;
using System;
using System.Diagnostics;
using System.IO;
using static FEM2DDynamicTestApplication.DataProvider;

namespace FEM2DDynamicTestApplication
{
    public class Calculations
    {
        public static void Initialize()
        {
            var dynamicProperties = GetSection();

            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.1,
                EndTime = 1,
                StartTime = 0,
                DampingRatio = 0.003,
            };
            var guid = Guid.NewGuid().ToString();
            Directory.CreateDirectory(Path.Combine(@"E:", "Files", guid));
            Action<ProgressMessage> progress = msg =>
            {
                var guid2 = Guid.NewGuid().ToString();
                var path = Path.Combine(@"E:", "Files", guid, $"{msg.Progress.ToString()}_{guid2}.txt");
                File.WriteAllText(path, msg.Progress.ToString());
            };

            var timer = new Stopwatch();
            timer.Start();
            var structure = new DynamicStructure(settings);
            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);
            structure.LoadFactory.AddPointMovingLoad(-1000, 0, 1);

            structure.Solve();
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1, 1);
            var beam2Result = results.GetResult(beam2, 1);

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
        }

        public static void DynamicLoadInCentre()
        {
            var dynamicProperties = GetSection();

            var settings = new DynamicSolverSettings
            {
                DeltaTime = 0.001,
                EndTime = 80,
                StartTime = 0,
                DampingRatio = 0.003,
            };
            Action<ProgressMessage> progress = GetProgress();

            var timer = new Stopwatch();
            timer.Start();
            var structure = new DynamicStructure(settings);

            var node1 = structure.NodeFactory.Create(0, 0);
            node1.SetPinnedSupport();
            var node2 = structure.NodeFactory.Create(10, 0);
            var node3 = structure.NodeFactory.Create(20, 0);
            node3.SetPinnedSupport();
            var node4 = structure.NodeFactory.Create(30, 0);
            var node5 = structure.NodeFactory.Create(40, 0);
            node5.SetPinnedSupport();
            var node6 = structure.NodeFactory.Create(50, 0);
            var node7 = structure.NodeFactory.Create(60, 0);
            var node8 = structure.NodeFactory.Create(70, 0);
            var node9 = structure.NodeFactory.Create(80, 0);
            node9.SetFixedSupport();

            var beam1 = structure.ElementFactory.CreateBeam(node1, node2, dynamicProperties);
            var beam2 = structure.ElementFactory.CreateBeam(node2, node3, dynamicProperties);
            var beam3 = structure.ElementFactory.CreateBeam(node3, node4, dynamicProperties);
            var beam4 = structure.ElementFactory.CreateBeam(node4, node5, dynamicProperties);
            var beam5 = structure.ElementFactory.CreateBeam(node5, node6, dynamicProperties);
            var beam6 = structure.ElementFactory.CreateBeam(node6, node7, dynamicProperties);
            var beam7 = structure.ElementFactory.CreateBeam(node7, node8, dynamicProperties);
            var beam8 = structure.ElementFactory.CreateBeam(node8, node9, dynamicProperties);

            structure.LoadFactory.AddPointMovingLoad(-1000, 0, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -1, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -2, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -3, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -4, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -5, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -6, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -7, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -8, 1);
            structure.LoadFactory.AddPointMovingLoad(-2000, -9, 1);
            structure.Solve(progress);
            var results = structure.Results.BeamResults;

            var beam1Result = results.GetResult(beam1, 1);
            var beam2Result = results.GetResult(beam2, 1);

            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);

            //Json
            //SaveToJson(settings, beam1, beam2, beam3, beam4, results);

            //dxf
            //SaveToDxf(settings, beam1, results);
        }

        private static Action<ProgressMessage> GetProgress()
        {
            var guid = Guid.NewGuid().ToString();
            Directory.CreateDirectory(Path.Combine(@"E:", "Files", guid));
            Action<ProgressMessage> progress = msg =>
            {
                var guid2 = Guid.NewGuid().ToString();
                var path = Path.Combine(@"E:", "Files", guid, $"{msg.Progress.ToString()}_{guid2}.txt");
                File.WriteAllText(path, msg.Progress.ToString());
            };
            return progress;
        }
    }
}