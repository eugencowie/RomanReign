using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    enum MouseButtons { Left, Middle, Right, XButton1, XButton2 }

    class Input
    {
        public static GamePadState[] PrevGamepad = new GamePadState[4];
        public static GamePadState[] Gamepad = new GamePadState[4];

        public static KeyboardState PrevKeyboard;
        public static KeyboardState Keyboard;

        public static MouseState PrevMouse;
        public static MouseState Mouse;

        public static void Begin()
        {
            Gamepad[0] = GamePad.GetState(PlayerIndex.One);
            Gamepad[1] = GamePad.GetState(PlayerIndex.Two);
            Gamepad[2] = GamePad.GetState(PlayerIndex.Three);
            Gamepad[3] = GamePad.GetState(PlayerIndex.Four);

            Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public static void End()
        {
            for (int i=0; i<4; i++)
            {
                PrevGamepad[i] = Gamepad[i];
            }

            PrevKeyboard = Keyboard;
            PrevMouse = Mouse;
        }

        public static bool IsKeyJustReleased(Keys keys)
        {
            return (Keyboard.IsKeyUp(keys) && PrevKeyboard.IsKeyDown(keys));
        }

        public static bool IsMouseButtonJustReleased(MouseButtons buttons)
        {
            ButtonState currentState = ButtonState.Released;
            ButtonState prevState = ButtonState.Released;

            switch (buttons)
            {
                case MouseButtons.Left:
                    currentState = Mouse.LeftButton;
                    prevState = PrevMouse.LeftButton;
                    break;

                case MouseButtons.Middle:
                    currentState = Mouse.MiddleButton;
                    prevState = PrevMouse.MiddleButton;
                    break;

                case MouseButtons.Right:
                    currentState = Mouse.RightButton;
                    prevState = PrevMouse.RightButton;
                    break;

                case MouseButtons.XButton1:
                    currentState = Mouse.XButton1;
                    prevState = PrevMouse.XButton1;
                    break;

                case MouseButtons.XButton2:
                    currentState = Mouse.XButton2;
                    prevState = PrevMouse.XButton2;
                    break;
            }

            return (currentState == ButtonState.Released && prevState == ButtonState.Pressed);
        }
    }
}
