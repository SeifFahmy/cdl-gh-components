using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.UI.ObjectProperties;

namespace LDNSL.Dev.Grasshopper.Library.Utilities
{
    public class General
    {
        public static double addNumbers(double a, double b)
        {
            return a + b;
        }
        
        public static double subtractNumbers(double a, double b)
        {
            return a - b;
        }

        public static List<bool> stringEquals(string txt, List<string> txtList)
        {
            List<bool> matchingText = new List<bool>();

            foreach (string str in txtList)
            {
                if (str == txt)
                {
                    matchingText.Add(true);
                }
                else if (str != txt)
                {
                    matchingText.Add(false);
                }
            }
            return matchingText;
        }
        
    }
}