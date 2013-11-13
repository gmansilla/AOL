using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Engine;
using System.Collections.ObjectModel;

namespace Dungeon
{
    class Player
    {
        public Vector2 Position
        { get { return sprite.Position; } }

        public Vector2 Origin
        { get { return sprite.Origin; } }

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }

        BoundingBox boundingBox;

        private float movement = 2.4f;

        private Sprite sprite;

        string animation;

        void UpdateBounds(Vector2 newPosition, int width, int height)
        {
            boundingBox = new BoundingBox(new Vector3(newPosition.X, newPosition.Y, 0f),
                                          new Vector3(newPosition.X + width, newPosition.Y + height, 0f));
        }

        public Player(Sprite newSprite)
        {
            animation = Global.STILL;
            sprite = newSprite;

            UpdateBounds(Position, Width, Height);
        }

        Vector2 LockToLevel(int width, int height, Vector2 position, int levelW, int levelH)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > levelW - width)
                position.X = levelW - width;
            if (position.Y > levelH - height)
                position.Y = levelH - height;
            return position;
        }

        BoundingBox[] GetBoundingBoxes(Object[] objects)
        {
            Collection<BoundingBox> collisions = new Collection<BoundingBox>();

            for (int i = 0; i < objects.Length; i++)
                if (objects[i].GetType() == typeof(BoundingBox))
                    collisions.Add((BoundingBox)objects[i]);
                else if (objects[i].GetType().IsArray && objects[i].GetType().GetElementType() == typeof(BoundingBox))
                {
                    foreach (BoundingBox obj in (IEnumerable<BoundingBox>)objects[i])
                        collisions.Add(obj);
                }

            return collisions.ToArray<BoundingBox>();
        }

        const float buffer = 0.01f;
        Vector2 DownCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (newMotion.Y > 0f)
                    {
                        if (boundingBox.Max.Y > collisions[i].Min.Y &&
                            boundingBox.Max.Y < collisions[i].Max.Y)
                        {
                            newMotion.Y = collisions[i].Min.Y - buffer - Position.Y - Height;
                        }
                    }
                }
            }

            return newMotion;
        }

        Vector2 UpCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Min.Y < collisions[i].Max.Y &&
                        boundingBox.Min.Y > collisions[i].Min.Y)
                    {
                        newMotion.Y = collisions[i].Max.Y + buffer - Position.Y;
                    }
                }
            }

            return newMotion;
        }

        Vector2 RightCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Max.X > collisions[i].Min.X &&
                        boundingBox.Max.X < collisions[i].Max.X)
                    {
                        newMotion.X = collisions[i].Min.X - buffer - Position.X - Width;
                    }
                }
            }

            return newMotion;
        }

        Vector2 LeftCollision(Vector2 newMotion, BoundingBox[] collisions)
        {

            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Min.X < collisions[i].Max.X &&
                        boundingBox.Min.X > collisions[i].Min.X)
                    {
                        newMotion.X = collisions[i].Max.X + buffer - Position.X;
                    }
                }
            }

            return newMotion;
        }

        public void Update(GameTime gameTime, int levelWidth, int levelHeight, params Object[] collisionObjects)
        {
            Vector2 motion = Vector2.Zero;
            Vector2 position = sprite.Position;

            BoundingBox[] collisions = GetBoundingBoxes(collisionObjects);

            motion.Y = (Global.GravityAccelation / Global.PixelsToMeter) * (float) gameTime.ElapsedGameTime.TotalSeconds;
            motion = DownCollision(motion, collisions);

            animation = Global.STILL;
            if (InputManager.IsKeyDown(Commands.Right))
            {
                animation = Global.RIGHT;
                motion.X += movement;
                motion = RightCollision(motion, collisions);
            }
            else if (InputManager.IsKeyDown(Commands.Left))
            {
                animation = Global.LEFT;
                motion.X -= movement;
                motion = LeftCollision(motion, collisions);
            }
            else if (motion != Vector2.Zero)
            {
                //motion.Normalize();

            }

            position += motion;
            position = LockToLevel(sprite.Width, sprite.Height, position, levelWidth, levelHeight);
            
            sprite.Update(gameTime, animation, position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
