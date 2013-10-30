using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LadyJava
{
    class Command
    {
        Keys key;
        public Keys Key
        { get { return key; } }

        Buttons button;
        public Buttons Button
        { get { return button; } }

        public Command(Keys keyCommand, Buttons buttonCommand)
        {
            key = keyCommand;
            button = buttonCommand;
        }

        static public bool operator ==(Command command1, Command command2)
        {
            return (command1.key == command2.key) && (command1.button == command2.button);
        }

        static public bool operator !=(Command command1, Command command2)
        {
            return !(command1 == command2);
        }

        public override bool Equals(System.Object command)
        {
            return this.GetHashCode() == command.GetHashCode();
        }

        public override int GetHashCode()
        {
            return key.GetHashCode() * button.GetHashCode();
        }

    }

    static class Commands
    {
        //static public Command None = null;
        public enum ThumbStick
        {
            Up,
            Down,
            Left,
            Right,
            Center
        }

        static public Command MoveCamUp = new Command(Keys.I, Buttons.DPadUp);
        static public Command MoveCamDown = new Command(Keys.K, Buttons.DPadDown);
        static public Command MoveCamLeft = new Command(Keys.J, Buttons.DPadLeft);
        static public Command MoveCamRight = new Command(Keys.L, Buttons.DPadRight);

        //static public Command ZoomIn = new Command(Keys.Add, Buttons.Start);
        //static public Command ZoomOut = new Command(Keys.Subtract, Buttons.Back);

        static public Command Up = new Command(Keys.Up, Buttons.LeftThumbstickUp);
        static public Command Down = new Command(Keys.Down, Buttons.LeftThumbstickDown);
        static public Command Left = new Command(Keys.Left, Buttons.LeftThumbstickLeft);
        static public Command Right = new Command(Keys.Right, Buttons.LeftThumbstickRight);

        static public Command Frames = new Command(Keys.F, Buttons.Start);

        static public Command Fire = new Command(Keys.Space, Buttons.RightTrigger);
        
        static public Command Execute = new Command(Keys.Space, Buttons.A);
        static public Command PauseGame = new Command(Keys.Enter, Buttons.Start);
        static public Command Exit = new Command(Keys.Escape, Buttons.Back);
    }

    static class InputManager
    {
        static KeyboardState keyState;
        static GamePadState padState;
        static KeyboardState previousKeyState;
        static GamePadState previousPadState;

        static Vector2 leftStickMotion;
        static float leftStickAngle;
        static float leftStickPreviousAngle;

        static Vector2 rightStickMotion;
        static float rightStickAngle;
        static float rightStickPreviousAngle;

        static Commands.ThumbStick leftStickDirection;// = Commands.ThumbStick.Center;
        static Commands.ThumbStick leftStickPreviousDirection;


        static public Vector2 LeftStickMotion
        { get { return leftStickMotion; } }

        static public Vector2 RightStickMotion
        { get { return rightStickMotion; } }

        static public Commands.ThumbStick LeftStickDirection
        { get { return leftStickDirection; } }//CurrentStickDirection(leftStickMotion, leftStickAngle); } }

        static public Commands.ThumbStick RightStickDirection
        { get { return CurrentStickDirection(rightStickMotion, rightStickAngle); } }

        static public float LeftStickAngle
        { get { return leftStickAngle; } }

        static public float RightStickAngle
        { get { return rightStickAngle; } }

        static public float LeftStickPreviousLength
        { get { return MathHelper.Clamp(previousPadState.ThumbSticks.Left.Length(), 0.5f, 1f); } }

        static public float LeftStickLength
        { get { return MathHelper.Clamp(padState.ThumbSticks.Left.Length(), 0.5f, 1f); } }

        static public bool RightStickNoLongerInUse
        { get { return previousPadState.ThumbSticks.Right != Vector2.Zero && padState.ThumbSticks.Right == Vector2.Zero; } }

        static public bool RightStickInUse
        { get { return padState.ThumbSticks.Right != Vector2.Zero; } }

        static public void Update()
        {
            previousKeyState = keyState;
            previousPadState = padState; 
            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);

            leftStickMotion = new Vector2(padState.ThumbSticks.Left.X, -padState.ThumbSticks.Left.Y);
            rightStickMotion = new Vector2(padState.ThumbSticks.Right.X, -padState.ThumbSticks.Right.Y);

            leftStickPreviousAngle = leftStickAngle;
            leftStickAngle = (float)Math.Atan2(leftStickMotion.Y, leftStickMotion.X);
            rightStickPreviousAngle = RightStickAngle;
            rightStickAngle = (float)Math.Atan2(rightStickMotion.Y, rightStickMotion.X);

            leftStickPreviousDirection = leftStickDirection;
            leftStickDirection = CurrentStickDirection(leftStickMotion, leftStickAngle);
        }

        static Commands.ThumbStick CurrentStickDirection(Vector2 stickMotion, float stickAngle)
        {
            Commands.ThumbStick stickCommand = Commands.ThumbStick.Center;

            if (stickMotion != Vector2.Zero)
            {
                //Right
                if (stickAngle < MathHelper.PiOver4 && stickAngle > -MathHelper.PiOver4)
                    return Commands.ThumbStick.Right;
                //Up
                else if (stickAngle < -MathHelper.PiOver4 && stickAngle > -(MathHelper.PiOver4 * 3))
                    return Commands.ThumbStick.Up;
                //Left
                else if ((stickAngle < -(MathHelper.PiOver4 * 3) && stickAngle >= -MathHelper.Pi) ||
                         (stickAngle < MathHelper.Pi && stickAngle > (MathHelper.PiOver4 * 3)))
                    return Commands.ThumbStick.Left;
                //Down
                else if (stickAngle > MathHelper.PiOver4 && stickAngle < (MathHelper.PiOver4 * 3))
                    return Commands.ThumbStick.Down;
            }
            return stickCommand;
        }

        static public bool IsKeyDown(Command command)
        {
            if (keyState.IsKeyDown(command.Key) ||
                padState.IsButtonDown(command.Button))
                return true;
            return false;
        }

        static public bool HasKeyBeenUp(Command command)
        {
            if ((keyState.IsKeyDown(command.Key) && 
                (previousKeyState.IsKeyUp(command.Key) || previousKeyState == null)) ||
                (padState.IsButtonDown(command.Button) &&
                (previousPadState.IsButtonUp(command.Button) || previousPadState == null)))
                return true;
            return false;
        }

        static public bool HasLeftStickChangedDriection(Commands.ThumbStick direction)
        {
            if (leftStickDirection == direction && leftStickPreviousDirection != direction)
                return true;
            return false;
        }
    }
}
