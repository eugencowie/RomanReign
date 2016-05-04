using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace RomanReign
{
    enum InputType { KBM, Gamepad }
    enum Thumbsticks { Left, Right }
    enum MouseButtons { Left, Middle, Right, XButton1, XButton2 }
    delegate bool InputAction();

    /// <summary>
    /// Contains the state of the keyboard, mouse and gamepads.
    /// </summary>
    class InputState
    {
        public GamePadState[] Gamepads = new GamePadState[4];
        public KeyboardState Keyboard;
        public MouseState Mouse;

        // These functions return true if the specified button, key or mouse button is down.

        public bool IsDown(Buttons buttons, int i = 0) => Gamepads[i].IsButtonDown(buttons);
        public bool IsDown(Keys keys) => Keyboard.IsKeyDown(keys);
        public bool IsDown(MouseButtons buttons) => (GetMouseButtonState(buttons) == ButtonState.Pressed);

        // These functions return true if the specified button, key or mouse button is up.

        public bool IsUp(Buttons buttons, int i = 0) => Gamepads[i].IsButtonUp(buttons);
        public bool IsUp(Keys keys) => Keyboard.IsKeyUp(keys);
        public bool IsUp(MouseButtons buttons) => (GetMouseButtonState(buttons) == ButtonState.Released);

        // These functions return true if the specified thumbstick is pressed in the specified direction.

        public bool IsStickDown(Thumbsticks sticks, float tolerance=0, int i=0) => GetThumbStick(sticks, i).Y < -tolerance;
        public bool IsStickUp(Thumbsticks sticks, float tolerance=0, int i=0) => GetThumbStick(sticks, i).Y > tolerance;
        public bool IsStickLeft(Thumbsticks sticks, float tolerance=0, int i=0) => GetThumbStick(sticks, i).X < -tolerance;
        public bool IsStickRight(Thumbsticks sticks, float tolerance=0, int i=0) => GetThumbStick(sticks, i).X > tolerance;

        /// <summary>
        /// Helper function for getting a ButtonState using our MouseButtons enumerator.
        /// </summary>
        private ButtonState GetMouseButtonState(MouseButtons buttons)
        {
            switch (buttons)
            {
                case MouseButtons.Left: return Mouse.LeftButton;
                case MouseButtons.Middle: return Mouse.MiddleButton;
                case MouseButtons.Right: return Mouse.RightButton;
                case MouseButtons.XButton1: return Mouse.XButton1;
                case MouseButtons.XButton2: return Mouse.XButton2;

                default:
                    throw new ArgumentOutOfRangeException("button " + buttons + " does not exist");
            }
        }

        /// <summary>
        /// Helper function for getting a thumbstick Vector2 using out Thumbsticks enumerator.
        /// </summary>
        private Vector2 GetThumbStick(Thumbsticks sticks, int i=0)
        {
            switch (sticks)
            {
                case Thumbsticks.Left: return Gamepads[i].ThumbSticks.Left;
                case Thumbsticks.Right: return Gamepads[i].ThumbSticks.Right;

                default:
                    throw new ArgumentOutOfRangeException("thumcstick " + sticks + " does not exist");
            }
        }
    }

    /// <summary>
    /// Keeps track of the current and previous input states and provides helper functions
    /// for checking if buttons or keys are pressed in one state and released in another.
    /// </summary>
    class InputManager
    {
        public GamePadState[] Gamepads => Current.Gamepads;
        public KeyboardState Keyboard => Current.Keyboard;
        public MouseState Mouse => Current.Mouse;

        public InputType MostRecentInputType;

        InputState Current = new InputState();
        InputState Prev = new InputState();

        public InputManager()
        {
            Update();

            MostRecentInputType = Current.Gamepads.Any(gp => gp.IsConnected) ? InputType.Gamepad : InputType.KBM;
        }

        public void Update()
        {
            // Update the previous input state with existing values.

            if (!CompareMouseState(Current.Mouse, Prev.Mouse) || !Current.Keyboard.Equals(Prev.Keyboard))
            {
                MostRecentInputType = InputType.KBM;
            }

            for (int i = 0; i < 4; i++)
            {
                if (!Current.Gamepads[i].Equals(Prev.Gamepads[i]))
                    MostRecentInputType = InputType.Gamepad;

                Prev.Gamepads[i] = Current.Gamepads[i];
            }

            Prev.Keyboard = Current.Keyboard;
            Prev.Mouse = Current.Mouse;

            // Update the current input state with the latest values.

            Current.Gamepads[0] = GamePad.GetState(PlayerIndex.One);
            Current.Gamepads[1] = GamePad.GetState(PlayerIndex.Two);
            Current.Gamepads[2] = GamePad.GetState(PlayerIndex.Three);
            Current.Gamepads[3] = GamePad.GetState(PlayerIndex.Four);

            Current.Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Current.Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        private bool CompareMouseState(MouseState ms1, MouseState ms2)
        {
            if (ms1.X == 0 || ms2.X == 0 || ms1.Y == 0 || ms2.Y == 0)
                return true;

            return
                ms1.LeftButton == ms2.LeftButton &&
                ms1.MiddleButton == ms2.MiddleButton &&
                ms1.RightButton == ms2.RightButton &&
                ms1.ScrollWheelValue == ms2.ScrollWheelValue &&
                (ms1.Position - ms2.Position).ToVector2().Length() < 10;
        }

        // These functions return true if the specified button, key or mouse button is down.

        public bool IsDown(Buttons buttons, int i = 0) => Current.IsDown(buttons, i);
        public bool IsDown(Keys keys) => Current.IsDown(keys);
        public bool IsDown(MouseButtons buttons) => Current.IsDown(buttons);

        // These functions return true if the specified button, key or mouse button is up.

        public bool IsUp(Buttons buttons, int i = 0) => Current.IsUp(buttons, i);
        public bool IsUp(Keys keys) => Current.IsUp(keys);
        public bool IsUp(MouseButtons buttons) => Current.IsUp(buttons);

        // These functions return true if the specified button, key or mouse button is down and was previously up.

        public bool IsJustPressed(Buttons buttons, int i=0) => Current.IsDown(buttons, i) && Prev.IsUp(buttons, i);
        public bool IsJustPressed(Keys keys) => Current.IsDown(keys) && Prev.IsUp(keys);
        public bool IsJustPressed(MouseButtons buttons) => Current.IsDown(buttons) && Prev.IsUp(buttons);

        // These functions return true if the specified button, key or mouse button is up and was previously down.

        public bool IsJustReleased(Buttons buttons, int i=0) => Current.IsUp(buttons, i) && Prev.IsDown(buttons, i);
        public bool IsJustReleased(Keys keys) => Current.IsUp(keys) && Prev.IsDown(keys);
        public bool IsJustReleased(MouseButtons buttons) => Current.IsUp(buttons) && Prev.IsDown(buttons);

        // These functions return true if the specified thumbstick is pressed in the specified direction.

        public bool IsStickDown(Thumbsticks sticks, float tolerance=0, int i=0) => Current.IsStickDown(sticks, tolerance, i);
        public bool IsStickUp(Thumbsticks sticks, float tolerance=0, int i=0) => Current.IsStickUp(sticks, tolerance, i);
        public bool IsStickLeft(Thumbsticks sticks, float tolerance=0, int i=0) => Current.IsStickLeft(sticks, tolerance, i);
        public bool IsStickRight(Thumbsticks sticks, float tolerance=0, int i=0) => Current.IsStickRight(sticks, tolerance, i);
    }
}
