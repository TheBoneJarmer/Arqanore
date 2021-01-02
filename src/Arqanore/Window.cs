using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Arqanore.Graphics;
using Arqanore.Input;
using Arqan;

namespace Arqanore
{
    public class Window
    {
        private IntPtr handle;
        private Color clearColor;
        private WindowState state;
        private int width;
        private int height;
        private string title;

        private GLFW.GLFWerrorfun glfwErrorFunction;
        private GLFW.GLFWwindowsizefun glfwWindowSizeFunction;
        private GLFW.GLFWwindowclosefun glfwWindowCloseFunction;
        private GLFW.GLFWwindowrefreshfun glfwWindowRefreshFunction;
        private GLFW.GLFWcursorposfun glfwCursorPosFunction;
        private GLFW.GLFWmousebuttonfun glfwMouseButtonFunction;
        private GLFW.GLFWkeyfun glfwKeyFunction;
        private GLFW.GLFWcharfun glfwCharFunction;

        internal static Window Current { get; private set;  }

        private IntPtr Handle
        {
            get { return handle; }
            set { handle = value; }
        }
        public WindowState State
        {
            get { return state; }
        }
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

        public Color ClearColor
        {
            get { return clearColor; }
            set { clearColor = value; }
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
                throw new GLFWException(-1, "Unable to initialize");
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
                throw new GLFWException(-1, "Unable to create window");
            }

            GLFW.glfwMakeContextCurrent(Handle);
        }
        private void InitEvents()
        {
            this.glfwErrorFunction = new GLFW.GLFWerrorfun(OnErrorFunction);
            this.glfwCharFunction = new GLFW.GLFWcharfun(OnCharFunction);
            this.glfwCursorPosFunction = new GLFW.GLFWcursorposfun(OnCursorPositionFunction);
            this.glfwKeyFunction = new GLFW.GLFWkeyfun(OnKeyFunction);
            this.glfwMouseButtonFunction = new GLFW.GLFWmousebuttonfun(OnMouseButtonFunction);
            this.glfwWindowCloseFunction = new GLFW.GLFWwindowclosefun(OnWindowCloseFunction);
            this.glfwWindowRefreshFunction = new GLFW.GLFWwindowrefreshfun(OnWindowRefreshFunction);
            this.glfwWindowSizeFunction = new GLFW.GLFWwindowsizefun(OnWindowSizeFunction);

            GLFW.glfwSetErrorCallback(this.glfwErrorFunction);
            GLFW.glfwSetWindowSizeCallback(Handle, this.glfwWindowSizeFunction);
            GLFW.glfwSetWindowCloseCallback(Handle, this.glfwWindowCloseFunction);
            GLFW.glfwSetWindowRefreshCallback(Handle, this.glfwWindowRefreshFunction);
            GLFW.glfwSetCursorPosCallback(Handle, this.glfwCursorPosFunction);
            GLFW.glfwSetMouseButtonCallback(Handle, this.glfwMouseButtonFunction);
            GLFW.glfwSetKeyCallback(Handle, this.glfwKeyFunction);
            GLFW.glfwSetCharCallback(Handle, this.glfwCharFunction);
        }
        private void InitFramework()
        {
            Mouse.Init();
            Keyboard.Init();
            Draw.Init(this);
        }

        private void Sync()
        {
            double dt = 1 / 60.0;
            double currentTime = GLFW.glfwGetTime();
            bool loaded = false;

            // Mark the window as open
            state = WindowState.Open;

            // Main loop
            while (GLFW.glfwWindowShouldClose(Handle) == 0)
            {
                // Render a background and enable some stuff for 2d rendering with alpha
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                GL.glViewport(0, 0, Width, Height);
                GL.glClearColor(clearColor.R / 255.0f, clearColor.G / 255.0f, clearColor.B / 255.0f, clearColor.A / 255.0f);
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
                        frameTime -= deltaTime;

                        OnTick?.Invoke(deltaTime);
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

                GLFW.glfwSwapInterval(1);
                GLFW.glfwSwapBuffers(Handle);
                GLFW.glfwPollEvents();
            }

            if (state != WindowState.Closed)
            {
                state = WindowState.Closed;
                OnClose?.Invoke();
            }

            GLFW.glfwDestroyWindow(Handle);
        }

        public void HideCursor()
        {
            GLFW.glfwSetInputMode(handle, GLFW.GLFW_CURSOR, GLFW.GLFW_CURSOR_HIDDEN);
        }
        public void ShowCursor()
        {
            GLFW.glfwSetInputMode(handle, GLFW.GLFW_CURSOR, GLFW.GLFW_CURSOR_NORMAL);
        }

        /* GENERAL FUNCTIONS */
        private void OnErrorFunction(int errorCode, string description)
        {
            throw new GLFWException(errorCode, description);
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
            if (state != WindowState.Closed)
            {
                OnClose?.Invoke();
                state = WindowState.Closed;
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

        public event OnTickDelegate OnTick;
        public event OnUpdateDelegate OnUpdate;
        public event OnRenderDelegate OnRender;
        public event OnRefreshDelegate OnRefresh;
        public event OnResizeDelegate OnResize;
        public event OnPositionDelegate OnPosition;
        public event OnCloseDelegate OnClose;
        public event OnLoadDelegate OnLoad;
        public event OnCharDelegate OnChar;
    }
}