using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    class Thwomp : Boss
    {
        const int FallBuffer = 5;
        const float normalSpeed = 8f;
        const float attackSpeed = 9f;
        const int stunTimer = 1500;

        int nextAttackTimer = 200;
        
        float stunTime = 0;
        float nextAttackTime = 0;
       
        public Thwomp(Texture2D newImage, AnimationInfo[] newAnimationInfo) :
                 this(new Sprite(newImage, Vector2.Zero, newAnimationInfo, 1f))
        { }

        Thwomp(Sprite newSprite)
        {
            hp = MaxHP;
            type = Global.Thwomp;

            sprite = newSprite;
            //position = sprite.Position;
            fightAreaTrigger = Global.InvalidBoundingBox;

            movement = new Dictionary<EnemyStatus, float>();
            movement.Add(EnemyStatus.Moving, normalSpeed);
            movement.Add(EnemyStatus.Stunned, normalSpeed);
            movement.Add(EnemyStatus.Attacking, attackSpeed);
            movement.Add(EnemyStatus.Hurt, normalSpeed);

            //moveCount = 1;

            status =  EnemyStatus.Moving;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, BoundingBox playerBounds, int screenWidth, int screenHeight, bool inBossFight)
        {
           Vector2 position = sprite.Position;
            if (startPosition.Y == 0 && position.Y != 0)
                startPosition = position;

            playerHit = false;
            //move if in range
            if (inBossFight)
            {
                CheckInvincibleDone(gameTime);
                if ((status == EnemyStatus.Moving || status == EnemyStatus.Hurt) && position.Y > startPosition.Y)
                {
                    targetPosition = new Vector2(position.X, startPosition.Y);
                    Vector2 distance = position - targetPosition;
                    distance.Normalize();
                    position.Y -= distance.Y * movement[status];
                    if (position.Y <= startPosition.Y)
                    {
                        position = targetPosition;
                        //targetPosition = Global.InvalidVector2;
                        //if (status == EnemyStatus.Hurt)
                        //    status = EnemyStatus.Moving;
                    }
                    
                }
                else if (status == EnemyStatus.Moving && position.X != playerPosition.X - Origin.X && !attacking)
                {
                    int buffer = 15;
                    
                    Vector2 topLeft = new Vector2(fightArea.Left - screenWidth / 2f, fightArea.Top - screenHeight / 2f);
                    Vector2 distance = playerPosition - Origin - position;
                    distance.Normalize();
                    position.X = position.X + movement[status] * distance.X;
                    if (position.X > topLeft.X + screenWidth + buffer - Width)
                        position.X = topLeft.X + screenWidth + buffer - Width;
                    if (position.X < topLeft.X + buffer)
                        position.X = topLeft.X + buffer;

                    distance = playerPosition - Origin - position;
                    if(Math.Abs(distance.X) < FallBuffer)
                    {
                        attacking = true;
                        nextAttackTime = 0;
                        targetPosition = playerPosition + new Vector2(0, 48);
                    }
                }
                else if(attacking)
                {
                    status = EnemyStatus.Attacking;
                    if (nextAttackTime >= nextAttackTimer)
                    {
                        Vector2 distance = playerPosition - position;
                        distance.Normalize();
                        position.Y += distance.Y * movement[status];

                        if (position.Y + Height > targetPosition.Y)
                        {
                            position.Y = targetPosition.Y - Height;
                            status = EnemyStatus.Stunned;
                            attacking = false;
                            stunTime = 0;
                        }
                    }
                    else
                        nextAttackTime += (float)gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (status == EnemyStatus.Stunned)
                {
                    stunTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (stunTime >= stunTimer)
                    {
                        status = EnemyStatus.Moving;
                        targetPosition = new Vector2(position.X, startPosition.Y);
                    }
                }
                
                boundingBox = getBounds(position);
                if (!playerHit && status != EnemyStatus.Hurt && boundingBox.Intersects(playerBounds))
                {
                    playerHit = true;
                    //hit player, run away
                    status = EnemyStatus.Moving;
                    attacking = false;
                    nextAttackTime = 0;
                }

                currentAnimation = sprite.Update(gameTime, status.ToString(), position, Direction.Right);
            }
        }

        /*
        public override void SetPosition(Vector2 newPosition)
        {
            sprite.SetPosition(newPosition);
        }
        */
        public override void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            sprite.Draw(spriteBatch, transparency);
        }

    }
}
