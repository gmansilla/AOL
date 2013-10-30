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

        private float movement = 2.4f;
        
        
        private Sprite sprite;
       

        string animation;

        public LadyJava(Sprite newSprite)
        {
            animation = Global.STILL;
            sprite = newSprite;
        }

        Vector2 LockToLevel(int width, int height, Vector2 position, int levelW, int levelH)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > levelW-width)
                position.X = levelW - width;
            if (position.Y > levelH - height)
                position.Y = levelH - height;
            return position;
        }

        public void Update(GameTime gameTime, int levelWidth, int levelHeight)
        {
            Vector2 motion = Vector2.Zero;
            Vector2 position = sprite.Position;

            if(InputManager.IsKeyDown(Commands.Up))
            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                
                animation = Global.UP;
                motion.Y--;//= movement;
            }
            if(InputManager.IsKeyDown(Commands.Down))
            //else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {

                animation = Global.DOWN;
                motion.Y++;//= movement;
            }
            if(InputManager.IsKeyDown(Commands.Right))
            //else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                animation = Global.RIGHT;

                motion.X++;//= movement;
            }
            if(InputManager.IsKeyDown(Commands.Left))
            //else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                animation = Global.LEFT;

                motion.X--;//= movement;
            }
            
            if(motion == Vector2.Zero)
            {
                animation = Global.STILL;
            }
            else
                motion.Normalize();
    
            position += motion * movement;

            position = LockToLevel(sprite.Width, sprite.Height, position, levelWidth, levelHeight);

            sprite.Update(gameTime, animation, position);


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            
        }

    }
}
