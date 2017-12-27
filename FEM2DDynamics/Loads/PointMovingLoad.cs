using Common.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Loads
{
    public class PointMovingLoad : IMovingPointLoad
    {
        private readonly double value;
        private readonly PointD basePosition;

        public PointMovingLoad(double value, double basePosition)
        {
            this.value = value;
            this.basePosition = new PointD( basePosition,0);
        }

        public PointD GetPosition(double time)
        {
            return this.basePosition;
        }

        public double GetValueY(double time)
        {
            return this.value;
        }
    }
}
