using FEM2DCommon.DTO;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements.Beam
{
    public interface IBeamElement : IBarElement
    {
        Matrix<double> GetLocalStiffnessMatrix();
    }
}