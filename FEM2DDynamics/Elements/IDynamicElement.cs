using FEM2D.Elements;
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
    }
}
