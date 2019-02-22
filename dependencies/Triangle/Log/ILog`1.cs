// Decompiled with JetBrains decompiler
// Type: TriangleNet.Log.ILog`1
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System.Collections.Generic;

namespace TriangleNet.Log
{
  public interface ILog<T> where T : ILogItem
  {
    void Add(T item);

    void Clear();

    void Info(string message);

    void Error(string message, string info);

    void Warning(string message, string info);

    IList<T> Data { get; }

    LogLevel Level { get; }
  }
}
