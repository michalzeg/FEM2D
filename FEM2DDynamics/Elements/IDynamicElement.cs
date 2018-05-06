using FEM2D.Elements;
using FEM2DDynamics.Solver;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2DDynamics.Elements
{
    public interface IDynamicElement : IElement
    {
        Matrix<double> GetMassMatrix();

        Matrix<double> GetDampingMatrix();

        void UpdateDampingFactors(IDampingFactors dampingFactors);
    }
}