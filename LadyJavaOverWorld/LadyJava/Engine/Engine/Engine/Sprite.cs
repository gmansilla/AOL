using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine
{
    public class Sprite
    {
        private Vector2 position;
        private Vector2 maxOrigin; //used to centre the camera because of the different sprite origins

        private Texture2D image; //allows for multiple sprite maps for one sprite update, use method ChangeImage to display different image
        private Dictionary<string, Animation> animations;
        //private int currentImage;
        private string currentAnimation;
        private string defaultAnimation;

        private float scale;
        private float rotation;

        public Texture2D Images
        { get { return image; } }

        //public Texture2D Image
        //{ get { return image[currentImage]; } }
                
        public float Scale
        { get { return scale; } }

        public Vector2 Position
        { get { return position; } }

        public Vector2 Origin
        { get { return animations[currentAnimation].Origin; } }
        
        public Vector2 MaxOrigin
        { get { return maxOrigin; } }

        public Animation CurrentAnimation
        { get { return animations[currentAnimation]; } }

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
            maxOrigin = Vector2.Zero;

            rotation = 0f;

            //currentImage = 0;
            image = spriteImage;

            scale = spriteScale;

            defaultAnimation = "Default";
            currentAnimation = defaultAnimation;
            animations = new Dictionary<string, Animation>();
            animations.Add(currentAnimation, 
                           new Animation(startY,
                                         spriteWidth,
                                         spriteHeight,
                                         totalFrames,
                                         speed, spriteScale));
            
            maxOrigin = new Vector2(spriteWidth / 2f, spriteHeight / 2f);
            
            position = spritePosition; 
        }
        

        public Sprite(Texture2D spriteImage, Vector2 spritePosition, AnimationInfo[] spriteAnimations, float spriteScale)
        {
            int startY = 0;
            int firstAnimation = 0;
            maxOrigin = Vector2.Zero;

            rotation = 0f;

            //currentImage = 0;
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
                                             spriteAnimations[animation].Speed, spriteScale));
                if (maxOrigin.X < spriteAnimations[animation].Width / 2f)
                    maxOrigin.X = spriteAnimations[animation].Width / 2f;
                if (maxOrigin.Y < spriteAnimations[animation].Height / 2f)
                    maxOrigin.Y = spriteAnimations[animation].Height / 2f;

                startY += spriteAnimations[animation].Height;
            }
        
            position = spritePosition;
        }

        /*
        public void ChangeImage()
        {
            currentImage++;
            if (currentImage > image.Length - 1)
                currentImage = 0;
        }
        */

        public int GetNextFrameTime(string newType)
        {
            return animations[newType].NextFrame * animations[newType].FrameCount;
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Update(GameTime gameTime, Vector2 newPosition)
        {
            position = newPosition;

            currentAnimation = defaultAnimation;
            animations[currentAnimation].Update(gameTime);//, animationType);
        }

        public void Update(GameTime gameTime, string animationType, Vector2 newPosition)
        {
            position = newPosition;

            //based on time elapsed change current frame
            currentAnimation = animationType;
            animations[currentAnimation].Update(gameTime);//, animationType);
        }

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
                             Color.White, rotation, Origin, scale, SpriteEffects.None, 0f);
        }
    }
}
