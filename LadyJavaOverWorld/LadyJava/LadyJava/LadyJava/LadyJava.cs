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

namespace LadyJava
{
    class LadyJava
    {
        const float BUFFER = 0.01f;

        public Vector2 previousPosition;

        public Vector2 PreviousPosition
        { get { return previousPosition; } }
        
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
                                          //new Vector3(newPosition.X + width, newPosition.Y + height, 0f),
                                          new Vector3(newPosition.X + width, newPosition.Y + height, 0f));
        }

        public LadyJava(Sprite newSprite)
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

        Vector2 EntranceCollision(Vector2 newMotion, BoundingBox[] newEntrances)
        {
            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < newEntrances.Length; i++) //For each tile
            {
                if (boundingBox.Intersects(newEntrances[i])) //compare Lady J's box with another square. 
                {
                    return new Vector2(newEntrances[i].Min.X, newEntrances[i].Min.Y);

                }
            }
            return Global.Invalid;
        }
        //#region ?
        //            //UpdateBounds(newPosition, Width, Height);
        //            //for (int i = 0; i < enter.Length; i++)
        //            //{
        //            //    if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\overworld.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\BoatHouse.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\Gym.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\house1.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\house2.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\house3.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\house4.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\PC.map";
        //            //        return getMap;
        //            //    }
        //            //    else if (boundingBox.Intersects(enter[i]))
        //            //    {
        //            //        getMap = "TileMaps\\ShirsStudy.map";
        //            //        return getMap;
        //            //    }
        //            //    else
        //            //    {
        //            //        return getMap;
        //            //    }
        //            //}
        //            //return getMap;
        //#endregion
        //        }

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
                            newMotion.Y = collisions[i].Min.Y - BUFFER - Position.Y - Height;
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
                        newMotion.Y = collisions[i].Max.Y + BUFFER - Position.Y;
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
                        newMotion.X = collisions[i].Min.X - BUFFER - Position.X - Width;
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
                        newMotion.X = collisions[i].Max.X + BUFFER - Position.X;
                    }
                }
            }

            return newMotion;
        }
        
        public Vector2 Update(GameTime gameTime, int levelWidth, int levelHeight, BoundingBox[] entrances, params Object[] collisionObjects)
        {
            Vector2 entranceLocation = Global.Invalid;
            Vector2 motion = Vector2.Zero;
            Vector2 position = sprite.Position;
            previousPosition = sprite.Position;

            bool collision = false;
            BoundingBox[] collisions = GetBoundingBoxes(collisionObjects);

            animation = Global.STILL;
            if (InputManager.IsKeyDown(Commands.Up))
            {
                animation = Global.UP;
                motion.Y = -movement;
                motion = UpCollision(motion, collisions);
                if (motion.Y != -movement)
                    collision = true;
            }
            if (InputManager.IsKeyDown(Commands.Down))
            {
                animation = Global.DOWN;
                motion.Y = movement;
                motion = DownCollision(motion, collisions);
                if (motion.Y != movement)
                    collision = true;
            }
            if (InputManager.IsKeyDown(Commands.Right))
            {
                animation = Global.RIGHT;
                motion.X = movement;
                motion = RightCollision(motion, collisions);
                if (motion.X != movement)
                    collision = true;
            }
            if (InputManager.IsKeyDown(Commands.Left))
            {
                animation = Global.LEFT;
                motion.X = -movement;
                motion = LeftCollision(motion, collisions);
                if (motion.X != -movement)
                    collision = true;
            }
            
            if (!collision && motion != Vector2.Zero)
            {
                motion.Normalize();
                motion *= movement;
            }

            position += motion;
            position = LockToLevel(sprite.Width, sprite.Height, position, levelWidth, levelHeight);
            entranceLocation = EntranceCollision(motion, entrances);
            sprite.Update(gameTime, animation, position);

            return entranceLocation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        internal void SetPosition(Vector2 newPosition)
        {
            sprite.SetPosition(newPosition);
        }
    }
}
