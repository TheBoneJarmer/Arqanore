using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore
{
    public static class Time
    {
        internal static double Now { get; set; }
        internal static double Then { get; set; }
        public static double DeltaTime { get; internal set; }
    }
}
