using Common.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
namespace Common.Geometry
{
    public static class Geometry
    {
        public static bool AreColinear(PointD startPoint, PointD endPoint, PointD point)
        {
            var x0 = startPoint.X;
            var y0 = startPoint.Y;
            var x1 = endPoint.X;
            var y1 = endPoint.Y;
            var x2 = point.X;
            var y2 = point.Y;

            var det = x0 * y1 + x1 * y2 + x2 * y0 - x2 * y1 - x0 * y2 - x1 * y0;

            return det.IsApproximatelyEqualToZero();
        }

        public static bool IsInsideSegment(PointD startPoint, PointD endPoint, PointD point)
        {
            var x1 = startPoint.X;
            var y1 = startPoint.Y;
            var x2 = endPoint.X;
            var y2 = endPoint.Y;
            var x = point.X;
            var y = point.Y;

            var result = AreColinear(startPoint, endPoint, point)
                && Math.Min(x1, x2) <= x
                && x <= Math.Max(x1, x2)
                && Math.Min(y1, y2) <= y
                && y <= Math.Max(y1, y2);

            return result;
        }

        public static bool IsInsideSegmentWithoutEnds(PointD startPoint, PointD endPoint, PointD point)
        {
            var x1 = startPoint.X;
            var y1 = startPoint.Y;
            var x2 = endPoint.X;
            var y2 = endPoint.Y;
            var x = point.X;
            var y = point.Y;

            var result = AreColinear(startPoint, endPoint, point)
                && Math.Min(x1, x2) < x
                && x < Math.Max(x1, x2)
                && Math.Min(y1, y2) < y
                && y < Math.Max(y1, y2);

            return result;
        }
    }
}
