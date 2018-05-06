using FEM2D.Nodes;
using FEM2D.Restraints;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Solvers
{
    public class MatrixReducer : IMatrixReducer
    {
        private Vector<double> bundaryVector;
        private IEnumerable<int> dofsToReduce;

        public MatrixReducer()
        {
        }

        public void Initialize(IEnumerable<Node> nodes, int dofCount)
        {
            this.bundaryVector = Vector.Build.Sparse(dofCount, 1);

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
            this.dofsToReduce = this.bundaryVector
                .Select((e, i) => new { value = e, index = i })
                .Where(e => e.value == 0)
                .Select(e => e.index);
        }

        public Matrix<double> ReduceMatrix(Matrix<double> matrix)
        {
            var reducedMatrix = matrix.Clone();

            foreach (var dof in dofsToReduce)
            {
                reducedMatrix.ClearColumn(dof);

                reducedMatrix.ClearRow(dof);

                reducedMatrix[dof, dof] = 1;
            }
            return reducedMatrix;
        }

        public Vector<double> ReduceVector(Vector<double> vector)
        {
            var reducedVector = vector.Clone();

            foreach (var dof in dofsToReduce)
            {
                reducedVector[dof] = 0;
            }

            return reducedVector;
        }
    }
}