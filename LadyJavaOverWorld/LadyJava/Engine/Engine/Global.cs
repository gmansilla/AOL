using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Engine
{
    public static class Global
    {
        public const float GravityAccelation = 9.807f;
        public const float PixelsToMeter = 0.018f;

        public const float GroundFriction = 0.70f;
        public const float AirFriction = 0.90f;
        
        public const float Buffer = 0.01f;

        public const string Up = "Up";
        public const string Down = "Down";
        //public const string Left = "Left";
        public const string Moving = "Moving";
        public const string None = "None";
        public const string Still = "Idle";
        public const string StartingAttack = "StartingAttack";
        public const string Attacking = "Attacking";
        public const string Dying = "Dying";

        public const string Start = "Start";
        public const string Resume = "Resume";

        public const string MainArea = "TileMaps\\overworld.map";
        public const string FinalDungeon = "TileMaps\\D4Final.map";
        public const string EndOfTheGame = "EndOfTheGame";
  
        public const string ContentPath = "..\\..\\..\\..\\LadyJavaContent\\";
        public const string DungeonContentPath = "..\\..\\..\\..\\..\\LadyJava\\LadyJavaContent\\";

        public const int InvalidInt = -1;
        public static Vector2 InvalidVector2 = new Vector2(-1, -1); //Must be static
        public static BoundingBox InvalidBoundingBox = 
            new BoundingBox(new Vector3(-1, -1, -1), new Vector3(-1, -1, -1));


        public static string[] imageExtensions = { ".jpg", ".png", ".tga" };

        public const int TecMan = 0;
        public const int SeeHash = 1;
        public const int TheOracle = 2;
        public const int TheScrumMaster = 3;
        public static string[] ToBeRecused = { "TecMan", "SeeHash", "TheOracle", "TheScrumMaster" };
        public static string[] RecuseAreas = { "TileMaps\\D1End.map", "TileMaps\\D2End.map", "TileMaps\\D3End.map", "TileMaps\\D4End.map" };

        public enum TilePosition
        {
            Centre,
            Right,
            Left
        }

        public enum StoryState
        {
            None,
            Default,
            TecManSaved,
            SeeHashSaved,
            TheOrcaleSaved,
            AllSaved,
            TheScrumMasterSaved
        }

        public enum PlayState
        {
            Playing,
            Message
        }

        public enum Direction
        {
            Right = 1,
            Left = -1
        }

        static public Texture2D LoadTexture(string newTexturePath, GraphicsDevice graphicsDevice)
        {
            Texture2D newTexture;
            foreach (string extension in imageExtensions)
                if (File.Exists(newTexturePath + extension))
                {
                    newTexturePath += extension;
                    break;
                }

            using (FileStream fileStream = new FileStream(newTexturePath, FileMode.Open, FileAccess.Read))
            {
                newTexture = Texture2D.FromStream(graphicsDevice, fileStream);
                fileStream.Close();
            }

            return newTexture;
        }


    }
}
