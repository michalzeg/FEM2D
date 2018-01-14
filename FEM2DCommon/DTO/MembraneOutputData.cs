using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.DTO
{
    //To be moved to web api
    public class MembraneOutputData
    {
        public MembraneInputData InputData { get; set; }

        public IEnumerable<NodeOutput> Nodes { get; set; }
        public IEnumerable<TriangleOutput> Triangles { get; set; }

        public double MaxSxx {get; set;}
        public double MaxSyy {get; set;}
        public double MaxTxy {get; set;}
        public double MinSxx {get; set;}
        public double MinSyy {get; set;}
        public double MinTxy {get; set;}

        public double SxxPercentile095 { get; set; }
        public double SxxPercentile005 { get; set; }

        public double SyyPercentile095 { get; set; }
        public double SyyPercentile005 { get; set; }

        public double TxyPercentile095 { get; set; }
        public double TxyPercentile005 { get; set; }

        public double MaxUx  {get; set;}
        public double MaxUy  {get; set;}
    }
}
