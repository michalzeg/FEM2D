// Decompiled with JetBrains decompiler
// Type: TriangleNet.Data.Triangle
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Geometry;

namespace TriangleNet.Data
{
  public class Triangle : ITriangle
  {
    internal int hash;
    internal int id;
    internal Otri[] neighbors;
    internal Vertex[] vertices;
    internal Osub[] subsegs;
    internal int region;
    internal double area;
    internal bool infected;

    public Triangle()
    {
      this.neighbors = new Otri[3];
      this.neighbors[0].triangle = Mesh.dummytri;
      this.neighbors[1].triangle = Mesh.dummytri;
      this.neighbors[2].triangle = Mesh.dummytri;
      this.vertices = new Vertex[3];
      this.subsegs = new Osub[3];
      this.subsegs[0].seg = Mesh.dummysub;
      this.subsegs[1].seg = Mesh.dummysub;
      this.subsegs[2].seg = Mesh.dummysub;
    }

    public int ID
    {
      get
      {
        return this.id;
      }
    }

    public int P0
    {
      get
      {
        if (!((Point) this.vertices[0] == (Point) null))
          return this.vertices[0].id;
        return -1;
      }
    }

    public int P1
    {
      get
      {
        if (!((Point) this.vertices[1] == (Point) null))
          return this.vertices[1].id;
        return -1;
      }
    }

    public Vertex GetVertex(int index)
    {
      return this.vertices[index];
    }

    public int P2
    {
      get
      {
        if (!((Point) this.vertices[2] == (Point) null))
          return this.vertices[2].id;
        return -1;
      }
    }

    public bool SupportsNeighbors
    {
      get
      {
        return true;
      }
    }

    public int N0
    {
      get
      {
        return this.neighbors[0].triangle.id;
      }
    }

    public int N1
    {
      get
      {
        return this.neighbors[1].triangle.id;
      }
    }

    public int N2
    {
      get
      {
        return this.neighbors[2].triangle.id;
      }
    }

    public ITriangle GetNeighbor(int index)
    {
      if (this.neighbors[index].triangle != Mesh.dummytri)
        return (ITriangle) this.neighbors[index].triangle;
      return (ITriangle) null;
    }

    public ISegment GetSegment(int index)
    {
      if (this.subsegs[index].seg != Mesh.dummysub)
        return (ISegment) this.subsegs[index].seg;
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
    }

    public override int GetHashCode()
    {
      return this.hash;
    }

    public override string ToString()
    {
      return string.Format("TID {0}", (object) this.hash);
    }
  }
}
