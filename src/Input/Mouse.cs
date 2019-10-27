using Arqanore.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Input
{
    public static class Mouse
    {
        public static Vector2 Position { get; internal set; }
        internal static int[] ButtonState { get; set; }

        internal static void Init()
        {
            Position = new Vector2(-1, -1);
            ButtonState = new int[15];
        }

        public static bool ButtonDown(MouseButton button)
        {
            return ButtonState[(int)button] > 0;
        }
        public static bool ButtonPressed(MouseButton button)
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
