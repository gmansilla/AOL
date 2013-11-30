using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LadyJava
{
    class PauseScreen : GameScene
    {
        public PauseScreen(Game1 game, Texture2D background, SpriteBatch spriteBatch) : base(game)
        {
            ImageComponent title = new ImageComponent(game, background, DrawMode.Stretch, spriteBatch);
            component.Add(title);
        }
    }
}
