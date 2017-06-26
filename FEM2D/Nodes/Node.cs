using Common.Point;
using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Nodes
{
    internal class Node : IEquatable<Node>
    {
        private static int counter = 1;

        public int Number { get; private set; }
        public PointD Coordinates { get; private set; }

        public Node( PointD coordinates)
        {
            Condition.Requires(coordinates);

            this.Number = counter;
            counter++;
            this.Coordinates = coordinates;
            
        }



        public override bool Equals(object obj)
        {
            var other = obj as Node;
            if (other == null)
                return false;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.Number.GetHashCode() ^ this.Coordinates.GetHashCode();
        }
        public bool Equals(Node other)
        {
            return (this.Number == other.Number
                && this.Coordinates == other.Coordinates);
        }
        public static bool operator ==(Node node1, Node node2)
        {
            return node1.Equals(node2);
        }
        public static bool operator !=(Node node1, Node node2)
        {
            return !(node1 == node2);
        }
    }
}
