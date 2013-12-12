using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileMapEditor
{
    class TileDisplay : GraphicsDeviceControl
    {
        public event EventHandler onInitialize;
        public event EventHandler onDraw;

        public void OnInitializeAdd(EventHandler newEvent)
        {
            onInitialize += newEvent;
        }

        public void OnDrawAdd(EventHandler newEvent)
        {
            onDraw += newEvent;
        }

        //public EventHandler OnDraw
        //{ get { return onDraw; } }

        protected override void Initialize()
        {
            if (onInitialize != null)
                onInitialize(this, null);
        }

        protected override void Draw()
        {
            if (onDraw != null)
                onDraw(this, null);
        }


    }
}
