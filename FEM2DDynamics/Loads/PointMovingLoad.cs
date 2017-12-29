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
        private readonly double speed;
        private readonly PointD basePosition;

        public PointMovingLoad(double value, double basePositionX, double speed)
        {
            this.value = value;
            this.speed = speed;
            this.basePosition = new PointD( basePositionX,0);
        }

        public PointD GetPosition(double time)
        {
            var deltaPosition = this.speed * time;

            var position = basePosition.Move(deltaPosition,0);
            return position;
        }

        public double GetValueY(double time)
        {
            return this.value;
        }
    }
}
