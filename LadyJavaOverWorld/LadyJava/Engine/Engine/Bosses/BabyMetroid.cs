using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Engine;

namespace Engine
{
    public enum BossStatus
    {
        Moving,
        Attacking,
        Stunned,
        Hurt,
    }

    public class BabyMetroid : Boss
    {
        Random rand = new Random();

        const int MaxHP = 5;
        const int MaxMoves = 3;
        const int MaxAttackSpots = 4;
        const int MovementBuffer = 5;
        const float radius = 15f;
        const float normalSpinSpeed = 5f;
        const float attackSpinSpeed = 10f;
        const float normalSpeed = 4;
        const float attackSpeed = 8;

        Dictionary<BossStatus, float> movement;
        Dictionary<BossStatus, float> spinSpeed;

        Vector2 targetPosition;

        Vector2 position;

        Vector2[] attackPositions;

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

            movement = new Dictionary<BossStatus, float>();
            movement.Add(BossStatus.Moving, normalSpeed);
            movement.Add(BossStatus.Stunned, normalSpeed);
            movement.Add(BossStatus.Attacking, attackSpeed);

            spinSpeed = new Dictionary<BossStatus, float>();
            spinSpeed.Add(BossStatus.Moving, normalSpinSpeed);
            spinSpeed.Add(BossStatus.Stunned, normalSpinSpeed);
            spinSpeed.Add(BossStatus.Attacking, attackSpinSpeed);
            bossStatus = BossStatus.Moving;
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
        }

        Vector2 circlePosition;
        const int changePositionTimer = 2500; //2.5secs
        float changePositionTime = 0;
        bool pauseTimer = false;
        BossStatus bossStatus;
        float currentAngle = 0;
        int attackSpot;
        int moveCount;
        public override void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (attackPositions == null && fightArea != new Rectangle())
                CreateAttackPositions();

            if(!pauseTimer)
                changePositionTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (bossStatus != BossStatus.Stunned)
            {
                if (changePositionTime >= changePositionTimer)
                {
                    pauseTimer = true;
                    changePositionTime = 0;
                    if (bossStatus == BossStatus.Moving)
                    {
                        int newAttackSpot;
                        do
                            newAttackSpot = rand.Next(0, MaxAttackSpots);
                        while (newAttackSpot == attackSpot);
                        attackSpot = newAttackSpot;
                        targetPosition = attackPositions[attackSpot];
                    }
                    else if (bossStatus == BossStatus.Attacking)
                    {
                        targetPosition = playerPosition;
                        float distance = (int)(targetPosition - position).Length();
                        float updateCount = distance / movement[bossStatus];
                        float newAngle = (updateCount * spinSpeed[bossStatus] + currentAngle) % 360;

                        Vector2 newTargetPosition = findPointOnCircle(targetPosition, newAngle);
                        int newY = -(int)(newTargetPosition.Y - targetPosition.Y);
                        
                        position -= new Vector2(0, newY);
                    }
                }
            }
            else if (changePositionTime >= changePositionTimer)
            {
                bossStatus = BossStatus.Moving;
                pauseTimer = false;
            }

            if (pauseTimer)
            {
                Vector2 distance = targetPosition - position;
                if (Math.Abs(distance.X) > MovementBuffer ||
                    Math.Abs(distance.Y) > MovementBuffer)
                {
                    distance.Normalize();
                    position += distance * movement[bossStatus];
                }
                else 
                {
                    pauseTimer = false;
                    Random rand = new Random();
                    if (bossStatus == BossStatus.Attacking)
                        bossStatus = BossStatus.Stunned;
                    else if (moveCount >= MaxMoves)
                    {
                        bossStatus = BossStatus.Attacking;
                        moveCount = 0;
                    }
                    else
                        if (rand.Next(0, 2) == 1)
                        {
                            bossStatus = BossStatus.Attacking;
                            moveCount = 0;
                        }
                        else
                        {
                            bossStatus = BossStatus.Moving;
                            moveCount++;
                        }
                }
            }

            if (bossStatus != BossStatus.Stunned)
            {
                currentAngle += spinSpeed[bossStatus];
                if (currentAngle > 360)
                    currentAngle = currentAngle - 360;
                circlePosition = findPointOnCircle(position, currentAngle);
            }
            boundingBox = getBounds(circlePosition);
            currentAnimation = sprite.Update(gameTime, currentAnimation, circlePosition, Direction.Right);
        }

        Vector2 findPointOnCircle(Vector2 pos, float angle)
        {
            return new Vector2((int)(pos.X + radius * (float)Math.Cos(MathHelper.ToRadians(angle))),
                               (int)(pos.Y + radius * (float)Math.Sin(MathHelper.ToRadians(angle))));
        }

        public override void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public override void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            sprite.Draw(spriteBatch, transparency);
        }

    }
}
