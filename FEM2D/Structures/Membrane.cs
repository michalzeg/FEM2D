using Common.DTO;
using FEM2D.Elements;
using FEM2D.Loads;
using FEM2D.Nodes;
using FEM2D.Results;
using FEM2D.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace FEM2D.Structures
{
    public class Membrane
    {
        private readonly Solver solver;
        private readonly MeshCreator meshCreator;
        public MembraneGeometry Geometry { get; private set; }

        public ResultProvider Results { get; private set; }

        public Membrane()
        {
            this.meshCreator = new MeshCreator();
            this.Geometry = new MembraneGeometry();
            this.solver = new Solver();
        }

        public void Solve(MembraneInputData membraneData)
        {
            var triangles = this.meshCreator.CreateMesh(membraneData);
            this.Geometry.CreateGeometry(triangles, membraneData);

            this.solver.Solve(this.Geometry);
            this.Results = this.solver.Results;
        }

       
    }
}
