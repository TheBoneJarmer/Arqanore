using Arqan;
using Arqanore.Math;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Arqanore.Input
{
    public static class Mouse
    {
        public static int X { get; set; }
        public static int Y { get; set; }
        internal static int[] ButtonState { get; set; }

        internal static void Init()
        {
            ButtonState = new int[15];
        }

        public static bool ButtonDown(MouseButton button)
        {
            return ButtonState[(int)button] > 0 && ButtonState[(int)button] < 3;
        }
        public static bool ButtonUp(MouseButton button)
        {
            return ButtonState[(int)button] == 3;
        }
        public static bool ButtonPressed(MouseButton button)
        {
            return ButtonState[(int)button] == 1;
        }
    }
}
