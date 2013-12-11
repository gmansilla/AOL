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
        const int MaxAttackSpots = 4;
        const int MovementBuffer = 5;
        float radius = 15f;
        float spinSpeed = 5f;
        float movement = 4;
        //Vector2 pointOnCircle;
        Vector2 targetPosition;

        Vector2 position;

        Vector2[] attackPositions;

        public BabyMetroid(Texture2D newImage, AnimationInfo[] newAnimationInfo) :
                      this(new Sprite(newImage, Vector2.Zero, newAnimationInfo, 1f))
        { }

        BabyMetroid(Sprite newSprite)
        {

            type = Global.BabyMetroid;

            sprite = newSprite;
            position = sprite.Position;
            fightAreaTrigger = Global.InvalidBoundingBox;
           
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
            targetPosition = attackPositions[3];
        }

        const int changePositionTimer = 3000; //3secs
        float changePositionTime = 0;
        int currentPostion = 0;
        bool pauseTimer = true;

        float currentAngle = 0;

        public override void Update(GameTime gameTime, Vector2 playerPosition)
        {
            if (attackPositions == null && fightArea != new Rectangle())
                CreateAttackPositions();

            if(!pauseTimer)
                changePositionTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (changePositionTime >= changePositionTimer)
            {
                changePositionTime = 0;
                currentPostion++;
                if (currentPostion > attackPositions.Length - 1)
                    currentPostion = 0;
                targetPosition = attackPositions[currentPostion];
                pauseTimer = true;
            }

            if (position != targetPosition)
            {
                Vector2 distance = (position - targetPosition);
                if(Math.Abs(distance.X) < 5 && Math.Abs(distance.Y) < 5)
                    position = targetPosition;
                else
                {
                    distance.Normalize();
                    position += -distance * movement;
                }
            }
            //else
            //    pauseTimer = false;

            currentAngle += spinSpeed;
            if (currentAngle > 360)
                currentAngle = currentAngle - 360;
            Vector2 circlePosition = findPointOnCircle(position, currentAngle);
            sprite.Update(gameTime, Global.Moving, circlePosition, Direction.Right);
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
