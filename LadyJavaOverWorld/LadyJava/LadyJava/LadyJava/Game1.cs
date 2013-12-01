using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

using Engine;

namespace LadyJava
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        LadyJava ladyJ;
        List<Npc> npcs;
        
        Camera camera;
        Texture2D collisionLayerImage;

        Global.StoryStates currentStoryState;

        Dictionary<string, TileMap> campus;
        string currentArea;
  
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
  
        protected override void LoadContent()
        {
            SpriteFont speechText = Content.Load<SpriteFont>("SpeechFont");
            Texture2D[] image = { Content.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld") };

            int screenWidth = graphics.GraphicsDevice.Viewport.Width;
            int screenHeigth = graphics.GraphicsDevice.Viewport.Height;

            npcs = new List<Npc>();
            campus = new Dictionary<string, TileMap>();
            currentArea = "TileMaps\\overworld.map";
            
            
            collisionLayerImage = Content.Load<Texture2D>("tileSelector");
            camera = new Camera(screenWidth, screenHeigth);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            #region campus.add
            campus.Add("TileMaps\\overworld.map", new TileMap(Global.ContentPath + "TileMaps\\overworld.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\house1.map", new TileMap(Global.ContentPath + "TileMaps\\house1.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\house2.map", new TileMap(Global.ContentPath + "TileMaps\\house2.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\house3.map", new TileMap(Global.ContentPath + "TileMaps\\house3.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\house4.map", new TileMap(Global.ContentPath + "TileMaps\\house4.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\PC.map", new TileMap(Global.ContentPath + "TileMaps\\PC.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\ShirsStudy.map", new TileMap(Global.ContentPath + "TileMaps\\ShirsStudy.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\Gym.map", new TileMap(Global.ContentPath + "TileMaps\\Gym.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\BoatHouse.map", new TileMap(Global.ContentPath + "TileMaps\\BoatHouse.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D1Water.map", new TileMap(Global.ContentPath + "TileMaps\\D1Water.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D2Basement.map", new TileMap(Global.ContentPath + "TileMaps\\D2Basement.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D3Tree.map", new TileMap(Global.ContentPath + "TileMaps\\D3Tree.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D1End.map", new TileMap(Global.ContentPath + "TileMaps\\D1End.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D2End.map", new TileMap(Global.ContentPath + "TileMaps\\D2End.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D3End.map", new TileMap(Global.ContentPath + "TileMaps\\D3End.map", Content, screenWidth, screenHeigth, speechText));
            campus.Add("TileMaps\\D4End.map", new TileMap(Global.ContentPath + "TileMaps\\D4End.map", Content, screenWidth, screenHeigth, speechText));
            #endregion
            
            AnimationInfo[] animations = { new AnimationInfo(Global.STILL, 32, 46, 1, 0),
                                           new AnimationInfo(Global.DOWN, 32, 46, 4, 100),
                                           new AnimationInfo(Global.LEFT, 32, 46, 4, 100),
                                           new AnimationInfo(Global.RIGHT, 32, 46, 4, 100),
                                           new AnimationInfo(Global.UP, 32, 46, 4, 100) };

            Sprite lady = new Sprite(Content.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld"), 
                                     campus[currentArea].StartingPosition, 
                                     animations, 
                                     1.0f);
            ladyJ = new LadyJava(lady);

            
            //create Amy (NPC)
            //npcs.Add(new Npc("Amy", new Vector2(200, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speechText));

            ////create Anna (NPC)
            //npcs.Add(new Npc("Anna", new Vector2(328, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Han (NPC)
            //npcs.Add(new Npc("Han", new Vector2(456, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Hugh (NPC)
            //npcs.Add(new Npc("Hugh", new Vector2(520, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Josh (NPC)
            //npcs.Add(new Npc("Josh", new Vector2(520, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Nei (NPC)
            //npcs.Add(new Npc("Nei", new Vector2(712, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Rika (NPC)
            //npcs.Add(new Npc("Rika", new Vector2(776, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Rolf (NPC)
            //npcs.Add(new Npc("Rolf", new Vector2(840, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////creare Rudo (NPC)
            //npcs.Add(new Npc("Rudo", new Vector2(904, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Rune (NPC)
            //npcs.Add(new Npc("Rune", new Vector2(968, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create SeeHash (NPC)
            //npcs.Add(new Npc("SeeHash", new Vector2(1032, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create Shir (NPC)
            //npcs.Add(new Npc("Shir", new Vector2(1096, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create TecMan (NPC)
            //npcs.Add(new Npc("TecMan", new Vector2(1160, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create TheOracle (NPC)
            //npcs.Add(new Npc("TheOracle", new Vector2(1224, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create TheScrumMaster (NPC)
            //npcs.Add(new Npc("TheScrumMaster", new Vector2(1288, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));
            
            currentStoryState = Global.StoryStates.Stage1;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        int talkingTo = Global.InvalidInt;
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (InputManager.IsKeyDown(Commands.Exit))
                this.Exit();

            Vector2 entrancePixelLocation = ladyJ.Update(gameTime, 
                                                         talkingTo,
                                                         campus[currentArea].PixelWidth, 
                                                         campus[currentArea].PixelHeight, 
                                                         campus[currentArea].CollisionLayer.ToEntranceBox,
                                                         campus[currentArea].NPCTalkRadii,
                                                         campus[currentArea].CollisionLayer.ToCollisionBox,
                                                         campus[currentArea].NPCsToBoundingBox);
            
            if (entrancePixelLocation != Global.InvalidVector2)
            {
                campus[currentArea].SetLastPosition(ladyJ.PreviousPosition);
                Vector2 entranceLocation = new Vector2(entrancePixelLocation.X / campus[currentArea].TileWidth,
                                                       entrancePixelLocation.Y / campus[currentArea].TileHeight);
                int currentIndex = campus[currentArea].CollisionLayer.GetCellIndex((int)entranceLocation.X, (int)entranceLocation.Y);
                currentArea = campus[currentArea].CollisionLayer.Entrances[currentIndex];
                if (campus[currentArea].LastPosition == Global.InvalidVector2)
                    ladyJ.SetPosition(campus[currentArea].StartingPosition);
                else
                {
                    ladyJ.SetPosition(campus[currentArea].LastPosition);
                    campus[currentArea].SetLastPosition(Global.InvalidVector2);
                }
            }

            talkingTo = campus[currentArea].NPCUpdate(gameTime,
                                                      camera, ladyJ.CurrentPlayState, ladyJ.TalkingTo,
                                                      GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, currentStoryState);

            camera.Update(gameTime, ladyJ.Position, ladyJ.Origin, campus[currentArea].PixelWidth, campus[currentArea].PixelHeight);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            campus[currentArea].Draw(spriteBatch);
            ladyJ.Draw(spriteBatch);

            foreach(Npc npc in campus[currentArea].NPCs)
                npc.Draw(spriteBatch);

            //campus[currentArea].CollisionLayer.Draw(spriteBatch, collisionLayerImage);
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}