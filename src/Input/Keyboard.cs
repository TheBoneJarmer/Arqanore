using System;
using System.Collections.Generic;
using System.Text;

namespace Arqanore.Input
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
            return KeyState[(int)code] > 0;
        }
        public static bool KeyUp(KeyCode code)
        {
            return KeyState[(int)code] == 4;
        }
        public static bool KeyPressed(KeyCode code)
        {
            return KeyState[(int)code] == 1 || KeyState[(int)code] == 3;
        }
    }
}
