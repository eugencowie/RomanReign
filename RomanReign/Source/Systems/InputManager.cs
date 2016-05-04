using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace RomanReign
{
    enum InputType { KBM, Gamepad }

    enum MouseButtons { Left, Middle, Right, XButton1, XButton2 }

    delegate bool InputAction();

    class InputManager
    {
        public GamePadState[] PrevGamepad = new GamePadState[4];
        public GamePadState[] Gamepad = new GamePadState[4];

        public KeyboardState PrevKeyboard;
        public KeyboardState Keyboard;

        public MouseState PrevMouse;
        public MouseState Mouse;

        public InputType MostRecentInputType;

        public InputManager()
        {
            Begin();
            End();

            MostRecentInputType = Gamepad.Any(gp => gp.IsConnected) ? InputType.Gamepad : InputType.KBM;
        }

        public void Begin()
        {
            Gamepad[0] = GamePad.GetState(PlayerIndex.One);
            Gamepad[1] = GamePad.GetState(PlayerIndex.Two);
            Gamepad[2] = GamePad.GetState(PlayerIndex.Three);
            Gamepad[3] = GamePad.GetState(PlayerIndex.Four);

            Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public void End()
        {
            if (!CompareMouseState(Mouse, PrevMouse) || !Keyboard.Equals(PrevKeyboard))
            {
                MostRecentInputType = InputType.KBM;
            }

            for (int i=0; i<4; i++)
            {
                if (!Gamepad[i].Equals(PrevGamepad[i]))
                    MostRecentInputType = InputType.Gamepad;

                PrevGamepad[i] = Gamepad[i];
            }

            PrevKeyboard = Keyboard;
            PrevMouse = Mouse;
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

        public bool IsKeyDown(Keys keys) => Keyboard.IsKeyDown(keys);
        public bool IsKeyUp(Keys keys) => Keyboard.IsKeyUp(keys);

        public bool IsKeyJustPressed(Keys keys)
        {
            return (Keyboard.IsKeyDown(keys) && PrevKeyboard.IsKeyUp(keys));
        }

        public bool IsKeyJustReleased(Keys keys)
        {
            return (Keyboard.IsKeyUp(keys) && PrevKeyboard.IsKeyDown(keys));
        }

        public bool IsButtonDown(Buttons buttons, int i=0) => Gamepad[i].IsButtonDown(buttons);
        public bool IsButtonUp(Buttons buttons, int i=0)   => Gamepad[i].IsButtonUp(buttons);

        public bool IsButtonJustPressed(Buttons buttons, int i=0)
        {
            return (Gamepad[i].IsButtonDown(buttons) && PrevGamepad[i].IsButtonUp(buttons));
        }

        public bool IsButtonJustReleased(Buttons buttons, int i=0)
        {
            return (Gamepad[i].IsButtonUp(buttons) && PrevGamepad[i].IsButtonDown(buttons));
        }

        public bool IsLeftStickDown(float tolerance = 0, int i=0)
        {
            return Gamepad[i].ThumbSticks.Left.Y < -tolerance;
        }

        public bool IsLeftStickUp(float tolerance = 0, int i=0)
        {
            return Gamepad[i].ThumbSticks.Left.Y > tolerance;
        }

        public bool IsLeftStickLeft(float tolerance = 0, int i=0)
        {
            return Gamepad[i].ThumbSticks.Left.X < -tolerance;
        }

        public bool IsLeftStickRight(float tolerance = 0, int i=0)
        {
            return Gamepad[i].ThumbSticks.Left.X > tolerance;
        }

        public ButtonState GetMouseButtonState(MouseButtons buttons)
        {
            switch (buttons)
            {
                case MouseButtons.Left:     return Mouse.LeftButton;
                case MouseButtons.Middle:   return Mouse.MiddleButton;
                case MouseButtons.Right:    return Mouse.RightButton;
                case MouseButtons.XButton1: return Mouse.XButton1;
                case MouseButtons.XButton2: return Mouse.XButton2;

                default:
                    throw new ArgumentOutOfRangeException("button " + buttons + "does not exist");
            }
        }

        public ButtonState GetPrevMouseButtonState(MouseButtons buttons)
        {
            switch (buttons)
            {
                case MouseButtons.Left:     return PrevMouse.LeftButton;
                case MouseButtons.Middle:   return PrevMouse.MiddleButton;
                case MouseButtons.Right:    return PrevMouse.RightButton;
                case MouseButtons.XButton1: return PrevMouse.XButton1;
                case MouseButtons.XButton2: return PrevMouse.XButton2;

                default:
                    throw new ArgumentOutOfRangeException("button " + buttons + "does not exist");
            }
        }

        public bool IsMouseButtonJustReleased(MouseButtons buttons)
        {
            return (GetMouseButtonState(buttons) == ButtonState.Released && GetPrevMouseButtonState(buttons) == ButtonState.Pressed);
        }

        public bool IsMouseButtonDown(MouseButtons buttons)
        {
            return GetMouseButtonState(buttons) == ButtonState.Pressed;
        }

        public bool IsMouseButtonUp(MouseButtons buttons)
        {
            return GetMouseButtonState(buttons) == ButtonState.Released;
        }
    }
}
