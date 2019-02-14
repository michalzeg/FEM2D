using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Elements
{
    public interface IBarElement : IElement
    {
        BarProperties BarProperties { get; }
        double Length { get; }

        Matrix<double> GetTransformMatrix();
    }
}