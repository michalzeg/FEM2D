// Decompiled with JetBrains decompiler
// Type: TriangleNet.Tools.VoronoiRegion
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System.Collections.Generic;
using TriangleNet.Data;
using TriangleNet.Geometry;

namespace TriangleNet.Tools
{
  public class VoronoiRegion
  {
    private int id;
    private Point generator;
    private List<Point> vertices;
    private bool bounded;

    public int ID
    {
      get
      {
        return this.id;
      }
    }

    public Point Generator
    {
      get
      {
        return this.generator;
      }
    }

    public ICollection<Point> Vertices
    {
      get
      {
        return (ICollection<Point>) this.vertices;
      }
    }

    public bool Bounded
    {
      get
      {
        return this.bounded;
      }
      set
      {
        this.bounded = value;
      }
    }

    public VoronoiRegion(Vertex generator)
    {
      this.id = generator.id;
      this.generator = (Point) generator;
      this.vertices = new List<Point>();
      this.bounded = true;
    }

    public void Add(Point point)
    {
      this.vertices.Add(point);
    }

    public void Add(List<Point> points)
    {
      this.vertices.AddRange((IEnumerable<Point>) points);
    }

    public override string ToString()
    {
      return string.Format("R-ID {0}", (object) this.id);
    }
  }
}
