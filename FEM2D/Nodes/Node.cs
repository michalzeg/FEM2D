﻿using Common.Point;
using CuttingEdge.Conditions;
using FEM2D.Restraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Nodes
{
    public class Node : IEquatable<Node>
    {
        public int Number { get; internal set; }
        public PointD Coordinates { get; private set; }
        public Restraint Restraint { get; internal set; }

        internal Node(PointD coordinates,int number,Restraint restraint = Restraint.Free)
        {
            Condition.Requires(coordinates);

            this.Number = number;

            this.Coordinates = coordinates;
            this.Restraint = restraint;
            
        }
        internal Node(double x, double y,int number,Restraint restraint = Restraint.Free)
            :this(new PointD(x,y),number,restraint)
        {
            
        }

        public int[] GetDOF()
        {
            var result = new[] { 2*Number - 2, 2*Number -1};
            return result;
        }

        public double DistanceTo(Node node)
        {
            var dx = this.Coordinates.X - node.Coordinates.X;
            var dy = this.Coordinates.Y - node.Coordinates.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
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
            return this.Coordinates.GetHashCode();
        }
        public bool Equals(Node other)
        {
            return this.Coordinates == other.Coordinates;
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
