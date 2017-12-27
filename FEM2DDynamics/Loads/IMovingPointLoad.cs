using Common.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DDynamics.Loads
{
    public interface IMovingPointLoad
    {
        double GetValueY(double time);
        PointD GetPosition(double time);
    }
}
