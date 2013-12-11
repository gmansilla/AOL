using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    public abstract class Enemy
    {
        protected Sprite sprite;

        protected string currentAnimation;

        protected int hp;

        public bool IsAlive
        { get { return hp > 0; } }

        protected BoundingBox boundingBox;
        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }
        
        public Vector2 Origin
        { get { return sprite.Origin; } }

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        abstract public void Update(GameTime gameTime, Vector2 playerPosition);
        
        abstract public void Draw(SpriteBatch spriteBatch, Color transparency);

        abstract public void SetPosition(Vector2 newPosition);

        public void WasHit()
        {
            currentAnimation = Global.Hurt;
            hp--;
        }

        protected BoundingBox getBounds(Vector2 newPosition)
        {
            return new BoundingBox(new Vector3(newPosition, 0f),
                                   new Vector3(newPosition.X + Width, newPosition.Y + Height, 0f));
        }
    }
}
