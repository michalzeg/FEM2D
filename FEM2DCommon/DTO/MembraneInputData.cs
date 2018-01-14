using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.DTO
{
    public class MembraneInputData
    {
        public IList<VertexInput> Vertices { get; set; }
        public IList<Edge> Edges { get; set; }

        public MembraneProperties Properties { get; set; }
    }
}
