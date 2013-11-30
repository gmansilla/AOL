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

        BoundingBox boundingBox;
        BoundingSphere talkRadius;

        int currentMessage;
        List<string> displayLines;
        Global.StoryStates storyState;
        Dictionary<Global.StoryStates, List<string>> dialog;

        bool displayText;

        int textHeight;
        int messageBoxWidth;
        int messageBoxHeight;

        Vector2 textPosition;
        Vector2 cameraPosition;
        Vector2 headshotPosition;
        Vector2 messageBoxPosition;

        public bool MessageBoxVisible
        { get { return displayText; } }
        
        Vector2 Position
        { get { return sprite.Position; } }

        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }

        public BoundingSphere TalkRadius
        { get { return talkRadius; } }

        public Npc(Sprite newSprite, string newName, ContentManager newContent, int newScreenWidth, int newScreenHeight, SpriteFont newSpeachText)
        {
            sprite = newSprite;
            name = newName;
            
            blank = newContent.Load<Texture2D>("Npc\\blank");
            headshot = newContent.Load<Texture2D>("Npc\\" + name + "\\headshot");

            dialog = new Dictionary<Global.StoryStates, List<string>>();
            foreach (Global.StoryStates stage in (Global.StoryStates[])Enum.GetValues(typeof(Global.StoryStates)))
                dialog.Add(stage, new List<String>());

            loadScript(newName);

            setMessageBoxSize(newScreenWidth, newScreenHeight);
            
            speechText = newSpeachText;

            boundingBox = new BoundingBox(new Vector3(Position, 0f),
                                          new Vector3(Position + new Vector2(sprite.Width, sprite.Height), 0f));
            
            talkRadius = new BoundingSphere(boundingBox.Min + new Vector3(sprite.Width / 2f, sprite.Height / 2f, 0f), 
                                            sprite.Height);
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

            //displayText = true;
            messageBoxPosition = new Vector2(cameraPosition.X, (int)cameraPosition.Y + newScreenHeight - messageBoxHeight);
            headshotPosition = messageBoxPosition;
            textPosition = headshotPosition + new Vector2(headshot.Width, 0);

            if (storyState != newStoryState)
            {
                storyState = newStoryState;
                currentMessage = 0;

                processMessageToDraw();
            }
        }

        private void processMessageToDraw()
        {
            displayLines = new List<string>();

            String message = getCurrentMessage(storyState);

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
                                    dialog[Global.StoryStates.Stage1].Add(line[y].Trim());
                                }
                                else if (readingStage2 == true) {
                                    readingStage1 = false;
                                    dialog[Global.StoryStates.Stage2].Add(line[y].Trim());
                                }
                                else if (readingStage3 == true)
                                {
                                    dialog[Global.StoryStates.Stage3].Add(line[y].Trim());
                                    readingStage2 = false;
                                }
                                else if (readingStage4 == true)
                                {
                                    dialog[Global.StoryStates.Stage4].Add(line[y].Trim());
                                    readingStage3 = false;
                                }
                                else if (readingOnCompleted == true)
                                {
                                    dialog[Global.StoryStates.Completed].Add(line[y].Trim());
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

        public void ShowMessageBox()
        {
            displayText = true;
        }

        public void HideMessageBox()
        {
            displayText = false;
        }

        public void ChangeMessage()
        {
            currentMessage++;
            processMessageToDraw();
        }

        string getCurrentMessage(Global.StoryStates stage)
        {
            if (currentMessage >= dialog[stage].Count)
                currentMessage = 0;

            return dialog[stage][currentMessage];
        }

    }
}
