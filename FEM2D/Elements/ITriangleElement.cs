using FEM2D.Materials;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements
{
    public interface ITriangleElement
    {
        double Area { get; }
        Material Material { get; }
        Node[] Nodes { get; }
        int Number { get; }
        double Thickness { get; }
        int NumberOfDOFs { get; }

        Matrix<double> GetB();
        Matrix<double> GetD();
        Matrix<double> GetK();
        int[] GetDOFs();
    }
}