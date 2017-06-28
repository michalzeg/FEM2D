using FEM2D.Nodes;
using FEM2D.Restraints;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Solvers
{
    public class BoundaryProvider
    {

        public void Cross(Matrix<double> stiffnessMatrix, Vector<double> loadVector, IEnumerable<Node> nodes)
        {
            var fixedNodes = nodes.Where(e => e.Restraint != Restraint.Free);

            foreach (var node in nodes)
            {
                var dofs = node.GetDOF();
                if (node.Restraint.HasFlag(Restraint.FixedX))
                {
                    var dof = dofs[0];
                    this.CrossVector(loadVector, dof);
                    this.CrossMatrix(stiffnessMatrix, dof);
                }
                if (node.Restraint.HasFlag(Restraint.FixedY))
                {
                    var dof = dofs[1];
                    this.CrossVector(loadVector, dof);
                    this.CrossMatrix(stiffnessMatrix, dof);
                }
               
            }
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
