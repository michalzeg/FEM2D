using FEM2D.Elements;
using FEM2DDynamics.Solver;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Elements
{
    public interface IDynamicElement :IElement
    {
        Matrix<double> GetMassMatrix();
        Matrix<double> GetDampingMatrix();
        void UpdateDampingFactors(IDampingFactors dampingFactors);
    }
}
