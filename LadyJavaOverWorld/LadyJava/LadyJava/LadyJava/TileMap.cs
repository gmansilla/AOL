using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.ObjectModel;

namespace LadyJava
{
    class TileMap
    {
        List<TileLayer> tileMap;
        List<Texture2D> tileTexture = new List<Texture2D>();

        public int TileWidth
        { get { return tileMap[0].TileWidth; } }
        public int TileHeight
        { get { return tileMap[0].TileHeight; } }

        public int Width
        { get { return tileMap[0].Width * TileWidth; } }
        public int Height
        { get { return tileMap[0].Height * TileHeight; } }


        public TileMap(string titleMapLocation, ContentManager gameContent)
        {
            tileMap = new List<TileLayer>();
            LoadTileMap(titleMapLocation, gameContent);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileLayer layer in tileMap)
                layer.Draw(spriteBatch, tileTexture);
        }

        private void LoadTileMap(String fileLocation, ContentManager gameContent)
        {
            bool readingDemensions = false;
            bool readingTileMap = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;

            int currentRow = 0;
            int layersCount = 0;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;

            int[,] tileLayer = new int[width, height];;

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

                                width = int.Parse(dimensions[0]);
                                height = int.Parse(dimensions[1]);
                                layersCount = int.Parse(dimensions[2]);
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
    }
}
