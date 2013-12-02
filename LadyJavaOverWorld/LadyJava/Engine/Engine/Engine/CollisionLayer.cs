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
        static public int CollisionCell = -2;
        static public int NothingCell = -1;
        static public int StartingCell = -3;

        BoundingBox[] collisionBoxes;
        BoundingBox[,] collisionBoxLayer;
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
            collisionLayer = new int[newHeight, newWidth];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    collisionLayer[y, x] = NothingCell;

            CreateCollisionLayer(tileWidth, tileHeight);
        }

        void CreateCollisionLayer(int tileWidth, int tileHeight)
        {
            int boxCount = 0;
            int entranceCount = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (collisionLayer[y, x] == CollisionCell)
                        boxCount++;
                    else if (collisionLayer[y, x] >= 0)
                        entranceCount++;

            collisionBoxes = new BoundingBox[boxCount];
            entranceBoxes = new BoundingBox[entranceCount];

            collisionBoxLayer = new BoundingBox[Height, Width];

            boxCount = 0;
            entranceCount = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (collisionLayer[y, x] == CollisionCell)
                    {
                        collisionBoxLayer[y, x] = new BoundingBox(new Vector3(x * tileWidth, y * tileHeight, 0f),
                                                                  new Vector3(x * tileWidth + tileWidth, y * tileHeight + tileHeight, 0f));
                        collisionBoxes[boxCount++] = new BoundingBox(new Vector3(x * tileWidth, y * tileHeight, 0f),
                                                                     new Vector3(x * tileWidth + tileWidth, y * tileHeight + tileHeight, 0f));
                    }
                    else if ((collisionLayer[y, x] >= 0))
                    {
                        entranceBoxes[entranceCount++] = new BoundingBox(new Vector3(x * tileWidth, y * tileHeight, 0f),
                                                                          new Vector3(x * tileWidth + tileWidth, y * tileHeight + tileHeight, 0f));
                    }
        }

        public BoundingBox[] GetSurroundingBoundingBoxes(Vector2 playerPosition, int tileWidth, int tileHeight)
        {
            const int YRange = 3;
            const int XRange = 3;
            List<BoundingBox> surrounding = new List<BoundingBox>();

            Point playerCell = new Point((int)(playerPosition.X / tileWidth), (int)(playerPosition.Y / tileHeight));

            for (int y = Math.Max(0, playerCell.Y - 1); y < Math.Min(playerCell.Y - 1 + YRange, Height); y++)
                for (int x = Math.Max(0, playerCell.X - 1); x < Math.Min(playerCell.X - 1 + XRange, Width); x++)
                    if (collisionBoxLayer[y, x] != new BoundingBox(Vector3.Zero, Vector3.Zero))
                        surrounding.Add(collisionBoxLayer[y, x]);
            
            return surrounding.ToArray();
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
            if (!entrances.Contains(fileLocation))
                entrances.Add(fileLocation);
        }

        public void RemoveEntrance(int x, int y)
        {
            SetCellIndex(x, y, NothingCell);
        }

        public void RemoveEntrance(string entranceString)
        {
            if (entrances.Contains(entranceString))
            {
                int index = entrances.IndexOf(entranceString);
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                        if (GetCellIndex(x, y) > index)
                            SetCellIndex(x, y, GetCellIndex(x, y) - 1);
                        else if (GetCellIndex(x, y) == index)
                            SetCellIndex(x, y, NothingCell);
                
                entrances.RemoveAt(index);
            }
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
