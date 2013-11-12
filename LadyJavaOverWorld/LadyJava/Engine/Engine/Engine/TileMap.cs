using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.ObjectModel;

namespace Engine
{
    public class TileMap
    {
        string[] imageExtensions = new string[] { ".jpg", ".png", ".tga" };

        List<TileLayer> tileMap;
        List<Tile> tiles = new List<Tile>();
        List<string> textureNames = new List<String>();

        CollisionLayer collisionLayer;

        public CollisionLayer CollisionLayer
        { get { return collisionLayer; } }

        public List<TileLayer> Layers
        { get { return tileMap; } }

        public List<Tile> Tiles
        { get { return tiles; } }

        public List<string> TextureNames
        { get { return textureNames; } }

        public int TileWidth
        { get { return tileMap[0].TileWidth; } }
        public int TileHeight
        { get { return tileMap[0].TileHeight; } }

        public int PixelWidth
        { get { return tileMap[0].Width * TileWidth; } }
        public int PixelHeight
        { get { return tileMap[0].Height * TileHeight; } }

        public int Width
        { get { return tileMap[0].Width; } }
        public int Height
        { get { return tileMap[0].Height; } }


        TileMap()
        {
            tileMap = new List<TileLayer>();
        }

        public TileMap(string titleMapLocation, ContentManager gameContent) : this()
        {
            Load(titleMapLocation, gameContent);
        }

        public TileMap(string titleMapLocation, GraphicsDevice graphicsDevice) : this()
        {
            Load(titleMapLocation, graphicsDevice);
        }

        public TileMap(int newWidth, int newHeight, int newTileWidth, int newTileHeight)
        {
            tileMap = new List<TileLayer>();
            tileMap.Add(new TileLayer(newWidth, newHeight, newTileWidth, newTileHeight));
            collisionLayer = new CollisionLayer(newWidth, newHeight, newTileWidth, newTileHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileLayer layer in tileMap)
                layer.Draw(spriteBatch, tiles);
        }

        //public Point GetTileDemensions(int layerIndex, int x, int y)
        //{
        //    return new Point(tiles[GetCellIndex(layerIndex, x, y)].Width, tiles[GetCellIndex(layerIndex, x, y)].Height);
        //}

        public void Resize(int newWidth, int newHeight, int newTileWidth, int newTileHeight)
        {
            List<TileLayer> newTileMap = new List<TileLayer>();
            for (int layerIndex = 0; layerIndex < Layers.Count; layerIndex++)
            {
                TileLayer newLayer = new TileLayer(newWidth, newHeight, newTileWidth, newTileHeight);
                for (int y = 0; y < Layers[layerIndex].Width; y++)
                    for (int x = 0; x < Layers[layerIndex].Height; x++)
                        //if(y >= Layers[layerIndex].Height || x >= Layers[layerIndex].Width)
                        //    newLayer.SetCellIndex(x, y, -1);
                        //else
                        newLayer.SetCellIndex(x, y, GetCellIndex(layerIndex, x, y));
                newTileMap.Add(newLayer);
            }
            tileMap = newTileMap;

            CollisionLayer newCollisionLayer = new CollisionLayer(Width, Height, TileWidth, TileHeight);
            for (int y = 0; y < collisionLayer.Width; y++)
                for (int x = 0; x < collisionLayer.Height; x++)
                    newCollisionLayer.SetCellIndex(x, y, collisionLayer.GetCellIndex(x, y));

            collisionLayer = newCollisionLayer;
        }

        public void AddTexture(string newTexturePath, int newWidth, int newHeight, ContentManager gameContent)
        {
            Texture2D newTexture = gameContent.Load<Texture2D>(newTexturePath);
            tiles.Add(new Tile(newTexture, newWidth, newHeight));
            textureNames.Add(newTexturePath);
        }

        public int GetCellIndex(int layerIndex, int x, int y)
        {
            return Layers[layerIndex].GetCellIndex(x, y);
        }

        public void SetCellIndex(int layerIndex, int x, int y, int cellIndex)
        {
            Layers[layerIndex].SetCellIndex(x, y, cellIndex);
        }
        
        public Texture2D AddTexture(string newTexturePath, string newTextureName, int newWidth, int newHeight, GraphicsDevice graphicsDevice)
        {
            Texture2D newTexture;
            foreach (string extension in imageExtensions)
                if (File.Exists(newTexturePath + extension))
                {
                    newTexturePath += extension;
                    break;
                }

            using (FileStream fileStream = new FileStream(newTexturePath, FileMode.Open, FileAccess.Read))
            {
                newTexture = Texture2D.FromStream(graphicsDevice, fileStream);
                fileStream.Close();
            }
            tiles.Add(new Tile(newTexture, newWidth, newHeight));
            textureNames.Add(newTextureName);

            return newTexture;
        }

        public Texture2D AddTexture(string newTexturePath, string newTextureName, GraphicsDevice graphicsDevice)
        {
            Texture2D newTexture;
            foreach (string extension in imageExtensions)
                if (File.Exists(newTexturePath + extension))
                {
                    newTexturePath += extension;
                    break;
                }

            using (FileStream fileStream = new FileStream(newTexturePath, FileMode.Open, FileAccess.Read))
            {
                newTexture = Texture2D.FromStream(graphicsDevice, fileStream);
                fileStream.Close();
            }
            tiles.Add(new Tile(newTexture, TileWidth, TileHeight));
            textureNames.Add(newTextureName);

            return newTexture;
        }

        
        public void RemoveTexture(int removeIndex)
        {
            tiles.RemoveAt(removeIndex);
            textureNames.RemoveAt(removeIndex);

            for(int z = 0; z < Layers.Count; z++)
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                        if (GetCellIndex(z, x, y) == removeIndex)
                            SetCellIndex(z, x, y, -1);
                        else if (GetCellIndex(z, x, y) > removeIndex)
                            SetCellIndex(z, x, y, GetCellIndex(z, x, y) - 1);
        }

        public void AddLayer()
        {
            tileMap.Add(new TileLayer(Width, Height,  TileWidth, TileHeight));
        }

        public void RemoveLayer(int removeIndex)
        {
            tileMap.RemoveAt(removeIndex);
        }

        private void Load(String fileLocation, ContentManager gameContent)
        {
            bool readingDemensions = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;
            bool readingEntrances = false;
            bool readingTileMap = false;
            bool readingCollisionLayer = false;

            List<string> entrances = new List<string>();
            int currentRow = 0;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;

            int[,] tileLayer = new int[height, width];

            try
            {
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });

                    for (int y = 0; y < line.Length; y++)
                    {
                        if (line[y].Trim() == "[TileDemensions]")
                        {
                            readingTileDemensions = true;
                        }
                        else if (line[y].Trim() == "[Textures]")
                        {
                            readingTextures = true;
                        }
                        else if (line[y].Trim() == "[Demensions]")
                        {
                            readingDemensions = true;
                        }
                        else if (line[y].Trim() == "[Entrances]")
                        {
                            readingEntrances = true;
                        }
                        else if (line[y].Trim() == "[TileLayer]")
                        {
                            readingTileMap = true;
                            currentRow = 0;
                            tileLayer = new int[height, width];
                        }
                        else if (line[y].Trim() == "[CollisionLayer]")
                        {
                            readingCollisionLayer = true;
                            currentRow = 0;
                            tileLayer = new int[height, width];
                        }
                        else if (readingTileDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                tileWidth = int.Parse(dimensions[0]);
                                tileHeight = int.Parse(dimensions[1]);
                            }
                            else
                                readingTileDemensions = false;
                        }
                        else if (readingTextures)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] textureInfo = line[y].Split(' ');
                                string textureName = textureInfo[0].Trim();
                                int textureWidth = int.Parse(textureInfo[1].Trim());
                                int textureHeight = int.Parse(textureInfo[2].Trim());

                                AddTexture(textureName, textureWidth, textureHeight, gameContent);
                            }
                            else
                                readingTextures = false;

                        }
                        else if (readingDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                width = int.Parse(dimensions[0]);
                                height = int.Parse(dimensions[1]);
                            }
                            else
                                readingDemensions = false;
                        }
                        else if (readingEntrances)
                        {
                            if (line[y].Trim() != "")
                            {
                                entrances.Add(line[y].Trim());
                            }
                            else
                                readingEntrances = false;
                        }
                        else if (readingTileMap)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    tileLayer[currentRow, i] = int.Parse(tiles[i]);
                                }
                                currentRow++;
                            }
                            else
                            {
                                tileMap.Add(new TileLayer(tileLayer, tileWidth, tileHeight));
                                readingTileMap = false;
                            }
                        }
                        else if (readingCollisionLayer)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    tileLayer[currentRow, i] = int.Parse(tiles[i]);
                                }
                                currentRow++;
                            }
                            else
                            {
                                collisionLayer = new CollisionLayer(tileLayer, tileWidth, tileHeight, entrances);
                                readingCollisionLayer = false;
                            }
                        }
                    }

                    //add the collision layer if there is no blank line at end of file
                    if (readingCollisionLayer)
                    {
                        collisionLayer = new CollisionLayer(tileLayer, tileWidth, tileHeight, entrances);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }


        private void Load(String fileLocation, GraphicsDevice graphicsDevice)
        {
            bool readingDemensions = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;
            bool readingEntrances = false;
            bool readingTileMap = false;
            bool readingCollisionLayer = false;

            List<string> entrances = new List<string>();
            int currentRow = 0;
            //int layersCount = 0;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;

            int[,] tileLayer = new int[height, width]; ;
            //int[,] collisionLayer = new int[width, height]; ;

            try
            {
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });

                    for (int y = 0; y < line.Length; y++)
                    {
                        if (line[y].Trim() == "[TileDemensions]")
                        {
                            readingTileDemensions = true;
                        }
                        else if (line[y].Trim() == "[Textures]")
                        {
                            readingTextures = true;
                        }
                        else if (line[y].Trim() == "[Demensions]")
                        {
                            readingDemensions = true;
                        }
                        else if (line[y].Trim() == "[Entrances]")
                        {
                            readingEntrances = true;
                        }
                        else if (line[y].Trim() == "[TileLayer]")
                        {
                            readingTileMap = true;
                            currentRow = 0;
                            tileLayer = new int[height, width];
                        }
                        else if (line[y].Trim() == "[CollisionLayer]")
                        {
                            readingCollisionLayer = true;
                            currentRow = 0;
                            tileLayer = new int[height, width];
                        }
                        else if (readingTileDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                tileWidth = int.Parse(dimensions[0]);
                                tileHeight = int.Parse(dimensions[1]);
                            }
                            else
                                readingTileDemensions = false;
                        }
                        else if (readingEntrances)
                        {
                            if (line[y].Trim() != "")
                            {
                                entrances.Add(line[y].Trim());
                            }
                            else
                                readingEntrances = false;
                        }
                        else if (readingTextures)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] textureInfo = line[y].Split(' ');
                                string textureName = textureInfo[0].Trim();
                                int textureWidth = int.Parse(textureInfo[1].Trim());
                                int textureHeight = int.Parse(textureInfo[2].Trim());

                                AddTexture(@"..\..\..\LadyJava\LadyJavaContent\" + textureName, textureName, 
                                           textureWidth, textureHeight, 
                                           graphicsDevice);
                            }
                            else
                                readingTextures = false;

                        }
                        else if (readingDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                width = int.Parse(dimensions[0]);
                                height = int.Parse(dimensions[1]);
                            }
                            else
                                readingDemensions = false;
                        }
                        else if (readingTileMap)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    tileLayer[currentRow, i] = int.Parse(tiles[i]);
                                }
                                currentRow++;
                            }
                            else
                            {
                                tileMap.Add(new TileLayer(tileLayer, tileWidth, tileHeight));
                                readingTileMap = false;
                            }
                        }
                        else if (readingCollisionLayer)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    tileLayer[currentRow, i] = int.Parse(tiles[i]);
                                }
                                currentRow++;
                            }
                            else
                            {
                                collisionLayer = new CollisionLayer(tileLayer, tileWidth, tileHeight, entrances);
                                readingCollisionLayer = false;
                            }
                        }
                    }

                    //add the collision layer if there is no blank line at end of file
                    if (readingCollisionLayer)
                    {
                        collisionLayer = new CollisionLayer(tileLayer, tileWidth, tileHeight, entrances);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        public void Save(String fileLocation)
        {
            using (StreamWriter writer = new StreamWriter(fileLocation))
            {
                writer.WriteLine("[Demensions]");
                writer.WriteLine(Width + "x" + Height);
                writer.WriteLine();

                writer.WriteLine("[TileDemensions]");
                writer.WriteLine(TileWidth + "x" + TileHeight);
                writer.WriteLine();

                writer.WriteLine("[Textures]");
                for(int i = 0; i < textureNames.Count; i++)
                    writer.WriteLine(textureNames[i] + " " + tiles[i].Width + " " + +tiles[i].Height);
                writer.WriteLine();

                writer.WriteLine("[Entrances]");
                for (int i = 0; i < collisionLayer.Entrances.Count; i++)
                    writer.WriteLine(collisionLayer.Entrances[i]);
                writer.WriteLine();
                
                foreach (TileLayer layer in Layers)
                {
                    writer.WriteLine("[TileLayer]");
                    for (int y = 0; y < layer.Height; y++)
                    {
                        string line = string.Empty;
                        
                        for (int x = 0; x < layer.Width; x++)
                        {
                            string cell = layer.Layer[y, x].ToString();
                            if (cell.Length < 2)
                                cell = "0" + cell;
                            if(x == 0)
                                line = cell;
                            else
                                line += "|" + cell;
                        }
                        writer.WriteLine(line);
                    }
                    writer.WriteLine();
                }

                writer.WriteLine("[CollisionLayer]");
                for (int y = 0; y < collisionLayer.Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < collisionLayer.Width; x++)
                    {
                        string cell = collisionLayer.GetCellIndex(x, y).ToString();
                        if (cell.Length < 2)
                            cell = "0" + cell;
                        if (x == 0)
                            line = cell;
                        else
                            line += "|" + cell;
                    }
                    writer.WriteLine(line);
                }
            }
        }
    }
}
