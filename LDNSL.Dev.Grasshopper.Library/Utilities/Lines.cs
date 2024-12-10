using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;

namespace LDNSL.Dev.Grasshopper.Library.Utilities
{
    public static class Lines
    {
        //Returns true if start/end points are equal under a tolerance value.
        public static bool isDuplicate(Line a, Line b, double tolerance)
        {
            //Compare starting points.
            if (Points.isSimilar(a.From, b.From, tolerance)
            && Points.isSimilar(a.To, b.To, tolerance)
            || Points.isSimilar(a.From, b.To, tolerance)
            && Points.isSimilar(a.To, b.From, tolerance))
            {
                return true;
            }
            return false;
        }

        public static List<Line> RemoveDuplicateLines(List<Line> lines, double tolerance)
        {
            //Copy original list.
            List<Line> clean = new List<Line>(lines);

            //Clean up the list of duplicates.
            for (int i = 0; i < clean.Count; i++)
            {
                Line line = clean[i];
                for (int j = clean.Count - 1; j > i; j--)
                {
                    Line other = clean[j];
                    bool dup = isDuplicate(line, other, tolerance);
                    if (dup)
                    {
                        clean.RemoveAt(j);
                    }
                }
            }
            return clean;
        }
    }
}