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
        // This code is depend on the current game
        //static public int NameEntry = 5;
        //static public int TitleScreen = 4;
        //static public int GamePlay = 0;
        //static public int Leaderboard = 1;
        //static public int Controls = 2;
        //static public int Quit = 3;

        protected Status status;
        protected Song bgSong;

        protected State id;

        public State ID
        { get { return id; } }

        //protected int index = 0; //state id number, unique

        public virtual Song BGSong
        { get { return bgSong; } }

        public Status CurrentStatus
        { get { return status; } }

        public GameState()
        {
            id = State.None;
            status = Status.Off;
        }

        public void ChangeStatus(Status newStatus)
        {
            status = newStatus;
        }

        public abstract State Update(GameTime gameTime, int screenWidth, int screenHeight);

        public abstract void Draw(SpriteBatch spriteBatch);

        public static bool IsOff(GameState state)
        {
            if (state.CurrentStatus == Status.Off)
                return true;

            return false;
        }
    }
}
