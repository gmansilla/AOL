using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LadyJava
{
    class Sprite
    {
        protected Vector2 position;
       

        private Texture2D[] image;
        private Animation[] animations;
        private int currentImage;
        private int currentAnimation;

        private int width;
        private int height;
        private float scale;
        private float rotation;

        public Texture2D[] Images
        { get { return image; } }

        public Texture2D Image
        { get { return image[currentImage]; } }
                
        public float Scale
        { get { return scale; } }

        public Vector2 Position
        { get { return position; } }

        public Vector2 Origin
        { get { return new Vector2(CurrentAnimation.CurrentFrame.Width / 2f, CurrentAnimation.CurrentFrame.Height / 2f); } }

        public int Width
        { get { return width; } }

        public int Height
        { get { return height; } }

        public Animation CurrentAnimation
        { get { return animations[currentAnimation]; } }

        public int CurrentAnimationNumber
        { get { return currentAnimation; } }

        public int TotalAnimations
        { get { return animations.Length; } }

        public float Rotation
        { get { return rotation;} }

        public Sprite(Texture2D[] spriteImage, Vector2 spritePosition, int spriteWidth, int spriteHeight,
               float spriteScale)
        {
            const int AnimationType = 0;
            const int TotalFrames = 1;
            const int AnimationTime = 2;

            int[,] spriteAnimationInfo = new int[1, 3];
            spriteAnimationInfo[0, AnimationType] = 0;
            spriteAnimationInfo[0, TotalFrames] = 1;
            spriteAnimationInfo[0, AnimationTime] = 0;

            rotation = 0f;
            

            image = spriteImage;
            position = spritePosition;

            scale = spriteScale;
            width = spriteWidth;
            height = spriteHeight;
            

            animations = new Animation[spriteAnimationInfo.GetLength(0)];

            currentAnimation = 0;
            for (int animation = 0; animation < animations.Length; animation++)
                animations[animation] = new Animation(width, height,
                                                      spriteAnimationInfo[animation, TotalFrames],
                                                      spriteAnimationInfo[animation, AnimationType],
                                                      spriteAnimationInfo[animation, AnimationTime], animation, spriteScale);
        }

       public Sprite(Texture2D[] spriteImage, Vector2 spritePosition, float spriteAngle, int spriteWidth, int spriteHeight,
               float spriteScale)
        {
            const int AnimationType = 0;
            const int TotalFrames = 1;
            const int AnimationTime = 2;

            int[,] spriteAnimationInfo = new int[1, 3];
            spriteAnimationInfo[0, AnimationType] = 0;
            spriteAnimationInfo[0, TotalFrames] = 1;
            spriteAnimationInfo[0, AnimationTime] = 0;

            rotation = spriteAngle;

            image = spriteImage;
            position = spritePosition;// +spriteOrigin;

            scale = spriteScale;
            width = spriteWidth;
            height = spriteHeight;

            animations = new Animation[spriteAnimationInfo.GetLength(0)];

            currentAnimation = 0;
            for (int animation = 0; animation < animations.Length; animation++)
                animations[animation] = new Animation(width, height,
                                                      spriteAnimationInfo[animation, TotalFrames],
                                                      spriteAnimationInfo[animation, AnimationType],
                                                      spriteAnimationInfo[animation, AnimationTime], animation, spriteScale);
        }
        
        public Sprite(Texture2D[] spriteImage, Vector2 spritePosition, int spriteWidth, int spriteHeight,
                      int[,] spriteAnimationInfo, float spriteScale)
        {
            const int AnimationType = 0;
            const int TotalFrames = 1;
            const int AnimationTime = 2;

            rotation = 0f;

            currentImage = 0;
            image = spriteImage;

            position = spritePosition;

            scale = spriteScale;
            width = spriteWidth;
            height = spriteHeight;
            
            animations = new Animation[spriteAnimationInfo.GetLength(0)];

            currentAnimation = 0;
            for (int animation = 0; animation < animations.Length; animation++)
                animations[animation] = new Animation(width, height, 
                                                      spriteAnimationInfo[animation, TotalFrames],
                                                      spriteAnimationInfo[animation, AnimationType],
                                                      spriteAnimationInfo[animation, AnimationTime], animation, spriteScale);
        }

        public void ChangeImage(int imageID)
        {
            if (imageID <= image.Length && imageID >= 0)
                currentImage = imageID;
        }

        public void ChangeImage()
        {
            currentImage++;
            if (currentImage > image.Length - 1)
                currentImage = 0;
        }

        public virtual void Update(GameTime gameTime, Vector2 newPosition)
        {
            position = newPosition;

            //based on time elapsed change current frame
            currentAnimation = 0;
            animations[currentAnimation].Update(gameTime);//, animationType);
        }

        public virtual void Update(GameTime gameTime, int animationType, Vector2 newPosition)
        {
            position = newPosition;

            //based on time elapsed change current frame
            currentAnimation = animationType;
            animations[currentAnimation].Update(gameTime);//, animationType);
        }

        public virtual void Update(GameTime gameTime, int animationType, Vector2 newPosition, float newRotation)
        {
            position = newPosition;
            rotation = newRotation;

            //based on time elapsed change current frame
            currentAnimation = animationType;
            animations[currentAnimation].Update(gameTime);//, animationType);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, position + Origin, animations[currentAnimation].CurrentFrame.ToRectangle, //position + origin
                             Color.White, rotation, Origin, scale, SpriteEffects.None, 0f);
        }

    }
}
