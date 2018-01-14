using FEM2DCommon.DTO;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements
{
    public interface ITriangleElement : IElement
    {
        double Area { get; }
        MembraneProperties Properties { get; }
        Matrix<double> GetB();
        Matrix<double> GetD();
    }
}