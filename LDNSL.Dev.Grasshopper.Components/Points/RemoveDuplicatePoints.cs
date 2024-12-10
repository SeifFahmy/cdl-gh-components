using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Display;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;
using LDNSL.Dev.Grasshopper.Library;

namespace LDNSL.Dev.Grasshopper.Components.General.RemoveDuplicatePoints
{
    public class RemoveDuplicatePoints : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AddNumbersComponent class.
        /// </summary>
        public RemoveDuplicatePoints()
          : base("RemoveDuplicatePoints", "RmvDupPts",
              "Removes duplicate points from a list",
              "LDNSL", "Points")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("PointList", "PtList", "List of points to check", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "t", "Tolerance", GH_ParamAccess.item, 0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("R", "R", "Result", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Define placeholder variables
            List<Point3d> a = new List<Point3d>();
            double b = 0;

            //Load values from inputs
            if (!DA.GetDataList(0, a)) return;
            if (!DA.GetData(1, ref b)) return;

            //Code
            //double sum = a + b;
            List<Point3d> cleanList = LDNSL.Dev.Grasshopper.Library.Utilities.Points.removeDuplicatePoints(a, b);

            //Outputs
            DA.SetDataList(0, cleanList);
        }

        //Hide the component if you want
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                //return @"Resources\Cundall_C_24x24.png";
                return LDNSL.Dev.Grasshopper.Components.Properties.Resources.Cundall_C_24x24;
                //return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5962628a-534f-4dce-932a-ad8b4ac36184"); }
        }
    }
}