using System;
using System.Collections.Generic;
using System.Text;

namespace Seanuts.Framework.Input
{
    public static class Keyboard
    {
        internal static int[] KeyState { get; private set; }
        public static Char PressedChar { get; set; }
        public static int PressedCharCode { get; set; }

        internal static void Init()
        {
            KeyState = new int[512];
        }

        public static bool KeyDown(KeyCode code)
        {
            return KeyDown((int)code);
        }
        public static bool KeyPressed(KeyCode code)
        {
            return KeyPressed((int)code);
        }
        public static bool KeyDown(int code)
        {
            return KeyState[code] > 0;
        }
        public static bool KeyPressed(int code)
        {
            if (KeyState[code] == 1)
            {
                KeyState[code] = 2;
                return true;
            }

            return false;
        }
    }
}
