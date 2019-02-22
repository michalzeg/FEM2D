﻿// Decompiled with JetBrains decompiler
// Type: TriangleNet.BadTriQueue
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using TriangleNet.Data;

namespace TriangleNet
{
  internal class BadTriQueue
  {
    private static readonly double SQRT2 = 1.4142135623731;
    private BadTriangle[] queuefront;
    private BadTriangle[] queuetail;
    private int[] nextnonemptyq;
    private int firstnonemptyq;
    private int count;

    public int Count
    {
      get
      {
        return this.count;
      }
    }

    public BadTriQueue()
    {
      this.queuefront = new BadTriangle[4096];
      this.queuetail = new BadTriangle[4096];
      this.nextnonemptyq = new int[4096];
      this.firstnonemptyq = -1;
      this.count = 0;
    }

    public void Enqueue(BadTriangle badtri)
    {
      ++this.count;
      double num1;
      int num2;
      if (badtri.key >= 1.0)
      {
        num1 = badtri.key;
        num2 = 1;
      }
      else
      {
        num1 = 1.0 / badtri.key;
        num2 = 0;
      }
      int num3 = 0;
      double num4;
      for (; num1 > 2.0; num1 *= num4)
      {
        int num5 = 1;
        for (num4 = 0.5; num1 * num4 * num4 > 1.0; num4 *= num4)
          num5 *= 2;
        num3 += num5;
      }
      int num6 = 2 * num3 + (num1 > BadTriQueue.SQRT2 ? 1 : 0);
      int index1 = num2 <= 0 ? 2048 + num6 : 2047 - num6;
      if (this.queuefront[index1] == null)
      {
        if (index1 > this.firstnonemptyq)
        {
          this.nextnonemptyq[index1] = this.firstnonemptyq;
          this.firstnonemptyq = index1;
        }
        else
        {
          int index2 = index1 + 1;
          while (this.queuefront[index2] == null)
            ++index2;
          this.nextnonemptyq[index1] = this.nextnonemptyq[index2];
          this.nextnonemptyq[index2] = index1;
        }
        this.queuefront[index1] = badtri;
      }
      else
        this.queuetail[index1].nexttriang = badtri;
      this.queuetail[index1] = badtri;
      badtri.nexttriang = (BadTriangle) null;
    }

    public void Enqueue(
      ref Otri enqtri,
      double minedge,
      Vertex enqapex,
      Vertex enqorg,
      Vertex enqdest)
    {
      this.Enqueue(new BadTriangle()
      {
        poortri = enqtri,
        key = minedge,
        triangapex = enqapex,
        triangorg = enqorg,
        triangdest = enqdest
      });
    }

    public BadTriangle Dequeue()
    {
      if (this.firstnonemptyq < 0)
        return (BadTriangle) null;
      --this.count;
      BadTriangle badTriangle = this.queuefront[this.firstnonemptyq];
      this.queuefront[this.firstnonemptyq] = badTriangle.nexttriang;
      if (badTriangle == this.queuetail[this.firstnonemptyq])
        this.firstnonemptyq = this.nextnonemptyq[this.firstnonemptyq];
      return badTriangle;
    }
  }
}
