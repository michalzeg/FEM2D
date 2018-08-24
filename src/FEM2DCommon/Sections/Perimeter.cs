using Common.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace FEM2DCommon.Sections
{
    public class Perimeter
    {
        public IList<PointD> Coordinates { get; }

        public Perimeter(IEnumerable<PointD> coordinates)
        {
            this.Coordinates = CheckIfCoordinatesAreClockwise(coordinates.ToList());
            this.CheckLastElement();
        }

        private void CheckLastElement()
        {
            //checks if last element is equal to first
            var firstPoint = this.Coordinates.First();
            var lastPoint = this.Coordinates.Last();

            if (firstPoint == lastPoint)
            {
                return;
            }
            else
            {
                this.Coordinates.Add(firstPoint);
            }
        }

        private double CrossProduct(PointD p0, PointD p1, PointD p2)
        {
            var vector1 = new double[2];
            var vector2 = new double[2];

            vector1[0] = p1.X - p0.X;
            vector1[1] = p1.Y - p0.Y;
            vector2[0] = p2.X - p1.X;
            vector2[1] = p2.Y - p1.Y;

            double result; //ax*by-ay*bz
            result = vector1[0] * vector2[1] - vector1[1] * vector2[0];
            return result;
        }

        private IList<PointD> CheckIfCoordinatesAreClockwise(IList<PointD> coordinates) //procedura sprawdza czy wspolrzedne przekroju sa wprowadzone zgodnie ze wskazowkami zegara
        {
            //function checks if coordinates are in clockwise or counterclockwise order. To check that cross product is used.
            //
            double crossProduct;
            var tempCoord = coordinates;
            for (int i = 0; i <= coordinates.Count - 3; i++)
            {
                crossProduct = this.CrossProduct(coordinates[i], coordinates[i + 1], coordinates[i + 2]);
                if (crossProduct > 0)
                {
                    //clockwise
                    break;
                }
                else if (crossProduct < 0)
                {
                    //counterclockwise
                    tempCoord = tempCoord.Reverse().ToList();
                    break;
                }
            }
            return tempCoord;
        }
    }
}