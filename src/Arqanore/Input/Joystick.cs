using System;
using System.Runtime.InteropServices;
using Arqan;

namespace Arqanore.Input
{
    public static class Joystick
    {
        public static bool Connected(int joystick)
        {
            return GLFW.glfwJoystickPresent(joystick) == 1;
        }

        public static float[] GetAxis(int joystick)
        {
            var arraySize = 0;
            var ptr = GLFW.glfwGetJoystickAxes(joystick, out arraySize);
            var states = new float[arraySize];
            
            Marshal.Copy(ptr, states, 0, arraySize);
            return states;
        }

        public static bool[] GetButtons(int joystick)
        {
            var arraySize = 0;
            var ptr = GLFW.glfwGetJoystickButtons(joystick, out arraySize);
            var states = new byte[arraySize];
            var result = new bool[states.Length];
            
            Marshal.Copy(ptr, states, 0, arraySize);

            for (var i=0; i<states.Length; i++)
            {
                if (states[i] == 1) result[i] = true;
            }

            return result;
        }

        public static byte[] GetHats(int joystick)
        {
            var arraySize = 0;
            var ptr = GLFW.glfwGetJoystickHats(joystick, out arraySize);
            var states = new byte[arraySize];
            
            Marshal.Copy(ptr, states, 0, arraySize);
            return states;
        }

        public static string GetName(int joystick)
        {
            return GLFW.glfwGetJoystickName(joystick);
        }
    }
}