using System;
using Seanuts;
using Seanuts.Framework;
using Seanuts.Framework.Input;
using Seanuts.Framework.Graphics;
using Seanuts.Framework.Math;

namespace FontEditor
{
    public class Editor
    {
        private SNWindow Window { get; set; }

        public Editor()
        {
            
        }

        public void Run()
        {
            Window = new SNWindow(1024, 768, "Font Editor");
            Window.OnLoad += Window_OnLoad;
            Window.OnUpdate += Window_OnUpdate;
            Window.OnRender += Window_OnRender;
            Window.Open(false, true, false);
        }

        /* EVENTS */
        public void Window_OnLoad()
        {
            // Set background color to white
            Window.ClearColor = SNColor.WHITE;
        }
        public void Window_OnUpdate()
        {

        }
        public void Window_OnRender()
        {

        }
    }
}