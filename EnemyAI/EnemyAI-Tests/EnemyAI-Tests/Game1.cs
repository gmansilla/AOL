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

namespace EnemyAI_Tests
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        EnemyTest dummy;

        KeyboardState keyboardState;
        AnimatedSprite sprite;

        Texture2D fighter;

        AnimatedSprite bubSprite;
        Texture2D bub;

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
            dummy = new EnemyTest();

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

            // TODO: use this.Content to load your game content here

            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            
            Animation animation = new Animation(3, 32, 32, 0, 0);
            Animation animation2 = new Animation(5, 22, 23, 0, 0);
            animations.Add(AnimationKey.Down, animation2);
            
            animation = new Animation(3, 32, 32, 0, 32);
            animation2 = new Animation(5, 22, 23, 0, 23);
            animations.Add(AnimationKey.Left, animation2);

            animation = new Animation(3, 32, 32, 0, 64);
            animation2 = new Animation(5, 22, 23, 110, 23);
            animations.Add(AnimationKey.Right, animation2);

            animation = new Animation(3, 32, 32, 0, 96);
            animation2 = new Animation(5, 22, 23, 110, 0);
            animations.Add(AnimationKey.Up, animation2);



            fighter = Content.Load<Texture2D>("Sprites\\malefighter");
            bub = Content.Load<Texture2D>("Sprites\\Bub");

            sprite = new AnimatedSprite(fighter, animations);
            bubSprite = new AnimatedSprite(bub, animations);

            dummy.LoadContent(this.Content, "Sprites\\Bub.png");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Vector2 motion = new Vector2();
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                bubSprite.IsAnimating = true;
                bubSprite.CurrentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                bubSprite.IsAnimating = true;
                bubSprite.CurrentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                bubSprite.IsAnimating = true;
                bubSprite.CurrentAnimation = AnimationKey.Left;
                motion.X = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                bubSprite.IsAnimating = true;
                bubSprite.CurrentAnimation = AnimationKey.Right;
                motion.X = 1;
            }

            if (motion != Vector2.Zero)
            {
                bubSprite.IsAnimating = true;
                motion.Normalize();
                bubSprite.Position += motion * sprite.Speed;
            }
            else
            {
                bubSprite.IsAnimating = false;
            }

            bubSprite.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //sprite.Draw(gameTime, spriteBatch);
            bubSprite.Draw(gameTime, spriteBatch);

            //dummy.Draw(this.spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
