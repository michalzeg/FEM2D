﻿using FEM2DDynamics.Results;

namespace FEM2DDynamics.Solver
{
    internal interface IEquationOfMotionSolver
    {
        void Solve();

        DynamicDisplacements Result { get; }
    }
}