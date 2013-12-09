using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine
{
    public class Sprite
    {
        Dictionary<Global.Direction, SpriteEffects> directions;

        private Vector2 position;

        private Texture2D image;
        private Dictionary<string, Animation> animations;
        private string currentAnimation;
        private string defaultAnimation;

        private float scale;
        private float rotation;

        private Global.Direction facingDirection;

        public Texture2D Images
        { get { return image; } }

        public float Scale
        { get { return scale; } }

        public Vector2 Position
        { get { return position; } }

        public Vector2 Origin
        { get { return animations[currentAnimation].Origin; } }
        
        public Animation CurrentAnimation
        { get { return animations[currentAnimation]; } }

        public String NextAnimation
        { get { return animations[currentAnimation].NextAnimation; } }

        public int Width
        { get { return CurrentAnimation.Width; } }

        public int Height
        { get { return CurrentAnimation.Height; } }

        public string CurrentAnimationName
        { get { return currentAnimation; } }

        public int TotalAnimations
        { get { return animations.Count; } }

        public float Rotation
        { get { return rotation;} }

        const int AnimationType = 0;
        const int TotalFrames = 1;
        const int AnimationTime = 2;
        const int FrameWidth = 3;
        const int FrameHeight = 4;
        
        public Sprite(Texture2D spriteImage, Vector2 spritePosition, int spriteWidth, int spriteHeight, float spriteScale)
        {
            const int totalFrames = 1;
            const int speed = 0;

            int startY = 0;

            rotation = 0f;

            image = spriteImage;

            scale = spriteScale;

            bool loopAnimation = true;

            defaultAnimation = Animation.Default;
            currentAnimation = defaultAnimation;

            animations = new Dictionary<string, Animation>();
            string nextAnimation = Animation.None;
            animations.Add(currentAnimation, 
                           new Animation(startY,
                                         spriteWidth,
                                         spriteHeight,
                                         totalFrames,
                                         speed, 
                                         nextAnimation,
                                         loopAnimation,
                                         spriteScale));
            
            position = spritePosition;

            facingDirection = Global.Direction.Right;
            directions = new Dictionary<Global.Direction, SpriteEffects>();
            directions.Add(Global.Direction.Left, SpriteEffects.FlipHorizontally);
            directions.Add(Global.Direction.Right, SpriteEffects.None);
        }
        
        public Sprite(Texture2D spriteImage, Vector2 spritePosition, AnimationInfo[] spriteAnimations, float spriteScale)
        {
            int startY = 0;
            int firstAnimation = 0;

            rotation = 0f;

            image = spriteImage;

            scale = spriteScale;

            animations = new Dictionary<string, Animation>();

            defaultAnimation = spriteAnimations[firstAnimation].Name;
            currentAnimation = defaultAnimation;

            for (int animation = 0; animation < spriteAnimations.Length; animation++)
            {
                animations.Add(spriteAnimations[animation].Name, 
                               new Animation(startY,
                                             spriteAnimations[animation].Width,
                                             spriteAnimations[animation].Height,
                                             spriteAnimations[animation].TotalFrames,
                                             spriteAnimations[animation].Speed, 
                                             spriteAnimations[animation].NextAnimation,
                                             spriteAnimations[animation].Loop,
                                             spriteScale));

                startY += spriteAnimations[animation].Height;
            }
        
            position = spritePosition;

            facingDirection = Global.Direction.Right;
            directions = new Dictionary<Global.Direction, SpriteEffects>();
            directions.Add(Global.Direction.Left, SpriteEffects.FlipHorizontally);
            directions.Add(Global.Direction.Right, SpriteEffects.None);
        }

        public int GetNextFrameTime(string newType)
        {
            return animations[newType].NextFrameTime * animations[newType].FrameCount;
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Update(GameTime gameTime, Vector2 newPosition)
        {
            position = newPosition;

            currentAnimation = defaultAnimation;
            CurrentAnimation.Update(gameTime);//, animationType);
        }

        public string Update(GameTime gameTime, 
                             string animationType, 
                             Vector2 newPosition,
                             Global.Direction direction)
        {
            position = newPosition;
            facingDirection = direction;

            //if the animation changed
            if (currentAnimation != animationType)//animationType
            {
                currentAnimation = animationType;
                CurrentAnimation.Activate();
            }
            //change current frame based on time elapsed
            CurrentAnimation.Update(gameTime);

            //Handle linked animations
            if (CurrentAnimation.NextAnimation != Animation.None &&
                CurrentAnimation.Status == AnimationStatus.Stopped)
            {
                currentAnimation = CurrentAnimation.NextAnimation;
                CurrentAnimation.Activate();
            }

            return CurrentAnimationName;
        }

        /*
        public void Update(GameTime gameTime, string animationType, Vector2 newPosition, Global.Direction direction)
        {
            position = newPosition;
            facingDirection = direction;

            currentAnimation = animationType;

            //change current frame based on time elapsed
            CurrentAnimation.Update(gameTime);
        }
        */

        public void Update(GameTime gameTime, string animationType, Vector2 newPosition, float newRotation)
        {
            position = newPosition;
            rotation = newRotation;

            //based on time elapsed change current frame
            currentAnimation = animationType;
            animations[currentAnimation].Update(gameTime);//, animationType);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position + Origin, animations[currentAnimation].CurrentFrame.ToRectangle, //position + origin
                             Color.White, rotation, Origin, scale, directions[facingDirection], 0f);
        }
    }
}
