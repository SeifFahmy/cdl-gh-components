using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;

namespace LDNSL.Dev.Grasshopper.Library.MSG
{
    public static class Holoplot
    {
        public static Rectangle3d createRectangle(Plane plane, double width, double height)
        {
            Interval w = new Rhino.Geometry.Interval(-(width / 2), (width / 2));
            Interval h = new Rhino.Geometry.Interval(-(height / 2), (height / 2));

            Rectangle3d rec = new Rectangle3d(plane, w, h);

            return rec;
        } 

        public static List<Line> createPerimeterLines(Rectangle3d rectangle, Plane plane, double depth, double xChordExt, double yChordExt)
        {
          List<Line> perimeterLines = new List<Line>();

          Vector3d vectorZ = plane.ZAxis;
          vectorZ.Unitize();
          Vector3d zVec = vectorZ * depth;

          Polyline pPolyline = rectangle.ToPolyline();
          Line[] pLines = pPolyline.GetSegments();

          foreach (Line ln in pLines)
          {
            int index = Array.IndexOf(pLines, ln);

            if (index % 2 == 0)
            {

              ln.Extend(xChordExt, xChordExt);
              perimeterLines.Add(ln);
              ln.Transform(Transform.Translation(zVec));
              perimeterLines.Add(ln);
            }
            else if (index % 2 == 1)
            {
              ln.Extend(yChordExt, yChordExt);
              perimeterLines.Add(ln);
              ln.Transform(Transform.Translation(zVec));
              perimeterLines.Add(ln);
            }
          }
          return perimeterLines;
        }

  public static List<Line> createCornerLines(Rectangle3d rectangle,
    Plane plane,
    double depth,
    double cornerExt,
    List<double> xOffsets,
    List<double> yOffsets)
  {
    List<Line> cornerLines = new List<Line>();

    Vector3d vectorX = plane.XAxis;
    vectorX.Unitize();
    Vector3d vectorY = plane.YAxis;
    vectorY.Unitize();
    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();

    Vector3d zVec = vectorZ * depth;

    for (int i = 0; i < 4; i++)
    {
      Point3d startPoint = rectangle.Corner(i);
      Point3d endPoint = rectangle.Corner(i);
      endPoint.Transform(Transform.Translation(zVec));
      Line ln = new Line(startPoint, endPoint);
      ln.Extend((cornerExt / 2), (cornerExt / 2));
      ln.Transform(Transform.Translation(vectorX * xOffsets[i]));
      ln.Transform(Transform.Translation(vectorY * yOffsets[i]));
      cornerLines.Add(ln);
    }
    return cornerLines;
  }

  public static List<Line> createVerticalBracingLines(Rectangle3d rectangle, Plane plane,
    List<double> xParams,
    List<double> yParams,
    double depth)
  {
    List<Line> vbLines = new List<Line>();

    Vector3d vectorX = plane.XAxis;
    vectorX.Unitize();
    Vector3d vectorY = plane.YAxis;
    vectorY.Unitize();
    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * depth;

    List<Vector3d> xVecs = new List<Vector3d>();
    foreach(double p in xParams)
    {
      xVecs.Add(new Vector3d(vectorX * p));
    }
    List<Vector3d> yVecs = new List<Vector3d>();
    foreach(double p in yParams)
    {
      yVecs.Add(new Vector3d(vectorY * p));
    }

    Polyline pPolyline = rectangle.ToPolyline();
    Line[] pLines = pPolyline.GetSegments();

    foreach (Line ln in pLines)
    {
      int index = Array.IndexOf(pLines, ln);

      Point3d startPt = ln.PointAt(0.5);
      Point3d endPt = ln.PointAt(0.5);
      endPt.Transform(Transform.Translation(zVec));

      if (index % 2 == 0)
      {
        foreach(Vector3d v in xVecs)
        {
          Line midLine = new Line(startPt, endPt);
          midLine.Transform(Transform.Translation(v));
          vbLines.Add(midLine);
        }
      }
      else if (index % 2 == 1)
      {
        foreach(Vector3d v in yVecs)
        {
          Line midLine = new Line(startPt, endPt);
          midLine.Transform(Transform.Translation(v));
          vbLines.Add(midLine);
        }
      }
    }

    return vbLines;
  }

  public static List<Line> createDiagonalBracingLines(Rectangle3d rectangle,
    Plane plane,
    List<double> xParams,
    List<double> yParams,
    List<double> xzBrParams,
    List<double> yzBrParams,
    double depth)
  {
    List<Line> dLines = new List<Line>();

    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * depth;

    Polyline pPolyline = rectangle.ToPolyline();
    Line[] pLines = pPolyline.GetSegments();

    foreach (Line ln in pLines)
    {
      int index = Array.IndexOf(pLines, ln);
      Vector3d lineVector = new Vector3d(ln.To - ln.From);
      lineVector.Unitize();

      if (index % 2 == 0)
      {
        for (int i = 0; i < xParams.Count; i += 2)
        {
          Point3d startPt = new Point3d(ln.PointAt(0.5));
          startPt.Transform(Transform.Translation(lineVector * xParams[i]));
          startPt.Transform(Transform.Translation(zVec * xzBrParams[i]));

          Point3d endPt = new Point3d(ln.PointAt(0.5));
          endPt.Transform(Transform.Translation(lineVector * xParams[i + 1]));
          endPt.Transform(Transform.Translation(zVec * xzBrParams[i + 1]));

          Line brace = new Line(startPt, endPt);
          dLines.Add(brace);

        }
      }
      else if (index % 2 == 1)
      {
        for (int i = 0; i < yParams.Count; i += 2)
        {
          Point3d startPt = new Point3d(ln.PointAt(0.5));
          startPt.Transform(Transform.Translation(lineVector * yParams[i]));
          startPt.Transform(Transform.Translation(zVec * yzBrParams[i]));

          Point3d endPt = new Point3d(ln.PointAt(0.5));
          endPt.Transform(Transform.Translation(lineVector * yParams[i + 1]));
          endPt.Transform(Transform.Translation(zVec * yzBrParams[i + 1]));

          Line brace = new Line(startPt, endPt);
          dLines.Add(brace);
        }
      }
    }

    return dLines;
  }

  public static List<Line> createXPlateLines(Rectangle3d rectangle,
    Plane plane,
    List<double> xParams,
    double depth)
  {
    List<Line> pltLines = new List<Line>();

    Vector3d vectorX = plane.XAxis;
    vectorX.Unitize();
    Vector3d xVec = vectorX * depth;

    List<Vector3d> xVecs = new List<Vector3d>();
    foreach(double p in xParams)
    {
      xVecs.Add(new Vector3d(vectorX * p));
    }

    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * depth;

    Polyline pPolyline = rectangle.ToPolyline();
    Line[] pLines = pPolyline.GetSegments();

    Point3d startPt = pLines[0].PointAt(0.5);
    Point3d endPt = pLines[2].PointAt(0.5);

    foreach (Vector3d v in xVecs)
    {
      Line midLine = new Line(startPt, endPt);
      midLine.Transform(Transform.Translation(v));
      midLine.Transform(Transform.Translation(zVec / 2));
      pltLines.Add(midLine);
    }

    return pltLines;
  }

  public static List<Line> createYPlateLines(Rectangle3d rectangle,
    Plane plane,
    List<double> yParams,
    double depth)
  {
    List<Line> pltLines = new List<Line>();

    Vector3d vectorY = plane.YAxis;
    vectorY.Unitize();
    Vector3d yVec = vectorY * depth;

    List<Vector3d> yVecs = new List<Vector3d>();
    foreach(double p in yParams)
    {
      yVecs.Add(new Vector3d(vectorY * p));
    }

    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * depth;

    Polyline pPolyline = rectangle.ToPolyline();
    Line[] pLines = pPolyline.GetSegments();

    Point3d startPt = pLines[1].PointAt(0.5);
    Point3d endPt = pLines[3].PointAt(0.5);

    foreach (Vector3d v in yVecs)
    {
      Line midLine = new Line(startPt, endPt);
      midLine.Transform(Transform.Translation(v));
      midLine.Transform(Transform.Translation(zVec / 5));
      pltLines.Add(midLine);
      midLine.Transform(Transform.Translation(zVec / 5));
      pltLines.Add(midLine);
      midLine.Transform(Transform.Translation(zVec / 5));
      pltLines.Add(midLine);
      midLine.Transform(Transform.Translation(zVec / 5));
      pltLines.Add(midLine);
    }

    return pltLines;
  }

  public static List<Line> createAddCornerLines(Rectangle3d rectangle, Plane plane, double depth, double cornerExt)
  {
    List<Line> cornerLines = new List<Line>();

    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * depth;

    for (int i = 0; i < 4; i++)
    {
      Point3d startPoint = rectangle.Corner(i);
      Point3d endPoint = rectangle.Corner(i);
      endPoint.Transform(Transform.Translation(zVec));
      Line ln = new Line(startPoint, endPoint);
      cornerLines.Add(ln);
    }
    return cornerLines;
  }

  public static List<Rectangle3d> createPerimeterBracketLines(Rectangle3d rectangle,
    Plane plane,
    List<double> pinSpacing,
    double xOffset,
    double zOffset)
  {
    List<Rectangle3d> rectangles = new List<Rectangle3d>();


    Vector3d vectorX = plane.XAxis;
    vectorX.Unitize();
    Vector3d vectorY = plane.YAxis;
    vectorY.Unitize();

    List<Vector3d> yVecs = new List<Vector3d>();
    foreach(double p in pinSpacing)
    {
      yVecs.Add(new Vector3d(vectorY * p));
    }

    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * -zOffset;

    Polyline pPolyline = rectangle.ToPolyline();
    Line[] pLines = pPolyline.GetSegments();

    Interval w = new Rhino.Geometry.Interval(-0.1, 0.1);
    Interval h = new Rhino.Geometry.Interval(-0.1, 0.1);

    foreach (Line ln in pLines)
    {
      int index = Array.IndexOf(pLines, ln);

      if (index == 1)
      {
        for (int i = 0; i < pinSpacing.Count; i++)
        {
          Point3d midPt = new Point3d(ln.PointAt(0.5));
          midPt.Transform(Transform.Translation(vectorX * -xOffset));
          midPt.Transform(Transform.Translation(vectorY * pinSpacing[i]));
          midPt.Transform(Transform.Translation(zVec));
          Plane newPlane = new Plane(plane);
          newPlane.Origin = midPt;
          Rectangle3d rec = new Rectangle3d(newPlane, w, h);
          rec.Transform(Transform.Rotation(3.141593, newPlane.ZAxis, newPlane.Origin));
          rectangles.Add(rec);
        }
      }
      else if (index == 3)
      {
        for (int i = 0; i < pinSpacing.Count; i++)
        {
          Point3d midPt = new Point3d(ln.PointAt(0.5));
          midPt.Transform(Transform.Translation(vectorX * xOffset));
          midPt.Transform(Transform.Translation(vectorY * pinSpacing[i]));
          midPt.Transform(Transform.Translation(zVec));
          Plane newPlane = new Plane(plane);
          newPlane.Origin = midPt;
          Rectangle3d rec = new Rectangle3d(newPlane, w, h);
          rectangles.Add(rec);
        }
      }
    }
    return rectangles;
  }

  public static List<Line> createInternalBracketLines(List<Line> plates,
    Plane plane,
    List<double> pinSpacing,
    double frameDepth)
  {
    List<Line> bracketLines = new List<Line>();


    Vector3d vectorX = plane.XAxis;
    vectorX.Unitize();
    Vector3d vectorY = plane.YAxis;
    vectorY.Unitize();

    List<Vector3d> yVecs = new List<Vector3d>();
    foreach(double p in pinSpacing)
    {
      yVecs.Add(new Vector3d(vectorY * p));
    }

    Vector3d vectorZ = plane.ZAxis;
    vectorZ.Unitize();
    Vector3d zVec = vectorZ * -(frameDepth / 2);
    Vector3d zVec2 = vectorZ * -((frameDepth / 2) + 0.5);

    foreach (Line ln in plates)
    {
      for (int i = 0; i < pinSpacing.Count; i++)
      {
        Point3d midPt = new Point3d(ln.PointAt(0.5));
        midPt.Transform(Transform.Translation(vectorY * pinSpacing[i]));
        midPt.Transform(Transform.Translation(zVec));

        Point3d midPtOffset = new Point3d(ln.PointAt(0.5));
        midPtOffset.Transform(Transform.Translation(vectorY * pinSpacing[i]));
        midPtOffset.Transform(Transform.Translation(zVec2));

        Line newLine = new Line(midPt, midPtOffset);
        bracketLines.Add(newLine);
      }
    }
    return bracketLines;
    }
  }  
}