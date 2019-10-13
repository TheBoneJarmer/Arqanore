using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using TilarGL;

namespace Seanuts.Framework
{
    public class GameWindow
    {
        private IntPtr handle;
        private Color clearColor;
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

        private IntPtr Handle
        {
            get { return handle; }
            set { handle = value; }
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

        public GameWindow(int width, int height, string title)
        {
            // Set the properties
            this.Width = width;
            this.Height = height;
            this.Title = title;
            this.ClearColor = Color.BLACK;
        }

        public void Open(bool fullscreen = false, bool vsync = true, bool pollEvents = true)
        {            
            InitGLFW();
            InitWindow(fullscreen);
            InitEvents();
            InitSettings(vsync);
            InitFramework();

            PrintInfo();                   
            Sync(pollEvents);
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
        private void InitWindow(bool fullscreen)
        {
            if (fullscreen)
            {
                Handle = GLFW.glfwCreateWindow(Width, Height, Encoding.ASCII.GetBytes(Title), GLFW.glfwGetPrimaryMonitor(), IntPtr.Zero);
            }
            else
            {
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
        private void InitSettings(bool vsync)
        {
            if (vsync)
            {
                GLFW.glfwSwapInterval(1);
            }
            else
            {
                GLFW.glfwSwapInterval(0);
            }
        }
        private void InitFramework()
        {
            Mouse.Init();
            Keyboard.Init();
            Draw.Init(this);
        }

        private void PrintInfo()
        {
            Console.WriteLine($"GL Version: {Device.GLVersion}");
            Console.WriteLine($"GLSL Version: {Device.GLSLVersion}");
            Console.WriteLine();
        }

        private void Sync(bool pollEvents)
        {
            // Execute onload event
            if (OnLoad != null)
            {
                OnLoad();
            }

            // Update time
            Time.Now = GLFW.glfwGetTime();

            // Main loop
            while (GLFW.glfwWindowShouldClose(Handle) == 0)
            {
                // Update time
                Time.Then = Time.Now;
                Time.Now = GLFW.glfwGetTime();

                // Update
                if (OnUpdate != null)
                {
                    OnUpdate();
                }

                // Render
                GL10.glEnable(GL11.GL_BLEND);
                GL10.glBlendFunc(GL11.GL_SRC_ALPHA, GL11.GL_ONE_MINUS_SRC_ALPHA);
                GL10.glViewport(0, 0, Width, Height);
                GL10.glClearColor(clearColor.R / 255.0f, clearColor.G / 255.0f, clearColor.B / 255.0f, clearColor.A / 255.0f);
                GL10.glClear(GL11.GL_COLOR_BUFFER_BIT);

                if (OnRender != null)
                {
                    OnRender();
                }

                // Swap buffers
                GLFW.glfwSwapBuffers(Handle);

                // Poll events or wait for events
                if (pollEvents)
                {
                    GLFW.glfwPollEvents();
                    Thread.Sleep(10);
                }
                else
                {
                    GLFW.glfwWaitEvents();
                }
            }

            GLFW.glfwDestroyWindow(Handle);
        }

        /* GENERAL FUNCTIONS */
        private void OnErrorFunction(int errorCode, string description)
        {
            throw new GLFWException(errorCode, description);
        }
        private void OnWindowSizeFunction(IntPtr windowHandle, int width, int height)
        {
            Width = width;
            Height = height;

            if (OnResize != null)
            {
                OnResize(width, height);
            }
        }
        private void OnWindowRefreshFunction(IntPtr windowHandle)
        {
            if (OnRefresh != null)
            {
                OnRefresh();
            }
        }
        private void OnPositionFunction(IntPtr windowHandle, int x, int y)
        {
            if (OnPosition != null)
            {
                OnPosition(x, y);
            }
        }
        private void OnWindowCloseFunction(IntPtr windowHandle)
        {
            if (OnClose != null)
            {
                OnClose();
            }
        }

        /* INPUT FUNCTIONS */
        private void OnCursorPositionFunction(IntPtr windowHandle, double x, double y)
        {
            Mouse.Position.X = (int)x;
            Mouse.Position.Y = (int)y;
        }
        private void OnMouseButtonFunction(IntPtr windowHandle, int button, int action, int mods)
        {
            Mouse.ButtonState[button] = action;
        }
        private void OnKeyFunction(IntPtr windowHandle, int key, int scanCode, int action, int mods)
        {
            Keyboard.KeyState[key] = action;
        }
        private void OnCharFunction(IntPtr windowHandle, uint codepoint)
        {
            Keyboard.PressedChar = (char)codepoint;
            Keyboard.PressedCharCode = (int)codepoint;

            if (OnChar != null)
            {
                OnChar((char)codepoint);
            }
        }

        /* EVENTS */
        public delegate void OnUpdateDelegate();
        public delegate void OnRenderDelegate();
        public delegate void OnRefreshDelegate();
        public delegate void OnResizeDelegate(int width, int height);
        public delegate void OnPositionDelegate(int x, int y);
        public delegate void OnLoadDelegate();
        public delegate void OnCloseDelegate();
        public delegate void OnCharDelegate(char c);

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