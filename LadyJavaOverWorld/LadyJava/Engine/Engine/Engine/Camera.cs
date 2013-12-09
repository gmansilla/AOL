using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Camera
    {
        Vector2 position;
        Vector2 origin;
        
        float scale;

        //CollisionBox collisionBox;

        int screenWidth;
        int screenHeight;

        public Vector2 Position
        { get { return position; } }

        public Vector2 Origin
        { get { return origin; } }

        public int Width
        { get { return screenWidth; } }

        public int Height
        { get { return screenHeight; } }

        public Matrix TransformMatrix
        { get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)) * 
                       Matrix.CreateScale(new Vector3(scale, scale, scale)); } }

        public Camera(int newScreenWidth, int newScreenHeight)
        {
            screenWidth = newScreenWidth;
            screenHeight = newScreenHeight;

            origin = new Vector2(screenWidth / 2, screenHeight / 2f);
            scale = 1.0f;

            
        }

        public Camera(int newScreenWidth, int newScreenHeight, 
                      Vector2 playerPosition, Vector2 playerOrigin,
                      int levelWidth, int levelHeight)
        {
            screenWidth = newScreenWidth;
            screenHeight = newScreenHeight;

            origin = new Vector2(screenWidth / 2, screenHeight / 2f);
            scale = 1.0f;

            position = playerPosition + playerOrigin - origin;
            LockToLevel(levelWidth, levelHeight);
        }


        public Camera(int newScreenWidth, int newScreenHeight, float newScale)
        {
            screenWidth = newScreenWidth;
            screenHeight = newScreenHeight;
            scale = newScale;
            origin = new Vector2(screenWidth / 2, screenHeight / 2f) * scale;
        }

        public void Update(GameTime gameTime, Vector2 playerPosition, 
                           Vector2 playerOrigin, 
                           int levelWidth, int levelHeight)
        {
            //position += playerMotion;// -origin.X;// +playerOrigin.X;
           
            position = playerPosition + playerOrigin - origin;

            LockToLevel(levelWidth, levelHeight);

            //collisionBox.Update(position, origin);
        }

        public void Update(Vector2 scrollbarPosition)
        {
            position.X = scrollbarPosition.X;
            position.Y = scrollbarPosition.Y;
        }

        public void Update(Vector2 scrollbarPosition, float newScale)
        {
            position.X = scrollbarPosition.X;
            position.Y = scrollbarPosition.Y;

            scale = newScale;
        }


        void LockToLevel(int width, int height)
        {
            if (position.X > width - screenWidth)
                position.X = width - screenWidth;
            if (position.Y > height - screenHeight)
                position.Y = height - screenHeight;

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
        }


        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen

            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }
    }
}