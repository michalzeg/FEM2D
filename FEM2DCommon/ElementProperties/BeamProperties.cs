﻿using FEM2DCommon.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.DTO
{
    public class BeamProperties
    {
        public double ModulusOfElasticity { get; set; }
        public Section Section { get; set; }

        public double Area => this.Section.SectionProperties.A;
        public double MomentOfInertia => this.Section.SectionProperties.Iy0;


        public static BeamProperties Default => new BeamProperties
        {
            ModulusOfElasticity = 200000000,
            Section = Section.FromRectangle(1,0.3),
        };
    }
}