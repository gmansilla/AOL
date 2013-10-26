using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LadyJava
{
    class TileMap
    {
        int[][,] tileMap;
        List<Texture2D> tileTexture = new List<Texture2D>();

        int tileWidth;
        int tileHeight;

        public int TileWidth
        { get { return TileWidth; } }
        public int TileHeight
        { get { return TileHeight; } }

        public int Width 
        { get { return tileMap[0].GetLength(0) * tileWidth; } }
        public int Height
        { get { return tileMap.GetLength(0) * tileHeight; } }


        public TileMap(string titleMapLocation, ContentManager gameContent)
        {
            LoadTileMap(titleMapLocation, gameContent);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int tileMapWidth = tileMap[0].GetLength(0);
            int tileMapHeight = tileMap.GetLength(0);

            for (int y = 0; y < tileMapHeight; y++)
            {
                for (int x = 0; x < tileMapWidth; x++)
                {
                    for (int z = 0; z < tileMap[y].GetLength(1); z++)
                    {
                        int textureIndex = tileMap[y][x, z];
                        if (textureIndex != -1)
                        {
                            //Texture2D texture = tileTexture[textureIndex];
                            spriteBatch.Draw(tileTexture[textureIndex], new Rectangle(
                            x * tileWidth,
                            y * tileHeight, //casting as int (rounding)
                            tileWidth, tileHeight), Color.White);
                        }
                    }
                }
            }

        }

        private void LoadTileMap(String fileLocation, ContentManager gameContent)
        {
            bool readingDemensions = false;
            bool readingTileMap = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;

            int currentRow = 0;
            int tileMapWidth = 0; 
            int tileMapHeigth = 0;
            int layersCount = 0;
            tileWidth = 0;
            tileHeight = 0;


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
                        else if (line[y].Trim() == "[TileMap]")
                        {
                            readingTileMap = true;
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
                                //string texturePath = line[y].Trim();
                                tileTexture.Add(gameContent.Load<Texture2D>(line[y].Trim()));
                            }
                            else
                                readingTextures = false;
         
                        }
                        else if (readingDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                tileMapWidth = int.Parse(dimensions[0]);
                                tileMapHeigth = int.Parse(dimensions[1]);
                                layersCount = int.Parse(dimensions[2]);
                                tileMap = new int[tileMapHeigth][,];
                            }
                            else
                                readingDemensions = false;
                        }
                        else if (readingTileMap)
                        {
                            if (line[y].Trim() != "")
                            {
                                tileMap[currentRow] = new int[tileMapWidth, layersCount];
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    string[] layers = tiles[i].Split(new Char[] { ',' });

                                    for (int j = 0; j < layers.Length; j++)
                                    {
                                        tileMap[currentRow][i, j] = int.Parse(layers[j]);
                                    }
                                }
                                currentRow++;
                            }
                            else
                                readingTileMap = false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
