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

using Engine;
using System.Collections.ObjectModel;

namespace Dungeon
{
    class Player
    {
        const int RightDirection = 1;
        const int LeftDirection = -1;

        bool jumpDone;
        bool delayJump;
        bool isJumping;
        bool isFalling;

        int jumpTime;
        int delayJumpTime;

        const int jumpHeight = 15;
        const int delayJumpTimer = 350; //msecs
        const int jumpTimer = 400; //msecs

        Vector2 motion;

        public Vector2 Position
        { get { return sprite.Position; } }

        public Vector2 Origin
        { get { return sprite.Origin; } }

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }

        BoundingBox boundingBox;

        private float movement = 2.4f;

        private Sprite sprite;

        string animation;

        void UpdateBounds(Vector2 newPosition, int width, int height)
        {
            boundingBox = new BoundingBox(new Vector3(newPosition.X, newPosition.Y, 0f),
                                          new Vector3(newPosition.X + width, newPosition.Y + height, 0f));
        }

        public Player(Sprite newSprite)
        {
            motion = Vector2.Zero; 
            
            animation = Global.STILL;
            sprite = newSprite;

            jumpDone = true;
            delayJump = false;
            isJumping = false;
            isFalling = false;

            UpdateBounds(Position, Width, Height);
        }

        Vector2 LockToLevel(int width, int height, Vector2 position, int levelW, int levelH)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > levelW - width)
                position.X = levelW - width;
            if (position.Y > levelH - height)
                position.Y = levelH - height;
            return position;
        }

        BoundingBox[] GetBoundingBoxes(Object[] objects)
        {
            Collection<BoundingBox> collisions = new Collection<BoundingBox>();

            for (int i = 0; i < objects.Length; i++)
                if (objects[i].GetType() == typeof(BoundingBox))
                    collisions.Add((BoundingBox)objects[i]);
                else if (objects[i].GetType().IsArray && objects[i].GetType().GetElementType() == typeof(BoundingBox))
                {
                    foreach (BoundingBox obj in (IEnumerable<BoundingBox>)objects[i])
                        collisions.Add(obj);
                }

            return collisions.ToArray<BoundingBox>();
        }

        #region Collision Detection
        Vector2 DownCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (newMotion.Y > 0f)
                    {
                        if (boundingBox.Max.Y > collisions[i].Min.Y &&
                            boundingBox.Max.Y < collisions[i].Max.Y)
                        {
                            newMotion.Y = collisions[i].Min.Y - Global.Buffer - Position.Y - Height;
                        }
                    }
                }
            }

            return newMotion;
        }

        Vector2 UpCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Min.Y < collisions[i].Max.Y &&
                        boundingBox.Min.Y > collisions[i].Min.Y)
                    {
                        newMotion.Y = collisions[i].Max.Y + Global.Buffer - Position.Y;
                    }
                }
            }

            return newMotion;
        }

        Vector2 RightCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Max.X > collisions[i].Min.X &&
                        boundingBox.Max.X < collisions[i].Max.X)
                    {
                        newMotion.X = collisions[i].Min.X - Global.Buffer - Position.X - Width;
                    }
                }
            }

            return newMotion;
        }

        Vector2 LeftCollision(Vector2 newMotion, BoundingBox[] collisions)
        {
            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Min.X < collisions[i].Max.X &&
                        boundingBox.Min.X > collisions[i].Min.X)
                    {
                        newMotion.X = collisions[i].Max.X + Global.Buffer - Position.X;
                    }
                }
            }

            return newMotion;
        }
        #endregion

        public void Update(GameTime gameTime, int levelWidth, int levelHeight, params Object[] collisionObjects)
        {
            Vector2 position = sprite.Position;

            BoundingBox[] collisions = GetBoundingBoxes(collisionObjects);

            motion.Y = (Global.GravityAccelation / Global.PixelsToMeter) * (float) gameTime.ElapsedGameTime.TotalSeconds;
            motion = DownCollision(motion, collisions);
            if (motion.Y > Global.Buffer)
                isFalling = true;
            else
            {
                isFalling = false;
                if (!delayJump && jumpDone && isJumping)
                {
                    delayJump = true;
                    delayJumpTime = 0;
                }
            }

            if (delayJump)
            {
                delayJumpTime += gameTime.ElapsedGameTime.Milliseconds;
                if (delayJumpTime > delayJumpTimer)
                {
                    isJumping = false;
                    delayJump = false;
                }
            }

            if (InputManager.IsKeyDown(Commands.Jump) && !isJumping && jumpDone && !isFalling && !delayJump)
            {
                jumpTime = 0;
                jumpDone = false;
                isJumping = true;
            }
            else if (!jumpDone)
            {
                motion = Jump(gameTime, motion);
            }
            else  if (motion.X == 0)
            {
                motion = initialMovement(motion, collisions);
            }
            else
            {
                if (motion.X > 0)
                {
                    motion = continuousMotion(motion, RightDirection, collisions);
                }
                else if (motion.X < 0)
                {
                    motion = continuousMotion(motion, LeftDirection, collisions);
                }
            }

            position += motion;
            position = LockToLevel(sprite.Width, sprite.Height, position, levelWidth, levelHeight);
            
            sprite.Update(gameTime, animation, position);
        }

        #region movement
        private Vector2 continuousMotion(Vector2 newMotion, int direction, BoundingBox[] collisions)
        {
            if(direction == RightDirection)
                animation = Global.RIGHT;
            else
                animation = Global.LEFT;

            if ((direction == RightDirection && !InputManager.IsKeyDown(Commands.Right)) ||
                (direction == LeftDirection && !InputManager.IsKeyDown(Commands.Left)))
            {
                if (direction == RightDirection && InputManager.IsKeyDown(Commands.Left))
                    animation = Global.LEFT;
                else if (direction == LeftDirection && InputManager.IsKeyDown(Commands.Right))
                    animation = Global.RIGHT;
                else
                    animation = Global.STILL;

                newMotion.X *= Global.GroundFriction;
                if (Math.Abs(newMotion.X) < Global.Buffer)
                    newMotion.X = 0f;
            }
            else
                newMotion.X = direction * movement;
            
            if (direction == RightDirection)
                newMotion = RightCollision(newMotion, collisions);
            else
                newMotion = LeftCollision(newMotion, collisions);

            return newMotion;
        }

        private Vector2 initialMovement(Vector2 newMotion, BoundingBox[] collisions)
        {
            animation = Global.STILL;
            if (InputManager.IsKeyDown(Commands.Right))
            {
                animation = Global.RIGHT;
                newMotion.X = movement;
                newMotion = RightCollision(newMotion, collisions);
            }
            else if (InputManager.IsKeyDown(Commands.Left))
            {
                animation = Global.LEFT;
                newMotion.X = -movement;
                newMotion = LeftCollision(newMotion, collisions);
            }

            return newMotion;
        }
        #endregion

        private Vector2 Jump(GameTime gameTime, Vector2 motion)
        {
            if ((InputManager.IsKeyDown(Commands.Left) && motion.X > 0) ||
               (InputManager.IsKeyDown(Commands.Right) && motion.X < 0))
                jumpDone = true;
            else if (jumpTime > jumpTimer)
                jumpDone = true;
            else
            {
                motion.Y -= jumpHeight;
                jumpTime += gameTime.ElapsedGameTime.Milliseconds;
            }

            return motion;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
