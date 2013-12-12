using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public enum AnimationStatus
    {
        Running,
        Stopped
    }

    public class Animation
    {
        public const string None = "None";
        public const string Default = "Default";

        private int currentFrame;
        private int animateTime;
        private int nextFrameTime;

        private AnimationStatus status;

        private string nextAnimation;

        private float scale;

        private Frame[] frames;

        bool loop;

        public bool Loop
        { get { return loop; } }

        public AnimationStatus Status
        { get { return status; } }

        public string NextAnimation
        { get { return nextAnimation; } }

        public int TotalFrames
        { get { return frames.Length; } }

        public int NextFrameTime
        { get { return nextFrameTime; } }

        public int FrameCount
        { get { return frames.Length; } }

        public float Scale
        { get { return scale; } }

        public int Width
        { get { return frames[currentFrame].Width; } }

        public int Height
        { get { return frames[currentFrame].Height; } }

        public Vector2 Origin
        { get { return new Vector2(frames[currentFrame].Width / 2f, frames[currentFrame].Height / 2f); } }

        public Frame CurrentFrame
        { get { return frames[currentFrame]; } }

        public Animation(int initialY, int frameWidth, int frameHeight, int totalFrames, int aniamtionTime, string newAnimationLink, bool loopAnimation, float animationScale)
        {
            //add time variable so that the animation will adjust based on time elapsed

            //type = animationType;
            nextAnimation = newAnimationLink;
            nextFrameTime = aniamtionTime;
            loop = loopAnimation;
            animateTime = 0;

            scale = animationScale;

            frames = new Frame[totalFrames];
            currentFrame = 0;

            for (int frame = 0; frame < TotalFrames; frame++)
                frames[frame] = new Frame(frameWidth, frameHeight, new Point(frame * frameWidth, initialY), scale);

            status = AnimationStatus.Running;
        }

        public void Activate()
        {
            status = AnimationStatus.Running;
            currentFrame = 0;
            animateTime = 0;
        }

        public void Update(GameTime animationTime)//, int animationType)
        {
            //based on time elapsed change current frame
            if(status == AnimationStatus.Running)
                animateTime += animationTime.ElapsedGameTime.Milliseconds;
            if (nextFrameTime != 0 && animateTime >= nextFrameTime)
            {
                animateTime = 0;
                currentFrame++;
                if (currentFrame >= frames.Length)
                {
                    if (!loop)
                    {
                        status = AnimationStatus.Stopped;
                        currentFrame = frames.Length - 1;
                    }
                    else if (nextAnimation != Animation.None)
                    {
                        status = AnimationStatus.Stopped;
                        currentFrame = 0;
                    }
                    else
                        currentFrame = 0;
                }

            }

        }
    }
}
