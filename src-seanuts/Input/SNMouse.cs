using Seanuts.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Input
{
    public static class SNMouse
    {
        public static SNVector2 Position { get; internal set; }
        internal static int[] ButtonState { get; set; }

        internal static void Init()
        {
            Position = new SNVector2(0, 0);
            ButtonState = new int[15];
        }

        public static bool ButtonDown(SNMouseButton button)
        {
            return ButtonState[(int)button] > 0;
        }
        public static bool ButtonPressed(SNMouseButton button)
        {
            if (ButtonState[(int)button] == 1)
            {
                ButtonState[(int)button] = 2;
                return true;
            }

            return false;
        }
    }
}
