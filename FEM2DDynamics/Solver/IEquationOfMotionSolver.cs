using FEM2D.Solvers;
using FEM2DDynamics.Loads;
using FEM2DDynamics.Matrix;
using FEM2DDynamics.Results;

namespace FEM2DDynamics.Solver
{
    internal interface IEquationOfMotionSolver
    {
        DynamicDisplacements Solve();
    }
}