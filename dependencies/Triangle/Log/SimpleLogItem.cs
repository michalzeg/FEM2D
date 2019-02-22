// Decompiled with JetBrains decompiler
// Type: TriangleNet.Log.SimpleLogItem
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;

namespace TriangleNet.Log
{
  public class SimpleLogItem : ILogItem
  {
    private DateTime time;
    private LogLevel level;
    private string message;
    private string info;

    public DateTime Time
    {
      get
      {
        return this.time;
      }
    }

    public LogLevel Level
    {
      get
      {
        return this.level;
      }
    }

    public string Message
    {
      get
      {
        return this.message;
      }
    }

    public string Info
    {
      get
      {
        return this.info;
      }
    }

    public SimpleLogItem(LogLevel level, string message)
      : this(level, message, "")
    {
    }

    public SimpleLogItem(LogLevel level, string message, string info)
    {
      this.time = DateTime.Now;
      this.level = level;
      this.message = message;
      this.info = info;
    }
  }
}
