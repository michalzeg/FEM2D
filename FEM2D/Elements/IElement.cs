using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Elements
{
    public interface IElement
    {
        Node[] Nodes { get; }
        Matrix<double> GetStiffnessMatrix();
        int[] GetDOFs();
        int Number { get; }
    }
}
