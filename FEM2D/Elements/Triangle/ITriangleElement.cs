using Common.DTO;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements
{
    public interface ITriangleElement
    {
        double Area { get; }
        MembraneProperties Properties { get; }
        Node[] Nodes { get; }
        int Number { get; }
        int NumberOfDOFs { get; }

        Matrix<double> GetB();
        Matrix<double> GetD();
        Matrix<double> GetK();
        int[] GetDOFs();
    }
}