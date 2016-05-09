using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    interface IScreen
    {
        /// <summary>
        /// Called when the screen is loaded.
        /// </summary>
        void LoadContent(ContentManager content);

        /// <summary>
        /// Called when the screen is removed or the game is exiting.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Called every frame when the screen should update.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Called every frame when the screen should draw.
        /// </summary>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Called when this screen is covered by another screen.
        /// </summary>
        void Covered(IScreen other);

        /// <summary>
        /// Called when the screen covering this one is removed.
        /// </summary>
        void Uncovered(IScreen other);
    }

    /// <summary>
    /// Maintains a list of currently active screens.
    /// </summary>
    class ScreenManager
    {
        ContentManager m_content;
        SpriteBatch m_spriteBatch;

        List<IScreen> m_screens = new List<IScreen>();

        List<IScreen> m_copy = new List<IScreen>();
        bool m_invalidateCopy = true;

        /// <summary>
        /// Get the top-most screen.
        /// </summary>
        public IScreen Top => m_screens[m_screens.Count - 1];

        public ScreenManager(ContentManager content, SpriteBatch spriteBatch)
        {
            m_content = content;
            m_spriteBatch = spriteBatch;
        }

        public void UnloadContent()
        {
            Clear();
        }

        /// <summary>
        /// Update all screens, from bottom-most to top-most.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (m_invalidateCopy)
            {
                m_copy = m_screens.ToList();
                m_invalidateCopy = false;
            }

            foreach (var screen in m_copy)
            {
                screen.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all screens, from bottom-most to top-most.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            if (m_invalidateCopy)
            {
                m_copy = m_screens.ToList();
                m_invalidateCopy = false;
            }

            foreach (var screen in m_copy)
            {
                screen.Draw(gameTime, m_spriteBatch);
            }
        }

        /// <summary>
        /// Remove all screens.
        /// </summary>
        public void Clear()
        {
            while (m_screens.Any())
            {
                Pop();
            }
        }

        /// <summary>
        /// Remove all existing screens and then add the specified screen.
        /// </summary>
        public void SwitchTo(IScreen screen)
        {
            Clear();

            Push(screen);
        }

        /// <summary>
        /// Add a new screen on top of the current screen.
        /// </summary>
        public void Push(IScreen screen)
        {
            if (m_screens.Any())
            {
                Top.Covered(screen);
            }

            m_screens.Add(screen);
            m_invalidateCopy = true;

            screen.LoadContent(m_content);
        }

        /// <summary>
        /// Remove the top-most screen.
        /// </summary>
        public void Pop()
        {
            if (m_screens.Any())
            {
                IScreen old = Top;

                old.UnloadContent();

                m_screens.Remove(old);
                m_invalidateCopy = true;

                if (m_screens.Any())
                {
                    Top.Uncovered(old);
                }
            }
        }
    }
}
