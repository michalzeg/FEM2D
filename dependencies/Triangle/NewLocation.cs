// Decompiled with JetBrains decompiler
// Type: TriangleNet.NewLocation
// Assembly: Triangle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E46F69E-BBCF-460A-9F37-AC970C590349
// Assembly location: D:\Programy\FEM2D\src\FEM2DTriangulation\bin\Debug\Triangle.dll

using System;
using TriangleNet.Data;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace TriangleNet
{
    internal class NewLocation
    {
        private const double EPS = 1E-50;
        private Mesh mesh;
        private Behavior behavior;

        public NewLocation(Mesh mesh)
        {
            this.mesh = mesh;
            this.behavior = mesh.behavior;
        }

        public Point FindLocation(
          Vertex torg,
          Vertex tdest,
          Vertex tapex,
          ref double xi,
          ref double eta,
          bool offcenter,
          Otri badotri)
        {
            if (this.behavior.MaxAngle == 0.0)
                return this.FindNewLocationWithoutMaxAngle(torg, tdest, tapex, ref xi, ref eta, true, badotri);
            return this.FindNewLocation(torg, tdest, tapex, ref xi, ref eta, true, badotri);
        }

        private Point FindNewLocationWithoutMaxAngle(
          Vertex torg,
          Vertex tdest,
          Vertex tapex,
          ref double xi,
          ref double eta,
          bool offcenter,
          Otri badotri)
        {
            double offconstant = this.behavior.offconstant;
            int num1 = 0;
            Otri neighotri = new Otri();
            double[] thirdpoint = new double[2];
            double xi1 = 0.0;
            double eta1 = 0.0;
            double[] p1 = new double[5];
            double[] p2 = new double[4];
            double num2 = 0.06;
            double num3 = 1.0;
            double num4 = 1.0;
            int num5 = 0;
            double[] newloc = new double[2];
            double num6 = 0.0;
            double num7 = 0.0;
            ++Statistic.CircumcenterCount;
            double num8 = tdest.x - torg.x;
            double num9 = tdest.y - torg.y;
            double num10 = tapex.x - torg.x;
            double num11 = tapex.y - torg.y;
            double num12 = tapex.x - tdest.x;
            double num13 = tapex.y - tdest.y;
            double dodist = num8 * num8 + num9 * num9;
            double aodist = num10 * num10 + num11 * num11;
            double dadist = (tdest.x - tapex.x) * (tdest.x - tapex.x) + (tdest.y - tapex.y) * (tdest.y - tapex.y);
            double num14;
            if (Behavior.NoExact)
            {
                num14 = 0.5 / (num8 * num11 - num10 * num9);
            }
            else
            {
                num14 = 0.5 / Primitives.CounterClockwise((Point)tdest, (Point)tapex, (Point)torg);
                --Statistic.CounterClockwiseCount;
            }
            double num15 = (num11 * dodist - num9 * aodist) * num14;
            double num16 = (num8 * aodist - num10 * dodist) * num14;
            Point point1 = new Point(torg.x + num15, torg.y + num16);
            Otri deltri = badotri;
            int num17 = this.LongestShortestEdge(aodist, dadist, dodist);
            double num18;
            double num19;
            double d1;
            double d2;
            double num20;
            Point point2;
            Point point3;
            Point point4;
            switch (num17)
            {
                case 123:
                    num18 = num10;
                    num19 = num11;
                    d1 = aodist;
                    d2 = dadist;
                    num20 = dodist;
                    point2 = (Point)tdest;
                    point3 = (Point)torg;
                    point4 = (Point)tapex;
                    break;

                case 132:
                    num18 = num10;
                    num19 = num11;
                    d1 = aodist;
                    d2 = dodist;
                    num20 = dadist;
                    point2 = (Point)tdest;
                    point3 = (Point)tapex;
                    point4 = (Point)torg;
                    break;

                case 213:
                    num18 = num12;
                    num19 = num13;
                    d1 = dadist;
                    d2 = aodist;
                    num20 = dodist;
                    point2 = (Point)torg;
                    point3 = (Point)tdest;
                    point4 = (Point)tapex;
                    break;

                case 231:
                    num18 = num12;
                    num19 = num13;
                    d1 = dadist;
                    d2 = dodist;
                    num20 = aodist;
                    point2 = (Point)torg;
                    point3 = (Point)tapex;
                    point4 = (Point)tdest;
                    break;

                case 312:
                    num18 = num8;
                    num19 = num9;
                    d1 = dodist;
                    d2 = aodist;
                    num20 = dadist;
                    point2 = (Point)tapex;
                    point3 = (Point)tdest;
                    point4 = (Point)torg;
                    break;

                default:
                    num18 = num8;
                    num19 = num9;
                    d1 = dodist;
                    d2 = dadist;
                    num20 = aodist;
                    point2 = (Point)tapex;
                    point3 = (Point)torg;
                    point4 = (Point)tdest;
                    break;
            }
            if (offcenter && offconstant > 0.0)
            {
                switch (num17)
                {
                    case 123:
                    case 132:
                        double num21 = 0.5 * num18 + offconstant * num19;
                        double num22 = 0.5 * num19 - offconstant * num18;
                        if (num21 * num21 + num22 * num22 < num15 * num15 + num16 * num16)
                        {
                            num15 = num21;
                            num16 = num22;
                            break;
                        }
                        num1 = 1;
                        break;

                    case 213:
                    case 231:
                        double num23 = 0.5 * num18 - offconstant * num19;
                        double num24 = 0.5 * num19 + offconstant * num18;
                        if (num23 * num23 + num24 * num24 < (num15 - num8) * (num15 - num8) + (num16 - num9) * (num16 - num9))
                        {
                            num15 = num8 + num23;
                            num16 = num9 + num24;
                            break;
                        }
                        num1 = 1;
                        break;

                    default:
                        double num25 = 0.5 * num18 - offconstant * num19;
                        double num26 = 0.5 * num19 + offconstant * num18;
                        if (num25 * num25 + num26 * num26 < num15 * num15 + num16 * num16)
                        {
                            num15 = num25;
                            num16 = num26;
                            break;
                        }
                        num1 = 1;
                        break;
                }
            }
            if (num1 == 1)
            {
                double num27 = (d2 + d1 - num20) / (2.0 * Math.Sqrt(d2) * Math.Sqrt(d1));
                bool isObtuse = num27 < 0.0 || Math.Abs(num27 - 0.0) <= 1E-50;
                num5 = this.DoSmoothing(deltri, torg, tdest, tapex, ref newloc);
                if (num5 > 0)
                {
                    ++Statistic.RelocationCount;
                    num15 = newloc[0] - torg.x;
                    num16 = newloc[1] - torg.y;
                    num6 = torg.x;
                    num7 = torg.y;
                    switch (num5)
                    {
                        case 1:
                            this.mesh.DeleteVertex(ref deltri);
                            break;

                        case 2:
                            deltri.LnextSelf();
                            this.mesh.DeleteVertex(ref deltri);
                            break;

                        case 3:
                            deltri.LprevSelf();
                            this.mesh.DeleteVertex(ref deltri);
                            break;
                    }
                }
                else
                {
                    double r = Math.Sqrt(d1) / (2.0 * Math.Sin(this.behavior.MinAngle * Math.PI / 180.0));
                    double num28 = (point3.x + point4.x) / 2.0;
                    double num29 = (point3.y + point4.y) / 2.0;
                    double num30 = num28 + Math.Sqrt(r * r - d1 / 4.0) * (point3.y - point4.y) / Math.Sqrt(d1);
                    double num31 = num29 + Math.Sqrt(r * r - d1 / 4.0) * (point4.x - point3.x) / Math.Sqrt(d1);
                    double num32 = num28 - Math.Sqrt(r * r - d1 / 4.0) * (point3.y - point4.y) / Math.Sqrt(d1);
                    double num33 = num29 - Math.Sqrt(r * r - d1 / 4.0) * (point4.x - point3.x) / Math.Sqrt(d1);
                    double x3_1;
                    double y3_1;
                    if ((num30 - point2.x) * (num30 - point2.x) + (num31 - point2.y) * (num31 - point2.y) <= (num32 - point2.x) * (num32 - point2.x) + (num33 - point2.y) * (num33 - point2.y))
                    {
                        x3_1 = num30;
                        y3_1 = num31;
                    }
                    else
                    {
                        x3_1 = num32;
                        y3_1 = num33;
                    }
                    bool neighborsVertex1 = this.GetNeighborsVertex(badotri, point3.x, point3.y, point2.x, point2.y, ref thirdpoint, ref neighotri);
                    double num34 = num15;
                    double num35 = num16;
                    if (!neighborsVertex1)
                    {
                        Point circumcenter = Primitives.FindCircumcenter((Point)neighotri.Org(), (Point)neighotri.Dest(), (Point)neighotri.Apex(), ref xi1, ref eta1);
                        double num36 = point3.y - point2.y;
                        double num37 = point2.x - point3.x;
                        double x2 = point1.x + num36;
                        double y2 = point1.y + num37;
                        this.CircleLineIntersection(point1.x, point1.y, x2, y2, x3_1, y3_1, r, ref p1);
                        double num38;
                        double num39;
                        if (this.ChooseCorrectPoint((point3.x + point2.x) / 2.0, (point3.y + point2.y) / 2.0, p1[3], p1[4], point1.x, point1.y, isObtuse))
                        {
                            num38 = p1[3];
                            num39 = p1[4];
                        }
                        else
                        {
                            num38 = p1[1];
                            num39 = p1[2];
                        }
                        this.PointBetweenPoints(num38, num39, point1.x, point1.y, circumcenter.x, circumcenter.y, ref p2);
                        if (p1[0] > 0.0)
                        {
                            if (Math.Abs(p2[0] - 1.0) <= 1E-50)
                            {
                                if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, circumcenter.x, circumcenter.y))
                                {
                                    num34 = num15;
                                    num35 = num16;
                                }
                                else
                                {
                                    num34 = p2[2] - torg.x;
                                    num35 = p2[3] - torg.y;
                                }
                            }
                            else if (this.IsBadTriangleAngle(point4.x, point4.y, point3.x, point3.y, num38, num39))
                            {
                                double num40 = Math.Sqrt((num38 - point1.x) * (num38 - point1.x) + (num39 - point1.y) * (num39 - point1.y));
                                double num41 = point1.x - num38;
                                double num42 = point1.y - num39;
                                double num43 = num41 / num40;
                                double num44 = num42 / num40;
                                double x3_2 = num38 + num43 * num2 * Math.Sqrt(d1);
                                double y3_2 = num39 + num44 * num2 * Math.Sqrt(d1);
                                if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, x3_2, y3_2))
                                {
                                    num34 = num15;
                                    num35 = num16;
                                }
                                else
                                {
                                    num34 = x3_2 - torg.x;
                                    num35 = y3_2 - torg.y;
                                }
                            }
                            else
                            {
                                num34 = num38 - torg.x;
                                num35 = num39 - torg.y;
                            }
                            if ((point2.x - point1.x) * (point2.x - point1.x) + (point2.y - point1.y) * (point2.y - point1.y) > num3 * ((point2.x - (num34 + torg.x)) * (point2.x - (num34 + torg.x)) + (point2.y - (num35 + torg.y)) * (point2.y - (num35 + torg.y))))
                            {
                                num34 = num15;
                                num35 = num16;
                            }
                        }
                    }
                    bool neighborsVertex2 = this.GetNeighborsVertex(badotri, point4.x, point4.y, point2.x, point2.y, ref thirdpoint, ref neighotri);
                    double num45 = num15;
                    double num46 = num16;
                    if (!neighborsVertex2)
                    {
                        Point circumcenter = Primitives.FindCircumcenter((Point)neighotri.Org(), (Point)neighotri.Dest(), (Point)neighotri.Apex(), ref xi1, ref eta1);
                        double num36 = point4.y - point2.y;
                        double num37 = point2.x - point4.x;
                        double x2 = point1.x + num36;
                        double y2 = point1.y + num37;
                        this.CircleLineIntersection(point1.x, point1.y, x2, y2, x3_1, y3_1, r, ref p1);
                        double num38;
                        double num39;
                        if (this.ChooseCorrectPoint((point4.x + point2.x) / 2.0, (point4.y + point2.y) / 2.0, p1[3], p1[4], point1.x, point1.y, false))
                        {
                            num38 = p1[3];
                            num39 = p1[4];
                        }
                        else
                        {
                            num38 = p1[1];
                            num39 = p1[2];
                        }
                        this.PointBetweenPoints(num38, num39, point1.x, point1.y, circumcenter.x, circumcenter.y, ref p2);
                        if (p1[0] > 0.0)
                        {
                            if (Math.Abs(p2[0] - 1.0) <= 1E-50)
                            {
                                if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, circumcenter.x, circumcenter.y))
                                {
                                    num45 = num15;
                                    num46 = num16;
                                }
                                else
                                {
                                    num45 = p2[2] - torg.x;
                                    num46 = p2[3] - torg.y;
                                }
                            }
                            else if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num38, num39))
                            {
                                double num40 = Math.Sqrt((num38 - point1.x) * (num38 - point1.x) + (num39 - point1.y) * (num39 - point1.y));
                                double num41 = point1.x - num38;
                                double num42 = point1.y - num39;
                                double num43 = num41 / num40;
                                double num44 = num42 / num40;
                                double x3_2 = num38 + num43 * num2 * Math.Sqrt(d1);
                                double y3_2 = num39 + num44 * num2 * Math.Sqrt(d1);
                                if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, x3_2, y3_2))
                                {
                                    num45 = num15;
                                    num46 = num16;
                                }
                                else
                                {
                                    num45 = x3_2 - torg.x;
                                    num46 = y3_2 - torg.y;
                                }
                            }
                            else
                            {
                                num45 = num38 - torg.x;
                                num46 = num39 - torg.y;
                            }
                            if ((point2.x - point1.x) * (point2.x - point1.x) + (point2.y - point1.y) * (point2.y - point1.y) > num3 * ((point2.x - (num45 + torg.x)) * (point2.x - (num45 + torg.x)) + (point2.y - (num46 + torg.y)) * (point2.y - (num46 + torg.y))))
                            {
                                num45 = num15;
                                num46 = num16;
                            }
                        }
                    }
                    if (isObtuse)
                    {
                        num15 = num34;
                        num16 = num35;
                    }
                    else if (num4 * ((point2.x - (num45 + torg.x)) * (point2.x - (num45 + torg.x)) + (point2.y - (num46 + torg.y)) * (point2.y - (num46 + torg.y))) > (point2.x - (num34 + torg.x)) * (point2.x - (num34 + torg.x)) + (point2.y - (num35 + torg.y)) * (point2.y - (num35 + torg.y)))
                    {
                        num15 = num45;
                        num16 = num46;
                    }
                    else
                    {
                        num15 = num34;
                        num16 = num35;
                    }
                }
            }
            Point point5 = new Point();
            if (num5 <= 0)
            {
                point5.x = torg.x + num15;
                point5.y = torg.y + num16;
            }
            else
            {
                point5.x = num6 + num15;
                point5.y = num7 + num16;
            }
            xi = (num11 * num15 - num10 * num16) * (2.0 * num14);
            eta = (num8 * num16 - num9 * num15) * (2.0 * num14);
            return point5;
        }

        private Point FindNewLocation(
          Vertex torg,
          Vertex tdest,
          Vertex tapex,
          ref double xi,
          ref double eta,
          bool offcenter,
          Otri badotri)
        {
            double offconstant = this.behavior.offconstant;
            int num1 = 0;
            Otri otri = new Otri();
            double[] thirdpoint = new double[2];
            double xi1 = 0.0;
            double eta1 = 0.0;
            double[] p1 = new double[5];
            double[] p2 = new double[4];
            double num2 = 0.06;
            double num3 = 1.0;
            double num4 = 1.0;
            int num5 = 0;
            double[] newloc = new double[2];
            double num6 = 0.0;
            double num7 = 0.0;
            double num8 = 0.0;
            double num9 = 0.0;
            double[] p3 = new double[3];
            double[] p4 = new double[4];
            ++Statistic.CircumcenterCount;
            double num10 = tdest.x - torg.x;
            double num11 = tdest.y - torg.y;
            double num12 = tapex.x - torg.x;
            double num13 = tapex.y - torg.y;
            double num14 = tapex.x - tdest.x;
            double num15 = tapex.y - tdest.y;
            double dodist = num10 * num10 + num11 * num11;
            double aodist = num12 * num12 + num13 * num13;
            double dadist = (tdest.x - tapex.x) * (tdest.x - tapex.x) + (tdest.y - tapex.y) * (tdest.y - tapex.y);
            double num16;
            if (Behavior.NoExact)
            {
                num16 = 0.5 / (num10 * num13 - num12 * num11);
            }
            else
            {
                num16 = 0.5 / Primitives.CounterClockwise((Point)tdest, (Point)tapex, (Point)torg);
                --Statistic.CounterClockwiseCount;
            }
            double num17 = (num13 * dodist - num11 * aodist) * num16;
            double num18 = (num10 * aodist - num12 * dodist) * num16;
            Point point1 = new Point(torg.x + num17, torg.y + num18);
            Otri deltri = badotri;
            int num19 = this.LongestShortestEdge(aodist, dadist, dodist);
            double num20;
            double num21;
            double d1;
            double d2;
            double d3;
            Point point2;
            Point point3;
            Point point4;
            switch (num19)
            {
                case 123:
                    num20 = num12;
                    num21 = num13;
                    d1 = aodist;
                    d2 = dadist;
                    d3 = dodist;
                    point2 = (Point)tdest;
                    point3 = (Point)torg;
                    point4 = (Point)tapex;
                    break;

                case 132:
                    num20 = num12;
                    num21 = num13;
                    d1 = aodist;
                    d2 = dodist;
                    d3 = dadist;
                    point2 = (Point)tdest;
                    point3 = (Point)tapex;
                    point4 = (Point)torg;
                    break;

                case 213:
                    num20 = num14;
                    num21 = num15;
                    d1 = dadist;
                    d2 = aodist;
                    d3 = dodist;
                    point2 = (Point)torg;
                    point3 = (Point)tdest;
                    point4 = (Point)tapex;
                    break;

                case 231:
                    num20 = num14;
                    num21 = num15;
                    d1 = dadist;
                    d2 = dodist;
                    d3 = aodist;
                    point2 = (Point)torg;
                    point3 = (Point)tapex;
                    point4 = (Point)tdest;
                    break;

                case 312:
                    num20 = num10;
                    num21 = num11;
                    d1 = dodist;
                    d2 = aodist;
                    d3 = dadist;
                    point2 = (Point)tapex;
                    point3 = (Point)tdest;
                    point4 = (Point)torg;
                    break;

                default:
                    num20 = num10;
                    num21 = num11;
                    d1 = dodist;
                    d2 = dadist;
                    d3 = aodist;
                    point2 = (Point)tapex;
                    point3 = (Point)torg;
                    point4 = (Point)tdest;
                    break;
            }
            if (offcenter && offconstant > 0.0)
            {
                switch (num19)
                {
                    case 123:
                    case 132:
                        double num22 = 0.5 * num20 + offconstant * num21;
                        double num23 = 0.5 * num21 - offconstant * num20;
                        if (num22 * num22 + num23 * num23 < num17 * num17 + num18 * num18)
                        {
                            num17 = num22;
                            num18 = num23;
                            break;
                        }
                        num1 = 1;
                        break;

                    case 213:
                    case 231:
                        double num24 = 0.5 * num20 - offconstant * num21;
                        double num25 = 0.5 * num21 + offconstant * num20;
                        if (num24 * num24 + num25 * num25 < (num17 - num10) * (num17 - num10) + (num18 - num11) * (num18 - num11))
                        {
                            num17 = num10 + num24;
                            num18 = num11 + num25;
                            break;
                        }
                        num1 = 1;
                        break;

                    default:
                        double num26 = 0.5 * num20 - offconstant * num21;
                        double num27 = 0.5 * num21 + offconstant * num20;
                        if (num26 * num26 + num27 * num27 < num17 * num17 + num18 * num18)
                        {
                            num17 = num26;
                            num18 = num27;
                            break;
                        }
                        num1 = 1;
                        break;
                }
            }
            if (num1 == 1)
            {
                double num28 = (d2 + d1 - d3) / (2.0 * Math.Sqrt(d2) * Math.Sqrt(d1));
                bool isObtuse = num28 < 0.0 || Math.Abs(num28 - 0.0) <= 1E-50;
                num5 = this.DoSmoothing(deltri, torg, tdest, tapex, ref newloc);
                if (num5 > 0)
                {
                    ++Statistic.RelocationCount;
                    num17 = newloc[0] - torg.x;
                    num18 = newloc[1] - torg.y;
                    num6 = torg.x;
                    num7 = torg.y;
                    switch (num5)
                    {
                        case 1:
                            this.mesh.DeleteVertex(ref deltri);
                            break;

                        case 2:
                            deltri.LnextSelf();
                            this.mesh.DeleteVertex(ref deltri);
                            break;

                        case 3:
                            deltri.LprevSelf();
                            this.mesh.DeleteVertex(ref deltri);
                            break;
                    }
                }
                else
                {
                    double num29 = Math.Acos((d2 + d3 - d1) / (2.0 * Math.Sqrt(d2) * Math.Sqrt(d3))) * 180.0 / Math.PI;
                    double num30 = this.behavior.MinAngle <= num29 ? num29 + 0.5 : this.behavior.MinAngle;
                    double r = Math.Sqrt(d1) / (2.0 * Math.Sin(num30 * Math.PI / 180.0));
                    double num31 = (point3.x + point4.x) / 2.0;
                    double num32 = (point3.y + point4.y) / 2.0;
                    double num33 = num31 + Math.Sqrt(r * r - d1 / 4.0) * (point3.y - point4.y) / Math.Sqrt(d1);
                    double num34 = num32 + Math.Sqrt(r * r - d1 / 4.0) * (point4.x - point3.x) / Math.Sqrt(d1);
                    double num35 = num31 - Math.Sqrt(r * r - d1 / 4.0) * (point3.y - point4.y) / Math.Sqrt(d1);
                    double num36 = num32 - Math.Sqrt(r * r - d1 / 4.0) * (point4.x - point3.x) / Math.Sqrt(d1);
                    double x3_1;
                    double y3_1;
                    if ((num33 - point2.x) * (num33 - point2.x) + (num34 - point2.y) * (num34 - point2.y) <= (num35 - point2.x) * (num35 - point2.x) + (num36 - point2.y) * (num36 - point2.y))
                    {
                        x3_1 = num33;
                        y3_1 = num34;
                    }
                    else
                    {
                        x3_1 = num35;
                        y3_1 = num36;
                    }
                    bool neighborsVertex1 = this.GetNeighborsVertex(badotri, point3.x, point3.y, point2.x, point2.y, ref thirdpoint, ref otri);
                    double num37 = num17;
                    double num38 = num18;
                    double num39 = Math.Sqrt((x3_1 - num31) * (x3_1 - num31) + (y3_1 - num32) * (y3_1 - num32));
                    double num40 = (x3_1 - num31) / num39;
                    double num41 = (y3_1 - num32) / num39;
                    double num42 = x3_1 + num40 * r;
                    double num43 = y3_1 + num41 * r;
                    double num44 = (2.0 * this.behavior.MaxAngle + num30 - 180.0) * Math.PI / 180.0;
                    double x3_2 = num42 * Math.Cos(num44) + num43 * Math.Sin(num44) + x3_1 - x3_1 * Math.Cos(num44) - y3_1 * Math.Sin(num44);
                    double y3_2 = -num42 * Math.Sin(num44) + num43 * Math.Cos(num44) + y3_1 + x3_1 * Math.Sin(num44) - y3_1 * Math.Cos(num44);
                    double x1_1 = num42 * Math.Cos(num44) - num43 * Math.Sin(num44) + x3_1 - x3_1 * Math.Cos(num44) + y3_1 * Math.Sin(num44);
                    double y1_1 = num42 * Math.Sin(num44) + num43 * Math.Cos(num44) + y3_1 - x3_1 * Math.Sin(num44) - y3_1 * Math.Cos(num44);
                    double num45;
                    double num46;
                    double num47;
                    double num48;
                    if (this.ChooseCorrectPoint(x1_1, y1_1, point3.x, point3.y, x3_2, y3_2, true))
                    {
                        num45 = x3_2;
                        num46 = y3_2;
                        num47 = x1_1;
                        num48 = y1_1;
                    }
                    else
                    {
                        num45 = x1_1;
                        num46 = y1_1;
                        num47 = x3_2;
                        num48 = y3_2;
                    }
                    double x1_2 = (point3.x + point2.x) / 2.0;
                    double y1_2 = (point3.y + point2.y) / 2.0;
                    double num49;
                    double num50;
                    if (!neighborsVertex1)
                    {
                        Point circumcenter = Primitives.FindCircumcenter((Point)otri.Org(), (Point)otri.Dest(), (Point)otri.Apex(), ref xi1, ref eta1);
                        double num51 = point3.y - point2.y;
                        double num52 = point2.x - point3.x;
                        double x2 = point1.x + num51;
                        double y2 = point1.y + num52;
                        this.CircleLineIntersection(point1.x, point1.y, x2, y2, x3_1, y3_1, r, ref p1);
                        double num53;
                        double num54;
                        if (this.ChooseCorrectPoint(x1_2, y1_2, p1[3], p1[4], point1.x, point1.y, isObtuse))
                        {
                            num53 = p1[3];
                            num54 = p1[4];
                        }
                        else
                        {
                            num53 = p1[1];
                            num54 = p1[2];
                        }
                        double x = point3.x;
                        double y = point3.y;
                        num49 = point4.x - point3.x;
                        num50 = point4.y - point3.y;
                        double x4 = num45;
                        double y4 = num46;
                        this.LineLineIntersection(point1.x, point1.y, x2, y2, x, y, x4, y4, ref p3);
                        if (p3[0] > 0.0)
                        {
                            num8 = p3[1];
                            num9 = p3[2];
                        }
                        this.PointBetweenPoints(num53, num54, point1.x, point1.y, circumcenter.x, circumcenter.y, ref p2);
                        if (p1[0] > 0.0)
                        {
                            if (Math.Abs(p2[0] - 1.0) <= 1E-50)
                            {
                                this.PointBetweenPoints(p2[2], p2[3], point1.x, point1.y, num8, num9, ref p4);
                                if (Math.Abs(p4[0] - 1.0) <= 1E-50 && p3[0] > 0.0)
                                {
                                    if ((point2.x - num45) * (point2.x - num45) + (point2.y - num46) * (point2.y - num46) > num3 * ((point2.x - num8) * (point2.x - num8) + (point2.y - num9) * (point2.y - num9)) && this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num45, num46) && this.MinDistanceToNeighbor(num45, num46, ref otri) > this.MinDistanceToNeighbor(num8, num9, ref otri))
                                    {
                                        num37 = num45 - torg.x;
                                        num38 = num46 - torg.y;
                                    }
                                    else if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num8, num9))
                                    {
                                        double num55 = Math.Sqrt((num8 - point1.x) * (num8 - point1.x) + (num9 - point1.y) * (num9 - point1.y));
                                        double num56 = point1.x - num8;
                                        double num57 = point1.y - num9;
                                        double num58 = num56 / num55;
                                        double num59 = num57 / num55;
                                        num8 += num58 * num2 * Math.Sqrt(d1);
                                        num9 += num59 * num2 * Math.Sqrt(d1);
                                        if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num8, num9))
                                        {
                                            num37 = num17;
                                            num38 = num18;
                                        }
                                        else
                                        {
                                            num37 = num8 - torg.x;
                                            num38 = num9 - torg.y;
                                        }
                                    }
                                    else
                                    {
                                        num37 = p4[2] - torg.x;
                                        num38 = p4[3] - torg.y;
                                    }
                                }
                                else if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, circumcenter.x, circumcenter.y))
                                {
                                    num37 = num17;
                                    num38 = num18;
                                }
                                else
                                {
                                    num37 = p2[2] - torg.x;
                                    num38 = p2[3] - torg.y;
                                }
                            }
                            else
                            {
                                this.PointBetweenPoints(num53, num54, point1.x, point1.y, num8, num9, ref p4);
                                if (Math.Abs(p4[0] - 1.0) <= 1E-50 && p3[0] > 0.0)
                                {
                                    if ((point2.x - num45) * (point2.x - num45) + (point2.y - num46) * (point2.y - num46) > num3 * ((point2.x - num8) * (point2.x - num8) + (point2.y - num9) * (point2.y - num9)) && this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num45, num46) && this.MinDistanceToNeighbor(num45, num46, ref otri) > this.MinDistanceToNeighbor(num8, num9, ref otri))
                                    {
                                        num37 = num45 - torg.x;
                                        num38 = num46 - torg.y;
                                    }
                                    else if (this.IsBadTriangleAngle(point4.x, point4.y, point3.x, point3.y, num8, num9))
                                    {
                                        double num55 = Math.Sqrt((num8 - point1.x) * (num8 - point1.x) + (num9 - point1.y) * (num9 - point1.y));
                                        double num56 = point1.x - num8;
                                        double num57 = point1.y - num9;
                                        double num58 = num56 / num55;
                                        double num59 = num57 / num55;
                                        num8 += num58 * num2 * Math.Sqrt(d1);
                                        num9 += num59 * num2 * Math.Sqrt(d1);
                                        if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num8, num9))
                                        {
                                            num37 = num17;
                                            num38 = num18;
                                        }
                                        else
                                        {
                                            num37 = num8 - torg.x;
                                            num38 = num9 - torg.y;
                                        }
                                    }
                                    else
                                    {
                                        num37 = p4[2] - torg.x;
                                        num38 = p4[3] - torg.y;
                                    }
                                }
                                else if (this.IsBadTriangleAngle(point4.x, point4.y, point3.x, point3.y, num53, num54))
                                {
                                    double num55 = Math.Sqrt((num53 - point1.x) * (num53 - point1.x) + (num54 - point1.y) * (num54 - point1.y));
                                    double num56 = point1.x - num53;
                                    double num57 = point1.y - num54;
                                    double num58 = num56 / num55;
                                    double num59 = num57 / num55;
                                    double x3_3 = num53 + num58 * num2 * Math.Sqrt(d1);
                                    double y3_3 = num54 + num59 * num2 * Math.Sqrt(d1);
                                    if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, x3_3, y3_3))
                                    {
                                        num37 = num17;
                                        num38 = num18;
                                    }
                                    else
                                    {
                                        num37 = x3_3 - torg.x;
                                        num38 = y3_3 - torg.y;
                                    }
                                }
                                else
                                {
                                    num37 = num53 - torg.x;
                                    num38 = num54 - torg.y;
                                }
                            }
                            if ((point2.x - point1.x) * (point2.x - point1.x) + (point2.y - point1.y) * (point2.y - point1.y) > num3 * ((point2.x - (num37 + torg.x)) * (point2.x - (num37 + torg.x)) + (point2.y - (num38 + torg.y)) * (point2.y - (num38 + torg.y))))
                            {
                                num37 = num17;
                                num38 = num18;
                            }
                        }
                    }
                    bool neighborsVertex2 = this.GetNeighborsVertex(badotri, point4.x, point4.y, point2.x, point2.y, ref thirdpoint, ref otri);
                    double num60 = num17;
                    double num61 = num18;
                    double x1_3 = (point4.x + point2.x) / 2.0;
                    double y1_3 = (point4.y + point2.y) / 2.0;
                    if (!neighborsVertex2)
                    {
                        Point circumcenter = Primitives.FindCircumcenter((Point)otri.Org(), (Point)otri.Dest(), (Point)otri.Apex(), ref xi1, ref eta1);
                        double num51 = point4.y - point2.y;
                        double num52 = point2.x - point4.x;
                        double x2 = point1.x + num51;
                        double y2 = point1.y + num52;
                        this.CircleLineIntersection(point1.x, point1.y, x2, y2, x3_1, y3_1, r, ref p1);
                        double num53;
                        double num54;
                        if (this.ChooseCorrectPoint(x1_3, y1_3, p1[3], p1[4], point1.x, point1.y, false))
                        {
                            num53 = p1[3];
                            num54 = p1[4];
                        }
                        else
                        {
                            num53 = p1[1];
                            num54 = p1[2];
                        }
                        double x = point4.x;
                        double y = point4.y;
                        num49 = point3.x - point4.x;
                        num50 = point3.y - point4.y;
                        double x4 = num47;
                        double y4 = num48;
                        this.LineLineIntersection(point1.x, point1.y, x2, y2, x, y, x4, y4, ref p3);
                        if (p3[0] > 0.0)
                        {
                            num8 = p3[1];
                            num9 = p3[2];
                        }
                        this.PointBetweenPoints(num53, num54, point1.x, point1.y, circumcenter.x, circumcenter.y, ref p2);
                        if (p1[0] > 0.0)
                        {
                            if (Math.Abs(p2[0] - 1.0) <= 1E-50)
                            {
                                this.PointBetweenPoints(p2[2], p2[3], point1.x, point1.y, num8, num9, ref p4);
                                if (Math.Abs(p4[0] - 1.0) <= 1E-50 && p3[0] > 0.0)
                                {
                                    if ((point2.x - num47) * (point2.x - num47) + (point2.y - num48) * (point2.y - num48) > num3 * ((point2.x - num8) * (point2.x - num8) + (point2.y - num9) * (point2.y - num9)) && this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num47, num48) && this.MinDistanceToNeighbor(num47, num48, ref otri) > this.MinDistanceToNeighbor(num8, num9, ref otri))
                                    {
                                        num60 = num47 - torg.x;
                                        num61 = num48 - torg.y;
                                    }
                                    else if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num8, num9))
                                    {
                                        double num55 = Math.Sqrt((num8 - point1.x) * (num8 - point1.x) + (num9 - point1.y) * (num9 - point1.y));
                                        double num56 = point1.x - num8;
                                        double num57 = point1.y - num9;
                                        double num58 = num56 / num55;
                                        double num59 = num57 / num55;
                                        double x3_3 = num8 + num58 * num2 * Math.Sqrt(d1);
                                        double y3_3 = num9 + num59 * num2 * Math.Sqrt(d1);
                                        if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, x3_3, y3_3))
                                        {
                                            num60 = num17;
                                            num61 = num18;
                                        }
                                        else
                                        {
                                            num60 = x3_3 - torg.x;
                                            num61 = y3_3 - torg.y;
                                        }
                                    }
                                    else
                                    {
                                        num60 = p4[2] - torg.x;
                                        num61 = p4[3] - torg.y;
                                    }
                                }
                                else if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, circumcenter.x, circumcenter.y))
                                {
                                    num60 = num17;
                                    num61 = num18;
                                }
                                else
                                {
                                    num60 = p2[2] - torg.x;
                                    num61 = p2[3] - torg.y;
                                }
                            }
                            else
                            {
                                this.PointBetweenPoints(num53, num54, point1.x, point1.y, num8, num9, ref p4);
                                if (Math.Abs(p4[0] - 1.0) <= 1E-50 && p3[0] > 0.0)
                                {
                                    if ((point2.x - num47) * (point2.x - num47) + (point2.y - num48) * (point2.y - num48) > num3 * ((point2.x - num8) * (point2.x - num8) + (point2.y - num9) * (point2.y - num9)) && this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num47, num48) && this.MinDistanceToNeighbor(num47, num48, ref otri) > this.MinDistanceToNeighbor(num8, num9, ref otri))
                                    {
                                        num60 = num47 - torg.x;
                                        num61 = num48 - torg.y;
                                    }
                                    else if (this.IsBadTriangleAngle(point4.x, point4.y, point3.x, point3.y, num8, num9))
                                    {
                                        double num55 = Math.Sqrt((num8 - point1.x) * (num8 - point1.x) + (num9 - point1.y) * (num9 - point1.y));
                                        double num56 = point1.x - num8;
                                        double num57 = point1.y - num9;
                                        double num58 = num56 / num55;
                                        double num59 = num57 / num55;
                                        double x3_3 = num8 + num58 * num2 * Math.Sqrt(d1);
                                        double y3_3 = num9 + num59 * num2 * Math.Sqrt(d1);
                                        if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, x3_3, y3_3))
                                        {
                                            num60 = num17;
                                            num61 = num18;
                                        }
                                        else
                                        {
                                            num60 = x3_3 - torg.x;
                                            num61 = y3_3 - torg.y;
                                        }
                                    }
                                    else
                                    {
                                        num60 = p4[2] - torg.x;
                                        num61 = p4[3] - torg.y;
                                    }
                                }
                                else if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, num53, num54))
                                {
                                    double num55 = Math.Sqrt((num53 - point1.x) * (num53 - point1.x) + (num54 - point1.y) * (num54 - point1.y));
                                    double num56 = point1.x - num53;
                                    double num57 = point1.y - num54;
                                    double num58 = num56 / num55;
                                    double num59 = num57 / num55;
                                    double x3_3 = num53 + num58 * num2 * Math.Sqrt(d1);
                                    double y3_3 = num54 + num59 * num2 * Math.Sqrt(d1);
                                    if (this.IsBadTriangleAngle(point3.x, point3.y, point4.x, point4.y, x3_3, y3_3))
                                    {
                                        num60 = num17;
                                        num61 = num18;
                                    }
                                    else
                                    {
                                        num60 = x3_3 - torg.x;
                                        num61 = y3_3 - torg.y;
                                    }
                                }
                                else
                                {
                                    num60 = num53 - torg.x;
                                    num61 = num54 - torg.y;
                                }
                            }
                            if ((point2.x - point1.x) * (point2.x - point1.x) + (point2.y - point1.y) * (point2.y - point1.y) > num3 * ((point2.x - (num60 + torg.x)) * (point2.x - (num60 + torg.x)) + (point2.y - (num61 + torg.y)) * (point2.y - (num61 + torg.y))))
                            {
                                num60 = num17;
                                num61 = num18;
                            }
                        }
                    }
                    if (isObtuse)
                    {
                        if (neighborsVertex1 && neighborsVertex2)
                        {
                            if (num4 * ((point2.x - x1_3) * (point2.x - x1_3) + (point2.y - y1_3) * (point2.y - y1_3)) > (point2.x - x1_2) * (point2.x - x1_2) + (point2.y - y1_2) * (point2.y - y1_2))
                            {
                                num17 = num60;
                                num18 = num61;
                            }
                            else
                            {
                                num17 = num37;
                                num18 = num38;
                            }
                        }
                        else if (neighborsVertex1)
                        {
                            if (num4 * ((point2.x - (num60 + torg.x)) * (point2.x - (num60 + torg.x)) + (point2.y - (num61 + torg.y)) * (point2.y - (num61 + torg.y))) > (point2.x - x1_2) * (point2.x - x1_2) + (point2.y - y1_2) * (point2.y - y1_2))
                            {
                                num17 = num60;
                                num18 = num61;
                            }
                            else
                            {
                                num17 = num37;
                                num18 = num38;
                            }
                        }
                        else if (neighborsVertex2)
                        {
                            if (num4 * ((point2.x - x1_3) * (point2.x - x1_3) + (point2.y - y1_3) * (point2.y - y1_3)) > (point2.x - (num37 + torg.x)) * (point2.x - (num37 + torg.x)) + (point2.y - (num38 + torg.y)) * (point2.y - (num38 + torg.y)))
                            {
                                num17 = num60;
                                num18 = num61;
                            }
                            else
                            {
                                num17 = num37;
                                num18 = num38;
                            }
                        }
                        else if (num4 * ((point2.x - (num60 + torg.x)) * (point2.x - (num60 + torg.x)) + (point2.y - (num61 + torg.y)) * (point2.y - (num61 + torg.y))) > (point2.x - (num37 + torg.x)) * (point2.x - (num37 + torg.x)) + (point2.y - (num38 + torg.y)) * (point2.y - (num38 + torg.y)))
                        {
                            num17 = num60;
                            num18 = num61;
                        }
                        else
                        {
                            num17 = num37;
                            num18 = num38;
                        }
                    }
                    else if (neighborsVertex1 && neighborsVertex2)
                    {
                        if (num4 * ((point2.x - x1_3) * (point2.x - x1_3) + (point2.y - y1_3) * (point2.y - y1_3)) > (point2.x - x1_2) * (point2.x - x1_2) + (point2.y - y1_2) * (point2.y - y1_2))
                        {
                            num17 = num60;
                            num18 = num61;
                        }
                        else
                        {
                            num17 = num37;
                            num18 = num38;
                        }
                    }
                    else if (neighborsVertex1)
                    {
                        if (num4 * ((point2.x - (num60 + torg.x)) * (point2.x - (num60 + torg.x)) + (point2.y - (num61 + torg.y)) * (point2.y - (num61 + torg.y))) > (point2.x - x1_2) * (point2.x - x1_2) + (point2.y - y1_2) * (point2.y - y1_2))
                        {
                            num17 = num60;
                            num18 = num61;
                        }
                        else
                        {
                            num17 = num37;
                            num18 = num38;
                        }
                    }
                    else if (neighborsVertex2)
                    {
                        if (num4 * ((point2.x - x1_3) * (point2.x - x1_3) + (point2.y - y1_3) * (point2.y - y1_3)) > (point2.x - (num37 + torg.x)) * (point2.x - (num37 + torg.x)) + (point2.y - (num38 + torg.y)) * (point2.y - (num38 + torg.y)))
                        {
                            num17 = num60;
                            num18 = num61;
                        }
                        else
                        {
                            num17 = num37;
                            num18 = num38;
                        }
                    }
                    else if (num4 * ((point2.x - (num60 + torg.x)) * (point2.x - (num60 + torg.x)) + (point2.y - (num61 + torg.y)) * (point2.y - (num61 + torg.y))) > (point2.x - (num37 + torg.x)) * (point2.x - (num37 + torg.x)) + (point2.y - (num38 + torg.y)) * (point2.y - (num38 + torg.y)))
                    {
                        num17 = num60;
                        num18 = num61;
                    }
                    else
                    {
                        num17 = num37;
                        num18 = num38;
                    }
                }
            }
            Point point5 = new Point();
            if (num5 <= 0)
            {
                point5.x = torg.x + num17;
                point5.y = torg.y + num18;
            }
            else
            {
                point5.x = num6 + num17;
                point5.y = num7 + num18;
            }
            xi = (num13 * num17 - num12 * num18) * (2.0 * num16);
            eta = (num10 * num18 - num11 * num17) * (2.0 * num16);
            return point5;
        }

        private int LongestShortestEdge(double aodist, double dadist, double dodist)
        {
            int num1;
            int num2;
            int num3;
            if (dodist < aodist && dodist < dadist)
            {
                num1 = 3;
                if (aodist < dadist)
                {
                    num2 = 2;
                    num3 = 1;
                }
                else
                {
                    num2 = 1;
                    num3 = 2;
                }
            }
            else if (aodist < dadist)
            {
                num1 = 1;
                if (dodist < dadist)
                {
                    num2 = 2;
                    num3 = 3;
                }
                else
                {
                    num2 = 3;
                    num3 = 2;
                }
            }
            else
            {
                num1 = 2;
                if (aodist < dodist)
                {
                    num2 = 3;
                    num3 = 1;
                }
                else
                {
                    num2 = 1;
                    num3 = 3;
                }
            }
            return num1 * 100 + num3 * 10 + num2;
        }

        private int DoSmoothing(
          Otri badotri,
          Vertex torg,
          Vertex tdest,
          Vertex tapex,
          ref double[] newloc)
        {
            double[] numArray = new double[6];
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            double[] points1 = new double[500];
            double[] points2 = new double[500];
            double[] points3 = new double[500];
            int starPoints1 = this.GetStarPoints(badotri, torg, tdest, tapex, 1, ref points1);
            if (torg.type == VertexType.FreeVertex && starPoints1 != 0 && this.ValidPolygonAngles(starPoints1, points1) && (this.behavior.MaxAngle != 0.0 ? this.GetWedgeIntersection(starPoints1, points1, ref newloc) : this.GetWedgeIntersectionWithoutMaxAngle(starPoints1, points1, ref newloc)))
            {
                numArray[0] = newloc[0];
                numArray[1] = newloc[1];
                ++num1;
                num2 = 1;
            }
            int starPoints2 = this.GetStarPoints(badotri, torg, tdest, tapex, 2, ref points2);
            if (tdest.type == VertexType.FreeVertex && starPoints2 != 0 && this.ValidPolygonAngles(starPoints2, points2) && (this.behavior.MaxAngle != 0.0 ? this.GetWedgeIntersection(starPoints2, points2, ref newloc) : this.GetWedgeIntersectionWithoutMaxAngle(starPoints2, points2, ref newloc)))
            {
                numArray[2] = newloc[0];
                numArray[3] = newloc[1];
                ++num1;
                num3 = 2;
            }
            int starPoints3 = this.GetStarPoints(badotri, torg, tdest, tapex, 3, ref points3);
            if (tapex.type == VertexType.FreeVertex && starPoints3 != 0 && this.ValidPolygonAngles(starPoints3, points3) && (this.behavior.MaxAngle != 0.0 ? this.GetWedgeIntersection(starPoints3, points3, ref newloc) : this.GetWedgeIntersectionWithoutMaxAngle(starPoints3, points3, ref newloc)))
            {
                numArray[4] = newloc[0];
                numArray[5] = newloc[1];
                ++num1;
                num4 = 3;
            }
            if (num1 > 0)
            {
                if (num2 > 0)
                {
                    newloc[0] = numArray[0];
                    newloc[1] = numArray[1];
                    return num2;
                }
                if (num3 > 0)
                {
                    newloc[0] = numArray[2];
                    newloc[1] = numArray[3];
                    return num3;
                }
                if (num4 > 0)
                {
                    newloc[0] = numArray[4];
                    newloc[1] = numArray[5];
                    return num4;
                }
            }
            return 0;
        }

        private int GetStarPoints(
          Otri badotri,
          Vertex p,
          Vertex q,
          Vertex r,
          int whichPoint,
          ref double[] points)
        {
            Otri neighotri = new Otri();
            double first_x = 0.0;
            double first_y = 0.0;
            double second_x = 0.0;
            double second_y = 0.0;
            double num1 = 0.0;
            double num2 = 0.0;
            double[] thirdpoint = new double[2];
            int index1 = 0;
            switch (whichPoint)
            {
                case 1:
                    first_x = p.x;
                    first_y = p.y;
                    second_x = r.x;
                    second_y = r.y;
                    num1 = q.x;
                    num2 = q.y;
                    break;

                case 2:
                    first_x = q.x;
                    first_y = q.y;
                    second_x = p.x;
                    second_y = p.y;
                    num1 = r.x;
                    num2 = r.y;
                    break;

                case 3:
                    first_x = r.x;
                    first_y = r.y;
                    second_x = q.x;
                    second_y = q.y;
                    num1 = p.x;
                    num2 = p.y;
                    break;
            }
            Otri badotri1 = badotri;
            points[index1] = second_x;
            int index2 = index1 + 1;
            points[index2] = second_y;
            int index3 = index2 + 1;
            thirdpoint[0] = second_x;
            thirdpoint[1] = second_y;
            while (!this.GetNeighborsVertex(badotri1, first_x, first_y, second_x, second_y, ref thirdpoint, ref neighotri))
            {
                badotri1 = neighotri;
                second_x = thirdpoint[0];
                second_y = thirdpoint[1];
                points[index3] = thirdpoint[0];
                int index4 = index3 + 1;
                points[index4] = thirdpoint[1];
                index3 = index4 + 1;
                if (Math.Abs(thirdpoint[0] - num1) <= 1E-50 && Math.Abs(thirdpoint[1] - num2) <= 1E-50)
                    goto label_8;
            }
            index3 = 0;
            label_8:
            return index3 / 2;
        }

        private bool GetNeighborsVertex(
          Otri badotri,
          double first_x,
          double first_y,
          double second_x,
          double second_y,
          ref double[] thirdpoint,
          ref Otri neighotri)
        {
            Otri o2 = new Otri();
            bool flag = false;
            Vertex vertex1 = (Vertex)null;
            Vertex vertex2 = (Vertex)null;
            Vertex vertex3 = (Vertex)null;
            int num1 = 0;
            int num2 = 0;
            for (badotri.orient = 0; badotri.orient < 3; ++badotri.orient)
            {
                badotri.Sym(ref o2);
                if (o2.triangle != Mesh.dummytri)
                {
                    vertex1 = o2.Org();
                    vertex2 = o2.Dest();
                    vertex3 = o2.Apex();
                    if ((vertex1.x != vertex2.x || vertex1.y != vertex2.y) && (vertex2.x != vertex3.x || vertex2.y != vertex3.y) && (vertex1.x != vertex3.x || vertex1.y != vertex3.y))
                    {
                        num1 = 0;
                        if (Math.Abs(first_x - vertex1.x) < 1E-50 && Math.Abs(first_y - vertex1.y) < 1E-50)
                            num1 = 11;
                        else if (Math.Abs(first_x - vertex2.x) < 1E-50 && Math.Abs(first_y - vertex2.y) < 1E-50)
                            num1 = 12;
                        else if (Math.Abs(first_x - vertex3.x) < 1E-50 && Math.Abs(first_y - vertex3.y) < 1E-50)
                            num1 = 13;
                        num2 = 0;
                        if (Math.Abs(second_x - vertex1.x) < 1E-50 && Math.Abs(second_y - vertex1.y) < 1E-50)
                            num2 = 21;
                        else if (Math.Abs(second_x - vertex2.x) < 1E-50 && Math.Abs(second_y - vertex2.y) < 1E-50)
                            num2 = 22;
                        else if (Math.Abs(second_x - vertex3.x) < 1E-50 && Math.Abs(second_y - vertex3.y) < 1E-50)
                            num2 = 23;
                    }
                }
                if (num1 == 11 && (num2 == 22 || num2 == 23) || num1 == 12 && (num2 == 21 || num2 == 23) || num1 == 13 && (num2 == 21 || num2 == 22))
                    break;
            }
            switch (num1)
            {
                case 0:
                    flag = true;
                    break;

                case 11:
                    switch (num2)
                    {
                        case 22:
                            thirdpoint[0] = vertex3.x;
                            thirdpoint[1] = vertex3.y;
                            break;

                        case 23:
                            thirdpoint[0] = vertex2.x;
                            thirdpoint[1] = vertex2.y;
                            break;

                        default:
                            flag = true;
                            break;
                    };
                    break;

                case 12:
                    switch (num2)
                    {
                        case 21:
                            thirdpoint[0] = vertex3.x;
                            thirdpoint[1] = vertex3.y;
                            break;

                        case 23:
                            thirdpoint[0] = vertex1.x;
                            thirdpoint[1] = vertex1.y;
                            break;

                        default:
                            flag = true;
                            break;
                    };
                    break;

                case 13:
                    switch (num2)
                    {
                        case 21:
                            thirdpoint[0] = vertex2.x;
                            thirdpoint[1] = vertex2.y;
                            break;

                        case 22:
                            thirdpoint[0] = vertex1.x;
                            thirdpoint[1] = vertex1.y;
                            break;

                        default:
                            flag = true;
                            break;
                    };
                    break;

                default:
                    if (num2 == 0)
                    {
                        flag = true;
                        break;
                    }
                    break;
            }
            neighotri = o2;
            return flag;
        }

        private bool GetWedgeIntersectionWithoutMaxAngle(
          int numpoints,
          double[] points,
          ref double[] newloc)
        {
            double[] numArray1 = new double[2 * numpoints];
            double[] numArray2 = new double[2 * numpoints];
            double[] numArray3 = new double[2 * numpoints];
            double[] numArray4 = new double[2000];
            double[] p = new double[3];
            double[] convexPoly = new double[500];
            int numpoints1 = 0;
            double x1 = points[2 * numpoints - 4];
            double y1 = points[2 * numpoints - 3];
            double x3 = points[2 * numpoints - 2];
            double y3 = points[2 * numpoints - 1];
            double num1 = this.behavior.MinAngle * Math.PI / 180.0;
            double num2;
            double num3;
            if (this.behavior.goodAngle == 1.0)
            {
                num2 = 0.0;
                num3 = 0.0;
            }
            else
            {
                num2 = 0.5 / Math.Tan(num1);
                num3 = 0.5 / Math.Sin(num1);
            }
            for (int index1 = 0; index1 < numpoints * 2; index1 += 2)
            {
                double point1 = points[index1];
                double point2 = points[index1 + 1];
                double num4 = x3 - x1;
                double num5 = y3 - y1;
                double num6 = Math.Sqrt(num4 * num4 + num5 * num5);
                numArray1[index1 / 2] = x1 + 0.5 * num4 - num2 * num5;
                numArray2[index1 / 2] = y1 + 0.5 * num5 + num2 * num4;
                numArray3[index1 / 2] = num3 * num6;
                numArray1[numpoints + index1 / 2] = numArray1[index1 / 2];
                numArray2[numpoints + index1 / 2] = numArray2[index1 / 2];
                numArray3[numpoints + index1 / 2] = numArray3[index1 / 2];
                double num7 = (x1 + x3) / 2.0;
                double num8 = (y1 + y3) / 2.0;
                double num9 = Math.Sqrt((numArray1[index1 / 2] - num7) * (numArray1[index1 / 2] - num7) + (numArray2[index1 / 2] - num8) * (numArray2[index1 / 2] - num8));
                double num10 = (numArray1[index1 / 2] - num7) / num9;
                double num11 = (numArray2[index1 / 2] - num8) / num9;
                double num12 = numArray1[index1 / 2] + num10 * numArray3[index1 / 2];
                double num13 = numArray2[index1 / 2] + num11 * numArray3[index1 / 2];
                double num14 = x3 - x1;
                double num15 = y3 - y1;
                double x2 = x3 * Math.Cos(num1) - y3 * Math.Sin(num1) + x1 - x1 * Math.Cos(num1) + y1 * Math.Sin(num1);
                double y2 = x3 * Math.Sin(num1) + y3 * Math.Cos(num1) + y1 - x1 * Math.Sin(num1) - y1 * Math.Cos(num1);
                numArray4[index1 * 16] = x1;
                numArray4[index1 * 16 + 1] = y1;
                numArray4[index1 * 16 + 2] = x2;
                numArray4[index1 * 16 + 3] = y2;
                num14 = x1 - x3;
                num15 = y1 - y3;
                double x4 = x1 * Math.Cos(num1) + y1 * Math.Sin(num1) + x3 - x3 * Math.Cos(num1) - y3 * Math.Sin(num1);
                double y4 = -x1 * Math.Sin(num1) + y1 * Math.Cos(num1) + y3 + x3 * Math.Sin(num1) - y3 * Math.Cos(num1);
                numArray4[index1 * 16 + 4] = x4;
                numArray4[index1 * 16 + 5] = y4;
                numArray4[index1 * 16 + 6] = x3;
                numArray4[index1 * 16 + 7] = y3;
                num14 = num12 - numArray1[index1 / 2];
                num15 = num13 - numArray2[index1 / 2];
                double num16 = num12;
                double num17 = num13;
                for (int index2 = 1; index2 < 4; ++index2)
                {
                    double num18 = num12 * Math.Cos((Math.PI / 3.0 - num1) * (double)index2) + num13 * Math.Sin((Math.PI / 3.0 - num1) * (double)index2) + numArray1[index1 / 2] - numArray1[index1 / 2] * Math.Cos((Math.PI / 3.0 - num1) * (double)index2) - numArray2[index1 / 2] * Math.Sin((Math.PI / 3.0 - num1) * (double)index2);
                    double num19 = -num12 * Math.Sin((Math.PI / 3.0 - num1) * (double)index2) + num13 * Math.Cos((Math.PI / 3.0 - num1) * (double)index2) + numArray2[index1 / 2] + numArray1[index1 / 2] * Math.Sin((Math.PI / 3.0 - num1) * (double)index2) - numArray2[index1 / 2] * Math.Cos((Math.PI / 3.0 - num1) * (double)index2);
                    numArray4[index1 * 16 + 8 + 4 * (index2 - 1)] = num18;
                    numArray4[index1 * 16 + 9 + 4 * (index2 - 1)] = num19;
                    numArray4[index1 * 16 + 10 + 4 * (index2 - 1)] = num16;
                    numArray4[index1 * 16 + 11 + 4 * (index2 - 1)] = num17;
                    num16 = num18;
                    num17 = num19;
                }
                double num20 = num12;
                double num21 = num13;
                for (int index2 = 1; index2 < 4; ++index2)
                {
                    double num18 = num12 * Math.Cos((Math.PI / 3.0 - num1) * (double)index2) - num13 * Math.Sin((Math.PI / 3.0 - num1) * (double)index2) + numArray1[index1 / 2] - numArray1[index1 / 2] * Math.Cos((Math.PI / 3.0 - num1) * (double)index2) + numArray2[index1 / 2] * Math.Sin((Math.PI / 3.0 - num1) * (double)index2);
                    double num19 = num12 * Math.Sin((Math.PI / 3.0 - num1) * (double)index2) + num13 * Math.Cos((Math.PI / 3.0 - num1) * (double)index2) + numArray2[index1 / 2] - numArray1[index1 / 2] * Math.Sin((Math.PI / 3.0 - num1) * (double)index2) - numArray2[index1 / 2] * Math.Cos((Math.PI / 3.0 - num1) * (double)index2);
                    numArray4[index1 * 16 + 20 + 4 * (index2 - 1)] = num20;
                    numArray4[index1 * 16 + 21 + 4 * (index2 - 1)] = num21;
                    numArray4[index1 * 16 + 22 + 4 * (index2 - 1)] = num18;
                    numArray4[index1 * 16 + 23 + 4 * (index2 - 1)] = num19;
                    num20 = num18;
                    num21 = num19;
                }
                if (index1 == 0)
                {
                    this.LineLineIntersection(x1, y1, x2, y2, x3, y3, x4, y4, ref p);
                    if (p[0] == 1.0)
                    {
                        convexPoly[0] = p[1];
                        convexPoly[1] = p[2];
                        convexPoly[2] = numArray4[index1 * 16 + 16];
                        convexPoly[3] = numArray4[index1 * 16 + 17];
                        convexPoly[4] = numArray4[index1 * 16 + 12];
                        convexPoly[5] = numArray4[index1 * 16 + 13];
                        convexPoly[6] = numArray4[index1 * 16 + 8];
                        convexPoly[7] = numArray4[index1 * 16 + 9];
                        convexPoly[8] = num12;
                        convexPoly[9] = num13;
                        convexPoly[10] = numArray4[index1 * 16 + 22];
                        convexPoly[11] = numArray4[index1 * 16 + 23];
                        convexPoly[12] = numArray4[index1 * 16 + 26];
                        convexPoly[13] = numArray4[index1 * 16 + 27];
                        convexPoly[14] = numArray4[index1 * 16 + 30];
                        convexPoly[15] = numArray4[index1 * 16 + 31];
                    }
                }
                x1 = x3;
                y1 = y3;
                x3 = point1;
                y3 = point2;
            }
            if (numpoints != 0)
            {
                int num4 = (numpoints - 1) / 2 + 1;
                int num5 = 0;
                int num6 = 0;
                int num7 = 1;
                int numvertices = 8;
                for (int index = 0; index < 32; index += 4)
                {
                    numpoints1 = this.HalfPlaneIntersection(numvertices, ref convexPoly, numArray4[32 * num4 + index], numArray4[32 * num4 + 1 + index], numArray4[32 * num4 + 2 + index], numArray4[32 * num4 + 3 + index]);
                    if (numpoints1 == 0)
                        return false;
                    numvertices = numpoints1;
                }
                for (int index1 = num6 + 1; index1 < numpoints - 1; ++index1)
                {
                    for (int index2 = 0; index2 < 32; index2 += 4)
                    {
                        numpoints1 = this.HalfPlaneIntersection(numvertices, ref convexPoly, numArray4[32 * (num7 + num4 * num5) + index2], numArray4[32 * (num7 + num4 * num5) + 1 + index2], numArray4[32 * (num7 + num4 * num5) + 2 + index2], numArray4[32 * (num7 + num4 * num5) + 3 + index2]);
                        if (numpoints1 == 0)
                            return false;
                        numvertices = numpoints1;
                    }
                    num7 += num5;
                    num5 = (num5 + 1) % 2;
                }
                this.FindPolyCentroid(numpoints1, convexPoly, ref newloc);
                if (!this.behavior.fixedArea)
                    return true;
            }
            return false;
        }

        private bool GetWedgeIntersection(int numpoints, double[] points, ref double[] newloc)
        {
            double[] numArray1 = new double[2 * numpoints];
            double[] numArray2 = new double[2 * numpoints];
            double[] numArray3 = new double[2 * numpoints];
            double[] numArray4 = new double[2000];
            double[] p1 = new double[3];
            double[] p2 = new double[3];
            double[] p3 = new double[3];
            double[] p4 = new double[3];
            double[] convexPoly = new double[500];
            int numpoints1 = 0;
            int num1 = 0;
            double x1 = points[2 * numpoints - 4];
            double y1 = points[2 * numpoints - 3];
            double x3 = points[2 * numpoints - 2];
            double y3 = points[2 * numpoints - 1];
            double num2 = this.behavior.MinAngle * Math.PI / 180.0;
            double num3 = Math.Sin(num2);
            double num4 = Math.Cos(num2);
            double num5 = this.behavior.MaxAngle * Math.PI / 180.0;
            double num6 = Math.Sin(num5);
            double num7 = Math.Cos(num5);
            double num8;
            double num9;
            if (this.behavior.goodAngle == 1.0)
            {
                num8 = 0.0;
                num9 = 0.0;
            }
            else
            {
                num8 = 0.5 / Math.Tan(num2);
                num9 = 0.5 / Math.Sin(num2);
            }
            for (int index1 = 0; index1 < numpoints * 2; index1 += 2)
            {
                double point1 = points[index1];
                double point2 = points[index1 + 1];
                double num10 = x3 - x1;
                double num11 = y3 - y1;
                double num12 = Math.Sqrt(num10 * num10 + num11 * num11);
                numArray1[index1 / 2] = x1 + 0.5 * num10 - num8 * num11;
                numArray2[index1 / 2] = y1 + 0.5 * num11 + num8 * num10;
                numArray3[index1 / 2] = num9 * num12;
                numArray1[numpoints + index1 / 2] = numArray1[index1 / 2];
                numArray2[numpoints + index1 / 2] = numArray2[index1 / 2];
                numArray3[numpoints + index1 / 2] = numArray3[index1 / 2];
                double num13 = (x1 + x3) / 2.0;
                double num14 = (y1 + y3) / 2.0;
                double num15 = Math.Sqrt((numArray1[index1 / 2] - num13) * (numArray1[index1 / 2] - num13) + (numArray2[index1 / 2] - num14) * (numArray2[index1 / 2] - num14));
                double num16 = (numArray1[index1 / 2] - num13) / num15;
                double num17 = (numArray2[index1 / 2] - num14) / num15;
                double num18 = numArray1[index1 / 2] + num16 * numArray3[index1 / 2];
                double num19 = numArray2[index1 / 2] + num17 * numArray3[index1 / 2];
                double num20 = x3 - x1;
                double num21 = y3 - y1;
                double x2_1 = x3 * num4 - y3 * num3 + x1 - x1 * num4 + y1 * num3;
                double y2_1 = x3 * num3 + y3 * num4 + y1 - x1 * num3 - y1 * num4;
                numArray4[index1 * 20] = x1;
                numArray4[index1 * 20 + 1] = y1;
                numArray4[index1 * 20 + 2] = x2_1;
                numArray4[index1 * 20 + 3] = y2_1;
                num20 = x1 - x3;
                num21 = y1 - y3;
                double x4_1 = x1 * num4 + y1 * num3 + x3 - x3 * num4 - y3 * num3;
                double y4_1 = -x1 * num3 + y1 * num4 + y3 + x3 * num3 - y3 * num4;
                numArray4[index1 * 20 + 4] = x4_1;
                numArray4[index1 * 20 + 5] = y4_1;
                numArray4[index1 * 20 + 6] = x3;
                numArray4[index1 * 20 + 7] = y3;
                num20 = num18 - numArray1[index1 / 2];
                num21 = num19 - numArray2[index1 / 2];
                double num22 = num18;
                double num23 = num19;
                double num24 = 2.0 * this.behavior.MaxAngle + this.behavior.MinAngle - 180.0;
                double num25;
                double num26;
                if (num24 <= 0.0)
                {
                    num1 = 4;
                    num25 = 1.0;
                    num26 = 1.0;
                }
                else if (num24 <= 5.0)
                {
                    num1 = 6;
                    num25 = 2.0;
                    num26 = 2.0;
                }
                else if (num24 <= 10.0)
                {
                    num1 = 8;
                    num25 = 3.0;
                    num26 = 3.0;
                }
                else
                {
                    num1 = 10;
                    num25 = 4.0;
                    num26 = 4.0;
                }
                double num27 = num24 * Math.PI / 180.0;
                for (int index2 = 1; (double)index2 < num25; ++index2)
                {
                    if (num25 != 1.0)
                    {
                        double num28 = num18 * Math.Cos(num27 / (num25 - 1.0) * (double)index2) + num19 * Math.Sin(num27 / (num25 - 1.0) * (double)index2) + numArray1[index1 / 2] - numArray1[index1 / 2] * Math.Cos(num27 / (num25 - 1.0) * (double)index2) - numArray2[index1 / 2] * Math.Sin(num27 / (num25 - 1.0) * (double)index2);
                        double num29 = -num18 * Math.Sin(num27 / (num25 - 1.0) * (double)index2) + num19 * Math.Cos(num27 / (num25 - 1.0) * (double)index2) + numArray2[index1 / 2] + numArray1[index1 / 2] * Math.Sin(num27 / (num25 - 1.0) * (double)index2) - numArray2[index1 / 2] * Math.Cos(num27 / (num25 - 1.0) * (double)index2);
                        numArray4[index1 * 20 + 8 + 4 * (index2 - 1)] = num28;
                        numArray4[index1 * 20 + 9 + 4 * (index2 - 1)] = num29;
                        numArray4[index1 * 20 + 10 + 4 * (index2 - 1)] = num22;
                        numArray4[index1 * 20 + 11 + 4 * (index2 - 1)] = num23;
                        num22 = num28;
                        num23 = num29;
                    }
                }
                num20 = x1 - x3;
                num21 = y1 - y3;
                double x4_2 = x1 * num7 + y1 * num6 + x3 - x3 * num7 - y3 * num6;
                double y4_2 = -x1 * num6 + y1 * num7 + y3 + x3 * num6 - y3 * num7;
                numArray4[index1 * 20 + 20] = x3;
                numArray4[index1 * 20 + 21] = y3;
                numArray4[index1 * 20 + 22] = x4_2;
                numArray4[index1 * 20 + 23] = y4_2;
                double num30 = num18;
                double num31 = num19;
                for (int index2 = 1; (double)index2 < num26; ++index2)
                {
                    if (num26 != 1.0)
                    {
                        double num28 = num18 * Math.Cos(num27 / (num26 - 1.0) * (double)index2) - num19 * Math.Sin(num27 / (num26 - 1.0) * (double)index2) + numArray1[index1 / 2] - numArray1[index1 / 2] * Math.Cos(num27 / (num26 - 1.0) * (double)index2) + numArray2[index1 / 2] * Math.Sin(num27 / (num26 - 1.0) * (double)index2);
                        double num29 = num18 * Math.Sin(num27 / (num26 - 1.0) * (double)index2) + num19 * Math.Cos(num27 / (num26 - 1.0) * (double)index2) + numArray2[index1 / 2] - numArray1[index1 / 2] * Math.Sin(num27 / (num26 - 1.0) * (double)index2) - numArray2[index1 / 2] * Math.Cos(num27 / (num26 - 1.0) * (double)index2);
                        numArray4[index1 * 20 + 24 + 4 * (index2 - 1)] = num30;
                        numArray4[index1 * 20 + 25 + 4 * (index2 - 1)] = num31;
                        numArray4[index1 * 20 + 26 + 4 * (index2 - 1)] = num28;
                        numArray4[index1 * 20 + 27 + 4 * (index2 - 1)] = num29;
                        num30 = num28;
                        num31 = num29;
                    }
                }
                num20 = x3 - x1;
                num21 = y3 - y1;
                double x2_2 = x3 * num7 - y3 * num6 + x1 - x1 * num7 + y1 * num6;
                double y2_2 = x3 * num6 + y3 * num7 + y1 - x1 * num6 - y1 * num7;
                numArray4[index1 * 20 + 36] = x2_2;
                numArray4[index1 * 20 + 37] = y2_2;
                numArray4[index1 * 20 + 38] = x1;
                numArray4[index1 * 20 + 39] = y1;
                if (index1 == 0)
                {
                    switch (num1)
                    {
                        case 4:
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_1, y4_1, ref p1);
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_2, y4_2, ref p2);
                            this.LineLineIntersection(x1, y1, x2_2, y2_2, x3, y3, x4_2, y4_2, ref p3);
                            this.LineLineIntersection(x1, y1, x2_2, y2_2, x3, y3, x4_1, y4_1, ref p4);
                            if (p1[0] == 1.0 && p2[0] == 1.0 && (p3[0] == 1.0 && p4[0] == 1.0))
                            {
                                convexPoly[0] = p1[1];
                                convexPoly[1] = p1[2];
                                convexPoly[2] = p2[1];
                                convexPoly[3] = p2[2];
                                convexPoly[4] = p3[1];
                                convexPoly[5] = p3[2];
                                convexPoly[6] = p4[1];
                                convexPoly[7] = p4[2];
                                break;
                            }
                            break;

                        case 6:
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_1, y4_1, ref p1);
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_2, y4_2, ref p2);
                            this.LineLineIntersection(x1, y1, x2_2, y2_2, x3, y3, x4_1, y4_1, ref p3);
                            if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0)
                            {
                                convexPoly[0] = p1[1];
                                convexPoly[1] = p1[2];
                                convexPoly[2] = p2[1];
                                convexPoly[3] = p2[2];
                                convexPoly[4] = numArray4[index1 * 20 + 8];
                                convexPoly[5] = numArray4[index1 * 20 + 9];
                                convexPoly[6] = num18;
                                convexPoly[7] = num19;
                                convexPoly[8] = numArray4[index1 * 20 + 26];
                                convexPoly[9] = numArray4[index1 * 20 + 27];
                                convexPoly[10] = p3[1];
                                convexPoly[11] = p3[2];
                                break;
                            }
                            break;

                        case 8:
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_1, y4_1, ref p1);
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_2, y4_2, ref p2);
                            this.LineLineIntersection(x1, y1, x2_2, y2_2, x3, y3, x4_1, y4_1, ref p3);
                            if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0)
                            {
                                convexPoly[0] = p1[1];
                                convexPoly[1] = p1[2];
                                convexPoly[2] = p2[1];
                                convexPoly[3] = p2[2];
                                convexPoly[4] = numArray4[index1 * 20 + 12];
                                convexPoly[5] = numArray4[index1 * 20 + 13];
                                convexPoly[6] = numArray4[index1 * 20 + 8];
                                convexPoly[7] = numArray4[index1 * 20 + 9];
                                convexPoly[8] = num18;
                                convexPoly[9] = num19;
                                convexPoly[10] = numArray4[index1 * 20 + 26];
                                convexPoly[11] = numArray4[index1 * 20 + 27];
                                convexPoly[12] = numArray4[index1 * 20 + 30];
                                convexPoly[13] = numArray4[index1 * 20 + 31];
                                convexPoly[14] = p3[1];
                                convexPoly[15] = p3[2];
                                break;
                            }
                            break;

                        case 10:
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_1, y4_1, ref p1);
                            this.LineLineIntersection(x1, y1, x2_1, y2_1, x3, y3, x4_2, y4_2, ref p2);
                            this.LineLineIntersection(x1, y1, x2_2, y2_2, x3, y3, x4_1, y4_1, ref p3);
                            if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0)
                            {
                                convexPoly[0] = p1[1];
                                convexPoly[1] = p1[2];
                                convexPoly[2] = p2[1];
                                convexPoly[3] = p2[2];
                                convexPoly[4] = numArray4[index1 * 20 + 16];
                                convexPoly[5] = numArray4[index1 * 20 + 17];
                                convexPoly[6] = numArray4[index1 * 20 + 12];
                                convexPoly[7] = numArray4[index1 * 20 + 13];
                                convexPoly[8] = numArray4[index1 * 20 + 8];
                                convexPoly[9] = numArray4[index1 * 20 + 9];
                                convexPoly[10] = num18;
                                convexPoly[11] = num19;
                                convexPoly[12] = numArray4[index1 * 20 + 28];
                                convexPoly[13] = numArray4[index1 * 20 + 29];
                                convexPoly[14] = numArray4[index1 * 20 + 32];
                                convexPoly[15] = numArray4[index1 * 20 + 33];
                                convexPoly[16] = numArray4[index1 * 20 + 34];
                                convexPoly[17] = numArray4[index1 * 20 + 35];
                                convexPoly[18] = p3[1];
                                convexPoly[19] = p3[2];
                                break;
                            }
                            break;
                    }
                }
                x1 = x3;
                y1 = y3;
                x3 = point1;
                y3 = point2;
            }
            if (numpoints != 0)
            {
                int num10 = (numpoints - 1) / 2 + 1;
                int num11 = 0;
                int num12 = 0;
                int num13 = 1;
                int numvertices = num1;
                for (int index = 0; index < 40; index += 4)
                {
                    if ((num1 != 4 || index != 8 && index != 12 && (index != 16 && index != 24) && (index != 28 && index != 32)) && ((num1 != 6 || index != 12 && index != 16 && (index != 28 && index != 32)) && (num1 != 8 || index != 16 && index != 32)))
                    {
                        numpoints1 = this.HalfPlaneIntersection(numvertices, ref convexPoly, numArray4[40 * num10 + index], numArray4[40 * num10 + 1 + index], numArray4[40 * num10 + 2 + index], numArray4[40 * num10 + 3 + index]);
                        if (numpoints1 == 0)
                            return false;
                        numvertices = numpoints1;
                    }
                }
                for (int index1 = num12 + 1; index1 < numpoints - 1; ++index1)
                {
                    for (int index2 = 0; index2 < 40; index2 += 4)
                    {
                        if ((num1 != 4 || index2 != 8 && index2 != 12 && (index2 != 16 && index2 != 24) && (index2 != 28 && index2 != 32)) && ((num1 != 6 || index2 != 12 && index2 != 16 && (index2 != 28 && index2 != 32)) && (num1 != 8 || index2 != 16 && index2 != 32)))
                        {
                            numpoints1 = this.HalfPlaneIntersection(numvertices, ref convexPoly, numArray4[40 * (num13 + num10 * num11) + index2], numArray4[40 * (num13 + num10 * num11) + 1 + index2], numArray4[40 * (num13 + num10 * num11) + 2 + index2], numArray4[40 * (num13 + num10 * num11) + 3 + index2]);
                            if (numpoints1 == 0)
                                return false;
                            numvertices = numpoints1;
                        }
                    }
                    num13 += num11;
                    num11 = (num11 + 1) % 2;
                }
                this.FindPolyCentroid(numpoints1, convexPoly, ref newloc);
                if (this.behavior.MaxAngle == 0.0)
                    return true;
                int num14 = 0;
                for (int index = 0; index < numpoints * 2 - 2; index += 2)
                {
                    if (this.IsBadTriangleAngle(newloc[0], newloc[1], points[index], points[index + 1], points[index + 2], points[index + 3]))
                        ++num14;
                }
                if (this.IsBadTriangleAngle(newloc[0], newloc[1], points[0], points[1], points[numpoints * 2 - 2], points[numpoints * 2 - 1]))
                    ++num14;
                if (num14 == 0)
                    return true;
                int num15 = numpoints <= 2 ? 20 : 30;
                for (int index1 = 0; index1 < 2 * numpoints; index1 += 2)
                {
                    for (int index2 = 1; index2 < num15; ++index2)
                    {
                        newloc[0] = 0.0;
                        newloc[1] = 0.0;
                        for (int index3 = 0; index3 < 2 * numpoints; index3 += 2)
                        {
                            double num16 = 1.0 / (double)numpoints;
                            if (index3 == index1)
                            {
                                newloc[0] = newloc[0] + 0.1 * (double)index2 * num16 * points[index3];
                                newloc[1] = newloc[1] + 0.1 * (double)index2 * num16 * points[index3 + 1];
                            }
                            else
                            {
                                double num17 = (1.0 - 0.1 * (double)index2 * num16) / ((double)numpoints - 1.0);
                                newloc[0] = newloc[0] + num17 * points[index3];
                                newloc[1] = newloc[1] + num17 * points[index3 + 1];
                            }
                        }
                        int num18 = 0;
                        for (int index3 = 0; index3 < numpoints * 2 - 2; index3 += 2)
                        {
                            if (this.IsBadTriangleAngle(newloc[0], newloc[1], points[index3], points[index3 + 1], points[index3 + 2], points[index3 + 3]))
                                ++num18;
                        }
                        if (this.IsBadTriangleAngle(newloc[0], newloc[1], points[0], points[1], points[numpoints * 2 - 2], points[numpoints * 2 - 1]))
                            ++num18;
                        if (num18 == 0)
                            return true;
                    }
                }
            }
            return false;
        }

        private bool ValidPolygonAngles(int numpoints, double[] points)
        {
            for (int index = 0; index < numpoints; ++index)
            {
                if (index == numpoints - 1)
                {
                    if (this.IsBadPolygonAngle(points[index * 2], points[index * 2 + 1], points[0], points[1], points[2], points[3]))
                        return false;
                }
                else if (index == numpoints - 2)
                {
                    if (this.IsBadPolygonAngle(points[index * 2], points[index * 2 + 1], points[(index + 1) * 2], points[(index + 1) * 2 + 1], points[0], points[1]))
                        return false;
                }
                else if (this.IsBadPolygonAngle(points[index * 2], points[index * 2 + 1], points[(index + 1) * 2], points[(index + 1) * 2 + 1], points[(index + 2) * 2], points[(index + 2) * 2 + 1]))
                    return false;
            }
            return true;
        }

        private bool IsBadPolygonAngle(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3)
        {
            double num1 = x1 - x2;
            double num2 = y1 - y2;
            double num3 = x2 - x3;
            double num4 = y2 - y3;
            double num5 = x3 - x1;
            double num6 = y3 - y1;
            double d1 = num1 * num1 + num2 * num2;
            double d2 = num3 * num3 + num4 * num4;
            double num7 = num5 * num5 + num6 * num6;
            return Math.Acos((d1 + d2 - num7) / (2.0 * Math.Sqrt(d1) * Math.Sqrt(d2))) < 2.0 * Math.Acos(Math.Sqrt(this.behavior.goodAngle));
        }

        private void LineLineIntersection(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3,
          double x4,
          double y4,
          ref double[] p)
        {
            double num1 = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
            double num2 = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
            double num3 = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
            if (Math.Abs(num1 - 0.0) < 1E-50 && Math.Abs(num3 - 0.0) < 1E-50 && Math.Abs(num2 - 0.0) < 1E-50)
                p[0] = 0.0;
            else if (Math.Abs(num1 - 0.0) < 1E-50)
            {
                p[0] = 0.0;
            }
            else
            {
                p[0] = 1.0;
                double num4 = num2 / num1;
                double num5 = num3 / num1;
                p[1] = x1 + num4 * (x2 - x1);
                p[2] = y1 + num4 * (y2 - y1);
            }
        }

        private int HalfPlaneIntersection(
          int numvertices,
          ref double[] convexPoly,
          double x1,
          double y1,
          double x2,
          double y2)
        {
            double[][] polys = new double[3][]
            {
        new double[2],
        new double[2],
        new double[2]
            };
            double[] numArray = (double[])null;
            int num1 = 0;
            int num2 = 0;
            double num3 = x2 - x1;
            double num4 = y2 - y1;
            int num5 = this.SplitConvexPolygon(numvertices, convexPoly, x1, y1, x2, y2, ref polys);
            if (num5 == 3)
            {
                num1 = numvertices;
            }
            else
            {
                for (int index1 = 0; index1 < num5; ++index1)
                {
                    double num6 = 1E+17;
                    double num7 = -1E+17;
                    for (int index2 = 1; (double)index2 <= 2.0 * polys[index1][0] - 1.0; index2 += 2)
                    {
                        double num8 = num3 * (polys[index1][index2 + 1] - y1) - num4 * (polys[index1][index2] - x1);
                        num6 = num8 < num6 ? num8 : num6;
                        num7 = num8 > num7 ? num8 : num7;
                    }
                    if ((Math.Abs(num6) > Math.Abs(num7) ? num6 : num7) > 0.0)
                    {
                        numArray = polys[index1];
                        num2 = 1;
                        break;
                    }
                }
                if (num2 == 1)
                {
                    for (; (double)num1 < numArray[0]; ++num1)
                    {
                        convexPoly[2 * num1] = numArray[2 * num1 + 1];
                        convexPoly[2 * num1 + 1] = numArray[2 * num1 + 2];
                    }
                }
            }
            return num1;
        }

        private int SplitConvexPolygon(
          int numvertices,
          double[] convexPoly,
          double x1,
          double y1,
          double x2,
          double y2,
          ref double[][] polys)
        {
            int num1 = 0;
            double[] p = new double[3];
            double[] numArray1 = new double[100];
            int num2 = 0;
            double[] numArray2 = new double[100];
            int num3 = 0;
            double num4 = 1E-12;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            int num12 = 0;
            for (int index1 = 0; index1 < 2 * numvertices; index1 += 2)
            {
                int index2 = index1 + 2 >= 2 * numvertices ? 0 : index1 + 2;
                this.LineLineSegmentIntersection(x1, y1, x2, y2, convexPoly[index1], convexPoly[index1 + 1], convexPoly[index2], convexPoly[index2 + 1], ref p);
                if (Math.Abs(p[0] - 0.0) <= num4)
                {
                    if (num1 == 1)
                    {
                        ++num3;
                        numArray2[2 * num3 - 1] = convexPoly[index2];
                        numArray2[2 * num3] = convexPoly[index2 + 1];
                    }
                    else
                    {
                        ++num2;
                        numArray1[2 * num2 - 1] = convexPoly[index2];
                        numArray1[2 * num2] = convexPoly[index2 + 1];
                    }
                    ++num5;
                }
                else if (Math.Abs(p[0] - 2.0) <= num4)
                {
                    ++num2;
                    numArray1[2 * num2 - 1] = convexPoly[index2];
                    numArray1[2 * num2] = convexPoly[index2 + 1];
                    ++num6;
                }
                else
                {
                    ++num7;
                    if (Math.Abs(p[1] - convexPoly[index2]) <= num4 && Math.Abs(p[2] - convexPoly[index2 + 1]) <= num4)
                    {
                        ++num8;
                        switch (num1)
                        {
                            case 0:
                                ++num11;
                                ++num2;
                                numArray1[2 * num2 - 1] = convexPoly[index2];
                                numArray1[2 * num2] = convexPoly[index2 + 1];
                                if (index1 + 4 < 2 * numvertices)
                                {
                                    int num13 = this.LinePointLocation(x1, y1, x2, y2, convexPoly[index1], convexPoly[index1 + 1]);
                                    int num14 = this.LinePointLocation(x1, y1, x2, y2, convexPoly[index1 + 4], convexPoly[index1 + 5]);
                                    if (num13 != num14 && num13 != 0 && num14 != 0)
                                    {
                                        ++num12;
                                        ++num3;
                                        numArray2[2 * num3 - 1] = convexPoly[index2];
                                        numArray2[2 * num3] = convexPoly[index2 + 1];
                                        ++num1;
                                        continue;
                                    }
                                    continue;
                                }
                                continue;
                            case 1:
                                ++num3;
                                numArray2[2 * num3 - 1] = convexPoly[index2];
                                numArray2[2 * num3] = convexPoly[index2 + 1];
                                ++num2;
                                numArray1[2 * num2 - 1] = convexPoly[index2];
                                numArray1[2 * num2] = convexPoly[index2 + 1];
                                ++num1;
                                continue;
                            default:
                                continue;
                        }
                    }
                    else if (Math.Abs(p[1] - convexPoly[index1]) > num4 || Math.Abs(p[2] - convexPoly[index1 + 1]) > num4)
                    {
                        ++num9;
                        ++num2;
                        numArray1[2 * num2 - 1] = p[1];
                        numArray1[2 * num2] = p[2];
                        ++num3;
                        numArray2[2 * num3 - 1] = p[1];
                        numArray2[2 * num3] = p[2];
                        switch (num1)
                        {
                            case 0:
                                ++num3;
                                numArray2[2 * num3 - 1] = convexPoly[index2];
                                numArray2[2 * num3] = convexPoly[index2 + 1];
                                break;

                            case 1:
                                ++num2;
                                numArray1[2 * num2 - 1] = convexPoly[index2];
                                numArray1[2 * num2] = convexPoly[index2 + 1];
                                break;
                        }
                        ++num1;
                    }
                    else
                    {
                        ++num10;
                        if (num1 == 1)
                        {
                            ++num3;
                            numArray2[2 * num3 - 1] = convexPoly[index2];
                            numArray2[2 * num3] = convexPoly[index2 + 1];
                        }
                        else
                        {
                            ++num2;
                            numArray1[2 * num2 - 1] = convexPoly[index2];
                            numArray1[2 * num2] = convexPoly[index2 + 1];
                        }
                    }
                }
            }
            int num15;
            if (num1 != 0 && num1 != 2)
            {
                num15 = 3;
            }
            else
            {
                num15 = num1 == 0 ? 1 : 2;
                numArray1[0] = (double)num2;
                numArray2[0] = (double)num3;
                polys[0] = numArray1;
                if (num1 == 2)
                    polys[1] = numArray2;
            }
            return num15;
        }

        private int LinePointLocation(
          double x1,
          double y1,
          double x2,
          double y2,
          double x,
          double y)
        {
            if (Math.Atan((y2 - y1) / (x2 - x1)) * 180.0 / Math.PI == 90.0)
            {
                if (Math.Abs(x1 - x) <= 1E-11)
                    return 0;
            }
            else if (Math.Abs(y1 + (y2 - y1) * (x - x1) / (x2 - x1) - y) <= 1E-50)
                return 0;
            double num = (x2 - x1) * (y - y1) - (y2 - y1) * (x - x1);
            if (Math.Abs(num - 0.0) <= 1E-11)
                return 0;
            return num > 0.0 ? 1 : 2;
        }

        private void LineLineSegmentIntersection(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3,
          double x4,
          double y4,
          ref double[] p)
        {
            double num1 = 1E-13;
            double num2 = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
            double num3 = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
            double num4 = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
            if (Math.Abs(num2 - 0.0) < num1)
            {
                if (Math.Abs(num4 - 0.0) < num1 && Math.Abs(num3 - 0.0) < num1)
                    p[0] = 2.0;
                else
                    p[0] = 0.0;
            }
            else
            {
                double num5 = num4 / num2;
                double num6 = num3 / num2;
                if (num5 < -num1 || num5 > 1.0 + num1)
                {
                    p[0] = 0.0;
                }
                else
                {
                    p[0] = 1.0;
                    p[1] = x1 + num6 * (x2 - x1);
                    p[2] = y1 + num6 * (y2 - y1);
                }
            }
        }

        private void FindPolyCentroid(int numpoints, double[] points, ref double[] centroid)
        {
            centroid[0] = 0.0;
            centroid[1] = 0.0;
            for (int index = 0; index < 2 * numpoints; index += 2)
            {
                centroid[0] = centroid[0] + points[index];
                centroid[1] = centroid[1] + points[index + 1];
            }
            centroid[0] = centroid[0] / (double)numpoints;
            centroid[1] = centroid[1] / (double)numpoints;
        }

        private void CircleLineIntersection(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3,
          double r,
          ref double[] p)
        {
            double num1 = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            double num2 = 2.0 * ((x2 - x1) * (x1 - x3) + (y2 - y1) * (y1 - y3));
            double num3 = x3 * x3 + y3 * y3 + x1 * x1 + y1 * y1 - 2.0 * (x3 * x1 + y3 * y1) - r * r;
            double d = num2 * num2 - 4.0 * num1 * num3;
            if (d < 0.0)
                p[0] = 0.0;
            else if (Math.Abs(d - 0.0) < 1E-50)
            {
                p[0] = 1.0;
                double num4 = -num2 / (2.0 * num1);
                p[1] = x1 + num4 * (x2 - x1);
                p[2] = y1 + num4 * (y2 - y1);
            }
            else if (d > 0.0 && Math.Abs(num1 - 0.0) >= 1E-50)
            {
                p[0] = 2.0;
                double num4 = (-num2 + Math.Sqrt(d)) / (2.0 * num1);
                p[1] = x1 + num4 * (x2 - x1);
                p[2] = y1 + num4 * (y2 - y1);
                double num5 = (-num2 - Math.Sqrt(d)) / (2.0 * num1);
                p[3] = x1 + num5 * (x2 - x1);
                p[4] = y1 + num5 * (y2 - y1);
            }
            else
                p[0] = 0.0;
        }

        private bool ChooseCorrectPoint(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3,
          bool isObtuse)
        {
            double num1 = (x2 - x3) * (x2 - x3) + (y2 - y3) * (y2 - y3);
            double num2 = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            return !isObtuse ? num2 < num1 : num2 >= num1;
        }

        private void PointBetweenPoints(
          double x1,
          double y1,
          double x2,
          double y2,
          double x,
          double y,
          ref double[] p)
        {
            if ((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y) < (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))
            {
                p[0] = 1.0;
                p[1] = (x - x2) * (x - x2) + (y - y2) * (y - y2);
                p[2] = x;
                p[3] = y;
            }
            else
            {
                p[0] = 0.0;
                p[1] = 0.0;
                p[2] = 0.0;
                p[3] = 0.0;
            }
        }

        private bool IsBadTriangleAngle(
          double x1,
          double y1,
          double x2,
          double y2,
          double x3,
          double y3)
        {
            double num1 = x1 - x2;
            double num2 = y1 - y2;
            double num3 = x2 - x3;
            double num4 = y2 - y3;
            double num5 = x3 - x1;
            double num6 = y3 - y1;
            double num7 = num1 * num1;
            double num8 = num2 * num2;
            double num9 = num3 * num3;
            double num10 = num4 * num4;
            double num11 = num5 * num5;
            double num12 = num6 * num6;
            double num13 = num7 + num8;
            double num14 = num9 + num10;
            double num15 = num11 + num12;
            double num16;
            if (num13 < num14 && num13 < num15)
            {
                double num17 = num3 * num5 + num4 * num6;
                num16 = num17 * num17 / (num14 * num15);
            }
            else if (num14 < num15)
            {
                double num17 = num1 * num5 + num2 * num6;
                num16 = num17 * num17 / (num13 * num15);
            }
            else
            {
                double num17 = num1 * num3 + num2 * num4;
                num16 = num17 * num17 / (num13 * num14);
            }
            double num18 = num13 <= num14 || num13 <= num15 ? (num14 <= num15 ? (num13 + num14 - num15) / (2.0 * Math.Sqrt(num13 * num14)) : (num13 + num15 - num14) / (2.0 * Math.Sqrt(num13 * num15))) : (num14 + num15 - num13) / (2.0 * Math.Sqrt(num14 * num15));
            return num16 > this.behavior.goodAngle || this.behavior.MaxAngle != 0.0 && num18 < this.behavior.maxGoodAngle;
        }

        private double MinDistanceToNeighbor(double newlocX, double newlocY, ref Otri searchtri)
        {
            Otri otri = new Otri();
            LocateResult locateResult = LocateResult.Outside;
            Point point = new Point(newlocX, newlocY);
            Vertex vertex1 = searchtri.Org();
            Vertex vertex2 = searchtri.Dest();
            if (vertex1.x == point.x && vertex1.y == point.y)
            {
                locateResult = LocateResult.OnVertex;
                searchtri.Copy(ref otri);
            }
            else if (vertex2.x == point.x && vertex2.y == point.y)
            {
                searchtri.LnextSelf();
                locateResult = LocateResult.OnVertex;
                searchtri.Copy(ref otri);
            }
            else
            {
                double num = Primitives.CounterClockwise((Point)vertex1, (Point)vertex2, point);
                if (num < 0.0)
                {
                    searchtri.SymSelf();
                    searchtri.Copy(ref otri);
                    locateResult = this.mesh.locator.PreciseLocate(point, ref otri, false);
                }
                else if (num == 0.0)
                {
                    if (vertex1.x < point.x == point.x < vertex2.x && vertex1.y < point.y == point.y < vertex2.y)
                    {
                        locateResult = LocateResult.OnEdge;
                        searchtri.Copy(ref otri);
                    }
                }
                else
                {
                    searchtri.Copy(ref otri);
                    locateResult = this.mesh.locator.PreciseLocate(point, ref otri, false);
                }
            }
            if (locateResult == LocateResult.OnVertex || locateResult == LocateResult.Outside)
                return 0.0;
            Vertex vertex3 = otri.Org();
            Vertex vertex4 = otri.Dest();
            Vertex vertex5 = otri.Apex();
            double num1 = (vertex3.x - point.x) * (vertex3.x - point.x) + (vertex3.y - point.y) * (vertex3.y - point.y);
            double num2 = (vertex4.x - point.x) * (vertex4.x - point.x) + (vertex4.y - point.y) * (vertex4.y - point.y);
            double num3 = (vertex5.x - point.x) * (vertex5.x - point.x) + (vertex5.y - point.y) * (vertex5.y - point.y);
            if (num1 <= num2 && num1 <= num3)
                return num1;
            if (num2 <= num3)
                return num2;
            return num3;
        }
    }
}