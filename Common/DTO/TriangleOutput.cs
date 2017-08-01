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

        public int Node0Number { get; set; }
        public int Node1Number { get; set; }
        public int Node2Number { get; set; }

        public double Node0X {get;set;}
        public double Node0Y {get;set;}
        public double Node1X {get;set;}
        public double Node1Y {get;set;}
        public double Node2X {get;set;}
        public double Node2Y { get; set; }


        public double AvgSxx0 { get; set; }
        public double AvgSyy0 { get; set; }
        public double AvgTxy0 { get; set; }

        public double AvgSxx1 { get; set; }
        public double AvgSyy1 { get; set; }
        public double AvgTxy1 { get; set; }

        public double AvgSxx2 { get; set; }
        public double AvgSyy2 { get; set; }
        public double AvgTxy2 { get; set; }

        public double Sxx { get; set; }
        public double Syy { get; set; }
        public double Txy { get; set; }

    }
}
