using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class MembraneData
    {
        public IList<Vertex> Vertices { get; set; }
        public IList<Edge> Edges { get; set; }

        public MembraneProperties Properties { get; set; }
    }
}
