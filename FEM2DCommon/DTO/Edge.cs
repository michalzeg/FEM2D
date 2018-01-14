using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class Edge
    {
        public int Number { get; set; }

        public VertexInput Start { get; set; }
        public VertexInput End { get; set; }

        public int LoadX { get; set; }
        public int LoadY { get; set; }
    }
}
