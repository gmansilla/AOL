﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Engine;
using System.Collections.Generic;

namespace LadyJava
{
    class TitleScreenState : GameState
    {
        SpriteFont normalText;

        Dictionary<State, DisplayText> actionText;

        Color selectedColor;
        Color unSelectedColor;

        Texture2D background;

        float scale;

        int width;
        int height;

        State selected;

        public TitleScreenState(ContentManager newContent, GraphicsDevice newGraphicsDevice, string StartMsg)
        {
            id = State.TitleScreen;
            selected = State.GamePlay;

            scale = 1f;

            Vector2 position;

            width = newGraphicsDevice.Viewport.Width;
            height = newGraphicsDevice.Viewport.Height;

            background = newContent.Load<Texture2D>("Screens\\TitleScreen");
            bgSong = newContent.Load<Song>("Music\\Chandelier");

            normalText = newContent.Load<SpriteFont>("Fonts\\TitleText");

            selectedColor = Color.Red;
            unSelectedColor = Color.MintCream;
            actionText = new Dictionary<State, DisplayText>(); //new DisplayText[State.Quit + 1];

            position = new Vector2(width / 2, height / 2);

            actionText.Add(selected, new DisplayText(position, StartMsg, normalText, selectedColor));
            actionText.Add(State.Options, new DisplayText(position, "Options", normalText, unSelectedColor));
            actionText.Add(State.Quit, new DisplayText(position, "Quit", normalText, unSelectedColor));

            position = new Vector2(width / 2, height);

            float menusHeight = 0f;
            foreach (KeyValuePair<State, DisplayText> text in actionText)
            {
                text.Value.MoveText(new Vector2(-text.Value.Width / 2, menusHeight));
                menusHeight += text.Value.Height;
            }

        }
        
        public override State Update(GameTime gameTime, int newScreenWidth, int newScreenHeight)
        {
            width = newScreenWidth;
            height = newScreenHeight;

            if (InputManager.HasKeyBeenUp(Commands.Down) || InputManager.HasLeftStickChangedDriection(Commands.ThumbStick.Down))
            {
                actionText[selected].ChangeColor(unSelectedColor);
                if (selected == State.GamePlay)
                    selected = State.Options;
                else if (selected == State.Options)
                    selected = State.Quit;
                else if (selected == State.Quit)
                    selected = State.GamePlay;
                actionText[selected].ChangeColor(selectedColor);
            }
            else if (InputManager.HasKeyBeenUp(Commands.Up) || InputManager.HasLeftStickChangedDriection(Commands.ThumbStick.Up))
            {
                actionText[selected].ChangeColor(unSelectedColor);

                if (selected == State.GamePlay)
                    selected = State.Quit;
                else if (selected == State.Options)
                    selected = State.GamePlay;
                else if (selected == State.Quit)
                    selected = State.Options;
                
                actionText[selected].ChangeColor(selectedColor);
            }
            else if (InputManager.HasKeyBeenUp(Commands.Execute))
            {
                status = Status.Off;
                if (selected == State.Options)
                    status = Status.Paused;
                
                return selected;
            }

            return State.TitleScreen;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(new Vector3(scale, scale, scale)));

            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0, 0, width, height), new Color(255f, 255f, 255f, 0.5f));

            foreach (KeyValuePair<State, DisplayText> text in actionText)
            {
                text.Value.DrawString(spriteBatch);
            }

            spriteBatch.End();
        }
    
    }
}