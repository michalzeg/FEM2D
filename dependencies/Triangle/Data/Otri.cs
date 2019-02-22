﻿// Decompiled with JetBrains decompiler
// Type: TriangleNet.Data.Otri
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

namespace TriangleNet.Data
{
  internal struct Otri
  {
    private static readonly int[] plus1Mod3 = new int[3]
    {
      1,
      2,
      0
    };
    private static readonly int[] minus1Mod3 = new int[3]
    {
      2,
      0,
      1
    };
    public Triangle triangle;
    public int orient;

    public override string ToString()
    {
      if (this.triangle == null)
        return "O-TID [null]";
      return string.Format("O-TID {0}", (object) this.triangle.hash);
    }

    public void Sym(ref Otri o2)
    {
      o2.triangle = this.triangle.neighbors[this.orient].triangle;
      o2.orient = this.triangle.neighbors[this.orient].orient;
    }

    public void SymSelf()
    {
      int orient = this.orient;
      this.orient = this.triangle.neighbors[orient].orient;
      this.triangle = this.triangle.neighbors[orient].triangle;
    }

    public void Lnext(ref Otri o2)
    {
      o2.triangle = this.triangle;
      o2.orient = Otri.plus1Mod3[this.orient];
    }

    public void LnextSelf()
    {
      this.orient = Otri.plus1Mod3[this.orient];
    }

    public void Lprev(ref Otri o2)
    {
      o2.triangle = this.triangle;
      o2.orient = Otri.minus1Mod3[this.orient];
    }

    public void LprevSelf()
    {
      this.orient = Otri.minus1Mod3[this.orient];
    }

    public void Onext(ref Otri o2)
    {
      o2.triangle = this.triangle;
      o2.orient = Otri.minus1Mod3[this.orient];
      int orient = o2.orient;
      o2.orient = o2.triangle.neighbors[orient].orient;
      o2.triangle = o2.triangle.neighbors[orient].triangle;
    }

    public void OnextSelf()
    {
      this.orient = Otri.minus1Mod3[this.orient];
      int orient = this.orient;
      this.orient = this.triangle.neighbors[orient].orient;
      this.triangle = this.triangle.neighbors[orient].triangle;
    }

    public void Oprev(ref Otri o2)
    {
      o2.triangle = this.triangle.neighbors[this.orient].triangle;
      o2.orient = this.triangle.neighbors[this.orient].orient;
      o2.orient = Otri.plus1Mod3[o2.orient];
    }

    public void OprevSelf()
    {
      int orient = this.orient;
      this.orient = this.triangle.neighbors[orient].orient;
      this.triangle = this.triangle.neighbors[orient].triangle;
      this.orient = Otri.plus1Mod3[this.orient];
    }

    public void Dnext(ref Otri o2)
    {
      o2.triangle = this.triangle.neighbors[this.orient].triangle;
      o2.orient = this.triangle.neighbors[this.orient].orient;
      o2.orient = Otri.minus1Mod3[o2.orient];
    }

    public void DnextSelf()
    {
      int orient = this.orient;
      this.orient = this.triangle.neighbors[orient].orient;
      this.triangle = this.triangle.neighbors[orient].triangle;
      this.orient = Otri.minus1Mod3[this.orient];
    }

    public void Dprev(ref Otri o2)
    {
      o2.triangle = this.triangle;
      o2.orient = Otri.plus1Mod3[this.orient];
      int orient = o2.orient;
      o2.orient = o2.triangle.neighbors[orient].orient;
      o2.triangle = o2.triangle.neighbors[orient].triangle;
    }

    public void DprevSelf()
    {
      this.orient = Otri.plus1Mod3[this.orient];
      int orient = this.orient;
      this.orient = this.triangle.neighbors[orient].orient;
      this.triangle = this.triangle.neighbors[orient].triangle;
    }

    public void Rnext(ref Otri o2)
    {
      o2.triangle = this.triangle.neighbors[this.orient].triangle;
      o2.orient = this.triangle.neighbors[this.orient].orient;
      o2.orient = Otri.plus1Mod3[o2.orient];
      int orient = o2.orient;
      o2.orient = o2.triangle.neighbors[orient].orient;
      o2.triangle = o2.triangle.neighbors[orient].triangle;
    }

    public void RnextSelf()
    {
      int orient1 = this.orient;
      this.orient = this.triangle.neighbors[orient1].orient;
      this.triangle = this.triangle.neighbors[orient1].triangle;
      this.orient = Otri.plus1Mod3[this.orient];
      int orient2 = this.orient;
      this.orient = this.triangle.neighbors[orient2].orient;
      this.triangle = this.triangle.neighbors[orient2].triangle;
    }

    public void Rprev(ref Otri o2)
    {
      o2.triangle = this.triangle.neighbors[this.orient].triangle;
      o2.orient = this.triangle.neighbors[this.orient].orient;
      o2.orient = Otri.minus1Mod3[o2.orient];
      int orient = o2.orient;
      o2.orient = o2.triangle.neighbors[orient].orient;
      o2.triangle = o2.triangle.neighbors[orient].triangle;
    }

    public void RprevSelf()
    {
      int orient1 = this.orient;
      this.orient = this.triangle.neighbors[orient1].orient;
      this.triangle = this.triangle.neighbors[orient1].triangle;
      this.orient = Otri.minus1Mod3[this.orient];
      int orient2 = this.orient;
      this.orient = this.triangle.neighbors[orient2].orient;
      this.triangle = this.triangle.neighbors[orient2].triangle;
    }

    public Vertex Org()
    {
      return this.triangle.vertices[Otri.plus1Mod3[this.orient]];
    }

    public Vertex Dest()
    {
      return this.triangle.vertices[Otri.minus1Mod3[this.orient]];
    }

    public Vertex Apex()
    {
      return this.triangle.vertices[this.orient];
    }

    public void SetOrg(Vertex ptr)
    {
      this.triangle.vertices[Otri.plus1Mod3[this.orient]] = ptr;
    }

    public void SetDest(Vertex ptr)
    {
      this.triangle.vertices[Otri.minus1Mod3[this.orient]] = ptr;
    }

    public void SetApex(Vertex ptr)
    {
      this.triangle.vertices[this.orient] = ptr;
    }

    public void Bond(ref Otri o2)
    {
      this.triangle.neighbors[this.orient].triangle = o2.triangle;
      this.triangle.neighbors[this.orient].orient = o2.orient;
      o2.triangle.neighbors[o2.orient].triangle = this.triangle;
      o2.triangle.neighbors[o2.orient].orient = this.orient;
    }

    public void Dissolve()
    {
      this.triangle.neighbors[this.orient].triangle = Mesh.dummytri;
      this.triangle.neighbors[this.orient].orient = 0;
    }

    public void Copy(ref Otri o2)
    {
      o2.triangle = this.triangle;
      o2.orient = this.orient;
    }

    public bool Equal(Otri o2)
    {
      if (this.triangle == o2.triangle)
        return this.orient == o2.orient;
      return false;
    }

    public void Infect()
    {
      this.triangle.infected = true;
    }

    public void Uninfect()
    {
      this.triangle.infected = false;
    }

    public bool IsInfected()
    {
      return this.triangle.infected;
    }

    public static bool IsDead(Triangle tria)
    {
      return tria.neighbors[0].triangle == null;
    }

    public static void Kill(Triangle tria)
    {
      tria.neighbors[0].triangle = (Triangle) null;
      tria.neighbors[2].triangle = (Triangle) null;
    }

    public void SegPivot(ref Osub os)
    {
      os = this.triangle.subsegs[this.orient];
    }

    public void SegBond(ref Osub os)
    {
      this.triangle.subsegs[this.orient] = os;
      os.seg.triangles[os.orient] = this;
    }

    public void SegDissolve()
    {
      this.triangle.subsegs[this.orient].seg = Mesh.dummysub;
    }
  }
}
