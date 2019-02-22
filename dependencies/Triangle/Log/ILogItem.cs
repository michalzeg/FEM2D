// Decompiled with JetBrains decompiler
// Type: TriangleNet.Log.ILogItem
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;

namespace TriangleNet.Log
{
  public interface ILogItem
  {
    DateTime Time { get; }

    LogLevel Level { get; }

    string Message { get; }

    string Info { get; }
  }
}
