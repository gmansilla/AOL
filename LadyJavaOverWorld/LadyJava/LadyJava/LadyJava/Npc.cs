using System;
using System.Collections.Generic;
using System.Linq;
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
        string name;
        Sprite sprite;
        Texture2D blank;
        Texture2D headshot;
        SpriteFont speechText;

        List<string> displayLines;
        Global.StoryStates storyState;
        Dictionary<Global.StoryStates, List<string>> dialog;
        string[,] script = new string[6, 4];

        bool displayText;

        int textHeight;
        int messageBoxWidth;
        int messageBoxHeight;

        Vector2 textPosition;
        Vector2 cameraPosition;
        Vector2 headshotPosition;
        Vector2 messageBoxPosition;

        Vector2 Position
        { get { return sprite.Position; } }

        public Npc(Sprite newSprite, string newName, ContentManager newContent, int newScreenWidth, int newScreenHeight, SpriteFont newSpeachText)
        {
            sprite = newSprite;
            name = newName;
            
            blank = newContent.Load<Texture2D>("Npc\\blank");
            headshot = newContent.Load<Texture2D>("Npc\\" + name + "\\headshot");

            dialog = new Dictionary<Global.StoryStates, List<string>>();
            Global.StoryStates stages;
            foreach (Global.StoryStates stage in stages)


            loadScript(newName);

            setMessageBoxSize(newScreenWidth, newScreenHeight);
            
            speechText = newSpeachText;
        }

        void setMessageBoxSize(int newWidth, int newHeight)
        {
            messageBoxWidth = newWidth;
            messageBoxHeight = (int)(newHeight / 6.5);
        }

        public Npc(string name, Vector2 position, int newWidth, int newHeight, float newScale, ContentManager content, int newScreenWidth, int newScreenHeight, SpriteFont newSpeachText) : 
               this(new Sprite(content.Load<Texture2D>("Npc\\" + name + "\\sprite"), position, newWidth, newHeight, newScale), name, content, newScreenWidth, newScreenHeight, newSpeachText)
        {
            
        }


        public void Update(Camera playerCam, int newScreenWidth, int newScreenHeight, Global.StoryStates newStoryState)
        {
            cameraPosition = playerCam.Position;

            setMessageBoxSize(newScreenWidth, newScreenHeight);

            displayText = false;
            messageBoxPosition = new Vector2(cameraPosition.X, (int)cameraPosition.Y + newScreenHeight - messageBoxHeight);
            headshotPosition = messageBoxPosition;
            textPosition = headshotPosition + new Vector2(headshot.Width, 0);

            if (storyState != newStoryState)
            {
                storyState = newStoryState;
                displayLines = new List<string>();

                String message = getMessage(1, 0);

                string[] words = message.Trim().Split(' ');
                string line = " ";
                Vector2 measureCurrentLine;
                for (int i = 0; i < words.Length; i++)
                {
                    measureCurrentLine = speechText.MeasureString(line);
                    textHeight = (int)measureCurrentLine.Y;
                    Vector2 currentWordSize = speechText.MeasureString(words[i]);

                    //add to current line
                    if ((measureCurrentLine.X + currentWordSize.X) < (messageBoxWidth - textPosition.X) && 
                        i < words.Length - 1)
                    {
                        line = line + words[i] + " ";
                    }
                    //last word
                    else if (i == words.Length - 1)
                    {
                        displayLines.Add(line + words[i]);
                        line = " ";
                    }
                    //next line
                    else
                    {
                        displayLines.Add(line);
                        line = " " + words[i] + " ";
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (displayText)
            {
                //draw messageBox
                spriteBatch.Draw(blank, messageBoxPosition,
                                 new Rectangle(0, 0, messageBoxWidth, (int)messageBoxPosition.Y),
                                 Color.Black);

                //draw headshot
                spriteBatch.Draw(headshot, headshotPosition, Color.White);
                
                //draw text
                foreach (string displayLine in displayLines)
                {
                    spriteBatch.DrawString(speechText, displayLine, textPosition, Color.White);
                    textPosition.Y += textHeight;
                }
            }

            //draw npc
            sprite.Draw(spriteBatch);
        }

        private void loadScript(string name)
        {
           
            bool readingStage1, readingStage2, readingStage3, readingStage4, readingOnCompleted;
            readingStage1 = readingStage2 = readingStage3 = readingStage4 = readingOnCompleted = false; 
            string fileLocation = Global.ContentPath + "\\NPC\\" + name + "\\script.txt";
            try
            {
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });
                    int i = 0;
                    for (int y = 0; y < line.Length; y++)
                    {
                        switch (line[y].Trim()) { 
                            case "[Stage1]":
                                i = 0;
                                readingStage1 = true;
                                readingStage2 = readingStage3 = readingStage4 = readingOnCompleted = false;
                            break;
                            case "[Stage2]":
                                i = 0;
                                readingStage2 = true;
                                readingStage1 = readingStage3 = readingStage4 = readingOnCompleted = false;
                            break;
                            case "[Stage3]":
                                i = 0;
                                readingStage3 = true;
                                readingStage2 = readingStage1 = readingStage4 = readingOnCompleted = false;
                            break;
                            case "[Stage4]":
                                i = 0;
                                readingStage4 = true;
                                readingStage2 = readingStage3 = readingStage1 = readingOnCompleted = false;
                            break;
                            case "[onCompleted]":
                                i = 0;
                                readingOnCompleted = true;
                                readingStage2 = readingStage3 = readingStage1 = readingStage4 = false;
                            break;
                            default:
                                if (readingStage1 == true) {
                                    dialog.Add(Global.StoryStates.Stage1, 
                                    script[1, i] = line[y].Trim();
                                    i++;
                                }
                                else if (readingStage2 == true) {
                                    readingStage1 = false;
                                    script[2, i] = line[y].Trim();
                                    i++;
                                }
                                else if (readingStage3 == true)
                                {
                                    script[3, i] = line[y].Trim();
                                    i++;
                                    readingStage2 = false;
                                }
                                else if (readingStage4 == true)
                                {
                                    script[4, i] = line[y].Trim();
                                    i++;
                                    readingStage3 = false;
                                }
                                else if (readingOnCompleted == true)
                                {
                                    script[5, i] = line[y].Trim();
                                    i++;
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

        public string getMessage(int stage, int position)
        {
            String message = "Hey! I don't have much to tell you";
            message = (script[stage, position] == null ? message : script[stage, position]); 
            return message;
        }

    }
}
