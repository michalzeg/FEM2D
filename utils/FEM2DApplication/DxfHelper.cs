using netDxf;
using netDxf.Entities;
using System.Collections.Generic;
using FEM2D.Elements;
using static FEM2DApplication.MockDataProvider;
using static FEM2DApplication.DxfHelper;

namespace FEM2DApplication
{
    public class DxfHelper
    {
        public static void ApplyTrianglesToDocument(DxfDocument document, IEnumerable<IElement> elements)
        {
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
                var content = string.Format("N1:{0} N2:{1} N3:{2}", node1, node2, node3);

                var face = new Face3d(new Vector2(x1, y1),
                    new Vector2(x2, y2),
                    new Vector2(x3, y3));
                var text = new Text(content, new Vector2(xc, yc), 0.05);
                document.AddEntity(text);
                document.AddEntity(face);
            }
        }

        public static void ApplyNodesToDocument(DxfDocument document, System.Collections.Generic.IEnumerable<FEM2D.Nodes.Node> nodes)
        {
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
        }
    }
}