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
    public class Structure
    {
        private readonly Solver solver;
        private readonly MeshCreator meshCreator;
        private IList<NodalLoad> loads;


        public MembraneCreator Geometry { get; private set; }
        public NodeFactory NodeFactory { get; private set; }
        public ElementFactory ElementFactory { get; private set; }
        public ResultProvider Results { get; private set; }

        public Structure()
        {
            this.meshCreator = new MeshCreator();
            this.Geometry = new MembraneCreator();
            this.solver = new Solver();
            this.NodeFactory = new NodeFactory();
            this.ElementFactory = new ElementFactory();
        }

        public void Solve()
        {
            this.solver.Solve(this.ElementFactory, this.NodeFactory, this.loads);
            this.Results = this.solver.Results;
        }

        public void AddMembraneGeometry(MembraneInputData membraneData)
        {
            var triangles = this.meshCreator.CreateMesh(membraneData);
            this.Geometry.CreateGeometry(triangles, membraneData);
            this.loads = this.Geometry.NodalLoads;
        }
       
    }
}
