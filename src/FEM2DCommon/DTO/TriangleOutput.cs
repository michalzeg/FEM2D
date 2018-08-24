using System.Collections.Generic;

namespace FEM2DCommon.DTO
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