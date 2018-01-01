using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sections
{
    public class SectionProperties
    {
        public double F   {get;set;}
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

        public double X0_max {get; set;}
        public double X0_min {get; set;}
        public double Y0_max {get; set;}
        public double Y0_min {get; set;}
        public double XI_max {get; set;}
        public double XI_min {get; set;}
        public double YI_max {get; set;}
        public double YI_min { get; set; }
    }
}
