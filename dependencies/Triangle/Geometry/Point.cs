// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.Point
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;

namespace TriangleNet.Geometry
{
  public class Point : IComparable<Point>, IEquatable<Point>
  {
    internal int id;
    internal double x;
    internal double y;
    internal int mark;
    internal double[] attributes;

    public Point()
      : this(0.0, 0.0, 0)
    {
    }

    public Point(double x, double y)
      : this(x, y, 0)
    {
    }

    public Point(double x, double y, int mark)
    {
      this.x = x;
      this.y = y;
      this.mark = mark;
    }

    public int ID
    {
      get
      {
        return this.id;
      }
    }

    public double X
    {
      get
      {
        return this.x;
      }
    }

    public double Y
    {
      get
      {
        return this.y;
      }
    }

    public int Boundary
    {
      get
      {
        return this.mark;
      }
    }

    public double[] Attributes
    {
      get
      {
        return this.attributes;
      }
    }

    public static bool operator ==(Point a, Point b)
    {
      if (object.ReferenceEquals((object) a, (object) b))
        return true;
      if ((object) a == null || (object) b == null)
        return false;
      return a.Equals(b);
    }

    public static bool operator !=(Point a, Point b)
    {
      return !(a == b);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      Point point = obj as Point;
      if ((object) point == null || this.x != point.x)
        return false;
      return this.y == point.y;
    }

    public bool Equals(Point p)
    {
      if ((object) p == null || this.x != p.x)
        return false;
      return this.y == p.y;
    }

    public int CompareTo(Point other)
    {
      if (this.x == other.x && this.y == other.y)
        return 0;
      return this.x >= other.x && (this.x != other.x || this.y >= other.y) ? 1 : -1;
    }

    public override int GetHashCode()
    {
      return this.x.GetHashCode() ^ this.y.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("[{0},{1}]", (object) this.x, (object) this.y);
    }
  }
}
