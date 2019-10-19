using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts
{
    public static class Time
    {
        internal static double Now { get; set; }
        internal static double Then { get; set; }
        internal static double Elapsed { get; set; }

        public static double DeltaTime
        {
            get { return Now - Then; }
        }
    }
}
