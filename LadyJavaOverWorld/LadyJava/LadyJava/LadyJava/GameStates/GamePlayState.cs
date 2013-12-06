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
using Microsoft.Xna.Framework.Input;

namespace LadyJava
{
    class GamePlayState : GameState
    {
        Camera camera;
        Dictionary<AreaType, Player> player;
        //OverWorldPlayer overworldPlayer;
        //DungeonPlayer dungeonPlayer;
        int talkingTo = Global.InvalidInt;

        Dictionary<string, RescueInfo> toBeRescued;


        Texture2D collisionLayerImage;
        
        Dictionary<string, TileMap> campus;
        string currentArea;
        string previousArea;

        bool drawCollision = false;

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
            
            AnimationInfo[] overworldAnimations = { new AnimationInfo(Global.Still, 32, 46, 1, 0),
                                                    new AnimationInfo(Global.Down, 32, 46, 4, 100),
                                                    new AnimationInfo(Global.Left, 32, 46, 4, 100),
                                                    new AnimationInfo(Global.Right, 32, 46, 4, 100),
                                                    new AnimationInfo(Global.Up, 32, 46, 4, 100) };

            player = new Dictionary<AreaType, Player>();
            Texture2D overworldImage = newContent.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld");
            player.Add(AreaType.OverWorld,
                       new OverWorldPlayer(new Sprite(overworldImage, campus[currentArea].StartingPosition, overworldAnimations, 1.0f), 
                                           campus[currentArea].TileWidth, campus[currentArea].TileHeight));

            Texture2D dungeonImage = newContent.Load<Texture2D>("Sprites\\LadyJavaDungeon");
            AnimationInfo[] dungeonAnimations = { new AnimationInfo(Global.Still, 16, 48, 1, 0),
                                                  new AnimationInfo(Global.Right, 30, 48, 8, 100),
                                                  new AnimationInfo(Global.Left, 30, 48, 8, 100),
                                                  new AnimationInfo(Global.StartingAttack, 27, 64, 3, 100),
                                                  new AnimationInfo(Global.Attacking, 61, 60, 4, 100),
                                                  new AnimationInfo(Global.Dying, 32, 47, 9, 100) };
            player.Add(AreaType.Dungeon,
                       new DungeonPlayer(new Sprite(dungeonImage, Vector2.Zero, dungeonAnimations, 1f)));

            collisionLayerImage = newContent.Load<Texture2D>("tileSelector");

            toBeRescued = new Dictionary<string, RescueInfo>();
            RescueInfo[] rescueInfo = new RescueInfo[Global.ToBeRecused.Length];
            for(int i = 0; i < Global.ToBeRecused.Length; i++)
                toBeRescued.Add(Global.ToBeRecused[i], new RescueInfo(Global.RecuseAreas[i]));
        }

        public override State Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            //return to titlescreen
            if (InputManager.IsKeyDown(Commands.Exit))
            {
                ChangeStatus(Status.Paused);
                return State.TitleScreen;
            }
            else if (InputManager.HasKeyBeenUp(new Command(Microsoft.Xna.Framework.Input.Keys.C, Buttons.LeftShoulder)))
                drawCollision = !drawCollision;

            campus[currentArea].UpdateRescueList(toBeRescued);

            Vector2 entrancePixelLocation = player[campus[currentArea].CurrentAreaType]
                                                  .Update(gameTime,
                                                          talkingTo,
                                                          campus[currentArea].FinalNPCIndex,
                                                          campus[currentArea].PixelWidth,
                                                          campus[currentArea].PixelHeight,
                                                          campus[currentArea].ToEntranceBox,
                                                          campus[currentArea].NPCTalkRadii,
                                                          campus[currentArea].CollisionLayer.
                                                            GetSurroundingBoundingBoxes(
                                                                player[campus[currentArea].CurrentAreaType].Position,
                                                                campus[currentArea].TileWidth,
                                                                campus[currentArea].TileHeight),
                                                          campus[currentArea].NPCsToBoundingBox);


            if (entrancePixelLocation != Global.InvalidVector2)
            {
                string newArea = campus[currentArea].Update(gameTime, 
                                                            entrancePixelLocation,
                                                            player[campus[currentArea].CurrentAreaType].PreviousPosition);

                if (newArea == Global.EndOfTheGame)
                {
                    status = Status.Off; 
                    return State.FinalStory;
                }
                //clear out the last position if the user doesn't reenter the same previous location
                if (previousArea != Global.MainArea && previousArea != null && previousArea != newArea)
                    campus[previousArea].SetLastPosition(Global.InvalidVector2);

                previousArea = currentArea;
                currentArea = newArea;

                //make Main Npc's Visible on overworld.map and not in the dungeon anymore
                foreach (KeyValuePair<string, RescueInfo> npc in toBeRescued)
                    if (previousArea == npc.Value.RescueArea)
                        npc.Value.Rescue();

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

            talkingTo = campus[currentArea].NPCUpdate(gameTime, toBeRescued, camera,
                                                      player[campus[currentArea].CurrentAreaType].CurrentPlayState,
                                                      player[campus[currentArea].CurrentAreaType].TalkingTo,
                                                      player[campus[currentArea].CurrentAreaType].SpokeWithFinalNPC,
                                                      screenWidth, screenHeight);

            return State.GamePlay;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);
            campus[currentArea].Draw(spriteBatch);

            player[campus[currentArea].CurrentAreaType].Draw(spriteBatch);

            if(drawCollision)
                campus[currentArea].CollisionLayer.Draw(spriteBatch, collisionLayerImage);

            spriteBatch.End();
        }
    }
}
