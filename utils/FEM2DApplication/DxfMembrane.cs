using FEM2D.Structures;
using FEM2DCommon.DTO;
using netDxf;
using netDxf.Entities;
using Newtonsoft.Json;
using Output.OutputCreator;
using static TestApplication.MockDataProvider;
using static TestApplication.DxfHelper;

namespace TestApplication
{
    public class DxfMembrane
    {
        public static void ResultsToDxfMembrane()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var membraneData = GetMembraneData();

            var structure = new Structure();
            structure.AddMembraneGeometry(membraneData);
            structure.Solve();

            var nodes = structure.NodeFactory.GetAll();
            var elements = structure.ElementFactory.GetAll();

            var result = structure.Results;

            var outputCrator = new OutputCreator(result, membraneData);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;

            var js = JsonConvert.SerializeObject(output);
            ApplyNodesToDocument(document, nodes);
            ApplyTrianglesToDocument(document, elements);

            document.Save(filePath);
        }
    }
}