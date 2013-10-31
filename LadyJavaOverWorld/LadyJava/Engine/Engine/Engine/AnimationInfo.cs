using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class AnimationInfo
    {
        string name;
        int width;
        int height;
        int framesCount;
        int speed;


        public string Name
        { get { return name; } }
        public int Width
        { get { return width; } }
        public int Height
        { get { return height; } }
        public int TotalFrames
        { get { return framesCount; } }
        public int Speed
        { get { return speed; } }

        public AnimationInfo(string newName, int newWidth, int newHeight, int newFramesCount, int newAnimationSpeed)
        {
            name = newName;
            width = newWidth;
            height = newHeight;
            framesCount = newFramesCount;
            speed = newAnimationSpeed;
        }
    }
}
