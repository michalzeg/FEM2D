using Common.Extensions;
using System;

namespace Common.Geometry
{
    public class Point3D : PointD, IEquatable<Point3D>
    {
        public double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D()
        {
        }

        public override bool Equals(object obj)
        {
            Point3D other = obj as Point3D;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public bool Equals(Point3D other)
        {
            return (this.X.IsApproximatelyEqualTo(other.X)
                && this.Y.IsApproximatelyEqualTo(other.Y));
        }

        public static bool operator ==(Point3D point1, Point3D point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point3D point1, Point3D point2)
        {
            return !(point1 == point2);
        }
    }
}