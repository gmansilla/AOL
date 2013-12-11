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
        public BabyMetroid(Texture2D newImage, AnimationInfo[] newAnimationInfo) :
                      this(new Sprite(newImage, Vector2.Zero, newAnimationInfo, 1f))
        { }

        BabyMetroid(Sprite newSprite)
        {
            type = Global.BabyMetroid;

            sprite = newSprite;
            fightAreaTrigger = Global.InvalidBoundingBox;
           
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 position = sprite.Position;

            sprite.Update(gameTime, Global.Moving, position, Direction.Right);
        }

        public override void Draw(SpriteBatch spriteBatch, Color transparency)
        {
            sprite.Draw(spriteBatch, transparency);
        }

    }
}
