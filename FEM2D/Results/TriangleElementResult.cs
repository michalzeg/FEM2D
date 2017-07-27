﻿using FEM2D.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Results
{
    public class TriangleElementResult
    {
        public ITriangleElement Element { get; internal set; }
        public double SigmaX { get; internal set; }
        public double SigmaY { get; internal set; }
        public double TauXY { get; internal set; }
    }
}
