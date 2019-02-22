// Decompiled with JetBrains decompiler
// Type: TriangleNet.Sampler
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace TriangleNet
{
  internal class Sampler
  {
    private static Random rand = new Random(DateTime.Now.Millisecond);
    private static int samplefactor = 11;
    private int samples = 1;
    private int triangleCount;
    private int[] keys;

    public void Reset()
    {
      this.samples = 1;
      this.triangleCount = 0;
    }

    public void Update(Mesh mesh)
    {
      this.Update(mesh, false);
    }

    public void Update(Mesh mesh, bool forceUpdate)
    {
      int count = mesh.triangles.Count;
      if (this.triangleCount == count && !forceUpdate)
        return;
      this.triangleCount = count;
      while (Sampler.samplefactor * this.samples * this.samples * this.samples < count)
        ++this.samples;
      this.keys = mesh.triangles.Keys.ToArray<int>();
    }

    public int[] GetSamples(Mesh mesh)
    {
      List<int> intList = new List<int>(this.samples);
      int num = this.triangleCount / this.samples;
      for (int index1 = 0; index1 < this.samples; ++index1)
      {
        int index2 = Sampler.rand.Next(index1 * num, (index1 + 1) * num - 1);
        if (!mesh.triangles.Keys.Contains<int>(this.keys[index2]))
        {
          this.Update(mesh, true);
          --index1;
        }
        else
          intList.Add(this.keys[index2]);
      }
      return intList.ToArray();
    }
  }
}
