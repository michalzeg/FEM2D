using FEM2D.Elements;
using FEM2D.Elements.Triangle;
using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Loads
{
    public class TriangleEdgeLoad
    {
        private ITriangleElement element;
        private Node node1;
        private Node node2;

        private double valueX;
        private double valueY;

        public TriangleEdgeLoad(ITriangleElement element, Node node1, Node node2, double valueX, double valueY)
        {
            if (!element.Nodes.Contains(node1) || !element.Nodes.Contains(node2))
                throw new ArgumentException("Wrong nodes provided for edge load");

            this.element = element;
            this.node1 = node1;
            this.node2 = node2;
            this.valueX = valueX;
            this.valueY = valueY;
        }

        public IEnumerable<NodalLoad> GetNodalLoads()
        {
            var length = node1.DistanceTo(node2);

            var loadX = 0.5 * this.valueX * length;
            var loadY = 0.5 * this.valueY * length;

            var load1 = new NodalLoad(this.node1, loadX, loadY);

            var load2 = new NodalLoad(this.node2, loadX, loadY);

            return new[] { load1, load2 };
        }
    }
}