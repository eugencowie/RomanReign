using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RomanReign
{
    /// <summary>
    /// The game screen, which contains the actual game.
    /// </summary>
    class GameScreen : IScreen
    {
        static Random Random = new Random();

        // These public fields allow certain game objects to be accessed from outside
        // of this class.

        public Hud    Hud    => m_hud;
        public Camera Camera => m_camera;
        public Map    Map    => m_map;
        public Player Player => m_player;

        public List<Enemy> Enemies => m_enemies;

        // This variable allows us to access important functions and variables in the
        // main game class. You will see this variable in *all* of the screen classes.

        RomanReignGame m_game;

        // Game objects.

        Hud    m_hud;
        Camera m_camera;
        Map    m_map;
        Player m_player;

        List<Enemy> m_enemies = new List<Enemy>();

        // This screen is designed to be covered up by other screens (such as the pause
        // screen).  We need a variable to keep track of when the screen is covered and
        // when it is not, which is what we use this boolean variable for.

        bool m_isCovered;

        // This boolean is used to toggle the 'roman rain' mode.

        bool m_romanRain;

        /// <summary>
        /// This constructor is run when the game screen object is created.
        /// </summary>
        public GameScreen(RomanReignGame game)
        {
            m_game = game;
        }

        /// <summary>
        /// This function is run when we add this screen to the screen manager.
        /// </summary>
        public void LoadContent(ContentManager content)
        {
            m_hud = new Hud(content);

            m_camera = new Camera(this, m_game) {
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_map = new Map(m_game, content, "Maps/Test");

            m_player = new Player(this, m_game, content);

            Property<Vector2> spawnPoint = m_map.Info.EnemySpawns[Random.Next(m_map.Info.EnemySpawns.Count)];
            m_enemies.Add(new Enemy(this, m_game, content, spawnPoint));

            // Load the intro cutscene AFTER the game content has been loaded, so that when the
            // intro is finished the game can start immediately without needing to load anything.
            m_game.Screens.Push(new IntroScreen(m_game));
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active.
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
#if DEBUG
            if (m_game.Input.IsJustReleased(Keys.F5))
            {
                m_game.Debug.Enabled = !m_game.Debug.Enabled;
            }

            if (m_game.Input.IsJustReleased(Keys.F8))
            {
                m_romanRain = !m_romanRain;
            }
#endif

            if (m_romanRain && m_enemies.Count < 5000)
            {
                for (int i = 0; i < 3; i++)
                {
                    Property<Vector2> spawnPoint = new Vector2(Random.Next(m_map.Bounds.Right), 0);
                    spawnPoint.Name = "enemy";
                    m_enemies.Add(new Enemy(this, m_game, m_game.Content, spawnPoint));
                }
            }

            if (!m_isCovered)
            {
                m_game.Physics.Update(1 / 60f);

                if (m_game.Input.IsJustReleased(Keys.Escape) || m_game.Input.IsJustReleased(Buttons.Start))
                {
                    m_game.Screens.Push(new PauseScreen(m_game));
                }

                m_player.Update(gameTime);

                m_camera.Update();

                foreach (var enemy in m_enemies)
                    enemy.Update(gameTime);
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: explanation of how the camera works.

            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            m_map.Draw(spriteBatch);
            m_player.Draw(spriteBatch);

            foreach (var enemy in m_enemies)
                enemy.Draw(spriteBatch);

            spriteBatch.End();

            // Now we want to draw the HUD without our coordinates being transformed using the
            // camera, so we need to call spriteBatch.Begin() again, this time without using
            // the camera transformation matrix.

            spriteBatch.Begin();

            m_hud.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen.
        /// </summary>
        public void Covered(IScreen other)
        {
            m_isCovered = true;
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed.
        /// </summary>
        public void Uncovered(IScreen other)
        {
            m_isCovered = false;
        }
    }
}
