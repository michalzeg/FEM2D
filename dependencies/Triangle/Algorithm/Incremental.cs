﻿// Decompiled with JetBrains decompiler
// Type: TriangleNet.Algorithm.Incremental
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Data;
using TriangleNet.Geometry;
using TriangleNet.Log;

namespace TriangleNet.Algorithm
{
  internal class Incremental
  {
    private Mesh mesh;

    private void GetBoundingBox()
    {
      Otri newotri = new Otri();
      BoundingBox bounds = this.mesh.bounds;
      double num = bounds.Width;
      if (bounds.Height > num)
        num = bounds.Height;
      if (num == 0.0)
        num = 1.0;
      this.mesh.infvertex1 = new Vertex(bounds.Xmin - 50.0 * num, bounds.Ymin - 40.0 * num);
      this.mesh.infvertex2 = new Vertex(bounds.Xmax + 50.0 * num, bounds.Ymin - 40.0 * num);
      this.mesh.infvertex3 = new Vertex(0.5 * (bounds.Xmin + bounds.Xmax), bounds.Ymax + 60.0 * num);
      this.mesh.MakeTriangle(ref newotri);
      newotri.SetOrg(this.mesh.infvertex1);
      newotri.SetDest(this.mesh.infvertex2);
      newotri.SetApex(this.mesh.infvertex3);
      Mesh.dummytri.neighbors[0] = newotri;
    }

    private int RemoveBox()
    {
      Otri o2_1 = new Otri();
      Otri o2_2 = new Otri();
      Otri o2_3 = new Otri();
      Otri o2_4 = new Otri();
      Otri o2_5 = new Otri();
      Otri o2_6 = new Otri();
      bool flag = !this.mesh.behavior.Poly;
      o2_4.triangle = Mesh.dummytri;
      o2_4.orient = 0;
      o2_4.SymSelf();
      o2_4.Lprev(ref o2_5);
      o2_4.LnextSelf();
      o2_4.SymSelf();
      o2_4.Lprev(ref o2_2);
      o2_2.SymSelf();
      o2_4.Lnext(ref o2_3);
      o2_3.SymSelf();
      if (o2_3.triangle == Mesh.dummytri)
      {
        o2_2.LprevSelf();
        o2_2.SymSelf();
      }
      Mesh.dummytri.neighbors[0] = o2_2;
      int num = -2;
      while (!o2_4.Equal(o2_5))
      {
        ++num;
        o2_4.Lprev(ref o2_6);
        o2_6.SymSelf();
        if (flag && o2_6.triangle != Mesh.dummytri)
        {
          Vertex vertex = o2_6.Org();
          if (vertex.mark == 0)
            vertex.mark = 1;
        }
        o2_6.Dissolve();
        o2_4.Lnext(ref o2_1);
        o2_1.Sym(ref o2_4);
        this.mesh.TriangleDealloc(o2_1.triangle);
        if (o2_4.triangle == Mesh.dummytri)
          o2_6.Copy(ref o2_4);
      }
      this.mesh.TriangleDealloc(o2_5.triangle);
      return num;
    }

    public int Triangulate(Mesh mesh)
    {
      this.mesh = mesh;
      Otri searchtri = new Otri();
      this.GetBoundingBox();
      foreach (Vertex newvertex in mesh.vertices.Values)
      {
        searchtri.triangle = Mesh.dummytri;
        Osub splitseg = new Osub();
        if (mesh.InsertVertex(newvertex, ref searchtri, ref splitseg, false, false) == InsertVertexResult.Duplicate)
        {
          if (Behavior.Verbose)
            SimpleLog.Instance.Warning("A duplicate vertex appeared and was ignored.", "Incremental.IncrementalDelaunay()");
          newvertex.type = VertexType.UndeadVertex;
          ++mesh.undeads;
        }
      }
      return this.RemoveBox();
    }
  }
}
