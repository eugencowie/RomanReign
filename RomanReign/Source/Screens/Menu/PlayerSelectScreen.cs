using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RomanReign
{
    class PlayerSelectScreen : IScreen
    {
        RomanReignGame m_game;

        Sprite m_start1;
        Sprite m_start2;
        Sprite m_start3;
        Sprite m_start4;

        enum SelectedButton { None, Start1, Start2, Start3, Start4, Final }
        SelectedButton m_selectedButton;

        SoundEffect m_selectedSound;

        public PlayerSelectScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_start1 = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_start1")) {
                Position = new Vector2(m_game.Viewport.Center.X, 300)
            };
            m_start1.SetRelativeOrigin(0.5f, 0.5f);

            m_start2 = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_start2")) {
                Position = new Vector2(m_game.Viewport.Center.X, 400)
            };
            m_start2.SetRelativeOrigin(0.5f, 0.5f);

            m_start3 = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_start3")) {
                Position = new Vector2(m_game.Viewport.Center.X, 500)
            };
            m_start3.SetRelativeOrigin(0.5f, 0.5f);

            m_start4 = new Sprite(content.Load<Texture2D>("Textures/Menu/btn_start4")) {
                Position = new Vector2(m_game.Viewport.Center.X, 600)
            };
            m_start4.SetRelativeOrigin(0.5f, 0.5f);

            m_selectedButton = SelectedButton.None;

            m_selectedSound = content.Load<SoundEffect>("Audio/sfx_menu_select");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            // If the most recent input was using the keyboard/mouse, then check if the
            // mouse is over any of the buttons. If not then set the currently selected
            // button to none.

            if (m_game.Input.MostRecentInputType == InputType.KBM)
            {
                m_selectedButton = SelectedButton.None;

                if (m_start1.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Start1;

                if (m_start2.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Start2;

                if (m_start3.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Start3;

                if (m_start4.Bounds.Contains(m_game.Input.Mouse.Position))
                    m_selectedButton = SelectedButton.Start4;
            }

            // If the most recent input was using a gamepad then make sure that the
            // currently selected button is not none. If for some reason it is then
            // default to the start button.

            if (m_game.Input.MostRecentInputType == InputType.Gamepad)
            {
                if (m_game.Input.IsJustPressed(Buttons.DPadUp) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickUp))
                {
                    m_selectedButton--;
                }

                if (m_game.Input.IsJustPressed(Buttons.DPadDown) || m_game.Input.IsJustPressed(Buttons.LeftThumbstickDown))
                {
                    m_selectedButton++;
                }

                if (m_selectedButton == SelectedButton.None)
                    m_selectedButton = SelectedButton.None + 1;

                if (m_selectedButton == SelectedButton.Final)
                    m_selectedButton = SelectedButton.Final - 1;
            }

            // Set the opacity of each button to 0.5 (semi-transparent) if the button
            // is selected, otherwise 1 (fully opaque).

            m_start1.SetOpacity(m_selectedButton == SelectedButton.Start1 ? 0.5f : 1f);
            m_start2.SetOpacity(m_selectedButton == SelectedButton.Start2 ? 0.5f : 1f);
            m_start3.SetOpacity(m_selectedButton == SelectedButton.Start3 ? 0.5f : 1f);
            m_start4.SetOpacity(m_selectedButton == SelectedButton.Start4 ? 0.5f : 1f);

            // Next, we check if the left mouse button has just been pressed and then
            // released. If so, we check to see if the mouse is over any of the buttons
            // and take any appropriate action.

            if (m_game.Input.IsJustReleased(MouseButtons.Left) || m_game.Input.IsJustReleased(Buttons.A))
            {
                m_game.Screens.SwitchTo(new GameScreen(m_game, (int)m_selectedButton));
                m_selectedSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
            }

            if (m_game.Input.IsJustReleased(Keys.Escape) || m_game.Input.IsJustReleased(Buttons.B))
            {
                m_game.Screens.Pop();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            m_start1.Draw(spriteBatch);
            m_start2.Draw(spriteBatch);
            m_start3.Draw(spriteBatch);
            m_start4.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Covered(IScreen other)
        {
        }

        public void Uncovered(IScreen other)
        {
        }
    }
}
