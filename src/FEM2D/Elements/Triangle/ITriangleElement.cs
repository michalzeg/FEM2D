using FEM2DCommon.DTO;
using FEM2DCommon.ElementProperties;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements.Triangle
{
    public interface ITriangleElement : IElement
    {
        double Area { get; }
        MembraneProperties Properties { get; }

        Matrix<double> GetB();

        Matrix<double> GetD();
    }
}