using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Restraints;
using FEM2D.Solvers;
using FEM2DCommon.DTO;
using Newtonsoft.Json;
using Output.OutputCreator;
using System;
using static TestApplication.MockDataProvider;
using static TestApplication.DxfHelper;

namespace TestApplication
{
    public class JsonResults
    {
        public static void ResultsToJSon()
        {
            var material = new MembraneProperties
            {
                ModulusOfElasticity = 30 * Math.Pow(10, 6),// 210000000,
                PoissonsRation = 0.25,// 0.3
                Thickness = 0.5
            };

            var nodes = new NodeFactory();

            var node1 = nodes.Create(3, 0, Restraint.FixedY);
            var node2 = nodes.Create(3, 2);
            var node3 = nodes.Create(0, 2, Restraint.Fixed);
            var node4 = nodes.Create(0, 0, Restraint.Fixed);

            var elements = new ElementFactory();

            var loads = new LoadFactory();
            loads.AddNodalLoad(node2, 0, -1000);

            var element1 = elements.CreateTriangle(node1, node2, node4, material);
            var element2 = elements.CreateTriangle(node3, node4, node2, material);

            var solver = new Solver(elements, nodes, loads);
            solver.Solve();
            var results = solver.Results;

            var outputCrator = new OutputCreator(results);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;
            var js = JsonConvert.SerializeObject(output);
        }
    }
}