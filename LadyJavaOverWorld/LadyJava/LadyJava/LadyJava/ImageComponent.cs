using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LadyJava
{
    class ImageComponent : DrawableGameComponent
    {
        Texture2D pic;
        Rectangle picR;
        SpriteBatch spriteBatch;

        public ImageComponent(Game game, Texture2D tex, DrawMode mode, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch; //Storing a reference to the spritebatch.
            pic = tex; //pic is a Texture2D
            Rectangle clientR = game.Window.ClientBounds; //Get Screen Dimensions

            switch (mode) //Switch between what was defined in the eNum DrawMode
            {
                case DrawMode.Center: //centering the picture
                    picR = new Rectangle((clientR.Width - pic.Width) / 2, (clientR.Height - pic.Height) / 2, pic.Width, pic.Height);
                    break;
                case DrawMode.Stretch: //streching the picture
                    picR = new Rectangle(0, 0, clientR.Width, clientR.Height);
                    break;
                default:
                    picR = new Rectangle(0, 0, pic.Width, pic.Height);
                    break;
            }
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(pic, picR, Color.Black); //(texture2D, Rectangle, Color)
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
