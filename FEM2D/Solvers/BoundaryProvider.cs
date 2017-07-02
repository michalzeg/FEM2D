using FEM2D.Nodes;
using FEM2D.Restraints;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Solvers
{
    public class BoundaryVector
    {
        private readonly IEnumerable<Node> nodes;
        private readonly int dofNumber;

        private Vector<double> bundaryVector;

        public BoundaryVector(IEnumerable<Node> nodes, int dofNumber)
        {
            this.nodes = nodes;
            this.dofNumber = dofNumber;
        }

        public Vector<double> GetVector()
        {
            if (this.bundaryVector == null)
            {
                this.bundaryVector = Vector.Build.Sparse(dofNumber,1);

                var fixedNodes = nodes.Where(e => e.Restraint != Restraint.Free);

                foreach (var node in nodes)
                {
                    var dofs = node.GetDOF();
                    
                    if (node.Restraint.HasFlag(Restraint.FixedX))
                    {
                        var dof = dofs[0];
                        this.bundaryVector[dof] = 0;
                    }
                    if (node.Restraint.HasFlag(Restraint.FixedY))
                    {
                        var dof = dofs[1];
                        this.bundaryVector[dof] = 0;
                    }
                }
            }
            return this.bundaryVector;
        }

        private void CrossMatrix(Matrix<double> stiffnessMatrix,int dof)
        {
                stiffnessMatrix.ClearColumn(dof);
  
                stiffnessMatrix.ClearRow(dof);

                stiffnessMatrix[dof, dof] = 1;

        }
        private void CrossVector(Vector<double> loadVector, int dof)
        {
            loadVector[dof] = 0;
        }
    }
}
