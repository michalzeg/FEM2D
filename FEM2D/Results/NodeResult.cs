using FEM2D.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results
{
    public class NodeResult
    {
        public Node Node { get; internal set; }
        public double UX { get; internal set; }
        public double UY { get; internal set; }
        public double AverageSigmaXX { get; internal set; }
        public double AverageSigmaYY { get; internal set; }
        public double AverageTauXY { get; internal set; }
    }
}
