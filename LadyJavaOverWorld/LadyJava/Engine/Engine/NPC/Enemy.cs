using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine
{
    public enum EnemyStatus
    {
        Moving,
        Attacking,
        Stunned,
        Hurt,
    }
    public abstract class Enemy
    {
        //public const int PlayerHit = -2;

        protected bool invincible = false;

        protected Sprite sprite;

        protected string currentAnimation;

        protected int hp;

        protected Dictionary<EnemyStatus, float> movement;

        public bool IsAlive
        { get { return hp > 0; } }

        protected EnemyStatus status;
        public EnemyStatus Status
        { get { return status; } }

        protected bool playerHit;
        public bool PlayerHit
        { get { return playerHit;} }

        protected BoundingBox boundingBox;
        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }
        
        public Vector2 Origin
        { get { return sprite.Origin; } }

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        abstract public void Update(GameTime gameTime, Vector2 playerPosition, BoundingBox playerBounds, int screenWidth, bool inBossFight);
        
        abstract public void Draw(SpriteBatch spriteBatch, Color transparency);

        abstract public void SetPosition(Vector2 newPosition);

        abstract protected void SetInvincible();

        public void WasHit()
        {
            if (!invincible)
            {
                currentAnimation = Global.Hurt;
                status = EnemyStatus.Hurt;
                hp--;
                SetInvincible();
            }
        }

        protected BoundingBox getBounds(Vector2 newPosition)
        {
            return new BoundingBox(new Vector3(newPosition, 0f),
                                   new Vector3(newPosition.X + Width, newPosition.Y + Height, 0f));
        }
    }
}
