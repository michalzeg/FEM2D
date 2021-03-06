﻿using FEM2D.Structures;
using FEM2DCommon.DTO;
using netDxf;
using netDxf.Entities;
using Newtonsoft.Json;
using System.Diagnostics;
using static FEM2DApplication.MockDataProvider;
using static FEM2DApplication.DxfHelper;
using FEM2DOutput.OutputCreator;

namespace FEM2DApplication
{
    public class DxfCantilever
    {
        public static void ResultsToDxfCantilever()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();
            var membraneData = GetMembraneData();

            var time = new Stopwatch();
            time.Start();

            var structure = new Structure();
            structure.AddMembraneGeometry(membraneData);
            structure.Solve();

            var nodes = structure.NodeFactory.GetAll();
            var elements = structure.ElementFactory.GetAll();

            var result = structure.Results;

            var outputCrator = new OutputCreator(result, membraneData);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;

            time.Stop();
            var check = time.Elapsed;

            var js = JsonConvert.SerializeObject(output);

            ApplyNodesToDocument(document, nodes);
            ApplyTrianglesToDocument(document, elements);

            document.Save(filePath);
        }
    }
}