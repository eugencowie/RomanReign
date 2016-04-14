using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    enum MouseButtons { Left, Middle, Right, XButton1, XButton2 }

    class InputManager
    {
        public GamePadState[] PrevGamepad = new GamePadState[4];
        public GamePadState[] Gamepad = new GamePadState[4];

        public KeyboardState PrevKeyboard;
        public KeyboardState Keyboard;

        public MouseState PrevMouse;
        public MouseState Mouse;

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
            for (int i=0; i<4; i++)
            {
                PrevGamepad[i] = Gamepad[i];
            }

            PrevKeyboard = Keyboard;
            PrevMouse = Mouse;
        }

        public bool IsKeyDown(Keys keys) => Keyboard.IsKeyDown(keys);
        public bool IsKeyUp(Keys keys) => Keyboard.IsKeyUp(keys);

        public bool IsKeyJustReleased(Keys keys)
        {
            return (Keyboard.IsKeyUp(keys) && PrevKeyboard.IsKeyDown(keys));
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
