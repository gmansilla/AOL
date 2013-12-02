//Anything with the below comments will need to be changed when importing to new game
//This code is depend on the current game

//Find a way to make this independant for each game

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Engine;

namespace LadyJava
{
    public class GameStateManager
    {
        List<GameState> gameStates;

        Song currentSong;
        Song lastSong;

        State currentState;
        //State activeState;

        //bool isGamePlayPaused;

        //Leaderboard leaderboard;
        //SpriteFont leaderboardFont;

        public State CurrentState
        { get { return currentState; } }

        //public State ActiveState
        //{ get { return activeState; } }

        //public bool IsGamePlayPaused
        //{ get { return isGamePlayPaused; } }

        public bool HasGameSongChanged
        { get { return currentSong != lastSong; } }

        public GameStateManager(State firstState)
        {
            gameStates = new List<GameState>();
            //isGamePlayPaused = false;

            //activeState = State.None;
            currentState = firstState;
        }

        public void AddState(GameState newState)
        {
            //This code is depend on the current game
            if (newState is TitleScreenState)//.GetType() == typeof(TitleScreen))
                gameStates.Add((TitleScreenState)newState);
            else if (newState is GamePlayState)
                gameStates.Add((GamePlayState)newState);
            else if (newState is OptionsState)
                gameStates.Add((OptionsState)newState);
        }

        public void UnloadContent()
        {
            MediaPlayer.Stop();
        }

        public bool Contains(State state)
        {
            foreach (GameState gameState in gameStates)
                if (gameState.ID == state) 
                    return true;

            return false;
        }

        public State Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            foreach (GameState gameState in gameStates)
            {
                //activate the current state
                if (currentState == gameState.ID && gameState.CurrentStatus != Status.Active)
                    gameState.ChangeStatus(Status.Active);

                //Update the currently active state
                if (gameState.CurrentStatus == Status.Active)
                    currentState = gameState.Update(gameTime, screenWidth, screenHeight);

                //playing select song based on status
                if (gameStates.Count > 1 && gameState.CurrentStatus == Status.Paused)
                    currentSong = gameState.BGSong;
                else if (gameStates.Count == 1 && gameState.CurrentStatus == Status.Active)
                    currentSong = gameState.BGSong;
            }

            //Play Music
            if (currentSong != null)
            {
                if (!MediaPlayer.State.Equals(MediaState.Playing))
                    MediaPlayer.Play(currentSong);
                else if (HasGameSongChanged)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(currentSong);
                }
                lastSong = currentSong;
            }

            gameStates.RemoveAll(GameState.IsOff);

            return currentState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw paused states first
            foreach (GameState gameState in gameStates)
                if (gameState.CurrentStatus == Status.Paused)
                    gameState.Draw(spriteBatch);
            
            //then draw the active state
            foreach (GameState gameState in gameStates)
                if(gameState.CurrentStatus == Status.Active)
                    gameState.Draw(spriteBatch); 
        }
    }
}
