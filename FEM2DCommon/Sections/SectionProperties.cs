using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.Sections
{
    public class SectionProperties
    {
        public double A   {get;set;}
        public double Sx  {get;set;}
        public double Sy  {get;set;}
        public double Ix  {get;set;}
        public double Iy  {get;set;}
        public double Ixy {get;set;}
        public double X0  {get;set;}
        public double Y0  {get;set;}
                        
        public double Ix0 {get;set;}
        public double Iy0 {get;set;}
        public double Ixy0{get;set;}
                     
        public double I1  {get;set;}
        public double I2 { get; set; }

        public double DX0_max {get; set;}
        public double DX0_min {get; set;}
        public double DY0_max {get; set;}
        public double DY0_min {get; set;}
        public double DXI_max {get; set;}
        public double DXI_min {get; set;}
        public double DYI_max {get; set;}
        public double DYI_min { get; set; }
    }
}
