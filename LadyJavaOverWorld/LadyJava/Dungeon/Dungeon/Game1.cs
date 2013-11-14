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
using Engine;

namespace Dungeon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<TileMap> dungeons;
        Camera camera;
        Player player;

        int currentDungeon;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentDungeon = 0;
            dungeons = new List<TileMap>();
            dungeons.Add(new TileMap(Global.DungeonContentPath + "TileMaps\\D1.map", Content));

            Texture2D[] playerImage = { Content.Load<Texture2D>("player") };

            AnimationInfo[] animationInfo = { new AnimationInfo(Global.STILL, 50, 100, 1, 0),
                                              new AnimationInfo(Global.RIGHT, 50, 100, 2, 100),
                                              new AnimationInfo(Global.LEFT, 50, 100, 2, 100) };

            player = new Player(new Sprite(playerImage, dungeons[currentDungeon].StartingPosition, animationInfo, 1f));

            camera = new Camera(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            // Allows the game to exit
            if(InputManager.IsKeyDown(Commands.Exit))
                this.Exit();

            player.Update(gameTime, dungeons[currentDungeon].PixelWidth, dungeons[currentDungeon].PixelWidth, dungeons[currentDungeon].CollisionLayer.ToCollisionBox);
            
            camera.Update(gameTime, player.Position, player.Origin, dungeons[currentDungeon].PixelWidth, dungeons[currentDungeon].PixelHeight);            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            dungeons[currentDungeon].Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
