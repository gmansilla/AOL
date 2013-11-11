using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class CollisionLayer
    {
        BoundingBox[] collisionBoxes;

        int[,] collisionLayer;


        public BoundingBox[] ToBoundingBox
        { get { return collisionBoxes; } }

        public int Width
        { get { return collisionLayer.GetLength(1); } }
        public int Height
        { get { return collisionLayer.GetLength(0); } }

        public CollisionLayer(int[,] newCollisionLayer, int tileWidth, int tileHeight)
        {
            collisionLayer = newCollisionLayer;
            CreateCollisionLayer(tileWidth, tileHeight);
        }

        public CollisionLayer(int newWidth, int newHeight, int tileWidth, int tileHeight)
        {
            collisionLayer = new int[newWidth, newHeight];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    collisionLayer[y, x] = 0;

            CreateCollisionLayer(tileWidth, tileHeight);
        }

        void CreateCollisionLayer(int tileWidth, int tileHeight)
        {
            int boxCount = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (collisionLayer[y, x] == -1)
                        boxCount++;

            collisionBoxes = new BoundingBox[boxCount];
            boxCount = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (collisionLayer[y, x] == -1)
                    {

                        collisionBoxes[boxCount++] = new BoundingBox(new Vector3(x * tileWidth, y * tileHeight, 0f),
                                                                     new Vector3(x * tileWidth + tileWidth, y * tileHeight + tileHeight, 0f));
                    }
        }

        public int GetCellIndex(int x, int y)
        {
            return collisionLayer[y, x];
        }

        public void SetCellIndex(int x, int y, int newIndex)
        {
            collisionLayer[y, x] = newIndex;
        }


    }
}
