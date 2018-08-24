using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Utils
{
    public class ProgressMessage : IEquatable<ProgressMessage>
    {
        private readonly double currentValue;
        private readonly double maxValue;

        public ProgressMessage(double currentValue, double maxValue)
        {
            this.currentValue = currentValue;
            this.maxValue = maxValue;
        }

        public int Progress => (int)(this.currentValue / this.maxValue * 100);

        public bool Equals(ProgressMessage other)
        {
            if (other == null)
                return false;
            return this.currentValue.IsApproximatelyEqualTo(other.currentValue)
                && this.maxValue.IsApproximatelyEqualTo(other.maxValue);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ProgressMessage);
        }

        public override int GetHashCode()
        {
            return this.currentValue.GetHashCode() ^ this.maxValue.GetHashCode();
        }
    }
}