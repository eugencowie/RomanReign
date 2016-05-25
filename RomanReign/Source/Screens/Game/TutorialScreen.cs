using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RomanReign
{
    class TutorialScreen : IScreen
    {
        RomanReignGame m_game;

        List<Texture2D> m_tutorialSprites;
        int m_tutorialSlide;

        public TutorialScreen(RomanReignGame game)
        {
            m_game = game;
        }

        public void LoadContent(ContentManager content)
        {
            m_tutorialSprites = new List<Texture2D>();

            m_tutorialSprites.Add(content.Load<Texture2D>("Textures/Game/bg_tutorial1"));
            m_tutorialSprites.Add(content.Load<Texture2D>("Textures/Game/bg_tutorial2"));
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (m_tutorialSlide >= m_tutorialSprites.Count ||
                m_game.Input.IsJustReleased(Buttons.B) ||
                m_game.Input.IsJustReleased(Keys.Escape))
            {
                m_game.Screens.Pop();
            }

            if (m_game.Input.IsJustReleased(Buttons.A) ||
                m_game.Input.IsJustReleased(Keys.Enter) ||
                m_game.Input.IsJustReleased(Keys.Space))
            {
                m_tutorialSlide++;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (m_tutorialSlide < m_tutorialSprites.Count)
            {
                spriteBatch.Draw(m_tutorialSprites[m_tutorialSlide], m_game.Viewport, Color.White);
            }

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
