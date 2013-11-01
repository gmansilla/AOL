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
        List<Texture2D> tileTextures = new List<Texture2D>();
        List<string> textureNames = new List<String>();

        public List<TileLayer> Layers
        { get { return tileMap; } }

        public List<Texture2D> TileTextures
        { get { return tileTextures; } }

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


        public TileMap(string titleMapLocation, ContentManager gameContent)
        {
            tileMap = new List<TileLayer>();
            Load(titleMapLocation, gameContent);
        }

        public TileMap(string titleMapLocation, GraphicsDevice graphicsDevice)
        {
            tileMap = new List<TileLayer>();
            Load(titleMapLocation, graphicsDevice);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileLayer layer in tileMap)
                layer.Draw(spriteBatch, tileTextures);
        }

        public void AddTexture(string newTexturePath, ContentManager gameContent)
        {
            tileTextures.Add(gameContent.Load<Texture2D>(newTexturePath));
        }

        public int GetCellIndex(int layerIndex, int x, int y)
        {
            return Layers[layerIndex].GetCellIndex(x, y);
        }

        public void SetCellIndex(int layerIndex, int x, int y, int cellIndex)
        {
            Layers[layerIndex].SetCellIndex(x, y, cellIndex);
        }
        
        public void AddTexture(string newTexturePath, GraphicsDevice graphicsDevice)
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
            tileTextures.Add(newTexture);
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
            bool readingTileMap = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;

            int currentRow = 0;
            //int layersCount = 0;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;

            int[,] tileLayer = new int[width, height]; ;

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
                        else if (line[y].Trim() == "[TileLayer]")
                        {
                            readingTileMap = true;
                            currentRow = 0;
                            tileLayer = new int[width, height];
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
                                AddTexture(line[y].Trim(), gameContent);
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
                                //layersCount = int.Parse(dimensions[2]);
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
                    }

                    //add final layer if there is no blank line at end of file
                    if (readingTileMap)
                    {
                        tileMap.Add(new TileLayer(tileLayer, tileWidth, tileHeight));
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
            bool readingTileMap = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;

            int currentRow = 0;
            //int layersCount = 0;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;

            int[,] tileLayer = new int[width, height]; ;

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
                        else if (line[y].Trim() == "[TileLayer]")
                        {
                            readingTileMap = true;
                            currentRow = 0;
                            tileLayer = new int[width, height];
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
                                string textureName = line[y].Trim();
                                textureNames.Add(textureName);

                                AddTexture(@"..\..\..\LadyJava\LadyJavaContent\" + textureName, graphicsDevice);
                                /*
                                string textureFile = @"..\..\..\LadyJava\LadyJavaContent\" + textureName;
                                foreach (string extension in imageExtensions)
                                    if (File.Exists(textureFile + extension))
                                    {
                                        textureFile += extension;
                                        break;
                                    }

                                using (FileStream fileStream = new FileStream(textureFile, FileMode.Open))
                                {
                                    tileTextures.Add(Texture2D.FromStream(graphicsDevice, fileStream));
                                }
                                */ 
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
                                //layersCount = int.Parse(dimensions[2]);
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
                    }

                    //add final layer if there is no blank line at end of file
                    if (readingTileMap)
                    {
                        tileMap.Add(new TileLayer(tileLayer, tileWidth, tileHeight));
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
                foreach (string textureName in textureNames)
                    writer.WriteLine(textureName);
                writer.WriteLine();

                foreach (TileLayer layer in Layers)
                {
                    writer.WriteLine("[TileLayer]");
                    for (int y = 0; y < layer.Height; y++)
                    {
                        string line = string.Empty;
                        
                        for (int x = 0; x < layer.Height; x++)
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
            }
        }
    }
}
