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
        const string ContentPath = @"..\..\..\LadyJava\LadyJavaContent\";
        
        Vector2 startingPosition;
        Vector2 lastPosition;

        List<TileLayer> tileMap;
        List<Tile> tiles = new List<Tile>();
        List<string> textureNames = new List<String>();

        CollisionLayer collisionLayer;

        List<Npc> npcs;
        BoundingBox[] npcBounds;
        BoundingSphere[] npcTalkRadii;

        public List<Npc> NPCs
        { get { return npcs; } }

        public BoundingBox[] NPCsToBoundingBox
        { get { return npcBounds; } }
        public BoundingSphere[] NPCTalkRadii
        { get { return npcTalkRadii; } }

        public Vector2 StartingPosition
        { get { return startingPosition; } }

        public Vector2 LastPosition
        { get { return lastPosition; } }

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

        public TileMap(string titleMapLocation, ContentManager gameContent, 
                       int screenWidth, int screenHeight, SpriteFont newText) : this()
        {
            Load(titleMapLocation, gameContent, null, screenWidth, screenHeight, newText);
            startingPosition = GetStartingPosition();
            collectBounds();
        }

        public TileMap(string titleMapLocation, GraphicsDevice graphicsDevice, 
                       int screenWidth, int screenHeight, SpriteFont newText) : this()
        {
            Load(titleMapLocation, null, graphicsDevice, screenWidth, screenHeight, newText);
            //Load(titleMapLocation, graphicsDevice);
            startingPosition = GetStartingPosition();
            collectBounds();
        }

        public TileMap(int newWidth, int newHeight, int newTileWidth, int newTileHeight) : this()
        {
            tileMap.Add(new TileLayer(newWidth, newHeight, newTileWidth, newTileHeight));
            collisionLayer = new CollisionLayer(newWidth, newHeight, newTileWidth, newTileHeight);

            startingPosition = GetStartingPosition();
            collectBounds();
        }

        void collectBounds()
        {
            if (npcs.Count > 0)
            {
                npcBounds = new BoundingBox[npcs.Count];
                npcTalkRadii = new BoundingSphere[npcs.Count];

                for (int i = 0; i < npcs.Count; i++)
                {
                    npcBounds[i] = npcs[i].ToBoundingBox;
                    npcTalkRadii[i] = npcs[i].TalkRadius;
                }
            }
        }

        public int NPCUpdate(GameTime gameTime, 
                             Camera playerCamera, Global.PlayStates playerPlayState, int playerTalkingTo,
                             int screenWidth, int screenHeight, Global.StoryStates currentStoryState)
        {
            
            for (int i = 0; i < npcs.Count; i++)
            {
                if (!npcs[i].MessageBoxVisible && playerTalkingTo == i)
                    npcs[i].ShowMessageBox();
                else
                    if (playerPlayState == Global.PlayStates.Playing)
                    {
                        npcs[i].HideMessageBox();
                        //ladyJ.EndConversation();
                        playerTalkingTo = Global.InvalidInt;
                    }

                npcs[i].Update(playerCamera, screenWidth, screenHeight, currentStoryState);
            }

            return playerTalkingTo;
        }

        Vector2 GetStartingPosition()
        {
            lastPosition = Global.InvalidVector2;

            for (int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    if (collisionLayer.GetCellIndex(x, y) == CollisionLayer.StartingCell)
                        return new Vector2(x * TileWidth, y * TileHeight);

            return Vector2.Zero;
        }

        public void SetLastPosition(Vector2 newLastPosition)
        {
            lastPosition = newLastPosition;
        }

        public void Resize(int newWidth, int newHeight, int newTileWidth, int newTileHeight)
        {
            List<TileLayer> newTileMap = new List<TileLayer>();
            for (int layerIndex = 0; layerIndex < Layers.Count; layerIndex++)
            {
                TileLayer newLayer = new TileLayer(newWidth, newHeight, newTileWidth, newTileHeight);
                for (int y = 0; y < Math.Min(Layers[layerIndex].Height, newLayer.Height); y++)
                    for (int x = 0; x < Math.Min(Layers[layerIndex].Width, newLayer.Width); x++)
                        newLayer.SetCellIndex(x, y, GetCellIndex(layerIndex, x, y));

                newTileMap.Add(newLayer);
            }
            tileMap = newTileMap;

            CollisionLayer newCollisionLayer = new CollisionLayer(Width, Height, TileWidth, TileHeight);
            for (int y = 0; y < Math.Min(collisionLayer.Height, newCollisionLayer.Height); y++)
                for (int x = 0; x < Math.Min(collisionLayer.Width, newCollisionLayer.Width); x++)
                    newCollisionLayer.SetCellIndex(x, y, collisionLayer.GetCellIndex(x, y));

            collisionLayer = newCollisionLayer;
        }

        public int GetCellIndex(int layerIndex, int x, int y)
        {
            return Layers[layerIndex].GetCellIndex(x, y);
        }

        public void SetCellIndex(int layerIndex, int x, int y, int cellIndex)
        {
            Layers[layerIndex].SetCellIndex(x, y, cellIndex);
        }

        public void AddTexture(string newTexturePath, int newWidth, int newHeight, ContentManager gameContent)
        {
            Texture2D newTexture = gameContent.Load<Texture2D>(newTexturePath);
            tiles.Add(new Tile(newTexture, newWidth, newHeight));
            textureNames.Add(newTexturePath);
        }

        public Texture2D AddTexture(string newTexturePath, string newTextureName, int newWidth, int newHeight, GraphicsDevice graphicsDevice)
        {
            Texture2D newTexture = Global.LoadTexture(newTexturePath, graphicsDevice);
            /*
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
            */
            tiles.Add(new Tile(newTexture, newWidth, newHeight));
            textureNames.Add(newTextureName);

            return newTexture;
        }

        public Texture2D AddTexture(string newTexturePath, string newTextureName, GraphicsDevice graphicsDevice)
        {
            Texture2D newTexture = Global.LoadTexture(newTexturePath, graphicsDevice);
            /*
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
            */
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

        private void Load(String fileLocation, ContentManager gameContent, GraphicsDevice graphicsDevice,
                          int screenWidth, int screenHeight, SpriteFont newText)
        {
            bool readingDemensions = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;
            bool readingEntrances = false;
            bool readingTileMap = false;
            bool readingCollisionLayer = false;
            bool readingNPCs = false;
            bool readingNPCLayer = false;

            List<string> entrances = new List<string>();
            
            npcs = new List<Npc>();
            npcBounds = new BoundingBox[0];
            npcTalkRadii = new BoundingSphere[0];

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
                        else if (line[y].Trim() == "[NPCs]")
                        {
                            readingNPCs = true;
                            currentRow = 0;
                        }
                        else if (line[y].Trim() == "[NPCLayer]")
                        {
                            readingNPCLayer = true;
                            currentRow = 0;
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

                                if(graphicsDevice == null)
                                    AddTexture(textureName, textureWidth, textureHeight, gameContent);
                                else
                                    AddTexture(ContentPath + textureName, textureName,
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
                        else if (readingEntrances)
                        {
                            if (line[y].Trim() != "")
                            {
                                entrances.Add(line[y].Trim());
                            }
                            else
                                readingEntrances = false;
                        }
                        else if (readingNPCs)
                        {
                            if (line[y].Trim() != "")
                            {
                                const int NPCName = 0;
                                const int NPCTilePosition = 1;
                                const int NPCWidth = 2;
                                const int NPCHeight = 3;
                                
                                string[] npcVariables = line[y].Trim().Split(new Char[] { ' ' });

                                if(graphicsDevice == null)
                                    npcs.Add(new Npc(npcVariables[NPCName], 
                                                     Vector2.Zero,
                                                     npcVariables[NPCTilePosition],
                                                     int.Parse(npcVariables[NPCWidth]),
                                                     int.Parse(npcVariables[NPCHeight]), 
                                                     gameContent, 
                                                     tileWidth,
                                                     screenWidth, 
                                                     screenHeight, 
                                                     newText));
                                else
                                    npcs.Add(new Npc(npcVariables[NPCName],
                                                     Vector2.Zero,
                                                     npcVariables[NPCTilePosition],
                                                     int.Parse(npcVariables[NPCWidth]),
                                                     int.Parse(npcVariables[NPCHeight]),
                                                     ContentPath,
                                                     graphicsDevice,
                                                     tileWidth,
                                                     screenWidth,
                                                     screenHeight,
                                                     newText));

                            }
                            else
                                readingNPCs = false;
                        }
                        else if (readingNPCLayer)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    int npc = int.Parse(tiles[i]);
                                    if(npc > Global.InvalidInt)
                                        npcs[npc].SetPosition(new Vector2(i * tileWidth, currentRow * tileHeight), tileWidth);
                                }
                                currentRow++;
                            }
                            else
                            {
                                readingNPCLayer = false;
                            }
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

        /*
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
        */
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

                writer.WriteLine("[NPCs]");
                for (int i = 0; i < npcs.Count; i++)
                    writer.WriteLine(npcs[i].Name + " " + 
                                     npcs[i].TileAlignment + " " + 
                                     npcs[i].Width + " " + 
                                     npcs[i].Height);
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
                writer.WriteLine();

                writer.WriteLine("[NPCLayer]");
                for (int y = 0; y < collisionLayer.Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < collisionLayer.Width; x++)
                    {
                        string cell = "-1";
                        for (int i = 0; i < npcs.Count; i++)
                        {
                            Vector2 npcCellPos = npcs[i].GetCellPosition(TileWidth, TileHeight);
                            if (npcCellPos.X == x && npcCellPos.Y == y)
                                if(i <= 9 && i >= 0)
                                    cell = "0" + i.ToString();
                        }

                        if (x == 0)
                            line = cell;
                        else
                            line += "|" + cell;
                    }
                    writer.WriteLine(line);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileLayer layer in tileMap)
                layer.Draw(spriteBatch, tiles);
        }
    }
}
