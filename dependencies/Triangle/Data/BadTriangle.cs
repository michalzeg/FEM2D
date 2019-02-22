// Decompiled with JetBrains decompiler
// Type: TriangleNet.Data.BadTriangle
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

namespace TriangleNet.Data
{
  internal class BadTriangle
  {
    public static int OTID;
    public int ID;
    public Otri poortri;
    public double key;
    public Vertex triangorg;
    public Vertex triangdest;
    public Vertex triangapex;
    public BadTriangle nexttriang;

    public BadTriangle()
    {
      this.ID = BadTriangle.OTID++;
    }

    public override string ToString()
    {
      return string.Format("B-TID {0}", (object) this.poortri.triangle.hash);
    }
  }
}
