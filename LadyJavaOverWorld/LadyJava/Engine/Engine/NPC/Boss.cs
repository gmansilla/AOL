using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public abstract class Boss : Enemy
    {
        protected const int MaxHP = 1;
        
        protected string type;

        protected Vector2 targetPosition;

        protected Vector2[] attackPositions;
        public Vector2 StartPosition
        { get { return attackPositions[0]; } }

        protected const int changePositionTimer = 2500; //2.5secs
        protected float changePositionTime = 0;

        protected bool Invincible
        { get { return invincible; } }

        float invincibleTime;
        const int invincibleTimer = 3000;

        protected Rectangle fightArea;
        public Rectangle FightArea
        { get { return fightArea; } }

        protected int FightAreaWidth
        { get { return fightArea.Width * 2; } }
        protected int FightAreaHeight
        { get { return fightArea.Height * 2; } }

        protected BoundingBox fightAreaTrigger;
        public BoundingBox FightAreaTrigger
        { get { return fightAreaTrigger; } }

        public void SetFightArea(Point midPoint, 
                                 int screenWidth, int screenHeight, 
                                 int tileWidth, int tileHeight)
        {
            fightAreaTrigger = new BoundingBox(new Vector3(midPoint.X, midPoint.Y - screenHeight / 2f, 0f),
                                               new Vector3(midPoint.X + tileWidth, midPoint.Y + tileHeight + screenHeight / 2f, 0f));

            fightArea = new Rectangle(midPoint.X,
                                      midPoint.Y,
                                      screenWidth / 2, screenHeight / 2);
            
        }

        protected override void SetInvincible()
        {
            invincibleTime = 0;
            changePositionTime = changePositionTimer;
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
                    status = EnemyStatus.Moving;
                    currentAnimation = Global.Moving;
                }
            }
        }
        
        public override void SetPosition(Vector2 newPosition)
        {
            sprite.SetPosition(newPosition);
        }


        internal void Reset(Vector2 newPosition)
        {
            hp = MaxHP;
            SetPosition(newPosition);
            targetPosition = newPosition;


        }
    }
}
