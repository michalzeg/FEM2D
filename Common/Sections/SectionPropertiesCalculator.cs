using Common.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sections
{
    public class SectionPropertiesCalculator
    {
        private SectionProperties result;
        public SectionPropertiesCalculator()
        {

        }
        public SectionProperties CalculateProperties(IEnumerable<Perimeter> perimeters)
        {
            this.result = new SectionProperties();
            CalculateBaseProperties(perimeters);
            CalculateCentreOfGravity();
            CalculateCentralMoments();
            CalculatePrincipalMoments();
            double alfa = CalculateAlfa();
            CalculateExtrimesInCentral(perimeters);
            CalculateExtrimesInPrincipal(perimeters, alfa);

            return result;
        }

        private void CalculateExtrimesInPrincipal(IEnumerable<Perimeter> perimeters, double alfa)
        {
            var cos = Math.Cos(alfa);
            var sin = Math.Sin(alfa);

            //coordinate of teh centre of gravity in rotated coordinate system
            var xo = result.X0 * cos - result.Y0 * sin;
            var yo = result.X0 * sin + result.Y0 * cos;

            result.DXI_max = perimeters.Max(s => s.Coordinates.Max(point => point.X * cos - point.Y * sin)) - xo;
            result.DXI_min = perimeters.Min(s => s.Coordinates.Min(point => point.X * cos - point.Y * sin)) - xo;
            result.DYI_max = perimeters.Max(s => s.Coordinates.Max(point => point.X * sin + point.Y * cos)) - yo;
            result.DYI_min = perimeters.Min(s => s.Coordinates.Min(point => point.X * sin + point.Y * cos)) - yo;
        }

        private void CalculateExtrimesInCentral(IEnumerable<Perimeter> perimeters)
        {
            result.DX0_max = perimeters.Max(s => s.Coordinates.Max(point => point.X)) - result.X0;
            result.DX0_min = perimeters.Min(s => s.Coordinates.Min(point => point.X)) - result.X0;
            result.DY0_max = perimeters.Max(s => s.Coordinates.Max(point => point.Y)) - result.Y0;
            result.DY0_min = perimeters.Min(s => s.Coordinates.Min(point => point.Y)) - result.Y0;
        }

        private double CalculateAlfa()
        {
            //alfa
            var alfa = Math.Atan(result.Ixy0 / (result.Iy0 - result.I1));
            if (double.IsNaN(alfa))
                alfa = Math.PI / 2;
            return alfa;
        }

        private void CalculatePrincipalMoments()
        {
            //principal properties
            result.I1 = (result.Ix0 + result.Iy0) / 2 + 0.5 * Math.Sqrt(Math.Pow(result.Iy0 - result.Ix0, 2) + 4 * result.Ixy0 * result.Ixy0);
            result.I2 = (result.Ix0 + result.Iy0) / 2 - 0.5 * Math.Sqrt(Math.Pow(result.Iy0 - result.Ix0, 2) + 4 * result.Ixy0 * result.Ixy0);
        }

        private void CalculateCentralMoments()
        {
            //in central coordinate system
            result.Ix0 = result.Ix - result.A * result.Y0 * result.Y0;
            result.Iy0 = result.Iy - result.A * result.X0 * result.X0;
            result.Ixy0 = result.Ixy - result.A * result.X0 * result.Y0;
        }

        private void CalculateCentreOfGravity()
        {
            result.X0 = result.Sy / result.A;
            result.Y0 = result.Sx / result.A;
        }

        private void CalculateBaseProperties(IEnumerable<Perimeter> perimeters)
        {
            foreach (var perimeter in perimeters)
            {
                for (int i = 0; i <= perimeter.Coordinates.Count - 2; i++)
                {
                    double x1, x2, y1, y2;
                    x1 = perimeter.Coordinates[i].X;
                    x2 = perimeter.Coordinates[i + 1].X;
                    y1 = perimeter.Coordinates[i].Y;
                    y2 = perimeter.Coordinates[i + 1].Y;
                    result.A = result.A + (x1 - x2) * (y2 + y1);
                    result.Sx = result.Sx + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
                    result.Sy = result.Sy + (y2 - y1) * (x1 * x1 + x1 * x2 + x2 * x2);
                    result.Ix = result.Ix + (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
                    result.Iy = result.Iy + (y2 - y1) * (x1 * x1 * x1 + x1 * x1 * x2 + x1 * x2 * x2 + x2 * x2 * x2);
                    result.Ixy = result.Ixy + (x1 - x2) * (x1 * (3 * y1 * y1 + y2 * y2 + 2 * y1 * y2) + x2 * (3 * y2 * y2 + y1 * y1 + 2 * y1 * y2));
                }

            }

            //applying mulipliers
            result.A = result.A / 2;
            result.Sx = result.Sx / 6;
            result.Sy = result.Sy / 6;
            result.Ix = result.Ix / 12;
            result.Iy = result.Iy / 12;
            result.Ixy = result.Ixy / 24;
        }
    }
}
