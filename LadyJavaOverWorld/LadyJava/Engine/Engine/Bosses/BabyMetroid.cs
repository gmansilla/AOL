using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine;

namespace Engine
{
    public class BabyMetroid : Boss
    {
        Random rand = new Random();

        const int MaxMoves = 3;
        const int MaxAttackSpots = 4;
        const int MovementBuffer = 5;
        const float radius = 15f;
        const float normalSpinSpeed = 5f;
        const float attackSpinSpeed = 10f;
        const float normalSpeed = 4;
        const float attackSpeed = 8;

        Dictionary<EnemyStatus, float> spinSpeed;

        Vector2 position;

        //Vector2[] attackPositions;

        public BabyMetroid(Texture2D newImage, AnimationInfo[] newAnimationInfo) :
                      this(new Sprite(newImage, Vector2.Zero, newAnimationInfo, 1f))
        { }

        BabyMetroid(Sprite newSprite)
        {
            hp = MaxHP;
            type = Global.BabyMetroid;

            sprite = newSprite;
            position = sprite.Position;
            fightAreaTrigger = Global.InvalidBoundingBox;

            movement = new Dictionary<EnemyStatus, float>();
            movement.Add(EnemyStatus.Moving, normalSpeed);
            movement.Add(EnemyStatus.Stunned, normalSpeed);
            movement.Add(EnemyStatus.Attacking, attackSpeed);
            movement.Add(EnemyStatus.Hurt, normalSpeed);

            spinSpeed = new Dictionary<EnemyStatus, float>();
            spinSpeed.Add(EnemyStatus.Moving, normalSpinSpeed);
            spinSpeed.Add(EnemyStatus.Stunned, normalSpinSpeed);
            spinSpeed.Add(EnemyStatus.Attacking, attackSpinSpeed);
            spinSpeed.Add(EnemyStatus.Hurt, normalSpinSpeed);
            status = EnemyStatus.Moving;
            moveCount = 1;

            currentAnimation = Global.Moving;
        }


        void CreateAttackPositions()
        {
            Vector2 start = new Vector2(fightArea.X - fightArea.Width, fightArea.Y - fightArea.Height);
            Vector2 buffer = position - start;

            attackPositions = new Vector2[MaxAttackSpots];

            attackPositions[0] = position;
            attackPositions[1] = new Vector2(start.X + FightAreaWidth / 2 - Width / 2, position.Y);
            attackPositions[2] = new Vector2(start.X + FightAreaWidth - buffer.X - Width, position.Y);
            attackPositions[3] = new Vector2(attackPositions[1].X, start.Y + FightAreaHeight / 2);
            
            attackSpot = 0;
            targetPosition = attackPositions[attackSpot];
            //position = targetPosition;
            SetPosition(targetPosition);
            //currentAnimation = sprite.Update(gameTime, currentAnimation, circlePosition, Direction.Right);

        }

        Vector2 circlePosition;
        bool pauseTimer = false;
        float currentAngle = 0;
        int attackSpot;
        int moveCount;

        public override void Update(GameTime gameTime, Vector2 playerPosition, BoundingBox playerBounds, int screenWidth, bool inBossFight)
        {

            if (attackPositions == null && fightArea != new Rectangle())
                CreateAttackPositions();

            playerHit = false;
            //move if in range
            if (inBossFight)//Math.Abs(position.X - playerPosition.X) < screenWidth * 0.8f)
            {
                
                CheckInvincibleDone(gameTime);

                if (!pauseTimer)
                    changePositionTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (status != EnemyStatus.Stunned)
                {
                    if (changePositionTime >= changePositionTimer)
                    {
                        pauseTimer = true;

                        changePositionTime = 0;
                        if (status == EnemyStatus.Moving || status == EnemyStatus.Hurt)
                        {
                            int newAttackSpot;
                            do
                                newAttackSpot = rand.Next(0, MaxAttackSpots);
                            while (newAttackSpot == attackSpot);
                            attackSpot = newAttackSpot;
                            targetPosition = attackPositions[attackSpot];
                        }
                        else if (status == EnemyStatus.Attacking)
                        {
                            targetPosition = playerPosition;
                        }
                    }
                }
                else if (changePositionTime >= changePositionTimer)
                {
                    status = EnemyStatus.Moving;
                    pauseTimer = false;
                }

                if (pauseTimer)
                {
                    Vector2 distance = targetPosition - position;
                    if (Math.Abs(distance.X) > MovementBuffer ||
                        Math.Abs(distance.Y) > MovementBuffer)
                    {
                        distance.Normalize();
                        position += distance * movement[status];
                    }
                    else
                    {
                        pauseTimer = false;
                        playerHit = false;
                        Random rand = new Random();
                        if (status == EnemyStatus.Attacking)
                            status = EnemyStatus.Stunned;
                        else if (moveCount >= MaxMoves)
                        {
                            status = EnemyStatus.Attacking;
                            moveCount = 0;
                        }
                        else
                        {
                            if (rand.Next(0, 2) == 1)
                            {
                                status = EnemyStatus.Attacking;
                                moveCount = 0;
                            }
                            else
                            {
                                status = EnemyStatus.Moving;
                                moveCount++;
                            }
                        }

                    }
                }

                //Vector2 distanceToTarget = targetPosition - position;
                //distanceToTarget.X = Math.Abs(distanceToTarget.X);
                //distanceToTarget.Y = Math.Abs(distanceToTarget.Y);
                if (status != EnemyStatus.Stunned)
                {
                    currentAngle += spinSpeed[status];
                    if (currentAngle > 360)
                        currentAngle = currentAngle - 360;
                    circlePosition = findPointOnCircle(position, currentAngle);
                }
                boundingBox = getBounds(circlePosition);
                if (!playerHit && status != EnemyStatus.Hurt && boundingBox.Intersects(playerBounds))
                {
                    playerHit = true;
                    //hit player, run away
                    status = EnemyStatus.Moving;
                    pauseTimer = false;
                    changePositionTime = changePositionTimer;
                }

                currentAnimation = sprite.Update(gameTime, currentAnimation, circlePosition, Direction.Right);
            }

        }

        Vector2 findPointOnCircle(Vector2 pos, float angle)
        {
            return new Vector2((int)(pos.X + radius * (float)Math.Cos(MathHelper.ToRadians(angle))),
                               (int)(pos.Y + radius * (float)Math.Sin(MathHelper.ToRadians(angle))));
        }

        public override void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            sprite.SetPosition(newPosition);
        }

        public override void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            sprite.Draw(spriteBatch, transparency);
        }

    }
}
