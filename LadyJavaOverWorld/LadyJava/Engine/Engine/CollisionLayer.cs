using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class CollisionLayer
    {
        int[,] collisionLayer;

        public int Width
        { get { return collisionLayer.GetLength(1); } }
        public int Height
        { get { return collisionLayer.GetLength(0); } }

        public CollisionLayer(int[,] newCollisionLayer)
        {
            collisionLayer = newCollisionLayer;
        }

        public CollisionLayer(int newWidth, int newHeight)
        {
            collisionLayer = new int[newWidth, newHeight];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    collisionLayer[y, x] = 0;

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
