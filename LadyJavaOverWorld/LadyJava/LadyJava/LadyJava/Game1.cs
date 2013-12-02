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
            gameStateManager = new GameStateManager(State.TitleScreen);//.GamePlay);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            ////create SeeHash (NPC)
            //npcs.Add(new Npc("SeeHash", new Vector2(1032, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));
 
            ////create TecMan (NPC)
            //npcs.Add(new Npc("TecMan", new Vector2(1160, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create TheOracle (NPC)
            //npcs.Add(new Npc("TheOracle", new Vector2(1224, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));

            ////create TheScrumMaster (NPC)
            //npcs.Add(new Npc("TheScrumMaster", new Vector2(1288, 200), 25, 50, 1.0f, Content, screenWidth, screenHeigth, speachText));
            
        }

        protected override void UnloadContent()
        {
            gameStateManager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            
            State state = gameStateManager.Update(gameTime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            if (state == State.GamePlay && !gameStateManager.Contains(State.GamePlay))
                gameStateManager.AddState(new GamePlayState(Content, GraphicsDevice));
            else if (state == State.TitleScreen && !gameStateManager.Contains(State.TitleScreen))
            {
                if (!gameStateManager.Contains(State.GamePlay))
                    gameStateManager.AddState(new TitleScreenState(Content, GraphicsDevice, "Start"));
                else
                    gameStateManager.AddState(new TitleScreenState(Content, GraphicsDevice, "Resume"));
            }
            else if (state == State.Options && !gameStateManager.Contains(State.Options))
                gameStateManager.AddState(new OptionsState(Content, GraphicsDevice));

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