using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using Triangulation;
using netDxf.Entities;
using FEM2DCommon.DTO;
using FEM2D.Structures;
using FEM2D.Nodes;
using FEM2D.Restraints;
using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Solvers;
using Output.OutputCreator;
using Newtonsoft.Json;
using MathNet.Numerics.Statistics;
using System.Diagnostics;
using FEM2DCommon.ElementProperties;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //ResultsToJSon();
            ResultsToDxfCantilever();
            //ResultsToDxfMembrane();
            //ResultsToDxfMembrane1x1();
            //AppTest();

            var sectionProperties = BeamPropertiesBuilder.Create()
                .SetSteel()
                .SetRectangularSection(20, 20)
                .Build();

        }

        private static void ResultsToJSon()
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



            var solver = new Solver();
            solver.Solve(elements, nodes, loads);
            var results = solver.Results;

            var outputCrator = new OutputCreator(results);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;
            var js = JsonConvert.SerializeObject(output);

        }

        private static void ResultsToDxfCantilever()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();

            var vertex1 = new VertexInput
            {
                Number = 1,
                X = 0,
                Y = 0,
                SupportX = true,
                SupportY = true,
            };
            var vertex2 = new VertexInput
            {
                Number = 2,
                X = 20,
                Y = 30,
                LoadY = -100,
            };
            var vertex3 = new VertexInput
            {
                Number = 3,
                X = 0,
                Y = 10,
                SupportX = true,
                SupportY = true,
            };

            var edge1 = new Edge
            {
                Number = 1,
                Start = vertex1,
                End = vertex2,
            };
            var edge2 = new Edge
            {
                Number = 2,
                Start = vertex2,
                End = vertex3,
            };
            var edge3 = new Edge
            {
                Number = 3,
                Start = vertex3,
                End = vertex1,

            };

            var membraneData = new MembraneInputData
            {
                Vertices = new[] { vertex1, vertex2, vertex3 },
                Edges = new[] { edge1, edge2, edge3 },
                Properties = new MembraneProperties
                {
                    ModulusOfElasticity = 200000000,
                    PoissonsRation = 0.25,
                    Thickness = 0.5
                },
            };
            var time = new Stopwatch();
            time.Start();

            var structure = new Structure();
            structure.AddMembraneGeometry(membraneData);
            structure.Solve();

            var nodes = structure.NodeFactory.GetAll();
            var elements = structure.ElementFactory.GetAll();

            var result = structure.Results;

            var outputCrator = new OutputCreator(result,membraneData);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;

            time.Stop();
            var e = time.Elapsed;

            var js = JsonConvert.SerializeObject(output);

            foreach (var node in nodes)
            {
                var x = node.Coordinates.X;
                var y = node.Coordinates.Y;
                var z = 0;

                var point = new Point(x, y, z);
                document.AddEntity(point);
                var text = new Text(node.Number.ToString(), new Vector3(x, y, z), 0.1);
                text.Color = AciColor.Red;
                document.AddEntity(text);
            }

            foreach (var triangle in elements)
            {


                var x1 = triangle.Nodes[0].Coordinates.X;
                var y1 = triangle.Nodes[0].Coordinates.Y;

                var x2 = triangle.Nodes[1].Coordinates.X;
                var y2 = triangle.Nodes[1].Coordinates.Y;

                var x3 = triangle.Nodes[2].Coordinates.X;
                var y3 = triangle.Nodes[2].Coordinates.Y;

                var xc = (x1 + x2 + x3) / 3;
                var yc = (y1 + y2 + y3) / 3;


                var node1 = triangle.Nodes[0].Number;
                var node2 = triangle.Nodes[1].Number;
                var node3 = triangle.Nodes[2].Number;
                var t = string.Format("N1:{0} N2:{1} N3:{2}", node1, node2, node3);

                Face3d face = new Face3d(new Vector2(x1, y1),
                    new Vector2(x2, y2),
                    new Vector2(x3, y3));
                var text = new Text(t, new Vector2(xc, yc), 0.05);
                document.AddEntity(text);
                document.AddEntity(face);
            }

            document.Save(filePath);
        }

        private static void ResultsToDxfMembrane()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();

            var vertex1 = new VertexInput
            {
                Number = 1,
                X = 0,
                Y = 0,
                SupportX = true,
                SupportY = true,
            };
            var vertex2 = new VertexInput
            {
                Number = 2,
                X = 20,
                Y = 0,
                SupportX = true,
                SupportY = true,
            };
            var vertex3 = new VertexInput
            {
                Number = 3,
                X = 20,
                Y = 30,
                LoadX = 500,
                
            };
            var vertex4 = new VertexInput
            {
                Number = 4,
                X = 10,
                Y = 30,
                LoadY = -1000,

            };
            var vertex5 = new VertexInput
            {
                Number = 5,
                X = 0,
                Y = 30,

            };

            var edge1 = new Edge
            {
                Number = 1,
                Start = vertex1,
                End = vertex2,
            };
            var edge2 = new Edge
            {
                Number = 2,
                Start = vertex2,
                End = vertex3,
            };
            var edge3 = new Edge
            {
                Number = 3,
                Start = vertex3,
                End = vertex4,

            };
            var edge4 = new Edge
            {
                Number = 3,
                Start = vertex4,
                End = vertex5,

            };
            var edge5 = new Edge
            {
                Number = 3,
                Start = vertex5,
                End = vertex1,

            };

            var membraneData = new MembraneInputData
            {
                Vertices = new[] { vertex1, vertex2, vertex3,vertex4,vertex5 },
                Edges = new[] { edge1, edge2, edge3,edge4,edge5 },
                Properties = new MembraneProperties
                {
                    ModulusOfElasticity = 200000000,
                    PoissonsRation = 0.25,
                    Thickness = 0.5
                },
            };

            var structure = new Structure();
            structure.AddMembraneGeometry(membraneData);
            structure.Solve();

            var nodes = structure.NodeFactory.GetAll();
            var elements = structure.ElementFactory.GetAll();

            var result = structure.Results;

            var outputCrator = new OutputCreator(result,membraneData);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;

            var js = JsonConvert.SerializeObject(output);

            foreach (var node in nodes)
            {
                var x = node.Coordinates.X;
                var y = node.Coordinates.Y;
                var z = 0;

                var point = new Point(x, y, z);
                document.AddEntity(point);
                var text = new Text(node.Number.ToString(), new Vector3(x, y, z), 0.1);
                text.Color = AciColor.Red;
                document.AddEntity(text);
            }

            foreach (var triangle in elements)
            {


                var x1 = triangle.Nodes[0].Coordinates.X;
                var y1 = triangle.Nodes[0].Coordinates.Y;

                var x2 = triangle.Nodes[1].Coordinates.X;
                var y2 = triangle.Nodes[1].Coordinates.Y;

                var x3 = triangle.Nodes[2].Coordinates.X;
                var y3 = triangle.Nodes[2].Coordinates.Y;

                var xc = (x1 + x2 + x3) / 3;
                var yc = (y1 + y2 + y3) / 3;


                var node1 = triangle.Nodes[0].Number;
                var node2 = triangle.Nodes[1].Number;
                var node3 = triangle.Nodes[2].Number;
                var t = string.Format("N1:{0} N2:{1} N3:{2}", node1, node2, node3);

                Face3d face = new Face3d(new Vector2(x1, y1),
                    new Vector2(x2, y2),
                    new Vector2(x3, y3));
                var text = new Text(t, new Vector2(xc, yc), 0.05);
                document.AddEntity(text);
                document.AddEntity(face);
            }

            document.Save(filePath);
        }

        private static void ResultsToDxfMembrane1x1()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();

            var vertex1 = new VertexInput
            {
                Number = 1,
                X = 0,
                Y = 0,
                SupportX = true,
                SupportY = true,
            };
            var vertex2 = new VertexInput
            {
                Number = 2,
                X = 1,
                Y = 0,
                
            };
            var vertex3 = new VertexInput
            {
                Number = 3,
                X = 1,
                Y = 1,
                LoadY=-100000,
            };
            var vertex4 = new VertexInput
            {
                Number = 4,
                X = 0,
                Y = 1,
                SupportX = true,
                SupportY = true,
            };
            

            var edge1 = new Edge
            {
                Number = 1,
                Start = vertex1,
                End = vertex2,
            };
            var edge2 = new Edge
            {
                Number = 2,
                Start = vertex2,
                End = vertex3,
            };
            var edge3 = new Edge
            {
                Number = 3,
                Start = vertex3,
                End = vertex4,

            };
            var edge4 = new Edge
            {
                Number = 3,
                Start = vertex4,
                End = vertex1,

            };
            

            var membraneData = new MembraneInputData
            {
                Vertices = new[] { vertex1, vertex2, vertex3, vertex4 },
                Edges = new[] { edge1, edge2, edge3, edge4 },
                Properties = new MembraneProperties
                {
                    ModulusOfElasticity = 200000000,
                    PoissonsRation = 0.25,
                    Thickness = 0.5
                },
            };

            var structure = new Structure();
            structure.AddMembraneGeometry(membraneData);
            structure.Solve();

            var nodes = structure.NodeFactory.GetAll();
            var elements = structure.ElementFactory.GetAll();

            var result = structure.Results;

            var outputCrator = new OutputCreator(result,membraneData);
            outputCrator.CreateOutput();
            var output = outputCrator.Output;

            var js = JsonConvert.SerializeObject(output);

            foreach (var node in nodes)
            {
                var x = node.Coordinates.X;
                var y = node.Coordinates.Y;
                var z = 0;

                var point = new Point(x, y, z);
                document.AddEntity(point);
                var text = new Text(node.Number.ToString(), new Vector3(x, y, z), 0.1);
                text.Color = AciColor.Red;
                document.AddEntity(text);
            }

            foreach (var triangle in elements)
            {


                var x1 = triangle.Nodes[0].Coordinates.X;
                var y1 = triangle.Nodes[0].Coordinates.Y;

                var x2 = triangle.Nodes[1].Coordinates.X;
                var y2 = triangle.Nodes[1].Coordinates.Y;

                var x3 = triangle.Nodes[2].Coordinates.X;
                var y3 = triangle.Nodes[2].Coordinates.Y;

                var xc = (x1 + x2 + x3) / 3;
                var yc = (y1 + y2 + y3) / 3;


                var node1 = triangle.Nodes[0].Number;
                var node2 = triangle.Nodes[1].Number;
                var node3 = triangle.Nodes[2].Number;
                var t = string.Format("N1:{0} N2:{1} N3:{2}", node1, node2, node3);

                Face3d face = new Face3d(new Vector2(x1, y1),
                    new Vector2(x2, y2),
                    new Vector2(x3, y3));
                var text = new Text(t, new Vector2(xc, yc), 0.05);
                document.AddEntity(text);
                document.AddEntity(face);
            }

            document.Save(filePath);
        }

        private static void AppTest()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();

            var vertex1 = new VertexInput
            {
                Number = 1,
                X = 0,
                Y = 2000,
                SupportX = false,
                SupportY = false,
                LoadX = 500,
                LoadY = 1000,
            };
            var vertex2 = new VertexInput
            {
                Number = 2,
                X = 500,
                Y = 0,
                SupportX = true,
                SupportY = true,
                LoadX = 0,
                LoadY = 0,

            };
            var vertex3 = new VertexInput
            {
                Number = 3,
                X = 1500,
                Y = 0,
                SupportX = true,
                SupportY = true,
                LoadX = 0,
                LoadY = 0,
            };
            var vertex4 = new VertexInput
            {
                Number = 4,
                X = 2000,
                Y = 2000,
                SupportX = false,
                SupportY = false,
                LoadX = -500,
                LoadY = 1000,
            };
            var vertex5 = new VertexInput
            {
                Number = 5,
                X = 1000,
                Y = 2000,
                SupportX = false,
                SupportY = false,
                LoadX = 0,
                LoadY = -2000,
            };

            var edge1 = new Edge
            {
                Number = 1,
                Start = vertex1,
                End = vertex2,
            };
            var edge2 = new Edge
            {
                Number = 2,
                Start = vertex2,
                End = vertex3,
            };
            var edge3 = new Edge
            {
                Number = 3,
                Start = vertex3,
                End = vertex4,

            };
            var edge4 = new Edge
            {
                Number = 3,
                Start = vertex4,
                End = vertex5,

            };
            var edge5 = new Edge
            {
                Number = 5,
                Start = vertex5,
                End = vertex1,


            };

            var membraneData = new MembraneInputData
            {
                Vertices = new[] { vertex1, vertex2, vertex3, vertex4,vertex5 },
                Edges = new[] { edge1, edge2, edge3, edge4,edge5 },
                Properties = new MembraneProperties
                {
                    ModulusOfElasticity = 200000000,
                    PoissonsRation = 0.25,
                    Thickness = 200
                },
            };

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

            foreach (var node in nodes)
            {
                var x = node.Coordinates.X;
                var y = node.Coordinates.Y;
                var z = 0;

                var point = new Point(x, y, z);
                document.AddEntity(point);
                var text = new Text(node.Number.ToString(), new Vector3(x, y, z), 0.1)
                {
                    Color = AciColor.Red
                };
                document.AddEntity(text);
            }

            foreach (var triangle in elements)
            {


                var x1 = triangle.Nodes[0].Coordinates.X;
                var y1 = triangle.Nodes[0].Coordinates.Y;

                var x2 = triangle.Nodes[1].Coordinates.X;
                var y2 = triangle.Nodes[1].Coordinates.Y;

                var x3 = triangle.Nodes[2].Coordinates.X;
                var y3 = triangle.Nodes[2].Coordinates.Y;

                var xc = (x1 + x2 + x3) / 3;
                var yc = (y1 + y2 + y3) / 3;


                var node1 = triangle.Nodes[0].Number;
                var node2 = triangle.Nodes[1].Number;
                var node3 = triangle.Nodes[2].Number;
                var t = string.Format("N1:{0} N2:{1} N3:{2}", node1, node2, node3);

                var face = new Face3d(new Vector2(x1, y1),
                    new Vector2(x2, y2),
                    new Vector2(x3, y3));
                var text = new Text(t, new Vector2(xc, yc), 0.05);
                document.AddEntity(text);
                document.AddEntity(face);
            }

            document.Save(filePath);
        }
    }
}
