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

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        abstract public void Update(GameTime gameTime, Vector2 playerPosition);
        
        abstract public void Draw(SpriteBatch spriteBatch, Color transparency);

        abstract public void SetPosition(Vector2 newPosition);
        //{
        //    sprite.SetPosition(newPosition);
        //}
    }
}
