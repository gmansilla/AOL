using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace LadyJava
{
    abstract class Player
    {
        protected Texture2D hpMarker;

        protected bool isAttacking;

        protected int jumpTime;

        protected const int MaxHP = 3;
        protected int hp = MaxHP;
        protected Vector2 hpOffsets = new Vector2(30, 0);

        protected bool isAlive
        { get { return hp > 0; } }

        protected bool animationDone
        { get { return sprite.CurrentAnimation.Status == AnimationStatus.Stopped; } }

        public bool PlayerNeedsReset
        { get { return !isAlive && animationDone; } }

        protected const float movement = 3.7f;

        protected Direction facingDirection;

        public abstract void Draw(SpriteBatch spriteBatch, Color transparency);

        public abstract Vector2 Update(GameTime gameTime,
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
                                       params Object[] collisionObjects);

        protected Vector2 motion;

        protected bool invincible = false;

        protected Sprite sprite;
        protected string animation;
        protected bool switchedTileMap;

        protected bool jumpDone;

        protected float invincibleTime;
        const int invincibleTimer = 3000;
        
        protected bool inBossFight;
        public bool InBossFight
        { get { return inBossFight; } }

        protected BoundingBox boundingBox;
        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }

        protected bool speakingToFinalNPC;

        protected bool finishedTalkingToFinalNPC;
        public bool SpokeWithFinalNPC
        { get { return finishedTalkingToFinalNPC; } }

        protected Vector2 previousPosition;
        public Vector2 PreviousPosition
        { get { return previousPosition; } }


        protected List<int> interactingWith;
        public int[] InteractingWith
        { get { return interactingWith.ToArray(); } }

        protected PlayState currentPlayState;
        public PlayState CurrentPlayState
        { get { return currentPlayState; } }
        
        public Vector2 Position
        { get { return sprite.Position; } }

        public Vector2 FeetPosition
        { get { return Position + new Vector2(0, Height); } }
        
        public Vector2 Origin
        { get { return sprite.Origin; } }

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        public Vector2 Motion
        { get { return motion; } }

        //position needs to adjust based of width or height change in sprite class
        protected Vector2 cameraFocus;
        public Vector2 CameraFocus
        { get { return cameraFocus; } }

        protected Vector2 EntranceCollision(Vector2 newMotion, BoundingBox[] newEntrances)
        {
            boundingBox = getBounds(Position + newMotion);//, Width, Height);
            for (int i = 0; i < newEntrances.Length; i++) //For each tile
            {
                if (boundingBox.Intersects(newEntrances[i])) //compare Lady J's box with another square. 
                {
                    return new Vector2(newEntrances[i].Min.X, newEntrances[i].Min.Y);
                }
            }
            return Global.InvalidVector2;
        }

        public void WasHit()
        {
            if (!invincible)
            {
                //currentAnimation = Global.Hurt;
                //status = EnemyStatus.Hurt;
                hp--;
                if(isAlive)
                    SetInvincible();
            }
        }

        protected void SetInvincible()
        {
            invincibleTime = 0;
            invincible = true;
        }

        protected void CheckInvincibleDone(GameTime gameTime)
        {
            if (invincible)
            {
                invincibleTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (invincibleTime >= invincibleTimer)
                {
                    invincible = false;
                    //currentAnimation = Global.Moving;
                }
            }
        }

        public void ResetPlayer(Vector2 startPosition, int tileWidth, int tileHeight)
        {
            hp = MaxHP;
            jumpDone = true;
            jumpTime = 0;
            isAttacking = false;
            inBossFight = false;
            SetPosition(startPosition, tileWidth, tileHeight, false, true);
        }

        protected Vector2 LockToLevel(Vector2 position, int levelW, int levelH)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > levelW - Width)
                position.X = levelW - Width;
            if (position.Y > levelH - Height)
                position.Y = levelH - Height;

            return position;
        }
        
        protected Vector2 LockToLevel(Vector2 position, int levelW, int levelH, out bool locked)
        {
            locked = false;
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
            if (position.X > levelW - Width)
            {
                position.X = levelW - Width;
                locked = true;
            }
            if (position.Y > levelH - Height)
            {
                position.Y = levelH - Height;
                locked = true;
            }

            return position;
        }

        protected Vector2 LockToLevel(Vector2 position, Vector2 motion, int screenWidth, int screenHeight, float areaWidth, float areaHeight)
        {
            //also adjust by the origin of the player
            //if (position.X < origin.X)
            //    position.X = origin.X;
            //if (position.Y < origin.Y)
            //    position.Y = origin.Y;

            if (motion.X < 0 && areaWidth - position.X + hpOffsets.X + motion.X <= screenWidth)// - origin.X)
                position.X = areaWidth - screenWidth + hpOffsets.X;//+ origin.X;
            else if (areaWidth - position.X + hpOffsets.X <= screenWidth)// - origin.X)
                position.X = areaWidth - screenWidth + hpOffsets.X;//+origin.X;
            if (motion.Y < 0 && areaHeight - position.Y + hpOffsets.Y + motion.Y <= screenHeight)// - origin.X)
                position.Y = areaHeight - screenHeight + hpOffsets.Y;//+ origin.X;
            else if (areaHeight - position.Y + hpOffsets.Y <= screenHeight)// - origin.X)
                position.Y = areaHeight - screenHeight + hpOffsets.Y;//+origin.X;


            //if (position.Y > areaHeight - origin.Y)
            //    position.Y = areaHeight - origin.Y;
            return position;
        }

        protected Vector2 LockToFightArea(Vector2 position, int screenWidth, int screenHeight, Vector2 areaStart, float areaWidth, float areaHeight)
        {
            if (motion.X < 0 && areaWidth - position.X + hpOffsets.X + motion.X <= screenWidth)
                position.X = areaWidth - screenWidth + hpOffsets.X * 1.5f;
            else if (areaWidth - position.X + hpOffsets.X <= screenWidth)
                position.X = areaWidth - screenWidth + hpOffsets.X * 1.5f;
            if (motion.Y < 0 && areaHeight - position.Y + hpOffsets.Y + motion.Y <= screenHeight)
                position.Y = areaHeight - screenHeight + hpOffsets.Y + hpMarker.Height / 2;
            else if (areaHeight - position.Y + hpOffsets.Y <= screenHeight)
                position.Y = areaHeight - screenHeight + hpOffsets.Y + hpMarker.Height / 2;

            return position;
        }

        protected Vector2 LockToFightArea(Vector2 position, Vector2 origin, Vector2 areaStart, float areaWidth, float areaHeight)
        {
            //also adjust by the origin of the player
            if (position.X < areaStart.X + origin.X)
                position.X = areaStart.X + origin.X;
            if (position.Y < areaStart.Y + origin.Y)
                position.Y = areaStart.Y + origin.Y;
            if (position.X > areaWidth - origin.X)
                position.X = areaWidth - origin.X;
            if (position.Y > areaHeight - origin.Y)
                position.Y = areaHeight - origin.Y;
            return position;
        }

        protected BoundingBox[] GetBoundingBoxes(Object[] objects)
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

        public void SetPosition(Vector2 newPosition, int tileWidth, int tileHeight, bool centreToTile, bool switchingTileMap)
        {
            if (centreToTile)
            {
                Vector2 offsets = new Vector2(tileWidth / 2f - Width / 2f, tileHeight / 2f - Height / 2f);
                sprite.SetPosition(newPosition + offsets);
            }
            else
                sprite.SetPosition(newPosition);

            cameraFocus = sprite.Position;
            switchedTileMap = switchingTileMap;
        }
        
        protected BoundingBox NoCollision(BoundingBox bounds, BoundingBox[] collisions)
        {
            foreach (BoundingBox collision in collisions)
                if (collision.Intersects(bounds))
                    return collision;

            return Global.InvalidBoundingBox;
        }

        protected Vector2 AdjustForCollision(Vector2 position, Vector2 newMotion,
                                             int width, int height,
                                             BoundingBox[] collisions,
                                             bool checkRightCollision)
        {
            Vector2 newPosition = position;
            int incrementCount = (int)(newMotion.Length() * 2) + 1;
            Vector2 increment = newMotion / incrementCount;

            for (int i = 1; i <= incrementCount; i++)
            {
                Vector2 adjustedPosition = position + increment * i;
                BoundingBox newBounds = getBounds(adjustedPosition);//, width, height);

                BoundingBox collision = NoCollision(newBounds, collisions);
                if (collision == Global.InvalidBoundingBox)
                {
                    newPosition = adjustedPosition;
                }
                else
                {
                    bool isDiagonalMove = newMotion.X != 0 && newMotion.Y != 0;
                    if (isDiagonalMove)
                    {
                        int stepsLeft = incrementCount - (i - 1);

                        Vector2 newMotionX = increment.X * Vector2.UnitX * stepsLeft;
                        Vector2 newPositionX =
                            AdjustForCollision(newPosition, newMotionX, width, height, collisions, true);
                        newPosition += newPositionX;

                        Vector2 newMotionY = increment.Y * Vector2.UnitY * stepsLeft;
                        Vector2 newPositionY =
                            AdjustForCollision(newPosition, newMotionY, width, height, collisions, false);
                        newPosition += newPositionY;
                    }
                    if ((newPosition - position).Y == 0 && newMotion.Y < 0)
                        jumpDone = true;

                    break;
                }
            }
            
            return newPosition - position;
        }

        protected BoundingBox getBounds(Vector2 newPosition)//, int width, int height)
        {
            return new BoundingBox(new Vector3(newPosition, 0f),
                                   new Vector3(newPosition.X + Width, newPosition.Y + Height, 0f));
        }

        protected BoundingBox getHitBounds(Vector2 newPosition, int radius)//, int width, int height)
        {

            return new BoundingBox(new Vector3(newPosition.X - radius, newPosition.Y - radius, 0f),
                                   new Vector3(newPosition.X + radius, newPosition.Y + radius, 0f));
        }
    }
}
