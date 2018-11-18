using FEM2DCommon.DTO;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Elements.Truss
{
    public interface ITrussElement : IBarElement
    {
        Matrix<double> GetTransformMatrix();
    }
}