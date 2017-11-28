using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2D.Nodes
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
