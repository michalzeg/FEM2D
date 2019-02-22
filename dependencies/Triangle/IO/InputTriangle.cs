// Decompiled with JetBrains decompiler
// Type: TriangleNet.IO.InputTriangle
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Data;
using TriangleNet.Geometry;

namespace TriangleNet.IO
{
  public class InputTriangle : ITriangle
  {
    internal int[] vertices;
    internal int region;
    internal double area;

    public InputTriangle(int p0, int p1, int p2)
    {
      this.vertices = new int[3]{ p0, p1, p2 };
    }

    public int ID
    {
      get
      {
        return 0;
      }
    }

    public int P0
    {
      get
      {
        return this.vertices[0];
      }
    }

    public int P1
    {
      get
      {
        return this.vertices[1];
      }
    }

    public int P2
    {
      get
      {
        return this.vertices[2];
      }
    }

    public Vertex GetVertex(int index)
    {
      return (Vertex) null;
    }

    public bool SupportsNeighbors
    {
      get
      {
        return false;
      }
    }

    public int N0
    {
      get
      {
        return -1;
      }
    }

    public int N1
    {
      get
      {
        return -1;
      }
    }

    public int N2
    {
      get
      {
        return -1;
      }
    }

    public ITriangle GetNeighbor(int index)
    {
      return (ITriangle) null;
    }

    public ISegment GetSegment(int index)
    {
      return (ISegment) null;
    }

    public double Area
    {
      get
      {
        return this.area;
      }
      set
      {
        this.area = value;
      }
    }

    public int Region
    {
      get
      {
        return this.region;
      }
      set
      {
        this.region = value;
      }
    }
  }
}
