using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dante
{
    public class DisplayText
    {
        Vector2 position;
        string text;
        SpriteFont fontType;
        Color fontColor;

        public Color TextColor
        { get { return fontColor; } }

        public string Text
        { get { return text; } }

        public Vector2 Position
        { get { return position; } }

        public int Height
        { get { return (int)fontType.MeasureString(text).Y; } }

        public int Width
        { get { return (int)fontType.MeasureString(text).X; } }

        public DisplayText(Vector2 textPosition, string outputString, SpriteFont outputFont, Color textColor)
        {
            text = outputString;
            fontType = outputFont;
            position = textPosition;
            fontColor = textColor;
        }

        public void MoveText(Vector2 movement)
        {
            position += movement;
        }

        public void MoveText(int x, int y)
        {
            position += new Vector2(x, y);
        }
        
        public void ChangeColor(Color color)
        {
            fontColor = color;
        }

        public void ChangeFont(SpriteFont font)
        {
            fontType = font;
        }

        public void ChangeText(string newText)
        {
            text = newText;
        }

        public void DrawString(SpriteBatch spriteBatch)
        {

            spriteBatch.DrawString(fontType, text, position, fontColor);
        }
    }
}
