﻿using System;

namespace FEM2D.Restraints
{
    [Flags]
    public enum Restraint
    {
        Free = 0,
        FixedX = 2,
        FixedY = 4,
        FixedR = 8,
        Fixed = FixedX | FixedY,
    }
}