// Decompiled with JetBrains decompiler
// Type: TriangleNet.Geometry.RegionPointer
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

namespace TriangleNet.Geometry
{
  public class RegionPointer
  {
    internal Point point;
    internal int id;

    public RegionPointer(double x, double y, int id)
    {
      this.point = new Point(x, y);
      this.id = id;
    }
  }
}
