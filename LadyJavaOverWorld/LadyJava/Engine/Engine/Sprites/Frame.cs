using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Frame
    {
        Point position;

        private int width;
        private int height;
        private float scale;

        public Point Position
        { get { return position; } }

        public float Scale
        { get { return scale; } }

        public int Width
        { get { return width; } }

        public int Height
        { get { return height; } }

        public Rectangle ToRectangle
        { get { return new Rectangle(position.X, position.Y, width, height); } }

        public Frame(int frameWidth, int frameHeight, float frameScale = 1f)
        {
            position = Point.Zero;
            width = frameWidth;
            height = frameHeight;
            scale = frameScale;
        }

        public Frame(int frameWidth, int frameHeight, Point framePosition, float frameScale = 1f)
        {
            position = framePosition;
            width = frameWidth;
            height = frameHeight;
            scale = frameScale;
        }

    }
}
