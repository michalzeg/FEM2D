// Decompiled with JetBrains decompiler
// Type: TriangleNet.Behavior
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using TriangleNet.Log;

namespace TriangleNet
{
  public class Behavior
  {
    private bool boundaryMarkers = true;
    private int steiner = -1;
    private double maxArea = -1.0;
    internal bool useSegments = true;
    private bool poly;
    private bool quality;
    private bool varArea;
    private bool usertest;
    private bool convex;
    private bool jettison;
    private bool noHoles;
    private bool conformDel;
    private TriangulationAlgorithm algorithm;
    private int noBisect;
    private double minAngle;
    private double maxAngle;
    internal bool fixedArea;
    internal bool useRegions;
    internal double goodAngle;
    internal double maxGoodAngle;
    internal double offconstant;

    public Behavior(bool quality = false, double minAngle = 20.0)
    {
      if (!quality)
        return;
      this.quality = true;
      this.minAngle = minAngle;
      this.Update();
    }

    private void Update()
    {
      this.quality = true;
      if (this.minAngle < 0.0 || this.minAngle > 60.0)
      {
        this.minAngle = 0.0;
        this.quality = false;
        SimpleLog.Instance.Warning("Invalid quality option (minimum angle).", "Mesh.Behavior");
      }
      if (this.maxAngle != 0.0 && this.maxAngle < 90.0 || this.maxAngle > 180.0)
      {
        this.maxAngle = 0.0;
        this.quality = false;
        SimpleLog.Instance.Warning("Invalid quality option (maximum angle).", "Mesh.Behavior");
      }
      this.useSegments = this.Poly || this.Quality || this.Convex;
      this.goodAngle = Math.Cos(this.MinAngle * Math.PI / 180.0);
      this.maxGoodAngle = Math.Cos(this.MaxAngle * Math.PI / 180.0);
      this.offconstant = this.goodAngle != 1.0 ? 0.475 * Math.Sqrt((1.0 + this.goodAngle) / (1.0 - this.goodAngle)) : 0.0;
      this.goodAngle *= this.goodAngle;
    }

    public static bool NoExact { get; set; }

    public static bool Verbose { get; set; }

    public bool Quality
    {
      get
      {
        return this.quality;
      }
      set
      {
        this.quality = value;
        if (!this.quality)
          return;
        this.Update();
      }
    }

    public double MinAngle
    {
      get
      {
        return this.minAngle;
      }
      set
      {
        this.minAngle = value;
        this.Update();
      }
    }

    public double MaxAngle
    {
      get
      {
        return this.maxAngle;
      }
      set
      {
        this.maxAngle = value;
        this.Update();
      }
    }

    public double MaxArea
    {
      get
      {
        return this.maxArea;
      }
      set
      {
        this.maxArea = value;
        this.fixedArea = value > 0.0;
      }
    }

    public bool VarArea
    {
      get
      {
        return this.varArea;
      }
      set
      {
        this.varArea = value;
      }
    }

    public bool Poly
    {
      get
      {
        return this.poly;
      }
      set
      {
        this.poly = value;
      }
    }

    public bool Usertest
    {
      get
      {
        return this.usertest;
      }
      set
      {
        this.usertest = value;
      }
    }

    public bool Convex
    {
      get
      {
        return this.convex;
      }
      set
      {
        this.convex = value;
      }
    }

    public bool ConformingDelaunay
    {
      get
      {
        return this.conformDel;
      }
      set
      {
        this.conformDel = value;
      }
    }

    public TriangulationAlgorithm Algorithm
    {
      get
      {
        return this.algorithm;
      }
      set
      {
        this.algorithm = value;
      }
    }

    public int NoBisect
    {
      get
      {
        return this.noBisect;
      }
      set
      {
        this.noBisect = value;
        if (this.noBisect >= 0 && this.noBisect <= 2)
          return;
        this.noBisect = 0;
      }
    }

    public int SteinerPoints
    {
      get
      {
        return this.steiner;
      }
      set
      {
        this.steiner = value;
      }
    }

    public bool UseBoundaryMarkers
    {
      get
      {
        return this.boundaryMarkers;
      }
      set
      {
        this.boundaryMarkers = value;
      }
    }

    public bool NoHoles
    {
      get
      {
        return this.noHoles;
      }
      set
      {
        this.noHoles = value;
      }
    }

    public bool Jettison
    {
      get
      {
        return this.jettison;
      }
      set
      {
        this.jettison = value;
      }
    }
  }
}
