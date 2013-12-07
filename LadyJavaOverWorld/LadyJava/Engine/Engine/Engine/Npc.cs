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

        bool finalDialogProcessed;
        string finalDialog;

        Global.TilePosition tilePosition;

        BoundingBox boundingBox;
        BoundingBox talkRadius;

        int currentMessage;
        List<string> displayLines;
        
        Global.StoryState storyState;
        
        Dictionary<Global.StoryState, List<string>> dialog;

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

        public bool FinalDialogProcessed
        { get { return finalDialogProcessed; } }
        
        Vector2 Position
        { get { return sprite.Position; } }

        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }

        public BoundingBox TalkRadius
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

            finalDialogProcessed = false;

            storyState = Global.StoryState.None;

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

            dialog = new Dictionary<Global.StoryState, List<string>>();
            //foreach (Global.StoryState stage in (Global.StoryState[])Enum.GetValues(typeof(Global.StoryState)))

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

        public void Update(Camera playerCam, int newScreenWidth, int newScreenHeight, Dictionary<string, RescueInfo> toBeRescued)
        {
            cameraPosition = playerCam.Position;

            setMessageBoxSize(newScreenWidth, newScreenHeight);

            //displayText = true;
            messageBoxPosition = new Vector2(cameraPosition.X, (int)cameraPosition.Y + newScreenHeight - messageBoxHeight);
            headshotPosition = messageBoxPosition;
            textPosition = headshotPosition + new Vector2(headshot.Width, 0);

            Global.StoryState newStoryState = findCurrentStoryState(toBeRescued);

            if (storyState != newStoryState)
            {
                storyState = newStoryState;
                currentMessage = 0;

                processMessageToDraw(cameraPosition);
            }
        }

        private Global.StoryState findCurrentStoryState(Dictionary<string, RescueInfo> rescueList)
        {
                if (dialog.ContainsKey(Global.StoryState.TheScrumMasterSaved) &&
                    rescueList[Global.ToBeRecused[Global.TheScrumMaster]].IsRescued)
                    return Global.StoryState.TheScrumMasterSaved;
                else if (dialog.ContainsKey(Global.StoryState.AllSaved) &&
                         rescueList[Global.ToBeRecused[Global.TheOracle]].IsRescued &&
                         rescueList[Global.ToBeRecused[Global.TecMan]].IsRescued &&
                         rescueList[Global.ToBeRecused[Global.SeeHash]].IsRescued)
                    return Global.StoryState.AllSaved;
                else if (dialog.ContainsKey(Global.StoryState.TheOrcaleSaved) && 
                         rescueList[Global.ToBeRecused[Global.TheOracle]].IsRescued)
                    return Global.StoryState.TheOrcaleSaved;
                else if (dialog.ContainsKey(Global.StoryState.SeeHashSaved) &&
                         rescueList[Global.ToBeRecused[Global.SeeHash]].IsRescued)
                    return Global.StoryState.SeeHashSaved;
                else if (dialog.ContainsKey(Global.StoryState.TecManSaved) &&
                         rescueList[Global.ToBeRecused[Global.TecMan]].IsRescued)
                    return Global.StoryState.TecManSaved;

            return Global.StoryState.Default;
        }

        private void processMessageToDraw(Vector2 cameraPosition)
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
                if ((measureCurrentLine.X + currentWordSize.X) < (cameraPosition.X + messageBoxWidth - textPosition.X) &&
                    i < words.Length - 1)
                {
                    line = line + words[i] + " ";
                }
                //next line
                else if ((measureCurrentLine.X + currentWordSize.X) >= (cameraPosition.X + messageBoxWidth - textPosition.X))
                {
                    displayLines.Add(line);
                    line = " " + words[i] + " ";

                    //last word
                    if (i == words.Length - 1)
                    {
                        displayLines.Add(line);
                        line = " ";
                    }
                }
                //last word
                else if (i == words.Length - 1)
                {
                    displayLines.Add(line + words[i]);
                    line = " ";
                }
            }

            //start process for openning the ending tile
            if (message == finalDialog)
                finalDialogProcessed = true;
        }

        public Vector2 GetCellPosition(int tileWidth, int tileHeight)
        {
            Vector2 position = Position - findOffsets(tileWidth);
            Vector2 cell = new Vector2((int)(Position.X / tileWidth), (int)(Position.Y / tileHeight));

            return cell;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw npc
            sprite.Draw(spriteBatch);

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
        }

        private void loadScript(string name)
        {
            bool readingDefault = false;
            bool readingTecManSaved = false;
            bool readingSeeHashSaved = false;
            bool readingTOSaved = false;
            bool readingAllSaved = false;
            bool readingTSMSaved = false;

            finalDialog = "";

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
                            case "[Default]":
                                readingDefault = true;
                                dialog.Add(Global.StoryState.Default, new List<String>());
                            break;
                            case "[TecManSaved]":
                                readingTecManSaved = true;
                                dialog.Add(Global.StoryState.TecManSaved, new List<String>());
                                readingDefault = readingSeeHashSaved = readingAllSaved = 
                                readingTOSaved = readingTSMSaved = false;
                            break;
                            case "[SeehashSaved]":
                                readingSeeHashSaved = true;
                                dialog.Add(Global.StoryState.SeeHashSaved, new List<String>());
                                readingDefault = readingTecManSaved = readingAllSaved = 
                                readingTOSaved = readingTSMSaved = false;
                            break;
                            case "[TheOracleSaved]":
                                readingTOSaved = true;
                                dialog.Add(Global.StoryState.TheOrcaleSaved, new List<String>());
                                readingDefault = readingSeeHashSaved = readingAllSaved = 
                                readingTecManSaved = readingTSMSaved = false;
                            break;
                            case "[TheScrumMasterSaved]":
                                readingTSMSaved = true;
                                dialog.Add(Global.StoryState.TheScrumMasterSaved, new List<String>());
                                readingDefault = readingSeeHashSaved = readingAllSaved = 
                                readingTOSaved = readingTecManSaved = false;
                            break;
                            case "[AllSaved]":
                                readingAllSaved = true;
                                dialog.Add(Global.StoryState.AllSaved, new List<String>());
                                readingDefault = readingSeeHashSaved = readingTSMSaved = 
                                readingTOSaved = readingTecManSaved = false;
                            break;
                            default:
                                if (readingDefault == true)
                                {
                                    if(line[y].Trim() != "")
                                        dialog[Global.StoryState.Default].Add(line[y].Trim());
                                }
                                else if (readingTecManSaved == true)
                                {
                                    //readingDefault = readingSeeHashSaved = readingAllSaved =
                                    //readingTOSaved = readingTSMSaved = false;

                                    if (line[y].Trim() != "")
                                        dialog[Global.StoryState.TecManSaved].Add(line[y].Trim());
                                }
                                else if (readingSeeHashSaved == true)
                                {
                                    //readingDefault = readingTSMSaved = readingAllSaved =
                                    //readingTOSaved = readingTecManSaved = false;

                                    if (line[y].Trim() != "")
                                        if (line[y].Trim() != "")
                                            dialog[Global.StoryState.SeeHashSaved].Add(line[y].Trim());
                                }
                                else if (readingTOSaved == true)
                                {
                                    //readingDefault = readingSeeHashSaved = readingAllSaved =
                                    //readingTSMSaved = readingTecManSaved = false;

                                    if (line[y].Trim() != "")
                                        dialog[Global.StoryState.TheOrcaleSaved].Add(line[y].Trim());
                                }
                                else if (readingTSMSaved == true)
                                {
                                    //readingDefault = readingSeeHashSaved = readingAllSaved =
                                    //readingTOSaved = readingTecManSaved = false;

                                    if (line[y].Trim() != "")
                                    {
                                        if (line[y-1].Trim() == "<FinalDialog>")
                                            finalDialog = line[y].Trim();

                                        if (line[y].Trim() != "<FinalDialog>")
                                            dialog[Global.StoryState.TheScrumMasterSaved].Add(line[y].Trim());
                                    }
                                }
                                else if (readingAllSaved == true)
                                {
                                    //readingDefault = readingSeeHashSaved = readingTSMSaved =
                                    //readingTOSaved = readingTecManSaved = false;

                                    if (line[y].Trim() != "")
                                        dialog[Global.StoryState.AllSaved].Add(line[y].Trim());
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

        public void ChangeMessage(Vector2 cameraPosition)
        {
            currentMessage++;
            processMessageToDraw(cameraPosition);
        }

        string getCurrentMessage(Global.StoryState stage)
        {
            if (currentMessage >= dialog[stage].Count)
                currentMessage = 0;

            return dialog[stage][currentMessage];
        }
        
        public void SetPosition(Vector2 newPosition, int tileWidth)
        {
            Vector2 offsets = findOffsets(tileWidth);

            sprite.SetPosition(newPosition + offsets);

            Vector3 talkRange = new Vector3(5f, 5f, 0f);

            boundingBox = new BoundingBox(new Vector3(Position, 0f),
                                          new Vector3(Position + new Vector2(Width, Height), 0f));

            talkRadius = new BoundingBox(boundingBox.Min - talkRange, boundingBox.Max + talkRange);
        }

    }
}
