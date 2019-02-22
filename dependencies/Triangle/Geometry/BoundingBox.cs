// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.BoundingBox
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;

namespace TriangleNet.Geometry
{
  public class BoundingBox
  {
    private double xmin;
    private double ymin;
    private double xmax;
    private double ymax;

    public BoundingBox()
    {
      this.xmin = double.MaxValue;
      this.ymin = double.MaxValue;
      this.xmax = double.MinValue;
      this.ymax = double.MinValue;
    }

    public BoundingBox(double xmin, double ymin, double xmax, double ymax)
    {
      this.xmin = xmin;
      this.ymin = ymin;
      this.xmax = xmax;
      this.ymax = ymax;
    }

    public double Xmin
    {
      get
      {
        return this.xmin;
      }
    }

    public double Ymin
    {
      get
      {
        return this.ymin;
      }
    }

    public double Xmax
    {
      get
      {
        return this.xmax;
      }
    }

    public double Ymax
    {
      get
      {
        return this.ymax;
      }
    }

    public double Width
    {
      get
      {
        return this.xmax - this.xmin;
      }
    }

    public double Height
    {
      get
      {
        return this.ymax - this.ymin;
      }
    }

    public void Update(double x, double y)
    {
      this.xmin = Math.Min(this.xmin, x);
      this.ymin = Math.Min(this.ymin, y);
      this.xmax = Math.Max(this.xmax, x);
      this.ymax = Math.Max(this.ymax, y);
    }

    public void Scale(double dx, double dy)
    {
      this.xmin -= dx;
      this.xmax += dx;
      this.ymin -= dy;
      this.ymax += dy;
    }

    public bool Contains(Point pt)
    {
      if (pt.x >= this.xmin && pt.x <= this.xmax && pt.y >= this.ymin)
        return pt.y <= this.ymax;
      return false;
    }
  }
}
