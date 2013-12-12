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

        AnimatedSprite hero;
        Texture2D bub;

        GroundAnimatedEnemy BUB, b1, b2, b3;

        int BUBDifference, b1Diff, b2Diff, b3Diff, targetDifference;

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
            hero = new AnimatedSprite(bub, animations);

            BUB = new GroundAnimatedEnemy(bub, animations);
            b1 = new GroundAnimatedEnemy(bub, animations);
            b2 = new GroundAnimatedEnemy(bub, animations);
            b3 = new GroundAnimatedEnemy(bub, animations);

            BUB.Position = new Vector2(25, 50);
            b1.Position = new Vector2(100, 100);
            b2.Position = new Vector2(175, 150);
            b3.Position = new Vector2(250, 200);

            targetDifference = 10;

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
                hero.IsAnimating = true;
                hero.CurrentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                hero.IsAnimating = true;
                hero.CurrentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                hero.IsAnimating = true;
                hero.CurrentAnimation = AnimationKey.Left;
                motion.X = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                hero.IsAnimating = true;
                hero.CurrentAnimation = AnimationKey.Right;
                motion.X = 1;
            }

            if (motion != Vector2.Zero)
            {
                hero.IsAnimating = true;
                motion.Normalize();
                hero.Position += motion * sprite.Speed;
            }
            else
            {
                hero.IsAnimating = false;
            }



           

            
            

            if (BUB != null)
            {
                if (BUB.IsAlive)
                {
                    BUBDifference = Math.Abs((int)hero.Position.X - (int)BUB.Position.X);

                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        if (BUBDifference <= targetDifference && BUBDifference >= 0)
                        {
                            BUB.IsAHit = true;
                        }
                    }
                    
                    BUB.Update(gameTime);
                }
                else if (!BUB.IsAlive)
                {
                    BUB = null;
                }
            }

            if (b1 != null)
            {
                if (b1.IsAlive)
                {
                    b1Diff = Math.Abs((int)hero.Position.X - (int)b1.Position.X);

                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        if (b1Diff <= targetDifference && b1Diff >= 0)
                        {
                            b1.IsAHit = true;
                        }
                    }

                    b1.Update(gameTime);
                }
                else if (!b1.IsAlive)
                {
                    b1 = null;
                }
            }

            if (b2 != null)
            {
                if (b2.IsAlive)
                {
                    b2Diff = Math.Abs((int)hero.Position.X - (int)b2.Position.X);

                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        if (b2Diff <= targetDifference && b2Diff >= 0)
                        {
                            b2.IsAHit = true;
                        }
                    }

                    b2.Update(gameTime);
                }
                else if (!b2.IsAlive)
                {
                    b2 = null;
                }
            }

            if (b3 != null)
            {
                if (b3.IsAlive)
                {
                    b3Diff = Math.Abs((int)hero.Position.X - (int)b3.Position.X);

                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        if (b3Diff <= targetDifference && b3Diff >= 0)
                        {
                            b3.IsAHit = true;
                        }
                    }

                    b3.Update(gameTime);
                }
                else if (!b3.IsAlive)
                {
                    b3 = null;
                }
            }

            hero.Update(gameTime);
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
            hero.Draw(gameTime, spriteBatch);
            if (BUB != null)
            {
                BUB.Draw(gameTime, spriteBatch);
            }
            if (b1 != null)
            {
                b1.Draw(gameTime, spriteBatch);
            }
            if (b2 != null)
            {
                b2.Draw(gameTime, spriteBatch);
            }
            if (b3 != null)
            {
                b3.Draw(gameTime, spriteBatch);
            }

            //dummy.Draw(this.spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
