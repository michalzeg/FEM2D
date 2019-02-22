// Decompiled with JetBrains decompiler
// Type: TriangleNet.Data.Segment
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Geometry;

namespace TriangleNet.Data
{
  public class Segment : ISegment
  {
    internal int hash;
    internal Osub[] subsegs;
    internal Vertex[] vertices;
    internal Otri[] triangles;
    internal int boundary;

    public Segment()
    {
      this.subsegs = new Osub[2];
      this.subsegs[0].seg = Mesh.dummysub;
      this.subsegs[1].seg = Mesh.dummysub;
      this.vertices = new Vertex[4];
      this.triangles = new Otri[2];
      this.triangles[0].triangle = Mesh.dummytri;
      this.triangles[1].triangle = Mesh.dummytri;
      this.boundary = 0;
    }

    public int P0
    {
      get
      {
        return this.vertices[0].id;
      }
    }

    public int P1
    {
      get
      {
        return this.vertices[1].id;
      }
    }

    public int Boundary
    {
      get
      {
        return this.boundary;
      }
    }

    public Vertex GetVertex(int index)
    {
      return this.vertices[index];
    }

    public ITriangle GetTriangle(int index)
    {
      if (this.triangles[index].triangle != Mesh.dummytri)
        return (ITriangle) this.triangles[index].triangle;
      return (ITriangle) null;
    }

    public override int GetHashCode()
    {
      return this.hash;
    }

    public override string ToString()
    {
      return string.Format("SID {0}", (object) this.hash);
    }
  }
}
