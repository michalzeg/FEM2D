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

namespace FEM2D.Structures
{
    public class MembraneGeometry
    {
        public IEnumerable<Node> Nodes { get; private set; }
        public IEnumerable<ITriangleElement> Elements { get; private set; }
        public IList<NodalLoad> NodalLoads { get; private set; }

        private IEnumerable<TriangleGeometry> triangles;
        private MembraneInputData membraneData;
        private NodeFactory nodeFactory;


        private Dictionary<PointD, Node> vertexNodeMap;

        public MembraneGeometry()
        {
            this.NodalLoads = new List<NodalLoad>();
            this.Elements = new List<ITriangleElement>();
            this.Nodes = new List<Node>();

            this.nodeFactory = new NodeFactory();
        }

        public void CreateGeometry(IEnumerable<TriangleGeometry> triangles, MembraneInputData membraneData)
        {
            this.triangles = triangles;
            this.membraneData = membraneData;

            this.CreateNodes();
            this.CreateTriangleElements();
            this.ApplyNodalLoads();
            this.ApplySupports();
            this.ApplyElementLoads();
        }

        private void CreateNodes()
        {
            var vertices = triangles.Select(e => new[] { e.Vertex1, e.Vertex2, e.Vertex3 })
                .SelectMany(e => e).Distinct();

            this.Nodes = vertices.Select(vertex => this.nodeFactory.Create(vertex)).ToList();
            this.vertexNodeMap = this.Nodes.ToDictionary(e => e.Coordinates, f => f);
        }

        private void CreateTriangleElements()
        {
            var triangleElements = new List<TriangleElement>();
            var counter = 1;
            foreach (var triangle in this.triangles)
            {
                var node1 = this.vertexNodeMap[triangle.Vertex1];
                var node2 = this.vertexNodeMap[triangle.Vertex2];
                var node3 = this.vertexNodeMap[triangle.Vertex3];
                var triangleElement = new TriangleElement(node1, node2, node3, this.membraneData.Properties,counter);
                counter++;
                triangleElements.Add(triangleElement);
            }
            this.Elements = triangleElements;
        }

        private void ApplyNodalLoads()
        {
            var verticesWithLoad = this.membraneData.Vertices.Where(e => e.LoadX != 0 || e.LoadY != 0);

            foreach (var vertex in verticesWithLoad)
            {
                var point = new PointD(vertex.X, vertex.Y);
                var node = this.vertexNodeMap[point];
                this.NodalLoads.Add(new NodalLoad
                {
                    Node = node,
                    ValueX = vertex.LoadX,
                    ValueY = vertex.LoadY,
                });
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
