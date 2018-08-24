using Common.Geometry;
using FEM2DCommon.DTO;
using TriangleNet.Data;

namespace Triangulation.Extensions
{
    public static class TriangleExtensions
    {
        public static TriangleGeometry ToTriangleGeometry(this Triangle triangle)
        {
            var vertex1 = triangle.GetVertex(0).ToPointD();
            var vertex2 = triangle.GetVertex(1).ToPointD();
            var vertex3 = triangle.GetVertex(2).ToPointD();

            var result = new TriangleGeometry
            {
                Number = triangle.ID,
                Vertex1 = vertex1,
                Vertex2 = vertex2,
                Vertex3 = vertex3,
            };

            return result;
        }

        public static PointD ToPointD(this TriangleNet.Data.Vertex vertex)
        {
            var result = new PointD
            {
                X = vertex.X,
                Y = vertex.Y
            };
            return result;
        }
    }
}