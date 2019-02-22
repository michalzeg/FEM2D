// Decompiled with JetBrains decompiler
// Type: TriangleNet.Log.SimpleLog
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System.Collections.Generic;

namespace TriangleNet.Log
{
  public sealed class SimpleLog : ILog<SimpleLogItem>
  {
    private List<SimpleLogItem> log = new List<SimpleLogItem>();
    private static readonly SimpleLog instance = new SimpleLog();
    private LogLevel level;

    private SimpleLog()
    {
    }

    public static ILog<SimpleLogItem> Instance
    {
      get
      {
        return (ILog<SimpleLogItem>) SimpleLog.instance;
      }
    }

    public void Add(SimpleLogItem item)
    {
      this.log.Add(item);
    }

    public void Clear()
    {
      this.log.Clear();
    }

    public void Info(string message)
    {
      this.log.Add(new SimpleLogItem(LogLevel.Info, message));
    }

    public void Warning(string message, string location)
    {
      this.log.Add(new SimpleLogItem(LogLevel.Warning, message, location));
    }

    public void Error(string message, string location)
    {
      this.log.Add(new SimpleLogItem(LogLevel.Error, message, location));
    }

    public IList<SimpleLogItem> Data
    {
      get
      {
        return (IList<SimpleLogItem>) this.log;
      }
    }

    public LogLevel Level
    {
      get
      {
        return this.level;
      }
    }
  }
}
