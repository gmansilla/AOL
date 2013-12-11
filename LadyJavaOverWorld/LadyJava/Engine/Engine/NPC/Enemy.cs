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

        abstract public void Update(GameTime gameTime);

        abstract public void Draw(SpriteBatch spriteBatch, Color transparency);

        public void SetPosition (Vector2 newPosition)
        {
            sprite.SetPosition(newPosition);
        }
    }
}
