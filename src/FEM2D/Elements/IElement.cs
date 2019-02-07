using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements
{
    public interface IElement
    {
        Node[] Nodes { get; }

        Matrix<double> GetStiffnessMatrix();

        int[] GetDOFs();

        int Number { get; }

        string Tag { get; set; }
    }
}