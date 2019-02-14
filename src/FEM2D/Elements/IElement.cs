using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;

namespace FEM2D.Elements
{
    public interface IElement
    {
        INode[] Nodes { get; }

        Matrix<double> GetStiffnessMatrix();

        int[] GetDOFs();

        int Number { get; }

        object Tag { get; set; }
    }
}