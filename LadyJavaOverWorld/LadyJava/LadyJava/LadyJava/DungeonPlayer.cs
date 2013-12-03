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

namespace LadyJava
{
    class DungeonPlayer : Player
    {
        const int RightDirection = 1;
        const int LeftDirection = -1;

        bool delayJump;
        bool isJumping;
        bool isFalling;

        int jumpTime;
        int delayJumpTime;

        const int jumpHeight = 15;
        const int delayJumpTimer = 350; //msecs
        const int jumpTimer = 400; //msecs

        Vector2 motion;

        private float movement = 3.7f;

        bool movingRight
        { get { return motion.X > 0; } }

        bool movingLeft
        { get { return motion.X < 0; } }

        bool isMovingOppositeDirection
        { get { return (movingRight && InputManager.IsKeyDown(Commands.Left)) ||
                       (movingLeft && InputManager.IsKeyDown(Commands.Right)); } }

        public DungeonPlayer(Sprite newSprite)
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
        
        public override Vector2 Update(GameTime gameTime,
                               int newNPC, //npc index
                               int levelWidth, int levelHeight,
                               BoundingBox[] entrances, BoundingBox[] talkingRadii,
                               params Object[] collisionObjects)
        {
            Vector2 entranceLocation = Global.InvalidVector2;
            Vector2 position = sprite.Position;
            previousPosition = sprite.Position;

            BoundingBox[] collisions = GetBoundingBoxes(collisionObjects);

            if (switchedTileMap)
                motion = Vector2.Zero;

            motion.Y = (Global.GravityAccelation / Global.PixelsToMeter) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.IsKeyDown(Commands.Jump) && 
                !isMovingOppositeDirection &&
                jumpDone && !isJumping && !isFalling && !delayJump)
            {
                jumpTime = 0;
                jumpDone = false;
                isJumping = true;
            }
            else if (!jumpDone)
            {
                motion = Jump(gameTime, motion, collisions);
            }
            else  if (motion.X == 0)
            {
                motion = initialMovement(motion, collisions);
            }
            else
            {
                motion = continuousMotion(motion, collisions);
            }

            motion = AdjustForCollision(position, motion, Width, Height, collisions);

            if (motion.Y != 0f)
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

            position += motion;
            position = LockToLevel(sprite.Width, sprite.Height, position, levelWidth, levelHeight);
            entranceLocation = EntranceCollision(motion, entrances);
            sprite.Update(gameTime, animation, position);

            return entranceLocation;
        }

        #region movement
        private Vector2 continuousMotion(Vector2 newMotion, BoundingBox[] collisions)
        {
            int direction = RightDirection;
            animation = Global.RIGHT;
            if (movingLeft)
            {
                direction = LeftDirection; 
                animation = Global.LEFT;
            }

            if(isMovingOppositeDirection)
            //if ((movingRight && !InputManager.IsKeyDown(Commands.Right)) ||
            //    (movingLeft && !InputManager.IsKeyDown(Commands.Left)))
            {
                if (movingRight && InputManager.IsKeyDown(Commands.Left))
                    animation = Global.LEFT;
                else if (movingLeft && InputManager.IsKeyDown(Commands.Right))
                    animation = Global.RIGHT;
                else if (!isJumping)
                    animation = Global.STILL;

                if (!isJumping || !isFalling)
                {
                    newMotion.X *= Global.GroundFriction;
                    if (Math.Abs(newMotion.X) < Global.Buffer)
                        newMotion.X = 0f;
                }
                else
                {
                    newMotion.X *= Global.AirFriction;
                    if (Math.Abs(newMotion.X) < Global.Buffer)
                        newMotion.X = 0f;
                }
            }
            else
                newMotion.X = direction * movement;
            
            return newMotion;
        }

        private Vector2 initialMovement(Vector2 newMotion, BoundingBox[] collisions)
        {
            animation = Global.STILL;
            if ((!switchedTileMap && InputManager.IsKeyDown(Commands.Right)) ||
                (switchedTileMap && InputManager.HasKeyBeenUp(Commands.Right)))
            {
                animation = Global.RIGHT;
                newMotion.X = movement;

                if (switchedTileMap)
                    switchedTileMap = false;
            }
            else if ((!switchedTileMap && InputManager.IsKeyDown(Commands.Left)) ||
                     (switchedTileMap && InputManager.HasKeyBeenUp(Commands.Left)))
            {
                animation = Global.LEFT;
                newMotion.X = -movement;

                if (switchedTileMap)
                    switchedTileMap = false;
            }

            return newMotion;
        }
        #endregion

        private Vector2 Jump(GameTime gameTime, Vector2 motion, BoundingBox[] collisions)
        {
            if (isMovingOppositeDirection)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
