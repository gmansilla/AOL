using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace EnemyAI_Tests
{
    class GroundAnimatedEnemy : AnimatedSprite
    {

        #region Fields Region

        bool isAlive, isStarting, isAHit;
        float elapsedTime, timer;
        Rectangle collisionRectangle;

        #endregion

        #region Porperties Region

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public bool IsAHit
        {
            get { return isAHit; }
            set { isAHit = value; }
        }
        #endregion

        #region Constructor

        public  GroundAnimatedEnemy(Texture2D sprite, Dictionary<AnimationKey, Animation> animation) : base(sprite, animation)
        {
            timer = 2.0f;
            isAlive = true;
            isStarting = true;
            isAHit = false;
            collisionRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Width, (int)this.Height);
        }

        #endregion

        #region Methods Region

        //public void Move()
        //{
        //    //if (isAlive)
        //    //{
        //        Vector2 motion = new Vector2();
        //        //int algo = 5;

        //        //while (algo == 5)
        //        //{
        //            this.IsAnimating = true;
        //            this.CurrentAnimation = AnimationKey.Left;
        //            motion.X = -1;

        //            for (int i = 0; i < 500; i++)
        //            {
        //                motion.Normalize();
        //                this.Position += motion * this.Speed;
        //                //this.Update(gameTime);
        //            }

        //            base.CurrentAnimation = AnimationKey.Left;
        //            motion.X = 1;

        //            for (int j = 0; j < 500; j++)
        //            {
        //                motion.Normalize();
        //                this.Position += motion * base.Speed;
        //                //this.Update(gameTime);
        //            }

        //        //}
        //    //}
        //}


        public override void Update(GameTime gameTime)
        {
            elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 motion = new Vector2();

            if (isAlive)
            {
                if (isStarting)
                {
                    timer -= elapsedTime;
                    this.IsAnimating = true;
                    this.CurrentAnimation = AnimationKey.Right;
                    motion.X = 1;
                    motion.Normalize();
                    this.Position += motion * this.Speed;
                    collisionRectangle.X = (int)this.Position.X;
                    collisionRectangle.Y = (int)this.Position.Y;

                    if (timer <= 0.0f)
                    {
                        timer = 2.0f;
                        isStarting = false;

                    }
                }
                else if (!isStarting)
                {
                    timer -= elapsedTime;
                    this.IsAnimating = true;
                    this.CurrentAnimation = AnimationKey.Left;
                    motion.X = -1;
                    motion.Normalize();
                    this.Position += motion * this.Speed;
                    collisionRectangle.X = (int)this.Position.X;
                    collisionRectangle.Y = (int)this.Position.Y;

                    if (timer <= 0.0f)
                    {
                        timer = 2.0f;
                        isStarting = true;

                    }
                }
            }

            if (isAHit)
            {
                isAlive = false;
            }
            
            base.Update(gameTime);
        }

        #endregion
    }
}
