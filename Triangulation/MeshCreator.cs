using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleNet.Geometry;
using TriangleNet.Smoothing;
using TriangleNet.Algorithm;
using TriangleNet;
using TriangleNet.Data;
using Common.DTO;
using Triangulation.Extensions;

namespace Triangulation
{
    public class MeshCreator
    {
        private const double areaFactor = 0.01;

        public IEnumerable<TriangleGeometry> CreateMesh(MembraneInputData membraneData)
        {
            var geometry = GetGeometry(membraneData);
            var area = GetArea(geometry);
            var mesh = GetMesh(area);

            mesh.Triangulate(geometry);
            mesh.Smooth();
            mesh.Refine();
            
            var triangles = mesh.Triangles.Select(e => e.ToTriangleGeometry());

            return triangles;

        }

        private static InputGeometry GetGeometry(MembraneInputData membraneGeometry)
        {
            var vertexCount = membraneGeometry.Vertices.Count;

            var geometry = new InputGeometry(vertexCount);

            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = membraneGeometry.Vertices[i];
                geometry.AddPoint(vertex.X, vertex.Y);
            }

            for (int i = 0; i < vertexCount - 1; i++)
            {
                geometry.AddSegment(i, i + 1, 1);
            }
            geometry.AddSegment(vertexCount - 1, 0);
            return geometry;
        }

        private static Mesh GetMesh(double area)
        {
            Mesh mesh = new Mesh();
            mesh.Behavior.Quality = true;
            mesh.Behavior.MaxArea = area*areaFactor;

            mesh.Behavior.ConformingDelaunay = true;
            return mesh;
        }

        private static double GetArea(InputGeometry geoemtry)
        {
            var boundingBox = geoemtry.Bounds;
            var area = boundingBox.Height * boundingBox.Width;
            return area;
        }
    }
}
