using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public abstract class Boss : Enemy
    {
        protected string type;

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

        public override void SetPosition(Vector2 newPosition)
        {
            sprite.SetPosition(newPosition);
        }

    }
}
