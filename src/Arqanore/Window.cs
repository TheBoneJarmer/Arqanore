using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using Arqanore.Input;
using Arqan;
using Arqanore.Graphics;
using Color = Arqanore.Graphics.Color;

namespace Arqanore
{
    public class Window
    {
        private int width;
        private int height;
        private string title;
        private Texture[] icon;

        private GLFW.GLFWerrorfun glfwErrorFunction;
        private GLFW.GLFWwindowsizefun glfwWindowSizeFunction;
        private GLFW.GLFWwindowclosefun glfwWindowCloseFunction;
        private GLFW.GLFWwindowrefreshfun glfwWindowRefreshFunction;
        private GLFW.GLFWcursorposfun glfwCursorPosFunction;
        private GLFW.GLFWmousebuttonfun glfwMouseButtonFunction;
        private GLFW.GLFWjoystickfun glfwJoystickFunction;
        private GLFW.GLFWkeyfun glfwKeyFunction;
        private GLFW.GLFWcharfun glfwCharFunction;

        internal static Window Current { get; private set;  }

        public IntPtr Handle { get; set; }
        public WindowState State { get; private set; }
        public Color ClearColor { get; set; }
        public bool VSync { get; set; }
        public int Width
        {
            get { return width; }
            set
            {
                width = value;

                if (Handle != IntPtr.Zero)
                {
                    GLFW.glfwSetWindowSize(Handle, width, height);
                }
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value;

                if (Handle != IntPtr.Zero)
                {
                    GLFW.glfwSetWindowSize(Handle, width, height);
                }
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;

                if (Handle != IntPtr.Zero)
                {
                    GLFW.glfwSetWindowTitle(Handle, value);
                }
            }
        }

        public Texture[] Icon
        {
            get { return icon; }
            set
            {
                icon = value;

                if (Handle != IntPtr.Zero)
                {
                    var images = new GLFW.GLFWImage[value.Length];

                    for (var i = 0; i < images.Length; i++)
                    {
                        images[i].width = value[i].Width;
                        images[i].height = value[i].Height;
                        images[i].pixels = value[i].Pixels;
                    }
                    
                    GLFW.glfwSetWindowIcon(Handle, images.Length, images);
                }
            }
        }

        public Window(int width, int height, string title)
        {
            // Set the properties
            this.Width = width;
            this.Height = height;
            this.Title = title;
            this.ClearColor = Color.BLACK;
        }

        public void Open(bool fullscreen = false, bool resizable = true, bool maximized = false)
        {
            Current = this;

            InitGLFW();
            InitWindow(fullscreen, resizable, maximized);
            InitEvents();
            InitFramework();

            Sync();
        }
        public void Close()
        {
            GLFW.glfwSetWindowShouldClose(Handle, 1);
        }

        private void InitGLFW()
        {
            if (GLFW.glfwInit() == 0)
            {
                throw new ArqanoreGlfwException(-1, "Unable to initialize glfw");
            }
        }
        private void InitWindow(bool fullscreen, bool resizable, bool maximized)
        {
            if (fullscreen)
            {
                Handle = GLFW.glfwCreateWindow(Width, Height, Encoding.ASCII.GetBytes(Title), GLFW.glfwGetPrimaryMonitor(), IntPtr.Zero);
            }
            else
            {
                GLFW.glfwDefaultWindowHints();
                
                if (resizable) GLFW.glfwWindowHint(GLFW.GLFW_RESIZABLE, GLFW.GLFW_TRUE);
                if (!resizable) GLFW.glfwWindowHint(GLFW.GLFW_RESIZABLE, GLFW.GLFW_FALSE);

                if (maximized) GLFW.glfwWindowHint(GLFW.GLFW_MAXIMIZED, GLFW.GLFW_TRUE);
                if (!maximized) GLFW.glfwWindowHint(GLFW.GLFW_MAXIMIZED, GLFW.GLFW_FALSE);
                
                Handle = GLFW.glfwCreateWindow(Width, Height, Encoding.ASCII.GetBytes(Title), IntPtr.Zero, IntPtr.Zero);
            }

            if (Handle == IntPtr.Zero)
            {
                GLFW.glfwTerminate();
                throw new ArqanoreGlfwException(-1, "Unable to create window");
            }

            GLFW.glfwMakeContextCurrent(Handle);
        }
        private void InitEvents()
        {
            glfwErrorFunction = OnErrorFunction;
            glfwCharFunction = OnCharFunction;
            glfwCursorPosFunction = OnCursorPositionFunction;
            glfwKeyFunction = OnKeyFunction;
            glfwMouseButtonFunction = OnMouseButtonFunction;
            glfwWindowCloseFunction = OnWindowCloseFunction;
            glfwWindowRefreshFunction = OnWindowRefreshFunction;
            glfwWindowSizeFunction = OnWindowSizeFunction;
            glfwJoystickFunction = OnJoystickFunction;

            GLFW.glfwSetErrorCallback(glfwErrorFunction);
            GLFW.glfwSetWindowSizeCallback(Handle, glfwWindowSizeFunction);
            GLFW.glfwSetWindowCloseCallback(Handle, glfwWindowCloseFunction);
            GLFW.glfwSetWindowRefreshCallback(Handle, glfwWindowRefreshFunction);
            GLFW.glfwSetCursorPosCallback(Handle, glfwCursorPosFunction);
            GLFW.glfwSetMouseButtonCallback(Handle, glfwMouseButtonFunction);
            GLFW.glfwSetKeyCallback(Handle, glfwKeyFunction);
            GLFW.glfwSetCharCallback(Handle, glfwCharFunction);
            GLFW.glfwSetJoystickCallback(glfwJoystickFunction);
        }
        private void InitFramework()
        {
            Mouse.Init();
            Keyboard.Init();
        }

        private void Sync()
        {
            double dt = 1 / 60.0;
            double currentTime = GLFW.glfwGetTime();
            bool loaded = false;

            // Mark the window as open
            State = WindowState.Open;

            // Main loop
            while (GLFW.glfwWindowShouldClose(Handle) == 0)
            {
                // Render a background and enable some stuff for 2d rendering with alpha
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glViewport(0, 0, Width, Height);
                GL.glClearColor(ClearColor.R / 255.0f, ClearColor.G / 255.0f, ClearColor.B / 255.0f, ClearColor.A / 255.0f);
                GL.glClear(GL.GL_COLOR_BUFFER_BIT);

                if (!loaded)
                {
                    OnLoad?.Invoke();
                    loaded = true;
                }
                else
                {
                    double newTime = GLFW.glfwGetTime();
                    double frameTime = newTime - currentTime;
                    currentTime = newTime;

                    while (frameTime > 0)
                    {
                        double deltaTime = System.Math.Min(frameTime, dt);
                        OnTick?.Invoke(deltaTime);
                        frameTime -= deltaTime;
                    }

                    OnUpdate?.Invoke();

                    // Update input states
                    for (var i = 0; i < Mouse.ButtonState.Length; i++)
                    {
                        // 1 means being hold down
                        // 2 means pressed
                        // 3 means released
                        if (Mouse.ButtonState[i] == 1)
                        {
                            Mouse.ButtonState[i] = 2;
                        }
                        if (Mouse.ButtonState[i] == 3)
                        {
                            Mouse.ButtonState[i] = 0;
                        }
                    }
                    for (var i = 0; i < Keyboard.KeyState.Length; i++)
                    {
                        if (Keyboard.KeyState[i] == 1)
                        {
                            Keyboard.KeyState[i] = 2;
                        }
                        if (Keyboard.KeyState[i] == 4)
                        {
                            Keyboard.KeyState[i] = 0;
                        }
                    }

                    OnRender?.Invoke();
                }

                GLFW.glfwSwapInterval(VSync ? 1 : 0);
                GLFW.glfwSwapBuffers(Handle);
                GLFW.glfwPollEvents();
            }

            if (State != WindowState.Closed)
            {
                State = WindowState.Closed;
                OnClose?.Invoke();
            }

            GLFW.glfwDestroyWindow(Handle);
            GLFW.glfwTerminate();
        }

        public void HideCursor()
        {
            GLFW.glfwSetInputMode(Handle, GLFW.GLFW_CURSOR, GLFW.GLFW_CURSOR_HIDDEN);
        }
        public void ShowCursor()
        {
            GLFW.glfwSetInputMode(Handle, GLFW.GLFW_CURSOR, GLFW.GLFW_CURSOR_NORMAL);
        }

        /* GENERAL FUNCTIONS */
        private void OnErrorFunction(int errorCode, string description)
        {
            throw new ArqanoreGlfwException(errorCode, description);
        }
        private void OnWindowSizeFunction(IntPtr windowHandle, int width, int height)
        {
            this.width = width;
            this.height = height;

            OnResize?.Invoke(width, height);
        }
        private void OnWindowRefreshFunction(IntPtr windowHandle)
        {
            OnRefresh?.Invoke();
        }
        private void OnPositionFunction(IntPtr windowHandle, int x, int y)
        {
            OnPosition?.Invoke(x, y);
        }
        private void OnWindowCloseFunction(IntPtr windowHandle)
        {
            if (State != WindowState.Closed)
            {
                State = WindowState.Closed;
                OnClose?.Invoke();
            }
        }

        /* INPUT FUNCTIONS */
        private void OnCursorPositionFunction(IntPtr windowHandle, double x, double y)
        {
            Mouse.X = (int)x;
            Mouse.Y = (int)y;
        }
        private void OnMouseButtonFunction(IntPtr windowHandle, int button, int action, int mods)
        {
            if (action == 1)
            {
                Mouse.ButtonState[button] = 1;
            }
            if (action == 0)
            {
                if (Mouse.ButtonState[button] == 2)
                {
                    Mouse.ButtonState[button] = 3;
                }
            }
        }
        private void OnKeyFunction(IntPtr windowHandle, int key, int scanCode, int action, int mods)
        {
            if (key < Keyboard.KeyState.Length)
            {
                if (action == 1)
                {
                    Keyboard.KeyState[key] = 1;
                }
                if (action == 2)
                {
                    Keyboard.KeyState[key] = 3;
                }
                if (action == 0 && Keyboard.KeyState[key] > 0)
                {
                    Keyboard.KeyState[key] = 4;
                }
            }
        }
        private void OnCharFunction(IntPtr windowHandle, uint codepoint)
        {
            Keyboard.PressedChar = (char)codepoint;
            Keyboard.PressedCharCode = (int)codepoint;

            OnChar?.Invoke((char)codepoint);
        }

        private void OnJoystickFunction(int jid, int ev)
        {
            if (ev == GLFW.GLFW_CONNECTED) OnJoystickConnected?.Invoke(jid);
            if (ev == GLFW.GLFW_DISCONNECTED) OnJoystickDisconnected?.Invoke(jid);
        }

        /* EVENTS */
        public delegate void OnTickDelegate(double deltaTime);
        public delegate void OnUpdateDelegate();
        public delegate void OnRenderDelegate();
        public delegate void OnRefreshDelegate();
        public delegate void OnResizeDelegate(int width, int height);
        public delegate void OnPositionDelegate(int x, int y);
        public delegate void OnLoadDelegate();
        public delegate void OnCloseDelegate();
        public delegate void OnCharDelegate(char c);
        public delegate void OnJoystickConnectedDelegate(int joystick);
        public delegate void OnJoystickDisconnectedDelegate(int joystick);

        public event OnTickDelegate OnTick;
        public event OnUpdateDelegate OnUpdate;
        public event OnRenderDelegate OnRender;
        public event OnRefreshDelegate OnRefresh;
        public event OnResizeDelegate OnResize;
        public event OnPositionDelegate OnPosition;
        public event OnCloseDelegate OnClose;
        public event OnLoadDelegate OnLoad;
        public event OnCharDelegate OnChar;
        public event OnJoystickConnectedDelegate OnJoystickConnected;
        public event OnJoystickDisconnectedDelegate OnJoystickDisconnected;
    }
}