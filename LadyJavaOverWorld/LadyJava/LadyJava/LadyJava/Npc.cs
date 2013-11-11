using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Engine;

namespace LadyJava
{
    class Npc
    {
        Sprite sprite;

        private Vector2 Position
        { get { return sprite.Position; } }

        public Npc(Sprite newSprite)
        {
            sprite = newSprite;
            loadScript("Amy");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        private void loadScript(string name)
        {
            string[,] script = new string[8, 5];
            script[0, 0] = "Hello";
            string fileLocation = name + "\\script.txt";
            try
            {
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });
                    for (int y = 0; y < line.Length; y++)
                    {
                        Console.WriteLine(line[y].Trim());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        
        }

        public string getMessage(int stage)
        {
            
            return "";
        }

    }
}
