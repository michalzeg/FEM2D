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
        
        private IList<NodalLoad> nodalLoads;

        public NodeFactory NodeFactory { get; private set; }
        public ElementFactory ElementFactory { get; private set; }
        public ResultFactory Results { get; private set; }

        public Structure()
        {
            this.solver = new Solver();
            this.NodeFactory = new NodeFactory();
            this.ElementFactory = new ElementFactory();
            this.nodalLoads = new List<NodalLoad>();
        }

        public void Solve()
        {
            this.solver.Solve(this.ElementFactory, this.NodeFactory, this.nodalLoads);
            this.Results = this.solver.Results;
        }

        public void AddMembraneGeometry(MembraneInputData membraneData)
        {
            var membraneCreator = new MembraneCreator(this.NodeFactory,this.ElementFactory);
            membraneCreator.CreateGeometry(membraneData);
            this.nodalLoads = this.nodalLoads.Concat(membraneCreator.NodalLoads).ToList();
        }
       
    }
}
