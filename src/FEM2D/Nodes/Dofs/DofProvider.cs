using System.Collections.Generic;
using System.Linq;

namespace FEM2D.Nodes.Dofs
{
    internal class DofNumberCalculator : IDofNumberCalculator
    {
        private IList<int> currentDofs;

        public DofNumberCalculator()
        {
            this.currentDofs = new List<int>();
        }

        public int GetFreeDOFNumber()
        {
            var result = this.GetFirstFreeNumber();
            this.currentDofs.Add(result);
            return result;
        }

        private int GetFirstFreeNumber()
        {
            var freeNumber = Enumerable.Range(0, int.MaxValue)
                                .Except(this.currentDofs)
                                .FirstOrDefault();

            return freeNumber;
        }
    }
}