using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;
using LDNSL.Dev.Grasshopper.Library;

namespace LDNSL.Dev.Grasshopper.Components.General.StringEquals
{
    public class StringEquals : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StringEquals Component class.
        /// </summary>
        public StringEquals()
          : base("StringEquals", "StrEqls",
              "Test equality of two strings",
              "LDNSL", "Examples")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("A", "A", "First Value", GH_ParamAccess.item, String.Empty);
            pManager.AddTextParameter("B", "B", "Second Value", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("R", "R", "Result", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Define placeholder variables
            string a = string.Empty;
            List<string> b = new List<string>();

            //Load values from inputs
            if (!DA.GetData(0, ref a)) return;
            if (!DA.GetDataList(1, b)) return;

            //Code
            List<bool> isEqual = LDNSL.Dev.Grasshopper.Library.Utilities.General.stringEquals(a,b);

            //Outputs
            DA.SetDataList(0, isEqual);
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
            get { return new Guid("248f3792-24f7-4a69-a442-a6fb58e254a4"); }
        }
    }
}