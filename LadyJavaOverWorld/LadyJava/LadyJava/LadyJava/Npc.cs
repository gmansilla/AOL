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
        string[,] script = new string[6, 1];
        private Vector2 Position
        { get { return sprite.Position; } }

        public Npc(Sprite newSprite, string newName)
        {
            sprite = newSprite;
            loadScript(newName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        private void loadScript(string name)
        {
           
            bool readingStage1, readingStage2, readingStage3, readingStage4, readingOnCompleted;
            readingStage1 = readingStage2 = readingStage3 = readingStage4 = readingOnCompleted = false; 
            string fileLocation = Global.ContentPath + name + "\\script.txt";
            try
            {
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });
                    for (int y = 0; y < line.Length; y++)
                    {
                        switch (line[y].Trim()) { 
                            case "[Stage1]":
                                readingStage1 = true;
                                readingStage2 = readingStage3 = readingStage4 = readingOnCompleted = false;
                            break;
                            case "[Stage2]":
                                readingStage2 = true;
                                readingStage1 = readingStage3 = readingStage4 = readingOnCompleted = false;
                            break;
                            case "[Stage3]":
                                readingStage3 = true;
                                readingStage2 = readingStage1 = readingStage4 = readingOnCompleted = false;
                            break;
                            case "[Stage4]":
                                readingStage4 = true;
                                readingStage2 = readingStage3 = readingStage1 = readingOnCompleted = false;
                            break;
                            case "[onCompleted]":
                                readingOnCompleted = true;
                                readingStage2 = readingStage3 = readingStage1 = readingStage4 = false;
                            break;
                            default:
                                if (readingStage1 == true) {
                                    script[1, 0] = line[y].Trim();
                                    readingStage1 = false;
                                }
                                else if (readingStage2 == true) {
                                    script[2, 0] = line[y].Trim();
                                    readingStage2 = false;
                                }
                                else if (readingStage3 == true)
                                {
                                    script[3, 0] = line[y].Trim();
                                    readingStage3 = false;
                                }
                                else if (readingStage4 == true)
                                {
                                    script[4, 0] = line[y].Trim();
                                    readingStage4 = false;
                                }
                                else if (readingOnCompleted == true)
                                {
                                    script[5, 0] = line[y].Trim();
                                    readingOnCompleted = false;
                                }
                            break;
                        }


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
