using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class Global
    {
        public const float GravityAccelation = 9.807f;
        public const float PixelsToMeter = 0.018f;
        
        public const string DOWN = "Down";
        public const string LEFT = "Left";
        public const string RIGHT = "Right";
        public const string UP = "Up";
        public const string STILL = "Idle";

        public const string ContentPath = "..\\..\\..\\..\\LadyJavaContent\\";
        public const string DungeonContentPath = "..\\..\\..\\..\\..\\LadyJava\\LadyJavaContent\\";

        public static Vector2 Invalid = new Vector2(-1,-1); //Must be static



    }
}
