﻿using FEM2D.Structures;
using netDxf;
using Newtonsoft.Json;

using static FEM2DApplication.MockDataProvider;
using static FEM2DApplication.DxfHelper;
using FEM2DOutput.OutputCreator;

namespace FEM2DApplication
{
    public class ApplicationTest
    {
        public static void AppTest()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var membraneData = GetMembraneData();

            var membrane = new Structure();
            membrane.AddMembraneGeometry(membraneData);
            membrane.Solve();

            var nodes = membrane.NodeFactory.GetAll();
            var elements = membrane.ElementFactory.GetAll();

            var result = membrane.Results;

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