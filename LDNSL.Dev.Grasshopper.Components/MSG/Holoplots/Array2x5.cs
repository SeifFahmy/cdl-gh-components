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
using LDNSL.Dev.Grasshopper.Library.MSG;

namespace LDNSL.Dev.Grasshopper.Components.MSG.Holoplots.Array2x5
{
    public class Array2x5 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Array2x2 Component class.
        /// </summary>
        public Array2x5()
          : base("2x5Array", "2x5",
              "Creates a 2x5 Array",
              "LDNSL", "MSG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "PL", "List of planes", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("PFr", "PFr", "PerimeterFrame", GH_ParamAccess.tree);
            pManager.AddLineParameter("Cnr", "Cnr", "Corners", GH_ParamAccess.tree);
            pManager.AddLineParameter("VB", "VB", "VerticalBracing", GH_ParamAccess.tree);
            pManager.AddLineParameter("DB", "DB", "DiagonalBracing", GH_ParamAccess.tree);
            pManager.AddLineParameter("xPl", "xPl", "xPlates", GH_ParamAccess.tree);
            pManager.AddLineParameter("yPl", "yPl", "yPlates", GH_ParamAccess.tree);
            pManager.AddLineParameter("AddCnr", "AddCnr", "AdditionalCorners", GH_ParamAccess.tree);
            pManager.AddRectangleParameter("PB", "PB", "PerimeterBrackets", GH_ParamAccess.tree);
            pManager.AddLineParameter("IB", "IB", "InternalBrackets", GH_ParamAccess.tree);
            pManager.AddLineParameter("St", "St", "Studs", GH_ParamAccess.tree);
            pManager.AddTextParameter("Mk", "Mk", "Mark", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Define placeholder variables
            List<Plane> a = new List<Plane>();

            //Data Trees
            DataTree<Line> perimeterLines = new DataTree<Line>();
            DataTree<Line> cornerLines = new DataTree<Line>();
            DataTree<Line> verticalBracingLines = new DataTree<Line>();
            DataTree<Line> diagonalBracingLines = new DataTree<Line>();
            DataTree<Line> xPlateLines = new DataTree<Line>();
            DataTree<Line> yPlateLines = new DataTree<Line>();
            DataTree<Line> additionalCornerLines = new DataTree<Line>();
            DataTree<Rectangle3d> perimeterBracketRectangles = new DataTree<Rectangle3d>();
            DataTree<Line> internalBracketLines = new DataTree<Line>();
            DataTree<Line> studLines = new DataTree<Line>();
            DataTree<string> marks = new DataTree<string>();

            //Dimensions
            double xChordOffset = 0.3333;
            double yChordOffset = 0.3333;
            double xCornerOffset = 0.0417;
            double yCornerOffset = 0.0052;

            double width = 5.3302 + xChordOffset;
            double height = 10.0719 + yChordOffset;
            double depth = 1.8045;
            double chordDepth = 0.1667;

            List<double> xCornerOffsets = new List<double>(){
                -xCornerOffset,
                xCornerOffset,
                xCornerOffset,
                -xCornerOffset};
            List<double> yCornerOffsets = new List<double>(){
                -yCornerOffset,
                -yCornerOffset,
                yCornerOffset,
                yCornerOffset};


            double xChordExt = 0.1667;
            double yChordExt = -0.2292;
            double cornerExt = 0.3333;

            double xSpacing = 1.3368;
            List<double> xParams = new List<double>(){0};

            double ySpacing = 1.0088;
            List<double> yParams = new List<double>(){-(ySpacing * 3.0),-ySpacing, ySpacing, (ySpacing * 3.0)};

            double brOffset = 0.2;
            List<double> xBrOffsets = new List<double>(){
                -(xSpacing * 2.0 - brOffset),
                -brOffset,
                brOffset,
                (xSpacing * 2.0) - brOffset};
            List<double> yBrOffsets = new List<double>(){
                -(ySpacing * 5.0 - brOffset),
                -(ySpacing * 3.0 + brOffset),
                -(ySpacing * 3.0 - brOffset),
                (-ySpacing - brOffset),
                (-ySpacing + brOffset),
                (ySpacing - brOffset),
                (ySpacing) -brOffset,
                (ySpacing * 3.0) - brOffset,
                (ySpacing * 3.0) + brOffset,
                (ySpacing * 5.0) - brOffset};
            List<double> zxBrOffsets = new List<double>(){
                1,
                0,
                0,
                1};
            List<double> zyBrOffsets = new List<double>(){
                1,
                0,
                0,
                1,
                1,
                0,
                0,
                1,
                1,
                0};

            double pinA = 0.2461;
            double pinB = 1.5258;
            double pinC = 0.4917;

            List<double> pinSpacing = new List<double>(){
                -(pinA + pinB + pinC + pinB + pinC),
                -(pinA + pinB + pinC + pinB),
                -(pinA + pinB + pinC),
                -(pinA + pinB),
                -pinA,
                pinA,
                (pinA + pinB),
                (pinA + pinB + pinC),
                (pinA + pinB + pinC + pinB),
                (pinA + pinB + pinC + pinB + pinC)};

            double pinXOffset = 0.1667;

            //Load values from inputs
            if (!DA.GetDataList(0, a)) return;

            //Code
            //Create Array
            foreach (Plane pl in a)
            {
            int index = a.IndexOf(pl);
            GH_Path path = new GH_Path(index);

            //Create Marks
            string mark = "ArrayFraming" + (index + 1).ToString();
            marks.Add(mark, path);

            //Create Unit Vectors;
            Vector3d vectorX = pl.XAxis;
            vectorX.Unitize();
            Vector3d vectorY = pl.YAxis;
            vectorY.Unitize();
            Vector3d vectorZ = pl.ZAxis;
            vectorZ.Unitize();

            //Create Vectors
            Vector3d xVec = vectorX * width;
            Vector3d yVec = vectorY * height;
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

            //Create setting out rectangle
            Rectangle3d rectangle = Holoplot.createRectangle(pl, width, height);

            //Create Perimeter Frame
            List<Line> pLines = Holoplot.createPerimeterLines(rectangle, pl, depth, xChordExt, yChordExt);
            foreach (Line ln in pLines)
            {
                perimeterLines.Add(ln, path);
            }

            //Create Corners
            List<Line> cLines = Holoplot.createCornerLines(rectangle,
                pl,
                depth,
                cornerExt,
                xCornerOffsets,
                yCornerOffsets);
            foreach (Line ln in cLines)
            {
                cornerLines.Add(ln, path);
            }

            //Create Vertical Bracing
            List<Line> vLines = Holoplot.createVerticalBracingLines(rectangle, pl, xParams, yParams, depth);
            foreach (Line ln in vLines)
            {
                verticalBracingLines.Add(ln, path);
            }

            //Create Diagonal Bracing
            List<Line> dLines = Holoplot.createDiagonalBracingLines(rectangle,
                pl,
                xBrOffsets,
                yBrOffsets,
                zxBrOffsets,
                zyBrOffsets,
                depth);
            foreach (Line ln in dLines)
            {
                diagonalBracingLines.Add(ln, path);
            }

            //Create X Plates
            List<Line> xPlateList = Holoplot.createXPlateLines(rectangle, pl, xParams, depth);
            foreach (Line ln in xPlateList)
            {
                xPlateLines.Add(ln, path);
            }

            //Create Y Plates
            List<Line> yPlateList = Holoplot.createYPlateLines(rectangle, pl, yParams, depth);
            foreach (Line ln in yPlateList)
            {
                yPlateLines.Add(ln, path);
            }

            //Create Additional Corners
            List<Line> addCLines = Holoplot.createAddCornerLines(rectangle, pl, depth, cornerExt);
            foreach (Line ln in addCLines)
            {
                int indexLine = addCLines.IndexOf(ln);

                if (indexLine == 0 || indexLine == 3)
                {
                ln.Transform(Transform.Translation(vectorX * 0.2));
                additionalCornerLines.Add(ln, path);
                }
                else if (indexLine == 1 || indexLine == 2)
                {
                ln.Transform(Transform.Translation(-vectorX * 0.2));
                additionalCornerLines.Add(ln, path);
                }
            }

            //Create Perimeter Bracket Rectangles
            List<Rectangle3d> perimeterBrackets = Holoplot.createPerimeterBracketLines(rectangle,
                pl,
                pinSpacing,
                pinXOffset,
                chordDepth);
            foreach (Rectangle3d rec in perimeterBrackets)
            {
                perimeterBracketRectangles.Add(rec, path);
            }


            //Create Internal Bracket Lines
            List<Line> internalBrackets = Holoplot.createInternalBracketLines(xPlateList,
                pl,
                pinSpacing,
                depth);
            foreach (Line ln in internalBrackets)
            {
                internalBracketLines.Add(ln, path);
            }
            }
    
            //Outputs
            DA.SetDataTree(0, perimeterLines);
            DA.SetDataTree(1, cornerLines);
            DA.SetDataTree(2, verticalBracingLines);
            DA.SetDataTree(3, diagonalBracingLines);
            DA.SetDataTree(4, xPlateLines);
            DA.SetDataTree(5, yPlateLines);
            DA.SetDataTree(6, additionalCornerLines);
            DA.SetDataTree(7, perimeterBracketRectangles);
            DA.SetDataTree(8, internalBracketLines);
            DA.SetDataTree(9, studLines);
            DA.SetDataTree(10, marks);
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
            get { return new Guid("b198c69c-04a7-4ea5-b5a5-4cb9b1071192"); }
        }
    }
}