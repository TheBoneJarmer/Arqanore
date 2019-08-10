using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Input;
using Seanuts.OpenGL;

namespace Seanuts.Framework
{
    public class GameWindow
    {
        public static GameWindow Current { get; private set; }

        private IntPtr handle;
        private int width;
        private int height;
        private string title;
        private Color clearColor;

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
        public bool Running
        {
            get { return Handle != IntPtr.Zero; }
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

            // Set the Current variable to this
            GameWindow.Current = this;
        }

        public void Open(bool fullscreen)
        {
            // Init GLFW
            if (GLFW.glfwInit() == 0)
            {
                throw new GLFWException("Unable to initialize");
            }

            // Create the window
            if (fullscreen) Handle = GLFW.glfwCreateWindow(Width, Height, Encoding.ASCII.GetBytes(Title), GLFW.glfwGetPrimaryMonitor(), IntPtr.Zero);
            if (!fullscreen) Handle = GLFW.glfwCreateWindow(Width, Height, Encoding.ASCII.GetBytes(Title), IntPtr.Zero, IntPtr.Zero);

            if (Handle == IntPtr.Zero)
            {
                GLFW.glfwTerminate();
                throw new Exception("Unable to create window");
            }

            // Make the window's context the current one
            GLFW.glfwMakeContextCurrent(Handle);

            // Set events
            GLFW.glfwSetErrorCallback(new GLFW.GLFWerrorfun(OnErrorFunction));
            GLFW.glfwSetWindowSizeCallback(Handle, new GLFW.GLFWwindowsizefun(OnResizeFunction));
            GLFW.glfwSetWindowCloseCallback(Handle, new GLFW.GLFWwindowclosefun(OnCloseFunction));
            GLFW.glfwSetCursorPosCallback(Handle, new GLFW.GLFWcursorposfun(OnCursorPositionFunction));
            GLFW.glfwSetMouseButtonCallback(Handle, new GLFW.GLFWmousebuttonfun(OnMouseButtonFunction));
            GLFW.glfwSetKeyCallback(Handle, new GLFW.GLFWkeyfun(OnKeyFunction));
            GLFW.glfwSetCharCallback(Handle, new GLFW.GLFWcharfun(OnCharFunction));

            // Enable VSYNC
            GLFW.glfwSwapInterval(1);

            // Init everything
            Mouse.Init();
            Keyboard.Init();
            Draw.Init(this);

            // Execute onload event
            if (OnLoad != null)
            {
                OnLoad();
            }

            // Start the threads
            Thread thrUpdate = new Thread(Update_Callback);
            thrUpdate.Start();

            // Update time
            Time.Now = GLFW.glfwGetTime();

            // Keep the main thread running for rendering
            while (GLFW.glfwWindowShouldClose(Handle) == 0)
            {
                // Update time
                Time.Then = Time.Now;
                Time.Now = GLFW.glfwGetTime();

                // Render
                GL10.glEnable(GL11.GL_BLEND);
                GL10.glBlendFunc(GL11.GL_SRC_ALPHA, GL11.GL_ONE_MINUS_SRC_ALPHA);
                GL10.glViewport(0, 0, Width, Height);
                GL10.glClearColor(clearColor.R, clearColor.G, clearColor.B, clearColor.A);
                GL10.glClear(GL11.GL_COLOR_BUFFER_BIT);

                if (OnRender != null)
                {
                    OnRender();
                }

                // Poll events
                GLFW.glfwPollEvents();

                // Swap buffers
                GLFW.glfwSwapBuffers(Handle);
            }

            // Clear the handle
            Handle = IntPtr.Zero;

            // Cleanup
            GLFW.glfwDestroyWindow(Handle);
        }
        public void Close()
        {
            GLFW.glfwSetWindowShouldClose(Handle, 1);
        }

        /* THREAD */
        private void Update_Callback()
        {
            while (Running)
            {
                // Update
                if (OnUpdate != null)
                {
                    OnUpdate();
                }
            }
        }

        /* GENERAL FUNCTIONS */
        private void OnErrorFunction(int errorCode, string description)
        {
            throw new Exception(errorCode + ": " + description);
        }
        private void OnResizeFunction(IntPtr windowHandle, int width, int height)
        {
            Width = width;
            Height = height;

            if (OnResize != null)
            {
                OnResize(width, height);
            }
        }
        private void OnPositionFunction(IntPtr windowHandle, int x, int y)
        {
            if (OnPosition != null)
            {
                OnPosition(x, y);
            }
        }
        private void OnCloseFunction(IntPtr windowHandle)
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
        public delegate void OnResizeDelegate(int width, int height);
        public delegate void OnPositionDelegate(int x, int y);
        public delegate void OnLoadDelegate();
        public delegate void OnCloseDelegate();
        public delegate void OnCharDelegate(char c);

        public event OnUpdateDelegate OnUpdate;
        public event OnRenderDelegate OnRender;
        public event OnResizeDelegate OnResize;
        public event OnPositionDelegate OnPosition;
        public event OnCloseDelegate OnClose;
        public event OnLoadDelegate OnLoad;
        public event OnCharDelegate OnChar;
    }
}