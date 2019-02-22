// Decompiled with JetBrains decompiler
// Type: TriangleNet.Tools.IVoronoi
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System.Collections.Generic;
using TriangleNet.Geometry;

namespace TriangleNet.Tools
{
  public interface IVoronoi
  {
    Point[] Points { get; }

    List<VoronoiRegion> Regions { get; }
  }
}
