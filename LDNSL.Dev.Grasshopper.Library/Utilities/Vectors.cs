using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;

namespace LDNSL.Dev.Grasshopper.Library.Utilities
{
    public static class Vectors
    {
        public static List<double> crossSectionRotations(List<Curve> curves, Vector3d vector)
        {
            List<Plane> planes = new List<Plane>();
            List<double> angles = new List<double>();

            foreach(Curve crv in curves)
            {
                Plane crvPlane;
                bool isPlane = crv.PerpendicularFrameAt(0.0, out crvPlane);
                planes.Add(crvPlane);

                Transform zMove = Transform.Translation(0, 0, 10);
                Transform projection = Transform.PlanarProjection(crvPlane);
                Transform projection2 = Transform.ProjectAlong(crvPlane, crvPlane.ZAxis);
                Transform vecRef = Transform.Translation(vector);

                Point3d p1 = new Point3d(crvPlane.Origin);
                Point3d p2 = new Point3d(crvPlane.Origin);
                p2.Transform(zMove);
                LineCurve zAxis = new LineCurve(p1, p2);

                Vector3d zVec = new Vector3d(Plane.WorldXY.ZAxis);
                zVec.Transform(projection);

                //Curve pCurve = Curve.ProjectToPlane(zAxis, crvPlane);
                zAxis.Transform(projection);

                //Point3d p3 = pCurve.PointAt(1.0);
                Point3d p3 = zAxis.PointAt(1.0);

                Vector3d vec = new Vector3d(p3);
                Point3d p4 = new Point3d(crvPlane.Origin);
                p4.Transform(vecRef);
                Vector3d vec2 = new Vector3d(p4);

                double angle = Vector3d.VectorAngle(vector, zVec, crvPlane);
                angles.Add(angle);
            }
            return angles;
        }
    }
}