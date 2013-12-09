using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class AnimationInfo
    {
        string name;
        string nextAnimation;
        int width;
        int height;
        int framesCount;
        int speed;
        bool loop;

        public string Name
        { get { return name; } }
        public string NextAnimation
        { get { return nextAnimation; } }
        public int Width
        { get { return width; } }
        public int Height
        { get { return height; } }
        public int TotalFrames
        { get { return framesCount; } }
        public int Speed
        { get { return speed; } }
        public bool Loop
        { get { return loop; } }

        public AnimationInfo(string newName, int newWidth, int newHeight, int newFramesCount, int newAnimationSpeed, string newAnimationLink, bool loopAnimation)
        {
            name = newName;
            nextAnimation = newAnimationLink;
            width = newWidth;
            height = newHeight;
            framesCount = newFramesCount;
            speed = newAnimationSpeed;
            loop = loopAnimation;
        }

        public AnimationInfo(string newName, int newWidth, int newHeight, int newFramesCount, int newAnimationSpeed, string newAnimationLink) : 
            this(newName, newWidth, newHeight, newFramesCount,newAnimationSpeed, newAnimationLink, true)
        { }
    }
}
