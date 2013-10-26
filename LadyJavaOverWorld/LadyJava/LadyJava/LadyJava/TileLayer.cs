using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LadyJava
{
    class TileLayer
    {
        int[,] tileLayer;
        int width;
        int height;
        
        int tileWidth;
        int tileHeight;

        public int Width
        { get { return width; } }
        public int Height
        { get { return height; } }

        public int TileWidth
        { get { return tileWidth; } }
        public int TileHeight
        { get { return tileHeight; } }

        public TileLayer(int[,] newTileLayer, int newTileWidth, int newTileHeight)
        {
            tileLayer = newTileLayer;
            tileWidth = newTileWidth;
            tileHeight = newTileHeight;
            width = tileLayer.GetLength(0);
            height = tileLayer.GetLength(1);
        }

        public void Draw(SpriteBatch spriteBatch, List<Texture2D> tileTexture)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int textureIndex = tileLayer[y, x];
                    if (textureIndex != -1)
                    {
                        spriteBatch.Draw(tileTexture[textureIndex], new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth, tileHeight), Color.White);
                    }
                }
            }
        }
    }
}
