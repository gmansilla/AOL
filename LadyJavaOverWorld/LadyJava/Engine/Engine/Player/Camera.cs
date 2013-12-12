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
        const float MaxCameraMovement = Global.GravityAccelation;

        Vector2 position;
        Vector2 origin;
        
        float scale;

        int screenWidth;
        public int ScreenWidth
        { get { return screenWidth; } }

        int screenHeight;
        public int ScreenHeight
        { get { return screenHeight; } }

        bool bossFightWasActive;

        bool inTransition;
        bool transitionUp;
        bool transitionLeft;
        Vector2 transitionTo;

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

        public void ResetCamera()
        {
            inTransition = false;
            transitionTo = Global.InvalidVector2;
            bossFightWasActive = false;
        }

        public void Update(GameTime gameTime, bool bossFightIsActive,
                           Vector2 playerPosition, Vector2 playerOrigin, 
                           int levelWidth, int levelHeight)
        {
            if (!inTransition)
            {
                Vector2 newPosition = playerPosition + playerOrigin - origin;
                    
                    if (Math.Abs(position.Y - newPosition.Y) > MaxCameraMovement && (bossFightIsActive || bossFightWasActive))
                    {
                        transitionUp = false;
                        transitionLeft = false;
                        if (newPosition.Y < position.Y)
                            transitionUp = true;
                        if (newPosition.X < position.X)
                            transitionLeft = true;

                        inTransition = true;
                        transitionTo = newPosition;
                        transitionTo.X = Math.Min(transitionTo.X, levelWidth - screenWidth);
                        transitionTo.Y = Math.Min(transitionTo.Y, levelHeight - screenHeight);
                    }
                    else
                    {
                        position = newPosition;
                    }
                    
            }
            else
            {
                bool horizontalDone = false;
                bool verticalDone = false;

                if (transitionLeft)
                {
                    position.X = Math.Max(transitionTo.X, position.X - MaxCameraMovement / 2f);
                    if (position.X <= transitionTo.X)
                        horizontalDone = true;
                }
                else
                {
                    position.X = Math.Min(transitionTo.X, position.X + MaxCameraMovement / 2f);
                    if (position.X >= transitionTo.X)
                        horizontalDone = true;
                }

                if (transitionUp)
                {
                    position.Y = Math.Max(transitionTo.Y, position.Y - MaxCameraMovement / 2f);
                    if (position.Y <= transitionTo.Y)
                        verticalDone = true;
                }
                else
                {
                    position.Y = Math.Min(transitionTo.Y, position.Y + MaxCameraMovement / 2f);
                    if (position.Y >= transitionTo.Y)
                        verticalDone = true;
                }

                if (horizontalDone && verticalDone)
                    inTransition = false;
            }
                
            LockToLevel(levelWidth, levelHeight);
            bossFightWasActive = bossFightIsActive;
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


        bool LockToLevel(int width, int height)
        {
            bool locked = false;
            if (position.X > width - screenWidth)
            {
                position.X = width - screenWidth;
                locked = true;
            }
            if (position.Y > height - screenHeight)
            {
                position.Y = height - screenHeight;
                locked = true;
            }

            if (position.X < 0)
            {
                position.X = 0;
                locked = true;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                locked = true;
            }

            return locked;
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