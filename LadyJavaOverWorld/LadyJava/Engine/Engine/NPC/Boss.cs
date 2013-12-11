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
        protected BoundingBox fightAreaTrigger;
        protected Rectangle fightArea;
        public BoundingBox FightAreaTrigger
        { get { return fightAreaTrigger; } }
        public Rectangle FightArea
        { get { return fightArea; } }

        public void SetFightArea(Point midPoint, 
                                 int screenWidth, int screenHeight, 
                                 int tileWidth, int tileHeight)
        {
            fightAreaTrigger = new BoundingBox(new Vector3(midPoint.X, midPoint.Y - screenHeight / 2f, 0f),
                                               new Vector3(midPoint.X + tileWidth, midPoint.Y + tileHeight + screenHeight / 2f, 0f));
                                       
            
            fightArea = new Rectangle(midPoint.X,
                                      midPoint.Y,
                                      screenWidth / 2, screenHeight/ 2);
            
        }
    }
}
