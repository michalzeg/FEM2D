using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using Triangulation;
using netDxf.Entities;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            var filePath = @"D:\test.dxf";

            var document = new DxfDocument();

            var meshing = new MeshCreator();
            var triangles = meshing.Create();

            foreach (var triangle in triangles)
            {
                var n1 = triangle.GetVertex(0);
                var n2 = triangle.GetVertex(1);
                var n3 = triangle.GetVertex(2);

                var v1 = new Vector2(n1.X, n1.Y);
                var v2 = new Vector2(n2.X, n2.Y);
                var v3 = new Vector2(n3.X, n3.Y);

                Face3d face = new Face3d(v1, v2, v3);
                document.AddEntity(face);
            }

            document.Save(filePath);

        }
    }
}
