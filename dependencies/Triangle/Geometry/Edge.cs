// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.Edge
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

namespace TriangleNet.Geometry
{
  public class Edge
  {
    public int P0 { get; private set; }

    public int P1 { get; private set; }

    public int Boundary { get; private set; }

    public Edge(int p0, int p1)
      : this(p0, p1, 0)
    {
    }

    public Edge(int p0, int p1, int boundary)
    {
      this.P0 = p0;
      this.P1 = p1;
      this.Boundary = boundary;
    }
  }
}
