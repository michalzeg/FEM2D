using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class TriangleOutput
    {
        public int Number { get; set; }

        public IEnumerable<NodeOutputDetailed> Nodes { get; set; }

        public double Sxx { get; set; }
        public double Syy { get; set; }
        public double Txy { get; set; }

    }
}
