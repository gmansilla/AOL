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

        //public CollisionBox CollisionBox
        //{ get { return collisionBox; } }

        public Matrix TransformMatrix
        { get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); } }

        public Camera(int newScreenWidth, int newScreenHeight)
        {
            screenWidth = newScreenWidth;
            screenHeight = newScreenHeight;

            origin = new Vector2(screenWidth / 2, screenHeight / 2f);

            //collisionBox = new CollisionBox(newLineImage, position, origin);
        }

        public void Update(GameTime gameTime, Vector2 playerPosition, Vector2 playerOrigin, int levelWidth, int levelHeight)
        {
            position.X = playerPosition.X + playerOrigin.X - origin.X;
            position.Y = playerPosition.Y + playerOrigin.Y - origin.Y;

            LockToLevel(levelWidth, levelHeight);

            //collisionBox.Update(position, origin);
        }

        public void Update(Vector2 scrollbarPosition, int levelWidth, int levelHeight)
        {
            position.X = scrollbarPosition.X;
            position.Y = scrollbarPosition.Y;

            LockToLevel(levelWidth, levelHeight);

            //collisionBox.Update(position, origin);
        }


        void LockToLevel(int width, int height)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > width - screenWidth)
                position.X = width - screenWidth;
            if (position.Y > height - screenHeight)
                position.Y = height - screenHeight;
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