using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class Tile
    {
        Texture2D image;
        int width;
        int height;

        public int Width
        { get { return width; } }
        public int Height
        { get { return height; } }

        public Texture2D Image
        { get { return image; } }

        public Tile(Texture2D newImage, int newWidth, int newHeight)
        {
            image = newImage;
            width = newWidth;
            height = newHeight;
        }

        public void Resize(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
        }
    }
}
