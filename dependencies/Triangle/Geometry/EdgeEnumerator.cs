// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.EdgeEnumerator
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Data;

namespace TriangleNet.Geometry
{
  public class EdgeEnumerator : IEnumerator<Edge>, IDisposable, IEnumerator
  {
    private Otri tri = new Otri();
    private Otri neighbor = new Otri();
    private Osub sub = new Osub();
    private IEnumerator<Triangle> triangles;
    private Edge current;
    private Vertex p1;
    private Vertex p2;

    public EdgeEnumerator(Mesh mesh)
    {
      this.triangles = (IEnumerator<Triangle>) mesh.triangles.Values.GetEnumerator();
      this.triangles.MoveNext();
      this.tri.triangle = this.triangles.Current;
      this.tri.orient = 0;
    }

    public Edge Current
    {
      get
      {
        return this.current;
      }
    }

    public void Dispose()
    {
      this.triangles.Dispose();
    }

    object IEnumerator.Current
    {
      get
      {
        return (object) this.current;
      }
    }

    public bool MoveNext()
    {
      if (this.tri.triangle == null)
        return false;
      this.current = (Edge) null;
      while (this.current == null)
      {
        if (this.tri.orient == 3)
        {
          if (!this.triangles.MoveNext())
            return false;
          this.tri.triangle = this.triangles.Current;
          this.tri.orient = 0;
        }
        this.tri.Sym(ref this.neighbor);
        if (this.tri.triangle.id < this.neighbor.triangle.id || this.neighbor.triangle == Mesh.dummytri)
        {
          this.p1 = this.tri.Org();
          this.p2 = this.tri.Dest();
          this.tri.SegPivot(ref this.sub);
          this.current = new Edge(this.p1.id, this.p2.id, this.sub.seg.boundary);
        }
        ++this.tri.orient;
      }
      return true;
    }

    public void Reset()
    {
      this.triangles.Reset();
    }
  }
}
