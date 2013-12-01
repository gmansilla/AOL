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
        
        Camera camera;
        Texture2D collisionLayerImage;
        //TitleScreen titleScreen;
        //GameState state = GameState.TitleScreen;   //Set the starting screen
        Global.StoryStates currentStoryState;

        Dictionary<string, TileMap> campus;
        string currentArea;

        int talkingTo = Global.InvalidInt;

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
            Texture2D image = Content.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            int screenWidth = graphics.GraphicsDevice.Viewport.Width;
            int screenHeigth = graphics.GraphicsDevice.Viewport.Height;

            //npcs = new List<Npc>();

            currentArea = "TileMaps\\overworld.map";
            campus = new Dictionary<string, TileMap>();

            //Adding the Title Screen
            //Services.AddService(typeof(SpriteBatch), spriteBatch);
            //titleScreen = new TitleScreen(this, Content.Load<Texture2D>("Screens\\TitleScreen"), spriteBatch);
            //Components.Add(titleScreen);
            
            collisionLayerImage = Content.Load<Texture2D>("tileSelector");
            camera = new Camera(screenWidth, screenHeigth);

            camera = new Camera(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

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
            campus.Add("TileMaps\\D4Final.map", new TileMap(Global.ContentPath + "TileMaps\\D4Final.map", Content, screenWidth, screenHeigth, speechText));
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

            Sprite lady = new Sprite(image, campus[currentArea].StartingPosition, animations, 1.0f);
            ladyJ = new LadyJava(lady, campus[currentArea].TileWidth, campus[currentArea].TileHeight);

            #region Music
            Song startSong = Content.Load<Song>("Music\\Chandelier");
            MediaPlayer.Play(startSong);
            MediaPlayer.IsRepeating = true;
            #endregion 

 
            ////create Nei (NPC)
            //npcs.Add(new Npc("Nei", new Vector2(712, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create SeeHash (NPC)
            //npcs.Add(new Npc("SeeHash", new Vector2(1032, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));
 
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
                    ladyJ.SetPosition(campus[currentArea].StartingPosition, 
                                        campus[currentArea].TileWidth,
                                        campus[currentArea].TileHeight, true, true);
                else
                {
                    ladyJ.SetPosition(campus[currentArea].LastPosition,
                                      campus[currentArea].TileWidth,
                                      campus[currentArea].TileHeight, false, true);
                    campus[currentArea].SetLastPosition(Global.InvalidVector2);
                }
            }

            camera.Update(gameTime, ladyJ.Position, ladyJ.Origin, campus[currentArea].PixelWidth, campus[currentArea].PixelHeight);

            talkingTo = campus[currentArea].NPCUpdate(gameTime,
                                                      camera, ladyJ.CurrentPlayState, ladyJ.TalkingTo,
                                                      GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, currentStoryState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);
            campus[currentArea].Draw(spriteBatch);
                
            ladyJ.Draw(spriteBatch);
                
            campus[currentArea].CollisionLayer.Draw(spriteBatch, collisionLayerImage);
                
            foreach(Npc npc in campus[currentArea].NPCs)
                npc.Draw(spriteBatch);
                
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}