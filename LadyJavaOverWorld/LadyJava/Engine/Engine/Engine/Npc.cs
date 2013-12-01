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

namespace Engine
{
    public class Npc
    {
        const float Scale = 1f;

        string name;
        Sprite sprite;
        Texture2D blank;
        Texture2D headshot;
        SpriteFont speechText;

        Global.TilePosition tilePosition;

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

        public string Name
        { get { return name; } }
        
        public Global.TilePosition TileAlignment
        { get { return tilePosition; } }

        public int Width
        { get { return sprite.Width; } }
        public int Height
        { get { return sprite.Height; } }

        public bool MessageBoxVisible
        { get { return displayText; } }
        
        Vector2 Position
        { get { return sprite.Position; } }

        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }

        public BoundingSphere TalkRadius
        { get { return talkRadius; } }

        public Npc(Sprite newSprite, string newName, ContentManager newContent, string newTilePosition, int tileWidth, int newScreenWidth, int newScreenHeight, SpriteFont newSpeechText)
        {
            SetupNPC(newSprite, newName, newContent, null, null, newTilePosition, tileWidth, newScreenWidth, newScreenHeight, newSpeechText);
        }

        public Npc(string name, 
                   Vector2 position, string newTilePosition, 
                   int newWidth, int newHeight, 
                   ContentManager content,
                   int tileWidth, int newScreenWidth, int newScreenHeight, SpriteFont newSpeachText) :
               this(new Sprite(content.Load<Texture2D>("Npc\\" + name + "\\sprite"),
                               position, newWidth, newHeight, Scale), 
                               name, content, newTilePosition, 
                               tileWidth, newScreenWidth, newScreenHeight, newSpeachText)
        { }

        public Npc(string name,
                   Vector2 position, string newTilePosition,
                   int newWidth, int newHeight,
                   string contentPath, GraphicsDevice newGraphicsDevice,
                   int tileWidth, int newScreenWidth, int newScreenHeight, SpriteFont newSpeachText)
        {
            Sprite newSprite = new Sprite(Global.LoadTexture(contentPath + "Npc\\" + name + "\\sprite", newGraphicsDevice), position, newWidth, newHeight, Scale);
            SetupNPC(newSprite, name, null, contentPath, newGraphicsDevice, newTilePosition, tileWidth, newScreenWidth, newScreenHeight, null);
            
        }

        void SetupNPC(Sprite newSprite, string newName,
                              ContentManager newContent,
                              string contentPath, GraphicsDevice newGraphicsDevice,
                              string newTilePosition, int tileWidth,
                              int newScreenWidth, int newScreenHeight, SpriteFont newSpeechText)
        {
            sprite = newSprite;
            name = newName;

            if (newGraphicsDevice == null)
            {
                blank = newContent.Load<Texture2D>("Npc\\blank");
                headshot = newContent.Load<Texture2D>("Npc\\" + name + "\\headshot");
            }
            else
            {
                blank = Global.LoadTexture(contentPath + "Npc\\blank", newGraphicsDevice);
                headshot = Global.LoadTexture(contentPath + "Npc\\" + name + "\\headshot", newGraphicsDevice);
            }

            dialog = new Dictionary<Global.StoryStates, List<string>>();
            foreach (Global.StoryStates stage in (Global.StoryStates[])Enum.GetValues(typeof(Global.StoryStates)))
                dialog.Add(stage, new List<String>());

            loadScript(newName);

            setMessageBoxSize(newScreenWidth, newScreenHeight);

            speechText = newSpeechText;

            tilePosition = ((Global.TilePosition)Enum.Parse(typeof(Global.TilePosition), newTilePosition));

            SetPosition(Position, tileWidth);
        }

        Vector2 findOffsets(int tileWidth)
        {
            Vector2 offsets = Vector2.Zero;

            if (tilePosition == Global.TilePosition.Centre)
                offsets = new Vector2(tileWidth / 2f - Width / 2f, 0f);
            else if (tilePosition == Global.TilePosition.Right)
                offsets = new Vector2(tileWidth - Width, 0f);

            return offsets;
        }

        void setMessageBoxSize(int newWidth, int newHeight)
        {
            messageBoxWidth = newWidth;
            messageBoxHeight = (int)(newHeight / 6.5);
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

        public Vector2 GetCellPosition(int tileWidth, int tileHeight)
        {
            Vector2 position = Position - findOffsets(tileWidth);
            Vector2 cell = new Vector2((int)(Position.X / tileWidth), (int)(Position.Y / tileHeight));

            return cell;
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
        
        public void SetPosition(Vector2 newPosition, int tileWidth)
        {
            Vector2 offsets = findOffsets(tileWidth);

            sprite.SetPosition(newPosition + offsets);

            boundingBox = new BoundingBox(new Vector3(Position, 0f),
                                          new Vector3(Position + new Vector2(Width, Height), 0f));

            talkRadius = new BoundingSphere(boundingBox.Min + new Vector3(Width / 2f, Height / 2f, 0f),
                                            sprite.Height);
        }

    }
}
