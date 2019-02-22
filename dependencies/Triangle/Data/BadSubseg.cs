// Decompiled with JetBrains decompiler
// Type: TriangleNet.Data.BadSubseg
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

namespace TriangleNet.Data
{
  internal class BadSubseg
  {
    private static int hashSeed;
    internal int Hash;
    public Osub encsubseg;
    public Vertex subsegorg;
    public Vertex subsegdest;

    public BadSubseg()
    {
      this.Hash = BadSubseg.hashSeed++;
    }

    public override int GetHashCode()
    {
      return this.Hash;
    }

    public override string ToString()
    {
      return string.Format("B-SID {0}", (object) this.encsubseg.seg.hash);
    }
  }
}
