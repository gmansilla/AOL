﻿using System;
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
    public enum AreaType
    {
        Dungeon,
        OverWorld
    }

    public struct TileMapUpdateInfo
    {
        public Vector2 PlayerPostion;
        public Vector2 LastPosition;
    }
    
    public class TileMap
    {
        const string ContentPath = @"..\..\..\LadyJava\LadyJavaContent\";
        const int BossAreaMidPoint = -2;

        string name;

        Vector2 startingPosition;
        Vector2 lastPosition;

        AreaType areaType;

        List<TileLayer> tileMap;
        List<Tile> tiles = new List<Tile>();
        List<string> textureNames = new List<String>();

        CollisionLayer collisionLayer;

        List<Npc> npcs;
        List<Npc> activeNPCs;
        BoundingBox[] npcBounds;
        BoundingBox[] npcTalkRadii;
        int finalNPCIndex;
        bool allSaved;

        List<Enemy> enemies;
        List<BoundingBox> enemyBounds;
        public BoundingBox[] EnemiesToBoundingBox
        { get { return enemyBounds.ToArray(); } }

        Boss boss;

        public bool BossIsAlive
        { get { if (areaType == AreaType.Dungeon)
                    return boss.IsAlive;
                else
                    return false; } }

        public BoundingBox BossAreaTrigger
        { get { if (areaType == AreaType.Dungeon && boss.IsAlive)
                    return boss.FightAreaTrigger;
                else
                    return Global.InvalidBoundingBox; } }

        public Rectangle BossArea
        { get { if (areaType == AreaType.Dungeon && boss.IsAlive)
                    return boss.FightArea;
                else
                    return new Rectangle(); } }

        bool playerHit;
        public bool PlayerHit
        { get { return playerHit; } }

        public List<string> Entrances
        { get { return collisionLayer.Entrances; } }

        List<BoundingBox> activeEntrances;
        public BoundingBox[] ToEntranceBox
        { get { return activeEntrances.ToArray(); } }

        public int FinalNPCIndex
        { get { return finalNPCIndex; } }

        public AreaType CurrentAreaType
        { get { return areaType; } }

        public List<Npc> NPCs
        { get { return npcs; } }

        public BoundingBox[] NPCsToBoundingBox
        { get { return npcBounds; } }
        public BoundingBox[] NPCTalkRadii
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

        //loading a tilemap for the game
        public TileMap(string tileMapPath, string tileMapName, ContentManager gameContent, 
                       int screenWidth, int screenHeight, SpriteFont newText) : this()
        {
            name = tileMapName;

            Load(tileMapPath + name, gameContent, null, screenWidth, screenHeight, newText);
            startingPosition = GetStartingPosition();

            activeNPCs = new List<Npc>();
            collectNPCBounds();
            collectEnemyBounds();

            //it will be set after the the final dialog message has been processed
            finalNPCIndex = Global.InvalidInt;
            allSaved = false;

            processActiveEntrances(false);
        }

        //loading a tilemap for the editor
        public TileMap(string tileMapLocation, GraphicsDevice graphicsDevice, 
                       int screenWidth, int screenHeight, SpriteFont newText) : this()
        {
            Load(tileMapLocation, null, graphicsDevice, screenWidth, screenHeight, newText);
            startingPosition = GetStartingPosition();
        }

        //creating a blank tilemap for the editor
        public TileMap(int newWidth, int newHeight, int newTileWidth, int newTileHeight) : this()
        {
            tileMap.Add(new TileLayer(newWidth, newHeight, newTileWidth, newTileHeight));
            collisionLayer = new CollisionLayer(newWidth, newHeight, newTileWidth, newTileHeight);

            startingPosition = GetStartingPosition();
        }

        void collectNPCBounds()
        {
            if (activeNPCs.Count >= 0)
            {
                npcBounds = new BoundingBox[activeNPCs.Count];
                npcTalkRadii = new BoundingBox[activeNPCs.Count];

                for (int i = 0; i < activeNPCs.Count; i++)
                {
                    npcBounds[i] = activeNPCs[i].ToBoundingBox;
                    npcTalkRadii[i] = activeNPCs[i].TalkRadius;
                }
            }
        }

        void collectEnemyBounds()
        {
            if (enemies.Count >= 0)
            {
                enemyBounds = new List<BoundingBox>();

                for (int i = 0; i < enemies.Count; i++)
                    if (enemies[i].IsAlive)
                        enemyBounds.Add(enemies[i].ToBoundingBox);
                //remove boss != null when all areas have a boss
                if(boss != null && areaType == AreaType.Dungeon && boss.IsAlive)
                    enemyBounds.Add(boss.ToBoundingBox);
            }
        }


        public BoundingBox[] GetSurroundingBoundingBoxes(Vector2 playerPosition)
        {
            return collisionLayer.GetSurroundingBoundingBoxes(playerPosition, TileWidth, TileHeight);
        }

        void processActiveEntrances(bool openEndOfTheGame)
        {
            activeEntrances = new List<BoundingBox>();
            foreach (KeyValuePair<string, BoundingBox> entranceBox in collisionLayer.ToEntranceBox)
            {
                
                if (entranceBox.Key == Global.EndOfTheGame)
                {
                    if (openEndOfTheGame)
                        activeEntrances.Add(entranceBox.Value);
                }
                else if (entranceBox.Key == Global.FinalDungeon)
                {
                    if (allSaved)
                        activeEntrances.Add(entranceBox.Value);
                }
                else
                    activeEntrances.Add(entranceBox.Value);
            }
        }

        public string Update(GameTime gameTime,
                             Vector2 entrancePixelLocation, 
                             Vector2 PreviousPlayerPosition)
                             //string previousArea, string currentArea)
        {
            SetLastPosition(PreviousPlayerPosition);
            Vector2 entranceLocation = new Vector2(entrancePixelLocation.X / TileWidth,
                                                    entrancePixelLocation.Y / TileHeight);

            int newAreaIndex = CollisionLayer.GetCellIndex((int)entranceLocation.X, (int)entranceLocation.Y);
            string newArea = CollisionLayer.Entrances[newAreaIndex];
                
            return newArea;
        }

        public void UpdateRescueList
            (Dictionary<string, RescueInfo> toBeRescued)
        {
            activeNPCs = new List<Npc>();

            for (int i = 0; i < npcs.Count; i++)
                if (name == Global.MainArea)
                {
                    if (toBeRescued.ContainsKey(npcs[i].Name))
                    {
                        if (toBeRescued[npcs[i].Name].IsRescued)
                            activeNPCs.Add(npcs[i]);
                    }
                    else
                        activeNPCs.Add(npcs[i]);
                }
                else //in dungeon area
                    if (toBeRescued.ContainsKey(npcs[i].Name))
                    {
                        if (!toBeRescued[npcs[i].Name].IsRescued)
                            activeNPCs.Add(npcs[i]);
                    }
                    else
                        activeNPCs.Add(npcs[i]);

            collectNPCBounds();

            if (!allSaved)
            {
                foreach (string name in Global.ToBeRecused)
                {
                    if (name != Global.ToBeRecused[Global.TheScrumMaster])
                    {
                        if (toBeRescued[name].IsRescued)
                            allSaved = true;
                        else
                        {
                            allSaved = false;
                            break;
                        }
                    }
                }
                if (allSaved)
                    processActiveEntrances(false);
            }
        }


        public int[] NPCUpdate(GameTime gameTime, Dictionary<string, RescueInfo> toBeRescued,
                               Camera playerCamera, Vector2 playerPosition, bool inBossFight, BoundingBox playerBounds,
                               PlayState playerPlayState, int[] playerInteractingWith, bool endGame,
                               int screenWidth, int screenHeight)
        {
            
            for (int i = 0; i < activeNPCs.Count; i++)
            {
                for (int h = 0; h < playerInteractingWith.Length; h++)
                {
                    if (!activeNPCs[i].MessageBoxVisible && playerInteractingWith[h] == i)
                        activeNPCs[i].ShowMessageBox();
                    else if (activeNPCs[i].MessageBoxVisible)
                        if (playerPlayState == PlayState.Playing)
                        {
                            activeNPCs[i].HideMessageBox();
                            playerInteractingWith[h] = Global.InvalidInt;
                            activeNPCs[i].ChangeMessage(playerCamera.Position);
                        }

                    activeNPCs[i].Update(playerCamera, screenWidth, screenHeight, toBeRescued);
                    //set final npc index if the dialog has been processed
                    if (activeNPCs[i].FinalDialogProcessed)
                        finalNPCIndex = i;
                }
            }

            if (areaType == AreaType.Dungeon)
            {
                playerHit = false;
                for (int i = 0; i < playerInteractingWith.Length; i++)
                    if (playerInteractingWith[i] > enemies.Count - 1)
                    {
                        boss.WasHit();
                        playerInteractingWith[i] = Global.InvalidInt;
                    }
                    else
                    {
                        enemies[i].WasHit();
                        playerInteractingWith[i] = Global.InvalidInt;
                    }

                for (int i = 0; i < enemies.Count; i++)
                    if (enemies[i].IsAlive)
                    {
                        enemies[i].Update(gameTime, playerPosition, playerBounds, screenWidth, screenHeight, inBossFight);
                        if (enemies[i].Status != EnemyStatus.Hurt && enemies[i].PlayerHit)
                            playerHit = true;
                    }
                if (boss.IsAlive)
                {
                    boss.Update(gameTime, playerPosition, playerBounds, screenWidth, screenHeight, inBossFight);
                    if (boss.Status != EnemyStatus.Hurt && boss.PlayerHit)
                        playerHit = true;
                }
                collectEnemyBounds();
            }

            if (endGame)
                processActiveEntrances(endGame);

            return playerInteractingWith;
        }

        Vector2 GetStartingPosition()
        {
            lastPosition = Global.InvalidVector2;

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
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
            tiles.Add(new Tile(newTexture, newWidth, newHeight));
            textureNames.Add(newTextureName);

            return newTexture;
        }

        public Texture2D AddTexture(string newTexturePath, string newTextureName, GraphicsDevice graphicsDevice)
        {
            Texture2D newTexture = Global.LoadTexture(newTexturePath, graphicsDevice);
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
            bool readingType = false;
            bool readingDemensions = false;
            bool readingTileDemensions = false;
            bool readingTextures = false;
            bool readingEntrances = false;
            bool readingTileMap = false;
            bool readingCollisionLayer = false;
            bool readingNPCs = false;
            bool readingNPCLayer = false;
            bool readingEnemies = false;
            bool readingBoss = false;
            bool readingEnemyLayer = false;

            enemies = new List<Enemy>();
            String bossType = "";
            List<AnimationInfo> bossAnimations = new List<AnimationInfo>();

            List<string> entrances = new List<string>();
            
            npcs = new List<Npc>();
            npcBounds = new BoundingBox[0];
            npcTalkRadii = new BoundingBox[0];

            int currentRow = 0;
            int width = 0;
            int height = 0;
            int tileWidth = 0;
            int tileHeight = 0;

            int[,] tileLayer = new int[height, width];

            //try
            //{
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });

                    for (int y = 0; y < line.Length; y++)
                    {
                        if (line[y].Trim() == "[Type]")
                        {
                            readingType = true;
                        }
                        else if (line[y].Trim() == "[Demensions]")
                        {
                            readingType = false;
                            readingDemensions = true;
                        }
                        else if (line[y].Trim() == "[TileDemensions]")
                        {
                            readingDemensions = false;
                            readingTileDemensions = true;
                        }
                        else if (line[y].Trim() == "[Textures]")
                        {
                            readingTileDemensions = false;
                            readingTextures = true;
                        }
                        else if (line[y].Trim() == "[Entrances]")
                        {
                            readingTextures = false;
                            readingEntrances = true;
                        }
                        else if (line[y].Trim() == "[NPCs]")
                        {
                            readingEntrances = false;
                            readingNPCs = true;
                        }
                        else if (line[y].Trim() == "[Enemies]")
                        {
                            readingEntrances = false;
                            readingNPCs = false;
                            
                            readingEnemies = true;
                            currentRow = 0;
                        }
                        else if (line[y].Trim() == "[TileLayer]")
                        {
                            readingEntrances = false;
                            readingNPCs = false;
                            readingEnemies = false;
                            readingBoss = false;

                            readingTileMap = true;
                            currentRow = 0;
                            tileLayer = new int[height, width];
                        }
                        else if (line[y].Trim() == "[CollisionLayer]")
                        {
                            readingTileMap = false;
                            readingCollisionLayer = true;
                            currentRow = 0;
                            tileLayer = new int[height, width];
                        }
                        else if (line[y].Trim() == "[NPCLayer]")
                        {
                            readingCollisionLayer = false;
                            readingNPCLayer = true;
                            currentRow = 0;
                        }
                        else if (line[y].Trim() == "[EnemyLayer]")
                        {
                            readingCollisionLayer = false;
                            readingNPCLayer = false;
                            
                            readingEnemyLayer = true;
                            currentRow = 0;
                        }
                        else if (readingType)
                        {
                            if (line[y].Trim() != "")
                            {
                                areaType = (AreaType)Enum.Parse(typeof(AreaType), line[y].Trim());
                            }
                        }
                        else if (readingDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                width = int.Parse(dimensions[0]);
                                height = int.Parse(dimensions[1]);
                            }
                        }
                        else if (readingTileDemensions)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                                tileWidth = int.Parse(dimensions[0]);
                                tileHeight = int.Parse(dimensions[1]);
                            }
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

                        }
                        else if (readingEntrances)
                        {
                            if (line[y].Trim() != "")
                                entrances.Add(line[y].Trim());
                        }
                        else if (readingNPCs)
                        {
                            ReadNPC(gameContent, graphicsDevice, screenWidth, screenHeight, newText, tileWidth, line[y]);
                        }
                        else if (readingEnemies)
                        {
                            if(line[y].Trim() == "<Boss>")
                            {
                                currentRow = 0;
                                readingBoss = true;
                            }
                            else if (readingBoss)
                            {
                                if(line[y].Trim() != "")
                                {
                                    const int AnimationName = 0;
                                    const int FrameWidth = 1;
                                    const int FrameHeight = 2;
                                    const int FramesCount = 3;
                                    const int AniiamtionSpeed = 4;
                                    const int NextAnimation = 5;

                                    currentRow++;
                                    if (currentRow == 1)
                                        bossType = line[y].Trim();
                                    else if (currentRow > 1)
                                    {
                                        string[] animationInfo = line[y].Trim().Split(' ');
                                        bossAnimations.Add(
                                            new AnimationInfo(
                                                animationInfo[AnimationName],
                                                int.Parse(animationInfo[FrameWidth]), int.Parse(animationInfo[FrameHeight]),
                                                int.Parse(animationInfo[FramesCount]), int.Parse(animationInfo[AniiamtionSpeed]),
                                                animationInfo[NextAnimation]));
                                    }
                                }
                                else if(readingBoss) 
                                {
                                    Texture2D bossImage;
                                    if (graphicsDevice != null)
                                        bossImage = Global.LoadTexture(ContentPath + "Bosses\\" + bossType, graphicsDevice);
                                    else
                                        bossImage = gameContent.Load<Texture2D>("Bosses\\" + bossType);

                                    if(bossType == Global.BabyMetroid)
                                        boss = new BabyMetroid(bossImage, bossAnimations.ToArray());
                                    else
                                        boss = new Thwomp(bossImage, bossAnimations.ToArray());

                                }
                            }
                            else if (line[y].Trim() != "")
                            {
                                //read normal enemies
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
                                tileMap.Add(new TileLayer(tileLayer, tileWidth, tileHeight));
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
                                collisionLayer = new CollisionLayer(tileLayer, tileWidth, tileHeight, entrances);
                        }
                        else if (readingNPCLayer)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    int npc = int.Parse(tiles[i]);
                                    if (npc > Global.InvalidInt)
                                        npcs[npc].SetPosition(new Vector2(i * tileWidth, currentRow * tileHeight), tileWidth);
                                }
                                currentRow++;
                            }
                        }
                        else if (readingEnemyLayer)
                        {
                            if (line[y].Trim() != "")
                            {
                                string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                                for (int i = 0; i < tiles.Length; i++)
                                {
                                    int enemy = int.Parse(tiles[i]);
                                    if (enemy > Global.InvalidInt)
                                    {
                                        if (enemy > enemies.Count - 1)
                                            boss.SetPosition(new Vector2(i * tileWidth, currentRow * tileHeight));
                                        else
                                            enemies[enemy].SetPosition(new Vector2(i * tileWidth, currentRow * tileHeight));
                                    }
                                    else if (enemy == TileMap.BossAreaMidPoint)
                                        boss.SetFightArea(new Point(i * tileWidth, currentRow * tileHeight), 
                                                          screenWidth, screenHeight, tileWidth, tileHeight);
                                }
                                currentRow++;
                            }
                        }
                    }

                    //add the collision layer if there is no blank line at end of file
                    //if (readingCollisionLayer)
                    //    collisionLayer = new CollisionLayer(tileLayer, tileWidth, tileHeight, entrances);
                }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("The file could not be read:");
            //    Console.WriteLine(e.Message);
            //}
        }

        private void ReadNPC(ContentManager gameContent, GraphicsDevice graphicsDevice, int screenWidth, int screenHeight, SpriteFont newText, int tileWidth, string line)
        {
            if (line.Trim() != "")
            {
                const int NPCName = 0;
                const int NPCTilePosition = 1;
                const int NPCWidth = 2;
                const int NPCHeight = 3;

                string[] npcVariables = line.Trim().Split(new Char[] { ' ' });

                if (graphicsDevice == null)
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
        }

        public void Save(String fileLocation)
        {
            using (StreamWriter writer = new StreamWriter(fileLocation))
            {
                writer.WriteLine("[Type]");
                writer.WriteLine(areaType.ToString());
                writer.WriteLine();

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

        public void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            for (int i = 0; i < tileMap.Count; i++)
                if (name == Global.MainArea)
                {
                    if (!allSaved && i < tileMap.Count - 1)
                        tileMap[i].Draw(spriteBatch, tiles, transparency);
                    else if (allSaved)
                        tileMap[i].Draw(spriteBatch, tiles, transparency);
                }
                else
                {
                    if (areaType == AreaType.Dungeon && i == tileMap.Count - 1)
                        boss.Draw(spriteBatch, transparency);
                    tileMap[i].Draw(spriteBatch, tiles, transparency);
                }
            foreach (Npc npc in activeNPCs)
                npc.Draw(spriteBatch, transparency);


        }

        public void ResetBoss()
        {
            boss.Reset(boss.StartPosition);
        }
    }
}
