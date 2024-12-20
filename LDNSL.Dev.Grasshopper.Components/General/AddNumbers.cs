using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;
using LDNSL.Dev.Grasshopper.Library;

namespace LDNSL.Dev.Grasshopper.Components.General.AddNumbers
{
    public class AddNumbers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the AddNumbers Component class.
        /// </summary>
        public AddNumbers()
          : base("AddNumbers", "AddNums",
              "Adds two numbers",
              "LDNSL", "Examples")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("A", "A", "First Value", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("B", "B", "Second Value", GH_ParamAccess.item, 0);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("R", "R", "Result", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Define placeholder variables
            double a = 0;
            double b = 0;

            //Load values from inputs
            if (!DA.GetData(0, ref a)) return;
            if (!DA.GetData(1, ref b)) return;

            //Code
            //double sum = a + b;
            double sum = LDNSL.Dev.Grasshopper.Library.Utilities.General.addNumbers(a,b);

            //Outputs
            DA.SetData(0, sum);
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
            get { return new Guid("b059bc17-fffe-4080-a021-3a3a1d6bc89f"); }
        }
    }
}
