using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEM2D.Nodes;
using MathNet.Numerics.LinearAlgebra;
using FEM2D.Elements.Beam;
using Common.DTO;

namespace FEM2DDynamics.Elements.Beam
{
    public class DynamicBeamElement : BeamElement, IDynamicBeamElement
    {
        internal DynamicBeamElement(Node node1, Node node2, BeamProperties beamProperties, int number)
            :base(node1,node2,beamProperties,number)
        {

        }
        internal DynamicBeamElement(IBeamElement beamElement)
            :base(beamElement.Nodes[0],beamElement.Nodes[1],beamElement.BeamProperties,beamElement.Number)
        {

        }

        public Matrix<double> GetMassMatrix()
        {
            throw new NotImplementedException();
        }
    }
}
