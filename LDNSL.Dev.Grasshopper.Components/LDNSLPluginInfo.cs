using Grasshopper;
using Grasshopper.Kernel;
//using LDNSL.Dev.Grasshopper.Components.Properties;
using System;
using System.Drawing;

namespace LDNSLPluginInfo
{
    public class LDNSLPluginInfo : GH_AssemblyInfo
    {
        public override string Name => "LDNSLPlugin";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        //public override Bitmap Icon => Resources.Hedgehog_Reduced_Transparent;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "A general toolbox of components";

        public override Guid Id => new Guid("b1d1af8f-d5ed-4473-8b47-4743c9c3a1d6");

        //Return a string identifying you or your company.
        public override string AuthorName => "Harry Spencer";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}