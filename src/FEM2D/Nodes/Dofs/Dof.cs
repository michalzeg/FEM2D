using System;
using System.Linq;

namespace FEM2D.Nodes.Dofs
{
    /// <summary>
    /// class representing degree of freedom of a node
    /// </summary>
    internal class Dof
    {
        private int? dofUx;
        private int? dofUy;
        private int? dofRz;

        private IDofNumberCalculator dofCalculator;

        public Dof(IDofNumberCalculator dofCalculator)
        {
            this.dofCalculator = dofCalculator;

            this.SetUxDof = this.SetUxDofAction;
            this.SetUyDof = this.SetUyDofAction;
            this.SetRzDof = this.SetRzDofAction;
        }

        public int[] GetDofs()
        {
            var dofs = new[] { this.dofUx, this.dofUy, this.dofRz }
                             .Where(e => e.HasValue)
                             .Select(e => e.Value)
                             .ToArray();
            return dofs;
        }

        public Action SetUxDof { get; private set; }

        public Action SetUyDof { get; private set; }
        public Action SetRzDof { get; private set; }

        private void SetUxDofAction()
        {
            this.dofUx = this.dofCalculator.GetFreeDOFNumber();
            this.SetUxDof = this.EmptyAction;
        }

        private void SetUyDofAction()
        {
            this.dofUy = this.dofCalculator.GetFreeDOFNumber();
            this.SetUyDof = this.EmptyAction;
        }

        private void SetRzDofAction()
        {
            this.dofRz = this.dofCalculator.GetFreeDOFNumber();
            this.SetRzDof = this.EmptyAction;
        }

        private void EmptyAction()
        {
        }
    }
}