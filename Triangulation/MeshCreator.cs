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

namespace Triangulation
{
    public class MeshCreator
    {

        public IEnumerable<Triangle> Create()
        {
            var numPoints = 6;

            var geometry = new InputGeometry(numPoints);
            geometry.AddPoint(0, 0);
            //geometry.AddPoint(5, 0);

            geometry.AddPoint(10, 0);
            //geometry.AddPoint(10, 5);

            geometry.AddPoint(10, 5);
            //geometry.AddPoint(5, 10);
            geometry.AddPoint(15, 5);
            geometry.AddPoint(15, 10);
            geometry.AddPoint(0, 10);
            //geometry.AddPoint(0, 5);

            for (int i = 0; i < numPoints-1; i++)
            {
                geometry.AddSegment(i, i + 1, 1);
            }

            geometry.AddSegment(numPoints - 1, 0);

            geometry.AddSegment(0, 1,1);
            geometry.AddSegment(1, 2,1);
            geometry.AddSegment(2, 3,1);
            geometry.AddSegment(3, 4,1);
            geometry.AddSegment(4, 5, 1);
            geometry.AddSegment(5, 6, 1);
            geometry.AddSegment(6, 7, 1);
            geometry.AddSegment(7, 0, 1);

            
            Mesh mesh = new Mesh();
            mesh.Behavior.Quality = true;
            mesh.Behavior.MaxArea = 0.1;
            //mesh.Behavior.MaxAngle = 60;
            //mesh.Behavior.MinAngle = 30;
            mesh.Behavior.ConformingDelaunay = true;
            mesh.Triangulate(geometry);
            mesh.Smooth();
            mesh.Refine();
            return mesh.Triangles;
 
        }
    }
}
