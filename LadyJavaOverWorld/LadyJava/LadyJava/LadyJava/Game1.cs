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
        Npc npcAmy;
        Camera camera;
        Texture2D collisionLayerImage;

       // TileMap overworld;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        Dictionary<string, TileMap> campus;
        string currentArea;
        protected override void LoadContent()
        {
            campus = new Dictionary<string, TileMap>();
            currentArea = "TileMaps\\overworld.map";
            Texture2D[] image = { Content.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld") };
            
            
            collisionLayerImage = Content.Load<Texture2D>("tileSelector");

            camera = new Camera(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            #region campus.add
            campus.Add("TileMaps\\overworld.map", new TileMap(Global.ContentPath + "TileMaps\\overworld.map", Content));
            campus.Add("TileMaps\\house1.map", new TileMap(Global.ContentPath + "TileMaps\\house1.map", Content));
            campus.Add("TileMaps\\house2.map", new TileMap(Global.ContentPath + "TileMaps\\house2.map", Content));
            campus.Add("TileMaps\\house3.map", new TileMap(Global.ContentPath + "TileMaps\\house3.map", Content));
            campus.Add("TileMaps\\house4.map", new TileMap(Global.ContentPath + "TileMaps\\house4.map", Content));
            campus.Add("TileMaps\\PC.map", new TileMap(Global.ContentPath + "TileMaps\\PC.map", Content));
            campus.Add("TileMaps\\ShirsStudy.map", new TileMap(Global.ContentPath + "TileMaps\\ShirsStudy.map", Content));
            campus.Add("TileMaps\\Gym.map", new TileMap(Global.ContentPath + "TileMaps\\Gym.map", Content));
            campus.Add("TileMaps\\BoatHouse.map", new TileMap(Global.ContentPath + "TileMaps\\BoatHouse.map", Content));
            campus.Add("TileMaps\\D1Water.map", new TileMap(Global.ContentPath + "TileMaps\\D1Water.map", Content));
            campus.Add("TileMaps\\D2Basement.map", new TileMap(Global.ContentPath + "TileMaps\\D2Basement.map", Content));
            campus.Add("TileMaps\\D3Tree.map", new TileMap(Global.ContentPath + "TileMaps\\D3Tree.map", Content));
            campus.Add("TileMaps\\D1End.map", new TileMap(Global.ContentPath + "TileMaps\\D1End.map", Content));
            campus.Add("TileMaps\\D2End.map", new TileMap(Global.ContentPath + "TileMaps\\D2End.map", Content));
            campus.Add("TileMaps\\D3End.map", new TileMap(Global.ContentPath + "TileMaps\\D3End.map", Content));
            campus.Add("TileMaps\\D4End.map", new TileMap(Global.ContentPath + "TileMaps\\D4End.map", Content));





            #endregion
            AnimationInfo[] animations = { new AnimationInfo(Global.STILL, 32, 46, 1, 0),
                                           new AnimationInfo(Global.DOWN, 32, 46, 4, 100),
                                           new AnimationInfo(Global.LEFT, 32, 46, 4, 100),
                                           new AnimationInfo(Global.RIGHT, 32, 46, 4, 100),
                                           new AnimationInfo(Global.UP, 32, 46, 4, 100) };

            Sprite lady = new Sprite(image, campus[currentArea].StartingPosition, animations, 1.0f);
            ladyJ = new LadyJava(lady);

            //create a Amy (NPC)
            Texture2D[] npcImage = { Content.Load<Texture2D>("Npc\\Amy\\sprite") };
            Sprite amy = new Sprite(npcImage, new Vector2(200, 200), 25, 50, 1.0f);
            npcAmy = new Npc(amy,"Amy");
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

            Vector2 entrancePixelLocation = ladyJ.Update(gameTime, campus[currentArea].PixelWidth, campus[currentArea].PixelHeight, campus[currentArea].CollisionLayer.ToEntranceBox, campus[currentArea].CollisionLayer.ToCollisionBox);
            if (entrancePixelLocation != Global.Invalid)
            {
                campus[currentArea].SetLastPosition(ladyJ.PreviousPosition);
                Vector2 entranceLocation = new Vector2(entrancePixelLocation.X / campus[currentArea].TileWidth,
                                                       entrancePixelLocation.Y / campus[currentArea].TileHeight);
                int currentIndex = campus[currentArea].CollisionLayer.GetCellIndex((int)entranceLocation.X, (int)entranceLocation.Y);
                currentArea = campus[currentArea].CollisionLayer.Entrances[currentIndex];
                if (campus[currentArea].LastPosition == Global.Invalid)
                    ladyJ.SetPosition(campus[currentArea].StartingPosition);
                else
                {
                    ladyJ.SetPosition(campus[currentArea].LastPosition);
                    campus[currentArea].SetLastPosition(Global.Invalid);
                }
            }
            camera.Update(gameTime, ladyJ.Position, ladyJ.Origin, campus[currentArea].PixelWidth, campus[currentArea].PixelHeight);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            campus[currentArea].Draw(spriteBatch);
            ladyJ.Draw(spriteBatch);
            npcAmy.Draw(spriteBatch);

            campus[currentArea].CollisionLayer.Draw(spriteBatch, collisionLayerImage);
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}