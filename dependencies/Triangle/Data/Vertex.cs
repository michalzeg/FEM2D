// Decompiled with JetBrains decompiler
// Type: TriangleNet.Data.Vertex
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using TriangleNet.Geometry;

namespace TriangleNet.Data
{
  public class Vertex : Point
  {
    internal int hash;
    internal VertexType type;
    internal Otri tri;

    public Vertex()
      : this(0.0, 0.0, 0, 0)
    {
    }

    public Vertex(double x, double y)
      : this(x, y, 0, 0)
    {
    }

    public Vertex(double x, double y, int mark)
      : this(x, y, mark, 0)
    {
    }

    public Vertex(double x, double y, int mark, int attribs)
      : base(x, y, mark)
    {
      this.type = VertexType.InputVertex;
      if (attribs <= 0)
        return;
      this.attributes = new double[attribs];
    }

    public VertexType Type
    {
      get
      {
        return this.type;
      }
    }

    public double this[int i]
    {
      get
      {
        if (i == 0)
          return this.x;
        if (i == 1)
          return this.y;
        throw new ArgumentOutOfRangeException("Index must be 0 or 1.");
      }
    }

    public override int GetHashCode()
    {
      return this.hash;
    }
  }
}
