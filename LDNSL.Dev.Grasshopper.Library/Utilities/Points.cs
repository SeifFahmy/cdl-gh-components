using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;

namespace LDNSL.Dev.Grasshopper.Library.Utilities
{
    public static class Points
    {
        //Calculate distance between two points is better.
        //If below a tolerance then we can say they are the same. 
        public static double distanceBetweenTwoPoints(Point3d a, Point3d b)
        {
            //Calculate sides of the triangle.
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double dz = b.Z - a.Z;

            //Calculate length of the hypotenuse.
            double d = Math.Sqrt(dx * dx + dy * dy + dz * dz);

            return d;
        }

          //More memory efficient to compare the squared distance instead of calculating the square root
        public static double squaredDistanceBetweenTwoPoints(Point3d a, Point3d b)
        {
            //Calculate sides of the triangle.
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double dz = b.Z - a.Z;
            //Calculate length of the hypotenuse.
            double d2 = dx * dx + dy * dy + dz * dz;
            return d2;
        }     

        //Not the best method due to double precision in memory.
        //0.1 => 0.100000000000332495
        public static bool isNumericallyEqual(Point3d a, Point3d b)
        {
            bool equal = a.X == b.X && a.Y == b.Y && a.Z == b.Z;
            return equal;
        }

        public static List<bool> isEqual(Point3d point, List<Point3d> points, double tolerance)
        {
            List<bool> areEqual = new List<bool>();

            for (int i = 0; i < points.Count; i++)
            {
                double d = squaredDistanceBetweenTwoPoints(point, points[i]);
                if (d < tolerance)
                {
                    areEqual.Add(true);
                }
                else
                {
                    areEqual.Add(false);
                }
            }
            return areEqual;
        }  

        //Returns true if coordinates of points are equal under a tolerance value.
        public static bool isSimilar(Point3d a, Point3d b, double tolerance)
        {
            bool similar = Math.Abs(a.X - b.X) < tolerance
            && Math.Abs(a.Y - b.Y) < tolerance
            && Math.Abs(a.Z - b.Z) < tolerance;

            return similar;
        }

        public static List<Point3d> removeDuplicatePoints(List<Point3d> points, double tolerance)
        {
            List<Point3d> clean = new List<Point3d>(points);
            double t2 = tolerance * tolerance;
            //Go over each point on the list
            for (int i = 0; i < clean.Count; i++)
            {
                //Compare with the rest of the points
                //Loop backwards to make sure we dont skip any items
                for (int j = clean.Count - 1; j > i; j--)
                {
                    double d2 = squaredDistanceBetweenTwoPoints(clean[i], clean[j]);
                    if (d2 < t2)
                    {
                        clean.RemoveAt(j);
                    }
                }
            }
            return clean;
        }
    }
}