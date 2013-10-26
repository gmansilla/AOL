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

        public Vector2 Position
        { get { return sprite.Position; } }

        public Vector2 Origin
        { get { return sprite.Origin; } }

        //private int skip;
       
        private Vector2 pos;
        //private float y;
        private Texture2D texture;
        private float movement = 2.4f;
        
        
        private Sprite sprite;
       

        int DOWN = 1, LEFT = 2, RIGHT = 3, UP = 4, STILL = 0;
        int anim;

        

        public LadyJava(Sprite newSprite)
        {
            anim = DOWN;
            sprite = newSprite;
            
            
            //skip = 0;
        }

        public void Update(GameTime gameTime)
        {


            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                
                anim = UP;
                pos.Y -= movement;
                //skip++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
               
                anim = DOWN;
                pos.Y += movement;
                //skip++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                anim = RIGHT;
                
                pos.X += movement;
                //skip++;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                anim = LEFT;
               
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

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            
        }

    }
}
