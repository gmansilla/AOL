using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LadyJava
{
    public enum Direction { Right = 2, Left = 1, Up = 3, Down = 0 }

    class LadyJava
    {
        private static Rectangle[,] frames;
        private int currentFrame;
        //private int skip;
        private double elapseTime;
        private Vector2 pos;
        //private float y;
        private static Texture2D texture;
        private static float movement = 2.4f;
        private static double delay = .35;
        private static int columns;

        private Direction direction;

        int DOWN = 1, LEFT = 2, RIGHT = 3, UP = 4, STILL = 0;
        int anim;

        public static void sides(Texture2D tex, int rows, int cols)
        {
            texture = tex;
            double height = texture.Height * 1 / rows;
            double width = texture.Width * 1 / cols;
            frames = new Rectangle[rows, cols];
            columns = cols;
            for (int r = 0; r < rows; r++)
            {
                int top = (int)(height * r);
                for (int c = 0; r < rows; r++)
                {
                    frames[r, c] = new Rectangle((int)(width * c), top, (int)width, (int)height);
                }
            }
        }

        public LadyJava()
        {
            anim = DOWN;
            currentFrame = 0;
            direction = Direction.Left;
            elapseTime = delay;
            //skip = 0;
        }

        public void Update(GameTime gameTime, Sprite sprite, Vector2 position)
        {


            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                direction = Direction.Up;
                anim = UP;
                pos.Y -= movement;
                //skip++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                direction = Direction.Down;
                anim = DOWN;
                pos.Y += movement;
                //skip++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                anim = RIGHT;
                direction = Direction.Right;
                pos.X += movement;
                //skip++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                anim = LEFT;
                direction = Direction.Left;
                pos.X -= movement;
                //skip++;
            }
            else
            {
                anim = STILL;
            }

            /*if (skip >= 5)
            {
                skip = 0;
                currentFrame++;
                if (currentFrame == columns)
                {
                    currentFrame = 0;
                }
            }
            */
            sprite.Update(gameTime, anim, pos);
        }

        public void Draw(Sprite sprite, SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, pos);
            
        }

    }
}
