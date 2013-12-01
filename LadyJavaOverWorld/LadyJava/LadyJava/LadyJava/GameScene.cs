using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LadyJava
{
    class GameScene : DrawableGameComponent
    {
        public List<GameComponent> component;

        public GameScene(Game game) : base(game)
        {
            component = new List<GameComponent>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Show()
        {
            Visible = true;
            Enabled = true;
        }

        public void Hide()
        {
            Visible = false;
            Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent gc in component)
            {
                if (gc.Enabled)
                {
                    gc.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent gc in component)
            {
                if ((gc is DrawableGameComponent) && ((DrawableGameComponent)gc).Visible)
                {
                    ((DrawableGameComponent)gc).Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }
    }
}
