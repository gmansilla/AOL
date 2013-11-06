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

        #region Background
        //List<Texture2D> tileTexture = new List<Texture2D>();
        //int[][,] tileMap;

        //int tileWidth = 64;
        //int tileHeight = 64;
        #endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        //int STILL = 0 , DOWN = 1, LEFT=2, RIGHT=3, UP=4;
        TileMap overworld;
        protected override void LoadContent()
        {
            Texture2D[] image = { Content.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld") };
            Texture2D[] npcImage = { Content.Load<Texture2D>("Sprites\\NPI\\Sprites\\NPC_Hugh") };

            camera = new Camera(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            overworld = new TileMap("..\\..\\..\\..\\..\\overworld.map", Content);

            AnimationInfo[] animations = { new AnimationInfo(Global.STILL, 32, 46, 1, 0),
                                           new AnimationInfo(Global.DOWN, 32, 46, 4, 100),
                                           new AnimationInfo(Global.LEFT, 32, 46, 4, 100),
                                           new AnimationInfo(Global.RIGHT, 32, 46, 4, 100),
                                           new AnimationInfo(Global.UP, 32, 46, 4, 100) };
            //    { { STILL, 1, 0 }, { DOWN, 4, 100 }, { LEFT, 4, 100 }, { RIGHT, 4, 100 }, { UP, 4, 100 } };
            Sprite lady = new Sprite(image, new Vector2(100, 100), animations, 1.0f);
            ladyJ = new LadyJava(lady);

            //create a npc
            Sprite amy = new Sprite(npcImage, new Vector2(200, 200), 38, 73, 1.0f);
            npcAmy = new Npc(amy);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            //KeyboardState keyState = Keyboard.GetState();

            // Allows the game to exit
            if (InputManager.IsKeyDown(Commands.Exit))
                this.Exit();

            ladyJ.Update(gameTime, overworld.PixelWidth, overworld.PixelHeight);
            camera.Update(gameTime, ladyJ.Position, ladyJ.Origin, overworld.PixelWidth, overworld.PixelHeight);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            #region DrawTile
            #endregion
            overworld.Draw(spriteBatch);
            ladyJ.Draw(spriteBatch);
            npcAmy.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}
