using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
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

        public bool IsDown(Buttons buttons, PlayerIndex i=0) => Gamepads[(int)i].IsButtonDown(buttons);
        public bool IsDown(Keys keys) => Keyboard.IsKeyDown(keys);
        public bool IsDown(MouseButtons buttons) => (GetMouseButtonState(buttons) == ButtonState.Pressed);

        // These functions return true if the specified button, key or mouse button is up.

        public bool IsUp(Buttons buttons, PlayerIndex i=0) => Gamepads[(int)i].IsButtonUp(buttons);
        public bool IsUp(Keys keys) => Keyboard.IsKeyUp(keys);
        public bool IsUp(MouseButtons buttons) => (GetMouseButtonState(buttons) == ButtonState.Released);

        // These functions return true if the specified thumbstick is pressed in the specified direction.

        public bool IsStickDown (Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => GetThumbStick(sticks, i).Y < -tolerance;
        public bool IsStickUp   (Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => GetThumbStick(sticks, i).Y > tolerance;
        public bool IsStickLeft (Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => GetThumbStick(sticks, i).X < -tolerance;
        public bool IsStickRight(Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => GetThumbStick(sticks, i).X > tolerance;

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
        private Vector2 GetThumbStick(Thumbsticks sticks, PlayerIndex i=0)
        {
            switch (sticks)
            {
                case Thumbsticks.Left: return Gamepads[(int)i].ThumbSticks.Left;
                case Thumbsticks.Right: return Gamepads[(int)i].ThumbSticks.Right;

                default:
                    throw new ArgumentOutOfRangeException("thumbstick " + sticks + " does not exist");
            }
        }
    }

    /// <summary>
    /// Keeps track of the current and previous input states and provides helper functions
    /// for checking if buttons or keys are pressed in one state and released in another.
    /// </summary>
    class InputManager
    {
        public GamePadState[] Gamepads => m_current.Gamepads;
        public KeyboardState Keyboard => m_current.Keyboard;
        public MouseState Mouse => m_current.Mouse;

        public InputType MostRecentInputType;

        InputState m_current = new InputState();
        InputState m_prev = new InputState();

        public InputManager()
        {
            Update();

            MostRecentInputType = m_current.Gamepads.Any(gp => gp.IsConnected) ? InputType.Gamepad : InputType.KBM;
        }

        public void Update()
        {
            // Update the previous input state with existing values.

            if (!CompareMouseState(m_current.Mouse, m_prev.Mouse) || !m_current.Keyboard.Equals(m_prev.Keyboard))
            {
                MostRecentInputType = InputType.KBM;
            }

            for (int i = 0; i < 4; i++)
            {
                if (!m_current.Gamepads[i].Equals(m_prev.Gamepads[i]))
                    MostRecentInputType = InputType.Gamepad;

                m_prev.Gamepads[i] = m_current.Gamepads[i];
            }

            m_prev.Keyboard = m_current.Keyboard;
            m_prev.Mouse = m_current.Mouse;

            // Update the current input state with the latest values.

            m_current.Gamepads[0] = GamePad.GetState(PlayerIndex.One);
            m_current.Gamepads[1] = GamePad.GetState(PlayerIndex.Two);
            m_current.Gamepads[2] = GamePad.GetState(PlayerIndex.Three);
            m_current.Gamepads[3] = GamePad.GetState(PlayerIndex.Four);

            m_current.Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            m_current.Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
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

        public bool IsDown(Buttons buttons, PlayerIndex i=0) => m_current.IsDown(buttons, i);
        public bool IsDown(Keys keys) => m_current.IsDown(keys);
        public bool IsDown(MouseButtons buttons) => m_current.IsDown(buttons);

        // These functions return true if the specified button, key or mouse button is up.

        public bool IsUp(Buttons buttons, PlayerIndex i=0) => m_current.IsUp(buttons, i);
        public bool IsUp(Keys keys) => m_current.IsUp(keys);
        public bool IsUp(MouseButtons buttons) => m_current.IsUp(buttons);

        // These functions return true if the specified button, key or mouse button is down and was previously up.

        public bool IsJustPressed(Buttons buttons, PlayerIndex i=0) => m_current.IsDown(buttons, i) && m_prev.IsUp(buttons, i);
        public bool IsJustPressed(Keys keys) => m_current.IsDown(keys) && m_prev.IsUp(keys);
        public bool IsJustPressed(MouseButtons buttons) => m_current.IsDown(buttons) && m_prev.IsUp(buttons);

        // These functions return true if the specified button, key or mouse button is up and was previously down.

        public bool IsJustReleased(Buttons buttons, PlayerIndex i=0) => m_current.IsUp(buttons, i) && m_prev.IsDown(buttons, i);
        public bool IsJustReleased(Keys keys) => m_current.IsUp(keys) && m_prev.IsDown(keys);
        public bool IsJustReleased(MouseButtons buttons) => m_current.IsUp(buttons) && m_prev.IsDown(buttons);

        // These functions return true if the specified thumbstick is pressed in the specified direction.

        public bool IsStickDown (Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => m_current.IsStickDown(sticks, i, tolerance);
        public bool IsStickUp   (Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => m_current.IsStickUp(sticks, i, tolerance);
        public bool IsStickLeft (Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => m_current.IsStickLeft(sticks, i, tolerance);
        public bool IsStickRight(Thumbsticks sticks, PlayerIndex i=0, float tolerance=0) => m_current.IsStickRight(sticks, i, tolerance);
    }
}
