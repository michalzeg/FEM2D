// Decompiled with JetBrains decompiler
// Type: TriangleNet.TriangleLocator
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Data;
using TriangleNet.Geometry;

namespace TriangleNet
{
  internal class TriangleLocator
  {
    private Sampler sampler;
    private Mesh mesh;
    internal Otri recenttri;

    public TriangleLocator(Mesh mesh)
    {
      this.mesh = mesh;
      this.sampler = new Sampler();
    }

    public void Update(ref Otri otri)
    {
      otri.Copy(ref this.recenttri);
    }

    public void Reset()
    {
      this.recenttri.triangle = (Triangle) null;
    }

    public LocateResult PreciseLocate(
      Point searchpoint,
      ref Otri searchtri,
      bool stopatsubsegment)
    {
      Otri o2 = new Otri();
      Osub os = new Osub();
      Vertex vertex1 = searchtri.Org();
      Vertex vertex2 = searchtri.Dest();
      for (Vertex vertex3 = searchtri.Apex(); vertex3.x != searchpoint.X || vertex3.y != searchpoint.Y; vertex3 = searchtri.Apex())
      {
        double num1 = Primitives.CounterClockwise((Point) vertex1, (Point) vertex3, searchpoint);
        double num2 = Primitives.CounterClockwise((Point) vertex3, (Point) vertex2, searchpoint);
        bool flag;
        if (num1 > 0.0)
          flag = num2 <= 0.0 || (vertex3.x - searchpoint.X) * (vertex2.x - vertex1.x) + (vertex3.y - searchpoint.Y) * (vertex2.y - vertex1.y) > 0.0;
        else if (num2 > 0.0)
        {
          flag = false;
        }
        else
        {
          if (num1 == 0.0)
          {
            searchtri.LprevSelf();
            return LocateResult.OnEdge;
          }
          if (num2 != 0.0)
            return LocateResult.InTriangle;
          searchtri.LnextSelf();
          return LocateResult.OnEdge;
        }
        if (flag)
        {
          searchtri.Lprev(ref o2);
          vertex2 = vertex3;
        }
        else
        {
          searchtri.Lnext(ref o2);
          vertex1 = vertex3;
        }
        o2.Sym(ref searchtri);
        if (this.mesh.checksegments && stopatsubsegment)
        {
          o2.SegPivot(ref os);
          if (os.seg != Mesh.dummysub)
          {
            o2.Copy(ref searchtri);
            return LocateResult.Outside;
          }
        }
        if (searchtri.triangle == Mesh.dummytri)
        {
          o2.Copy(ref searchtri);
          return LocateResult.Outside;
        }
      }
      searchtri.LprevSelf();
      return LocateResult.OnVertex;
    }

    public LocateResult Locate(Point searchpoint, ref Otri searchtri)
    {
      Otri otri = new Otri();
      Vertex vertex1 = searchtri.Org();
      double num1 = (searchpoint.X - vertex1.x) * (searchpoint.X - vertex1.x) + (searchpoint.Y - vertex1.y) * (searchpoint.Y - vertex1.y);
      if (this.recenttri.triangle != null && !Otri.IsDead(this.recenttri.triangle))
      {
        Vertex vertex2 = this.recenttri.Org();
        if (vertex2.x == searchpoint.X && vertex2.y == searchpoint.Y)
        {
          this.recenttri.Copy(ref searchtri);
          return LocateResult.OnVertex;
        }
        double num2 = (searchpoint.X - vertex2.x) * (searchpoint.X - vertex2.x) + (searchpoint.Y - vertex2.y) * (searchpoint.Y - vertex2.y);
        if (num2 < num1)
        {
          this.recenttri.Copy(ref searchtri);
          num1 = num2;
        }
      }
      this.sampler.Update(this.mesh);
      foreach (int sample in this.sampler.GetSamples(this.mesh))
      {
        otri.triangle = this.mesh.triangles[sample];
        if (!Otri.IsDead(otri.triangle))
        {
          Vertex vertex2 = otri.Org();
          double num2 = (searchpoint.X - vertex2.x) * (searchpoint.X - vertex2.x) + (searchpoint.Y - vertex2.y) * (searchpoint.Y - vertex2.y);
          if (num2 < num1)
          {
            otri.Copy(ref searchtri);
            num1 = num2;
          }
        }
      }
      Vertex vertex3 = searchtri.Org();
      Vertex vertex4 = searchtri.Dest();
      if (vertex3.x == searchpoint.X && vertex3.y == searchpoint.Y)
        return LocateResult.OnVertex;
      if (vertex4.x == searchpoint.X && vertex4.y == searchpoint.Y)
      {
        searchtri.LnextSelf();
        return LocateResult.OnVertex;
      }
      double num3 = Primitives.CounterClockwise((Point) vertex3, (Point) vertex4, searchpoint);
      if (num3 < 0.0)
        searchtri.SymSelf();
      else if (num3 == 0.0 && vertex3.x < searchpoint.X == searchpoint.X < vertex4.x && vertex3.y < searchpoint.Y == searchpoint.Y < vertex4.y)
        return LocateResult.OnEdge;
      return this.PreciseLocate(searchpoint, ref searchtri, false);
    }
  }
}
