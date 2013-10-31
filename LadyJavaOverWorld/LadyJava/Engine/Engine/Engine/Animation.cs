using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Animation
    {
        private int currentFrame;
        private int animateTime;
        private int nextFrame;

        private float scale;

        private Frame[] frames;

        public int TotalFrames
        { get { return frames.Length; } }

        public int NextFrame
        { get { return nextFrame; } }

        public int FrameCount
        { get { return frames.Length; } }

        public float Scale
        { get { return scale; } }

        public int Width
        { get { return frames[currentFrame].Width; } }
        //{ get { return width; } }

        public int Height
        { get { return frames[currentFrame].Height; } }
        //{ get { return height; } }

        public Vector2 Origin
        { get { return new Vector2(frames[currentFrame].Width / 2f, frames[currentFrame].Height / 2f); } }

        public Frame CurrentFrame
        { get { return frames[currentFrame]; } }

        public Animation(int initialY, int frameWidth, int frameHeight, int totalFrames, int aniamtionTime, float animationScale)
        {
            //add time variable so that the animation will adjust based on time elapsed

            //type = animationType;
            nextFrame = aniamtionTime;
            animateTime = 0;
            
            scale = 1f;

            frames = new Frame[totalFrames];
            currentFrame = 0;

            for (int frame = 0; frame < TotalFrames; frame++)
                frames[frame] = new Frame(frameWidth, frameHeight, new Point(frame * frameWidth, initialY), animationScale);
        }

        public void Update(GameTime animationTime)//, int animationType)
        {
            //based on time elapsed change current frame
            animateTime += animationTime.ElapsedGameTime.Milliseconds;
            if (nextFrame != 0 && animateTime >= nextFrame)
            {
                currentFrame++;
                animateTime = 0;
                if (currentFrame >= frames.Length)
                    currentFrame = 0;
            }

        }
    }
}
