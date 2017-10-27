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
    public class BoundaryProvider : IBoundaryProvider
    {

        public Vector<double> BundaryVector { get; private set; }

        public BoundaryProvider()
        {

        }

        public void CreateVector(IEnumerable<Node> nodes, int dofNumber)
        {
            if (this.BundaryVector == null)
            {
                this.BundaryVector = Vector.Build.Sparse(dofNumber,1);

                var fixedNodes = nodes.Where(e => e.Restraint != Restraint.Free);

                foreach (var node in nodes)
                {
                    var dofs = node.GetDOF();
                    
                    if (node.Restraint.HasFlag(Restraint.FixedX))
                    {
                        var dof = dofs[0];
                        this.BundaryVector[dof] = 0;
                    }
                    if (node.Restraint.HasFlag(Restraint.FixedY))
                    {
                        var dof = dofs[1];
                        this.BundaryVector[dof] = 0;
                    }
                }
            }
        }

        public void Reduce(Matrix<double> matrix,Vector<double> vector)
        {

            var dofsToReduce = this.BundaryVector
                .Select((e, i) => new { value = e, index = i })
                .Where(e => e.value == 0)
                .Select(e => e.index);

            foreach (var dof in dofsToReduce)
            {
                matrix.ClearColumn(dof);

                matrix.ClearRow(dof);

                matrix[dof, dof] = 1;

                vector[dof] = 0;
            }
        }


    }
}
