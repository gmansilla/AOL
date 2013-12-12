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

        GameStateManager gameStateManager;

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
            gameStateManager = new GameStateManager(State.TitleScreen);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            gameStateManager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            
            State state = gameStateManager.Update(gameTime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            if (state == State.TitleScreen && !gameStateManager.Contains(State.TitleScreen))
            {
                if (!gameStateManager.Contains(State.GamePlay))
                    gameStateManager.AddState(new TitleScreenState(Content, GraphicsDevice, Global.Start));
                else
                    gameStateManager.AddState(new TitleScreenState(Content, GraphicsDevice, Global.Resume));
            }
            else if (state == State.Options && !gameStateManager.Contains(State.Options))
                gameStateManager.AddState(new OptionsState(Content, GraphicsDevice));
            else if (state == State.InitialStory && !gameStateManager.Contains(State.InitialStory))
                gameStateManager.AddState(new InitialStoryState(Content, GraphicsDevice));
            else if (state == State.GamePlay && !gameStateManager.Contains(State.GamePlay))
                gameStateManager.AddState(new GamePlayState(Content, GraphicsDevice));
            else if (state == State.FinalStory && !gameStateManager.Contains(State.FinalStory))
                gameStateManager.AddState(new FinalStoryState(Content, GraphicsDevice));
            else if (state == State.Credits && !gameStateManager.Contains(State.Credits))
                gameStateManager.AddState(new CreditsState(Content, GraphicsDevice));

            // Allows the game to exit
            if (gameStateManager.CurrentState == State.Quit)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            gameStateManager.Draw(spriteBatch);
            
            base.Draw(gameTime);
        }


    }
}