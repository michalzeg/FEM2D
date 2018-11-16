using Common.Extensions;
using System;

namespace Common.Geometry
{
    /// <summary>
    /// Base class for representating double point
    /// </summary>
    public class PointD : IEquatable<PointD>
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public PointD()
        {
        }

        public override bool Equals(object obj)
        {
            PointD other = obj as PointD;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public bool Equals(PointD other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return (this.X.IsApproximatelyEqualTo(other.X)
                && this.Y.IsApproximatelyEqualTo(other.Y));
        }

        public static bool operator ==(PointD point1, PointD point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(PointD point1, PointD point2)
        {
            return !(point1 == point2);
        }

        public PointD Move(PointD point)
        {
            var result = new PointD(this.X + point.X, this.Y + point.Y);
            return result;
        }

        public PointD Move(double x, double y)
        {
            return this.Move(new PointD(x, y));
        }

        public double DistanceTo(PointD point)
        {
            var dx = this.X - point.X;
            var dy = this.Y - point.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }
    }
}