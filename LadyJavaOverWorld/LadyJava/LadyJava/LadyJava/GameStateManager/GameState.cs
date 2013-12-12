//Anything with the below comments will need to be changed when importing to new game
//This code is depend on the current game

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LadyJava
{
    public enum Status
    {
        Unloaded,
        Active,
        Transition,
        Off,
        Paused,
    }

    public enum State
    {
        None,
        TitleScreen,
        InitialStory,
        GamePlay,
        FinalStory,
        Credits,
        Options,
        Quit
    }
    
    public abstract class GameState
    {
        const float Increment = 0.1f;
        protected const float Clear = 0.0f;
        protected const float Half = 0.5f;
        protected const float Opaque = 1.0f;

        protected Texture2D background;
        protected Color backgroundColor = new Color(255, 255, 255, Opaque);
        protected Status exitStatus;

        float transparency = Opaque;

        protected Status status;
        protected Song bgSong;

        protected State id;

        public State ID
        { get { return id; } }

        public virtual Song BGSong
        { get { return bgSong; } }

        public Status CurrentStatus
        { get { return status; } }

        public void ChangeStatus(Status newStatus)
        {
            status = newStatus;
        }

        public void Transition()
        {
            transparency = Math.Max(0, transparency - Increment);
            backgroundColor = new Color(255f, 255f, 255f, transparency);

            if (transparency == Clear)
                status = Status.Off;
        }

        public abstract State Update(GameTime gameTime, int screenWidth, int screenHeight);

        public abstract void Draw(SpriteBatch spriteBatch);

        public static bool IsOff(GameState state)
        {
            if (state.CurrentStatus == Status.Off)
                return true;

            return false;
        }

        public static bool isTransitioning(GameState state)
        {
            if (state.CurrentStatus == Status.Transition)
                return true;

            return false;
        }

    }
}
