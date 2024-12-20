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

namespace LDNSL.Dev.Grasshopper.Components.Vectors.CrossSectionRotations
{
    public class CrossSectionRotations : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CrossSectionRotations Component class.
        /// </summary>
        public CrossSectionRotations()
          : base("CrossSectionRotations", "XSectRots",
              "Finds the cross-section rotation for a beam",
              "LDNSL", "Vectors")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("CurveList", "CrvList", "List of curves to check", GH_ParamAccess.list);
            pManager.AddVectorParameter("Vector", "V", "Vector", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("R", "R", "Result", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Define placeholder variables
            List<Curve> a = new List<Curve>();
            Vector3d b = new Vector3d(0,0,0);

            //Load values from inputs
            if (!DA.GetDataList(0, a)) return;
            if (!DA.GetData(1, ref b)) return;

            //Code
            List<double> rotations = LDNSL.Dev.Grasshopper.Library.Utilities.Vectors.crossSectionRotations(a, b);

            //Outputs
            DA.SetDataList(0, rotations);
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
            get { return new Guid("d1f2a68d-f729-409c-b522-dad8910c9b1e"); }
        }
    }
}