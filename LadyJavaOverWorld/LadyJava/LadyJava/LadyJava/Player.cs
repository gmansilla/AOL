using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace LadyJava
{
    abstract class Player
    {
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract Vector2 Update(GameTime gameTime,
                                       int newNPC, //npc index
                                       int levelWidth, int levelHeight,
                                       BoundingBox[] entrances, BoundingSphere[] talkingRadii,
                                       params Object[] collisionObjects);

        const float BUFFER = 0.01f;
        
        protected Sprite sprite;
        protected string animation;
        protected bool switchedTileMap;
        
        protected bool jumpDone;

        protected BoundingBox boundingBox;
        public BoundingBox ToBoundingBox
        { get { return boundingBox; } }
        
        protected Vector2 previousPosition;
        public Vector2 PreviousPosition
        { get { return previousPosition; } }

        //bool talking;
        protected int talkingTo;
        public int TalkingTo
        { get { return talkingTo; } }

        protected Global.PlayState currentPlayState;
        public Global.PlayState CurrentPlayState
        { get { return currentPlayState; } }
        
        public Vector2 Position
        { get { return sprite.Position; } }

        public Vector2 Origin
        { get { return sprite.Origin; } }

        public int Width
        { get { return sprite.Width; } }

        public int Height
        { get { return sprite.Height; } }

        protected void UpdateBounds(Vector2 newPosition, int width, int height)
        {
            boundingBox = new BoundingBox(new Vector3(newPosition.X, newPosition.Y, 0f),
                                          new Vector3(newPosition.X + width, newPosition.Y + height, 0f));
        }

        protected Vector2 EntranceCollision(Vector2 newMotion, BoundingBox[] newEntrances)
        {
            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < newEntrances.Length; i++) //For each tile
            {
                if (boundingBox.Intersects(newEntrances[i])) //compare Lady J's box with another square. 
                {
                    return new Vector2(newEntrances[i].Min.X, newEntrances[i].Min.Y);

                }
            }
            return Global.InvalidVector2;
        }

        protected Vector2 DownCollision(Vector2 newMotion, BoundingBox[] collisions)
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

        protected Vector2 UpCollision(Vector2 newMotion, BoundingBox[] collisions)
        {
            UpdateBounds(Position + newMotion, Width, Height);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (boundingBox.Intersects(collisions[i]))
                {
                    if (boundingBox.Min.Y < collisions[i].Max.Y &&
                        boundingBox.Min.Y > collisions[i].Min.Y)
                    {
                        jumpDone = true;
                        newMotion.Y = collisions[i].Max.Y + Global.Buffer - Position.Y;
                    }
                }
            }

            return newMotion;
        }

        protected Vector2 RightCollision(Vector2 newMotion, BoundingBox[] collisions)
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

        protected Vector2 LeftCollision(Vector2 newMotion, BoundingBox[] collisions)
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

        protected Vector2 LockToLevel(int width, int height, Vector2 position, int levelW, int levelH)
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

        protected BoundingBox[] GetBoundingBoxes(Object[] objects)
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

        public void SetPosition(Vector2 newPosition, int tileWidth, int tileHeight, bool centreToTile, bool switchingTileMap)
        {
            Vector2 offsets = new Vector2(tileWidth / 2f - Width / 2f, tileHeight / 2f - Height / 2f);
            if (centreToTile)
                sprite.SetPosition(newPosition + offsets);
            else
                sprite.SetPosition(newPosition);

            switchedTileMap = switchingTileMap;
        }
    }
}
