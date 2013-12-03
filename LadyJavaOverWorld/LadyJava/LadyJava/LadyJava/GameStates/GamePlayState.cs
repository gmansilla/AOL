using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Engine;

namespace LadyJava
{
    class GamePlayState : GameState
    {
        Camera camera;
        Dictionary<AreaType, Player> player;
        Dictionary<string, bool> toBeRescued;
        //OverWorldPlayer overworldPlayer;
        //DungeonPlayer dungeonPlayer;
        int talkingTo = Global.InvalidInt;

        Texture2D collisionLayerImage;
        
        Global.StoryState currentStoryState;

        Dictionary<string, TileMap> campus;
        string currentArea;
        string previousArea;

        public GamePlayState(ContentManager newContent, GraphicsDevice newGraphicsDevice)
        {
            int screenWidth = newGraphicsDevice.Viewport.Width;
            int screenHeight = newGraphicsDevice.Viewport.Height;

            SpriteFont speechText = newContent.Load<SpriteFont>("Fonts\\SpeechFont");

            id = State.GamePlay;

            bgSong = bgSong = newContent.Load<Song>("Music\\Chandelier");

            currentArea = Global.MainArea;
            campus = new Dictionary<string, TileMap>();

            campus.Add("TileMaps\\overworld.map", new TileMap(Global.ContentPath, "TileMaps\\overworld.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\house1.map", new TileMap(Global.ContentPath, "TileMaps\\house1.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\house2.map", new TileMap(Global.ContentPath, "TileMaps\\house2.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\house3.map", new TileMap(Global.ContentPath, "TileMaps\\house3.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\house4.map", new TileMap(Global.ContentPath, "TileMaps\\house4.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\PC.map", new TileMap(Global.ContentPath, "TileMaps\\PC.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\ShirsStudy.map", new TileMap(Global.ContentPath, "TileMaps\\ShirsStudy.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\Gym.map", new TileMap(Global.ContentPath, "TileMaps\\Gym.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\BoatHouse.map", new TileMap(Global.ContentPath, "TileMaps\\BoatHouse.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D1Water.map", new TileMap(Global.ContentPath, "TileMaps\\D1Water.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D2Basement.map", new TileMap(Global.ContentPath, "TileMaps\\D2Basement.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D3Tree.map", new TileMap(Global.ContentPath, "TileMaps\\D3Tree.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D4Final.map", new TileMap(Global.ContentPath, "TileMaps\\D4Final.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D1End.map", new TileMap(Global.ContentPath, "TileMaps\\D1End.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D2End.map", new TileMap(Global.ContentPath, "TileMaps\\D2End.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D3End.map", new TileMap(Global.ContentPath, "TileMaps\\D3End.map", newContent, screenWidth, screenHeight, speechText));
            campus.Add("TileMaps\\D4End.map", new TileMap(Global.ContentPath, "TileMaps\\D4End.map", newContent, screenWidth, screenHeight, speechText));

            camera = new Camera(screenWidth, screenHeight);
            
            AnimationInfo[] overworldAnimations = { new AnimationInfo(Global.STILL, 32, 46, 1, 0),
                                                    new AnimationInfo(Global.DOWN, 32, 46, 4, 100),
                                                    new AnimationInfo(Global.LEFT, 32, 46, 4, 100),
                                                    new AnimationInfo(Global.RIGHT, 32, 46, 4, 100),
                                                    new AnimationInfo(Global.UP, 32, 46, 4, 100) };

            player = new Dictionary<AreaType, Player>();
            Texture2D overworldImage = newContent.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld");
            player.Add(AreaType.OverWorld,
                       new OverWorldPlayer(new Sprite(overworldImage, campus[currentArea].StartingPosition, overworldAnimations, 1.0f), 
                                           campus[currentArea].TileWidth, campus[currentArea].TileHeight));

            Texture2D dungeonImage = newContent.Load<Texture2D>("Sprites\\LadyJavaDungeon");
            AnimationInfo[] dungeonAnimations = { new AnimationInfo(Global.STILL, 30, 48, 1, 0),
                                                  new AnimationInfo(Global.RIGHT, 30, 48, 8, 100),
                                                  new AnimationInfo(Global.LEFT, 30, 48, 8, 100) };
            player.Add(AreaType.Dungeon,
                       new DungeonPlayer(new Sprite(dungeonImage, Vector2.Zero, dungeonAnimations, 1f)));

            currentStoryState = Global.StoryState.Stage1;
            collisionLayerImage = newContent.Load<Texture2D>("tileSelector");

            toBeRescued = new Dictionary<string, bool>();
            foreach (string name in Global.ToBeRecused)
                toBeRescued.Add(name, true);
        }

        public override State Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            //return to titlescreen
            if (InputManager.IsKeyDown(Commands.Exit))
            {
                ChangeStatus(Status.Paused);
                return State.TitleScreen;
            }

            Vector2 entrancePixelLocation = player[campus[currentArea].CurrentAreaType]
                                                  .Update(gameTime,
                                                          talkingTo,
                                                          campus[currentArea].PixelWidth,
                                                          campus[currentArea].PixelHeight,
                                                          campus[currentArea].CollisionLayer.ToEntranceBox,
                                                          campus[currentArea].NPCTalkRadii,
                                                          campus[currentArea].CollisionLayer.
                                                            GetSurroundingBoundingBoxes(
                                                                player[campus[currentArea].CurrentAreaType].Position,
                                                                campus[currentArea].TileWidth,
                                                                campus[currentArea].TileHeight),
                                                          campus[currentArea].NPCsToBoundingBox);

            campus[currentArea].UpdateRescueList(toBeRescued);

            if (entrancePixelLocation != Global.InvalidVector2)
            {
                string newArea = campus[currentArea].Update(gameTime, 
                                                            entrancePixelLocation,
                                                            player[campus[currentArea].CurrentAreaType].PreviousPosition);


                //clear out the last position if the user doesn't reenter the same previous location
                if (previousArea != Global.MainArea && previousArea != null && previousArea != newArea)
                    campus[previousArea].SetLastPosition(Global.InvalidVector2);

                previousArea = currentArea;
                currentArea = newArea;

                if (campus[currentArea].LastPosition == Global.InvalidVector2)
                    player[campus[currentArea].CurrentAreaType].SetPosition(campus[currentArea].StartingPosition,
                                                campus[currentArea].TileWidth,
                                                campus[currentArea].TileHeight, true, true);
                else
                {
                    player[campus[currentArea].CurrentAreaType].SetPosition(campus[currentArea].LastPosition,
                                                campus[currentArea].TileWidth,
                                                campus[currentArea].TileHeight, false, true);
                    campus[currentArea].SetLastPosition(Global.InvalidVector2);
                }
            }
            camera.Update(gameTime,
                          player[campus[currentArea].CurrentAreaType].Position,
                          player[campus[currentArea].CurrentAreaType].Origin, 
                          campus[currentArea].PixelWidth, campus[currentArea].PixelHeight);

            talkingTo = campus[currentArea].NPCUpdate(gameTime, camera,
                                                      player[campus[currentArea].CurrentAreaType].CurrentPlayState,
                                                      player[campus[currentArea].CurrentAreaType].TalkingTo,
                                                      screenWidth, screenHeight, currentStoryState);


            return State.GamePlay;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);
            campus[currentArea].Draw(spriteBatch);

            player[campus[currentArea].CurrentAreaType].Draw(spriteBatch);

            campus[currentArea].CollisionLayer.Draw(spriteBatch, collisionLayerImage);

            spriteBatch.End();
        }

    }
}
