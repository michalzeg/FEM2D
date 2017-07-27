using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using Triangulation;
using netDxf.Entities;
using Common.DTO;
using FEM2D.Structures;
using FEM2D.Nodes;
using FEM2D.Restraints;
using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Solvers;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ResultsToDxf();

        }

        private static void ResultsToJSon()
        {
            var material = new MembraneProperties
            {
                ModulusOfElasticity = 30 * Math.Pow(10, 6),// 210000000,
                PoissonsRation = 0.25,// 0.3
                Thickness = 0.5
            };

            var node1 = new Node(3, 0, Restraint.FixedY);
            var node2 = new Node(3, 2);
            var node3 = new Node(0, 2, Restraint.Fixed);
            var node4 = new Node(0, 0, Restraint.Fixed);
            var nodes = new[] { node1, node2, node3, node4 };

            var nodeLoad = new NodalLoad
            {
                Node = node2,
                ValueY = -1000,// -100000
            };
            var loads = new[] { nodeLoad };

            var element1 = new TriangleElement(node1, node2, node4, material);
            var element2 = new TriangleElement(node3, node4, node2, material);
            var elements = new[] { element1, element2 };


            var solver = new Solver();
            solver.Solve(elements, nodes, loads);
            var results = solver.Results;
        }

        private static void ResultsToDxf()
        {
            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();

            var vertex1 = new Vertex
            {
                Number = 1,
                X = 0,
                Y = 0,
                SupportX = true,
                SupportY = true,
            };
            var vertex2 = new Vertex
            {
                Number = 2,
                X = 20,
                Y = 30,
                LoadY = -100,
            };
            var vertex3 = new Vertex
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

            var membraneData = new MembraneData
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

            var membrane = new Membrane();
            membrane.Solve(membraneData);

            var nodes = membrane.Geometry.Nodes;
            var elements = membrane.Geometry.Elements;

            var result = membrane.Results;


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
    }
}
