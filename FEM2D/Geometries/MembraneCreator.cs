using Common.DTO;
using Common.Point;
using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Restraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace FEM2D.Structures
{
    public class MembraneCreator
    {
        private readonly MeshCreator meshCreator;
        private readonly NodeFactory nodeFactory;
        private readonly ElementFactory elementFactory;
        private readonly LoadFactory loadFactory;


        private IEnumerable<TriangleGeometry> triangles;
        private MembraneInputData membraneData;

        

        private Dictionary<PointD, Node> vertexNodeMap;

        public MembraneCreator(NodeFactory nodeFactory, ElementFactory elementFactory, LoadFactory loadFactory)
        {
            this.meshCreator = new MeshCreator();
            

            this.nodeFactory = nodeFactory;
            this.elementFactory = elementFactory;
            this.loadFactory = loadFactory;
        }

        public void CreateGeometry(MembraneInputData membraneData)
        {
            this.triangles = this.meshCreator.CreateMesh(membraneData);
            this.membraneData = membraneData;

            this.CreateNodes();
            this.CreateTriangleElements();
            this.ApplyNodalLoads();
            this.ApplySupports();
            this.ApplyElementLoads();
        }

        private void CreateNodes()
        {
            var vertices = this.triangles.Select(e => new[] { e.Vertex1, e.Vertex2, e.Vertex3 })
                .SelectMany(e => e).Distinct();

            var nodes = vertices.Select(vertex => this.nodeFactory.Create(vertex)).ToList();
            this.vertexNodeMap = nodes.ToDictionary(e => e.Coordinates, f => f);
        }

        private void CreateTriangleElements()
        {
            foreach (var triangle in this.triangles)
            {
                var node1 = this.vertexNodeMap[triangle.Vertex1];
                var node2 = this.vertexNodeMap[triangle.Vertex2];
                var node3 = this.vertexNodeMap[triangle.Vertex3];
                var triangleElement = this.elementFactory.CreateTriangle(node1, node2, node3, this.membraneData.Properties);

            }
        }

        private void ApplyNodalLoads()
        {
            var verticesWithLoad = this.membraneData.Vertices.Where(e => e.LoadX != 0 || e.LoadY != 0);

            foreach (var vertex in verticesWithLoad)
            {
                var point = new PointD(vertex.X, vertex.Y);
                var node = this.vertexNodeMap[point];
                this.loadFactory.AddNodalLoad(node, vertex.LoadX, vertex.LoadY);
            }
        }

        private void ApplyElementLoads()
        {

        }

        private void ApplySupports()
        {
            var verticesWithSupport = this.membraneData.Vertices.Where(
                e => e.SupportX || e.SupportY);
            foreach (var vertex in verticesWithSupport)
            {
                var point = new PointD(vertex.X, vertex.Y);
                var node = this.vertexNodeMap[point];
                if (vertex.SupportX)
                    node.Restraint |= Restraint.FixedX;
                if (vertex.SupportY)
                    node.Restraint |= Restraint.FixedY;
            }
        }
    }
}
