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
        Texture2D attackImage;

        bool isJumping;
        bool isFalling;

        bool isAttacking;
        Vector2 attackPosition;
        BoundingBox attackBounds;
        SpriteEffects attackDirection;

        int jumpTime;

        const int jumpHeight = 15;
        const int delayJumpTimer = 350; //msecs
        const int jumpTimer = 400; //msecs

        bool movingRight
        { get { return motion.X > 0; } }

        bool movingLeft
        { get { return motion.X < 0; } }

        bool isMovingOppositeDirection
        { get { return (movingRight && InputManager.IsKeyDown(Commands.Left)) ||
                       (movingLeft && InputManager.IsKeyDown(Commands.Right)); } }

        public DungeonPlayer(Sprite newSprite, Texture2D newAttackImage)
        {
            attackImage = newAttackImage;

            motion = Vector2.Zero; 
            
            animation = Global.Still;
            sprite = newSprite;
            cameraFocus = sprite.Position;

            jumpDone = true;
            isJumping = false;
            isFalling = false;
            inBossFight = false;

            facingDirection = Direction.Right;

            boundingBox = getBounds(Position, Width, Height);
        }

        void GenerateAttackBounds()
        {
            if (animation == Global.Attacking)
            {
                Vector2 attackPos = Vector2.Zero;
                if (facingDirection == Direction.Left)
                {
                    attackPos = new Vector2(Position.X - Width, Position.Y);
                    attackDirection = SpriteEffects.FlipHorizontally;
                }
                else if (facingDirection == Direction.Right)
                {
                    attackPos = new Vector2(Position.X + Width, Position.Y);
                    attackDirection = SpriteEffects.None;
                }
                attackBounds = new BoundingBox(new Vector3(attackPos, 0f),
                                               new Vector3(attackPos.X + attackImage.Width,
                                                           attackPos.Y + attackImage.Height, 0f));
                attackPosition = new Vector2(attackBounds.Min.X, attackBounds.Min.Y);
            }
            else
            {
                attackBounds = Global.InvalidBoundingBox;
                attackPosition = Global.InvalidVector2;
            }
        }

        public override Vector2 Update(GameTime gameTime,
                               int newNPC, //npc index
                               int finalNPC,  //final npc index
                               int levelWidth, int levelHeight,
                               Rectangle bossArea,
                               BoundingBox bossAreaTrigger,
                               BoundingBox[] entrances, 
                               BoundingBox[] talkingRadii,
                               params Object[] collisionObjects)
        {
            Vector2 entranceLocation = Global.InvalidVector2;
            Vector2 position = sprite.Position;

            BoundingBox[] collisions = GetBoundingBoxes(collisionObjects);

            motion = UpdateMotion(gameTime, position, motion, collisions);

            position += motion;

            CheckForBossFight(bossAreaTrigger, position);
            if (!inBossFight)
                position = LockToLevel(position, levelWidth, levelHeight);
            else
                position = LockToFightArea(position,
                                           new Vector2(bossArea.X - bossArea.Width, bossArea.Y - bossArea.Height),
                                           bossArea.X + bossArea.Width, bossArea.Y + bossArea.Height);
            entranceLocation = EntranceCollision(motion, entrances);
            animation = sprite.Update(gameTime, animation, position, facingDirection);

            GenerateAttackBounds();
            return entranceLocation;
        }

        private void CheckForBossFight(BoundingBox bossArea, Vector2 position)
        {
            if (!inBossFight)
            {
                if (movingLeft)
                {
                    if (getBounds(position + new Vector2(Width, 0f), Width, Height).Intersects(bossArea))
                        inBossFight = true;

                }
                else if (movingRight)
                {
                    if (getBounds(position, Width, Height).Intersects(bossArea))
                        inBossFight = true;

                }
            }
        }

        private Vector2 UpdateMotion(GameTime gameTime, 
                                     Vector2 currentPosition, Vector2 newMotion, 
                                     BoundingBox[] collisions)
        {
            if (switchedTileMap)
                newMotion = Vector2.Zero;

            newMotion.Y = (Global.GravityAccelation / Global.PixelsToMeter) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.HasKeyBeenUp(Commands.Attack) && !isAttacking)
            {
                isAttacking = true;
                if (animation == Global.Moving)
                    animation = Global.Attacking;
                else
                    animation = Global.StartingAttack;
            }
            else if (((!switchedTileMap && InputManager.HasKeyBeenUp(Commands.Jump)) ||
                     (switchedTileMap && InputManager.HasKeyBeenUp(Commands.Jump)))
                     &&
                     !isMovingOppositeDirection &&
                     jumpDone && !isJumping && !isFalling)// && !delayJump)
            {
                jumpTime = 0;
                jumpDone = false;
                isJumping = true;

                switchedTileMap = false;
            }
            else if (!jumpDone)
            {
                newMotion = Jump(gameTime, newMotion, collisions);
            }
            else if (newMotion.X == 0 && !isAttacking)
            {
                newMotion = initialMovement(newMotion, collisions);
            }
            else //if (!isAttacking)
            {
                newMotion = continuousMotion(newMotion, collisions);
            }

            newMotion = AdjustForCollision(currentPosition, newMotion, Width, Height, collisions, true);

            if (isAttacking &&
                sprite.CurrentAnimationName == Global.Attacking &&
                sprite.CurrentAnimation.Status == AnimationStatus.Stopped)
                    isAttacking = false;


            if (newMotion.Y != 0f)
                isFalling = true;
            else
            {
                isFalling = false;
                isJumping = false;
            }

            return newMotion;
        }

        #region movement
        private Vector2 continuousMotion(Vector2 newMotion, BoundingBox[] collisions)
        {
            int direction = Direction.Right.GetHashCode();// FacingRight;

            if (!isAttacking)
            {
                animation = Global.Moving;
                facingDirection = Direction.Right;

                if (movingLeft)
                {
                    direction = Direction.Left.GetHashCode(); // FacingLeft; 
                    animation = Global.Moving;
                    facingDirection = Direction.Left;
                }

                if ((movingRight && !InputManager.IsKeyDown(Commands.Right)) ||
                    (movingLeft && !InputManager.IsKeyDown(Commands.Left)))
                {
                    if (movingRight && InputManager.IsKeyDown(Commands.Left))
                    {
                        animation = Global.Moving;
                        facingDirection = Direction.Left;
                    }
                    else if (movingLeft && InputManager.IsKeyDown(Commands.Right))
                    {
                        animation = Global.Moving;
                        facingDirection = Direction.Right;
                    }
                    else if (!isJumping)
                        animation = Global.Still;


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
                //still moving
                else
                    newMotion.X = direction * movement;
            }
            //slow down
            else
                newMotion.X *= Global.AttackFriction;
            
            return newMotion;
        }

        private Vector2 initialMovement(Vector2 newMotion, BoundingBox[] collisions)
        {
            animation = Global.Still;
            if ((!switchedTileMap && InputManager.IsKeyDown(Commands.Right)) ||
                (switchedTileMap && InputManager.HasKeyBeenUp(Commands.Right)))
            {
                animation = Global.Moving;
                facingDirection = Direction.Right;

                newMotion.X = movement;

                if (switchedTileMap)
                    switchedTileMap = false;
            }
            else if ((!switchedTileMap && InputManager.IsKeyDown(Commands.Left)) ||
                     (switchedTileMap && InputManager.HasKeyBeenUp(Commands.Left)))
            {
                animation = Global.Moving;
                facingDirection = Direction.Left;
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

        public override void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            sprite.Draw(spriteBatch, transparency);
            if (isAttacking &&
                sprite.CurrentAnimationName == Global.Attacking)
                    spriteBatch.Draw(attackImage, attackPosition, null, Color.White, 0f, Vector2.Zero, sprite.Scale, attackDirection, 0f);
        }
    }
}
