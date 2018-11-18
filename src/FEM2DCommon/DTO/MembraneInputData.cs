using FEM2DCommon.ElementProperties;
using System.Collections.Generic;

namespace FEM2DCommon.DTO
{
    public class MembraneInputData
    {
        public IList<VertexInput> Vertices { get; set; }
        public IList<Edge> Edges { get; set; }

        public MembraneProperties Properties { get; set; }
    }
}