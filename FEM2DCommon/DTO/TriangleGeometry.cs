
using Common.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM2DCommon.DTO
{
    public class TriangleGeometry : IEquatable<TriangleGeometry>
    {
        public int Number { get; set; }
        public PointD Vertex1 { get; set; }
        public PointD Vertex2 { get; set; }
        public PointD Vertex3 { get; set; }


        public override bool Equals(object obj)
        {
            var other = obj as TriangleGeometry;
            if (other == null)
                return false;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.Number.GetHashCode() 
                ^ this.Vertex1.GetHashCode()
                ^ this.Vertex2.GetHashCode()
                ^ this.Vertex3.GetHashCode();
        }

        public bool Equals(TriangleGeometry other)
        {
            return
                this.Number == other.Number
                && this.Vertex1 == other.Vertex1
                && this.Vertex2 == other.Vertex2
                && this.Vertex3 == other.Vertex3;
        }

        public static bool operator ==(TriangleGeometry geometry1, TriangleGeometry geometry2)
        {
            return geometry1.Equals(geometry2);
        }
        public static bool operator !=(TriangleGeometry geometry1, TriangleGeometry geometry2)
        {
            return !(geometry1 == geometry2);
        }
    }
}
