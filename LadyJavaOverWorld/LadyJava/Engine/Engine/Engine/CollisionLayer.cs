using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class CollisionLayer
    {
        BoundingBox[] collisionBoxes;
        BoundingBox[] entranceBoxes;

        int[,] collisionLayer;
        
        List<string> entrances;

        public List<string> Entrances
        { get { return entrances; } }

        public BoundingBox[] ToCollisionBox
        { get { return collisionBoxes; } }

        public BoundingBox[] ToEntranceBox
        { get { return entranceBoxes; } }

        public int Width
        { get { return collisionLayer.GetLength(1); } }
        public int Height
        { get { return collisionLayer.GetLength(0); } }

        public CollisionLayer(int[,] newCollisionLayer, int tileWidth, int tileHeight, List<string> newEntrances)
        {
            entrances = newEntrances;
            collisionLayer = newCollisionLayer;
            CreateCollisionLayer(tileWidth, tileHeight);
        }

        public CollisionLayer(int newWidth, int newHeight, int tileWidth, int tileHeight)
        {
            entrances = new List<string>();
            collisionLayer = new int[newWidth, newHeight];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    collisionLayer[y, x] = 0;

            CreateCollisionLayer(tileWidth, tileHeight);
        }

        void CreateCollisionLayer(int tileWidth, int tileHeight)
        {
            int boxCount = 0;
            int entranceCount = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (collisionLayer[y, x] == -1)
                        boxCount++;
                    else if (collisionLayer[y, x] > 0)
                        entranceCount++;

            collisionBoxes = new BoundingBox[boxCount];
            entranceBoxes = new BoundingBox[entranceCount];

            boxCount = 0;
            entranceCount = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (collisionLayer[y, x] == -1)
                    {

                        collisionBoxes[boxCount++] = new BoundingBox(new Vector3(x * tileWidth, y * tileHeight, 0f),
                                                                     new Vector3(x * tileWidth + tileWidth, y * tileHeight + tileHeight, 0f));
                    }
                    else if ((collisionLayer[y, x] > 0))
                    {
                        collisionBoxes[entranceCount++] = new BoundingBox(new Vector3(x * tileWidth, y * tileHeight, 0f),
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

        public void AddEntrance(string fileLocation, int x, int y, int index)
        {
            SetCellIndex(x, y, index);
            entrances.Add(fileLocation);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            for (int i = 0; i < collisionBoxes.Length; i++)
                spriteBatch.Draw(texture,
                                 new Rectangle((int)collisionBoxes[i].Min.X,
                                               (int)collisionBoxes[i].Min.Y,
                                               (int)collisionBoxes[i].Max.X - (int)collisionBoxes[i].Min.X,
                                               (int)collisionBoxes[i].Max.Y - (int)collisionBoxes[i].Min.Y),
                                 Color.Red);

            for (int i = 0; i < entranceBoxes.Length; i++)
                spriteBatch.Draw(texture,
                                 new Rectangle((int)entranceBoxes[i].Min.X,
                                               (int)entranceBoxes[i].Min.Y,
                                               (int)entranceBoxes[i].Max.X - (int)entranceBoxes[i].Min.X,
                                               (int)entranceBoxes[i].Max.Y - (int)entranceBoxes[i].Min.Y),
                                 Color.Green);

        }
    }
}
