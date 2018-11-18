using Common.Geometry;
using CuttingEdge.Conditions;
using FEM2D.Nodes.Dofs;
using FEM2D.Restraints;
using System;

namespace FEM2D.Nodes
{
    public class Node : IEquatable<Node>
    {
        private Dof dof;

        public int Number { get; internal set; }
        public PointD Coordinates { get; private set; }
        public Restraint Restraint { get; internal set; }

        internal Node(PointD coordinates, int number, IDofNumberCalculator dofCalculator, Restraint restraint = Restraint.Free)
        {
            Condition.Requires(coordinates);

            this.dof = new Dof(dofCalculator);

            this.Number = number;

            this.Coordinates = coordinates;
            this.Restraint = restraint;
        }

        internal Node(double x, double y, int number, IDofNumberCalculator dofCalculator, Restraint restraint = Restraint.Free)
            : this(new PointD(x, y), number, dofCalculator, restraint)
        {
        }

        public void SetTranslationDofs()
        {
            this.dof.SetUxDof();
            this.dof.SetUyDof();
        }

        public void SetRotationDofs()
        {
            this.SetTranslationDofs();
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

        public double DistanceTo(Node node) => this.Coordinates.DistanceTo(node.Coordinates);

        public void SetFixedSupport()
        {
            this.SetRestraint(Restraint.Fixed);
        }

        public void SetPinnedSupport()
        {
            this.SetRestraint(Restraint.FixedX | Restraint.FixedY);
        }

        public void SetRestraint(Restraint restraint)
        {
            this.Restraint = restraint;
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