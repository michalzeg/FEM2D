using Common.Point;
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
        private Dof dof;

        public int Number { get; internal set; }
        public PointD Coordinates { get; private set; }
        public Restraint Restraint { get; internal set; }

        internal Node(PointD coordinates,int number,IDofNumberCalculator dofCalculator, Restraint restraint = Restraint.Free)
        {
            Condition.Requires(coordinates);

            this.dof = new Dof(dofCalculator);

            this.Number = number;

            this.Coordinates = coordinates;
            this.Restraint = restraint;
            
        }
        internal Node(double x, double y,int number,IDofNumberCalculator dofCalculator, Restraint restraint = Restraint.Free)
            :this(new PointD(x,y),number,dofCalculator,restraint)
        {
            
        }

        public void SetMembraneDofs()
        {
            this.dof.SetUxDof();
            this.dof.SetUyDof();

        }
        public void SetBeamDofs()
        {
            this.SetMembraneDofs();
            this.dof.SetRzDof();
        }

        public int[] GetDOF()
        {
            return this.dof.GetDofs();
        }
        public int TryGetRotationDOF()
        {
            var dofs = this.GetDOF();
            var result = dofs.Length == 3 ? dofs[2] : -1;
            return result;
        }

        public double DistanceTo(Node node)
        {
            var dx = this.Coordinates.X - node.Coordinates.X;
            var dy = this.Coordinates.Y - node.Coordinates.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        public void SetFixedSupport()
        {
            this.Restraint = Restraint.Fixed;
        }

        public void SetPinnedSupport()
        {
            this.Restraint = Restraint.FixedX | Restraint.FixedY;
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
