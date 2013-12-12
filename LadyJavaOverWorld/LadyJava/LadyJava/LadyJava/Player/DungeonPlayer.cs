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

        Vector2 hpLocation;

        bool isJumping;
        bool isFalling;

        Vector2 attackPosition;
        BoundingBox attackBounds;
        SpriteEffects attackDirection;

        const int jumpHeight = 15;
        //const int delayJumpTimer = 350; //msecs
        const int jumpTimer = 400; //msecs

        bool movingRight
        { get { return motion.X > 0; } }

        bool movingLeft
        { get { return motion.X < 0; } }

        bool isMovingOppositeDirection
        { get { return (movingRight && InputManager.IsKeyDown(Commands.Left)) ||
                       (movingLeft && InputManager.IsKeyDown(Commands.Right)); } }

        public DungeonPlayer(Sprite newSprite, Texture2D newAttackImage, Texture2D newHPMarker, int screenWidth, int screenHeight)
        {
            attackImage = newAttackImage;
            hpMarker = newHPMarker;

            motion = Vector2.Zero; 
            
            animation = Global.Still;
            sprite = newSprite;
            cameraFocus = sprite.Position;

            jumpDone = true;
            isJumping = false;
            isFalling = false;
            inBossFight = false;

            facingDirection = Direction.Right;

            interactingWith = new List<int>();

            //hpOffsets = new Vector2(screenWidth * 0.075f, screenHeight * 0.925f);

            //boundingBox = getBounds(Position);
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
                                       Camera camera,
                                       int[] newNPC, //npc index
                                       int finalNPC,  //final npc index
                                       int levelWidth, int levelHeight,
                                       bool playerHit,
                                       Rectangle bossArea,
                                       bool bossIsAlive,
                                       BoundingBox bossAreaTrigger,
                                       BoundingBox[] entrances,
                                       BoundingBox[] talkingRadii,
                                       BoundingBox[] enemyBounds,
                                       params Object[] collisionObjects)
        {
            Vector2 entranceLocation = Global.InvalidVector2;

            Vector2 position = sprite.Position;
            previousPosition = sprite.Position;


            BoundingBox[] collisions = GetBoundingBoxes(collisionObjects);

            CheckInvincibleDone(gameTime);

            if (isAlive)
            {
                if (inBossFight && !bossIsAlive)
                    inBossFight = false;

                interactingWith = new List<int>();
                foreach (int npc in newNPC)
                    if (npc != Global.InvalidInt)
                        interactingWith.Add(npc);

                motion = UpdateMotion(gameTime, position, motion, collisions);

                position += motion;

                CheckForBossFight(bossAreaTrigger, position);

                if (!inBossFight)
                    position = LockToLevel(position, levelWidth, levelHeight);
                else
                    position = LockToFightArea(position, Origin,
                                               new Vector2(bossArea.X - bossArea.Width, bossArea.Y - bossArea.Height),
                                               bossArea.X + bossArea.Width, bossArea.Y + bossArea.Height);
                entranceLocation = EntranceCollision(motion, entrances);
                animation = sprite.Update(gameTime, animation, position, facingDirection);

                //change to screen width and height
                Vector2 hpMotion = motion;
                //if(levelLocked)
                //    hpMotion = Vector2.Zero;

                hpOffsets = new Vector2(hpOffsets.X, camera.ScreenHeight - hpOffsets.X - hpMarker.Height / 2);
                hpLocation = camera.Position + hpMotion + new Vector2(hpOffsets.X, hpOffsets.Y);
                if (!inBossFight)
                    hpLocation = LockToLevel(hpLocation,
                                             motion,
                        //new Vector2(hpMarker.Width * hp / 2, hpMarker.Height / 2),
                                             camera.ScreenWidth, camera.ScreenHeight,
                                             levelWidth, levelHeight);
                else
                    hpLocation = LockToFightArea(hpLocation, //new Vector2(hpMarker.Width * hp / 2, hpMarker.Height / 2),
                                                 camera.ScreenWidth, camera.ScreenHeight,
                                                 new Vector2(bossArea.X - bossArea.Width, bossArea.Y - bossArea.Height),
                                                 bossArea.X + bossArea.Width, bossArea.Y + bossArea.Height);

                GenerateAttackBounds();
                boundingBox = getHitBounds(position + Origin, (int)Origin.X / 2);
                interactingWith = new List<int>();
                for (int i = 0; i < enemyBounds.Length; i++)
                    if (attackBounds.Intersects(enemyBounds[i]) &&
                        ((facingDirection == Direction.Right && attackBounds.Min.X < enemyBounds[i].Min.X) ||
                         (facingDirection == Direction.Left && attackBounds.Max.X > enemyBounds[i].Max.X)))
                        interactingWith.Add(i);

            }
            else
            {
                animation = Global.Dying;
                motion = UpdateMotion(gameTime, position, motion, collisions);
                animation = sprite.Update(gameTime, animation, position, facingDirection);
                //if (sprite.CurrentAnimation.Status == AnimationStatus.Stopped)
                //    ResetPlayer();
            }

            return entranceLocation;
        }

        private void CheckForBossFight(BoundingBox bossArea, Vector2 position)
        {

            if (!inBossFight)
            {
                if (movingLeft)
                {
                    if (getBounds(position + new Vector2(Width, 0f)).Intersects(bossArea))//, Width, Height).Intersects(bossArea))
                        inBossFight = true;

                }
                else if (movingRight)
                {
                    if (getBounds(position).Intersects(bossArea))//, Width, Height).Intersects(bossArea))
                        inBossFight = true;

                }
            }
        }

        private Vector2 UpdateMotion(GameTime gameTime, 
                                     Vector2 currentPosition, Vector2 newMotion, 
                                     BoundingBox[] collisions)
        {
            if(isAlive)
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
            if (invincible && (int)invincibleTime % 2 == 0)
            { }
            else
            {
                sprite.Draw(spriteBatch, transparency);
                if (isAttacking &&
                    sprite.CurrentAnimationName == Global.Attacking)
                    spriteBatch.Draw(attackImage, attackPosition, null, Color.White, 0f, Vector2.Zero, sprite.Scale, attackDirection, 0f);
            }

            for (int i = 0; i < hp; i++)
            {
                spriteBatch.Draw(hpMarker, hpLocation, transparency);
                hpLocation.X += hpMarker.Width;

            }
        }
    }
}
