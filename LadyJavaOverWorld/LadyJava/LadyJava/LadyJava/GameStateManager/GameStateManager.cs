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

        Status nextStatus;

        State currentState;
        public State CurrentState
        { get { return currentState; } }

        public bool HasGameSongChanged
        { get { return currentSong != lastSong; } }

        public GameStateManager(State firstState)
        {
            gameStates = new List<GameState>();

            currentState = firstState;
            nextStatus = Status.Active;
        }

        public void AddState(GameState newState)
        {
            //This code is depend on the current game
            if (newState is TitleScreenState)//.GetType() == typeof(TitleScreen))
                gameStates.Add((TitleScreenState)newState);
            else if (newState is OptionsState)
                gameStates.Add((OptionsState)newState);
            else if (newState is InitialStoryState)
                gameStates.Add((InitialStoryState)newState);
            else if (newState is GamePlayState)
                gameStates.Add((GamePlayState)newState);
            else if (newState is FinalStoryState)
                gameStates.Add((FinalStoryState)newState);
            else if (newState is CreditsState)
                gameStates.Add((CreditsState)newState);
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
                if (currentState == gameState.ID && gameState.CurrentStatus != nextStatus)
                    gameState.ChangeStatus(nextStatus);

                //Update the currently active state
                //if (gameState.CurrentStatus == Status.Active)
                //{
                if (gameState.CurrentStatus == Status.Transition)
                    gameState.Transition();
                else if (gameState.CurrentStatus == Status.Active)
                    currentState = gameState.Update(gameTime, screenWidth, screenHeight);
                //}

                if (gameStates.Exists(GameState.isTransitioning))
                {
                    if(gameState.CurrentStatus == Status.Transition)
                        nextStatus = Status.Paused;
                    else if (gameState.CurrentStatus == Status.Off)
                        nextStatus = Status.Active;
                }
                else if (!gameStates.Exists(GameState.isTransitioning))
                    nextStatus = Status.Active;

                //playing select song based on status
                if (gameStates.Count > 1 && gameState.CurrentStatus == Status.Paused)
                {
                    currentSong = gameState.BGSong;
                }
                else if (gameStates.Count == 1 && gameState.CurrentStatus == Status.Active)
                {
                    currentSong = gameState.BGSong;
                }
            }

            //Play Music
            if (currentSong != null && currentSong == null)
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

            //draw transition states last
            foreach (GameState gameState in gameStates)
                if (gameState.CurrentStatus == Status.Transition)
                    gameState.Draw(spriteBatch);
        }
    }
}
