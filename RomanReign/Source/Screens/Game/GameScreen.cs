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

        public PhysicsManager Physics;

        public Hud Hud;
        public Camera Camera;
        public Map Map;
        public Player Player;
        public List<Enemy> Enemies = new List<Enemy>();

        // These variables are used for spawning enemies.

        public int Wave;
        public int WaveEnemies;
        public int WaveEnemiesSpawned;
        public int WaveEnemiesKilled;

        public const float WAVE_COOLDOWN = 3f;
        public float TimeSinceWaveStarted;

        // This variable allows us to access important functions and variables in the
        // main game class. You will see this variable in *all* of the screen classes.

        RomanReignGame m_game;

        // This screen is designed to be covered up by other screens (such as the pause
        // screen).  We need a variable to keep track of when the screen is covered and
        // when it is not, which is what we use these boolean variables for.

        bool m_paused;
        bool m_gameOver;
        bool m_hideHud;

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
            Physics = new PhysicsManager();

            Hud = new Hud(this, m_game, content);

            Camera = new Camera(this, m_game) {
                Origin = new Vector2(0.5f, 0.5f)
            };

            Map = new Map(this, m_game, content, "Maps/Test");

            Player = new Player(this, m_game, content);

            WaveEnemies = 1;
            WaveEnemiesKilled = 1;

            m_game.Audio.BackgroundMusic.OnLoop += () => {
                float pitch;
                if      (Wave < 3)  pitch = 0;
                else if (Wave < 5)  pitch = 0.1f;
                else if (Wave < 10) pitch = 0.2f;
                else                pitch = 0.3f;
                m_game.Audio.BackgroundMusic.Pitch = pitch;
                m_game.Audio.BackgroundMusic.TargetPitch = pitch;
            };

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
                m_game.Debug.Enabled = !m_game.Debug.Enabled;

            if (m_game.Input.IsJustReleased(Keys.F7))
                m_hideHud = !m_hideHud;

            if (m_game.Input.IsJustReleased(Keys.F8))
            {
                m_romanRain = !m_romanRain;
                Player.Invincible = true;
            }
#endif

            Camera.Update(gameTime);

            if (!m_paused && !m_gameOver)
            {
                if (m_romanRain)
                {
                    if (Enemies.Count > 300)
                        Enemies.RemoveAt(0);

                    Property<Vector2> spawnPoint = new Vector2(Random.Next(Map.Bounds.Right), 0);
                    spawnPoint.Name = "enemy";
                    Enemies.Add(new Enemy(this, m_game, m_game.Content, spawnPoint));
                }
                else
                {
                    if (WaveEnemiesKilled >= WaveEnemies)
                    {
                        Wave++;
                        WaveEnemies = (int)Math.Ceiling(WaveEnemies * 1.2f);
                        WaveEnemiesSpawned = 0;
                        WaveEnemiesKilled = 0;

                        TimeSinceWaveStarted = 0;
                    }

                    int spawnChance;    // out of 100
                    if      (Wave < 3)  spawnChance = 2;
                    else if (Wave < 5)  spawnChance = 3;
                    else if (Wave < 10) spawnChance = 4;
                    else                spawnChance = 6;

                    bool spawnEnemy = (Random.Next(100) < spawnChance || Enemies.Count == 0);
                    if (WaveEnemiesSpawned < WaveEnemies && TimeSinceWaveStarted > WAVE_COOLDOWN && spawnEnemy)
                    {
                        // 50% chance of spawning on the left
                        bool spawnOnLeft = (Random.NextDouble() >= 0.5);
                        float posX = spawnOnLeft ? Map.Bounds.Left + 30 : Map.Bounds.Right - 30;

                        if (posX < Map.Bounds.Left)
                            posX = Map.Bounds.Left + 30;

                        if (posX > Map.Bounds.Right)
                            posX = Map.Bounds.Right - 30;

                        // 50% change of spawning on the wall
                        bool spawnOnWall = (Random.NextDouble() >= 0.5);
                        float posY = spawnOnWall ? 373 : 610;

                        // TODO: fix spawn points
                        Property<Vector2> spawnPoint = new Vector2(posX, posY);
                        spawnPoint.Name = "enemy";
                        Enemies.Add(new Enemy(this, m_game, m_game.Content, spawnPoint));
                        WaveEnemiesSpawned++;
                    }
                }

                TimeSinceWaveStarted += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Physics.Update(1 / 60f);

                if (m_game.Input.IsJustReleased(Keys.Escape) || m_game.Input.IsJustReleased(Buttons.Start))
                {
                    m_game.Screens.Push(new PauseScreen(m_game));
                }

                Player.Update(gameTime);

                if (Player.Position.Y > Map.Bounds.Bottom)
                    Player.Position += new Vector2(0, 610);

                foreach (var enemy in Enemies)
                {
                    enemy.Update(gameTime);

                    if (!Map.Bounds.Contains(enemy.Position))
                    {
                        // 50% chance of spawning on the left
                        bool spawnOnLeft = (Random.NextDouble() >= 0.5);
                        float posX = spawnOnLeft ? Camera.Bounds.Left + 30 : Camera.Bounds.Right - 30;

                        if (posX < Map.Bounds.Left)
                            posX = Map.Bounds.Left + 30;

                        if (posX > Map.Bounds.Right)
                            posX = Map.Bounds.Right - 30;

                        // 50% change of spawning on the wall
                        bool spawnOnWall = (Random.NextDouble() >= 0.5);
                        float posY = spawnOnWall ? 373 : 610;

                        // TODO: fix spawn points
                        enemy.Position = new Vector2(posX, posY);
                    }

                    if (enemy.Lives <= 0)
                    {
                        WaveEnemiesKilled++;
                    }
                }

                Enemies.RemoveAll(e => e.Lives <= 0);

                if (Player.Lives <= 0)
                {
                    m_game.Screens.Push(new EndScreen(m_game));
                }

                if (Wave > 10)
                {
                    // TODO: 'you won' screen
                }
            }
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: explanation of how the camera works.

            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

            Map.Draw(spriteBatch);
            Player.Draw(spriteBatch);

            foreach (var enemy in Enemies)
                enemy.Draw(spriteBatch);

            spriteBatch.End();

            if (!m_paused && !m_gameOver && !m_hideHud)
            {
                // Now we want to draw the HUD without our coordinates being transformed using the
                // camera, so we need to call spriteBatch.Begin() again, this time without using
                // the camera transformation matrix.

                spriteBatch.Begin();

                Hud.Draw(spriteBatch);

                spriteBatch.End();
            }
        }

        /// <summary>
        /// This function is called when the screen is covered up by another screen.
        /// </summary>
        public void Covered(IScreen other)
        {
            if (other is PauseScreen || other is IntroScreen)
                m_paused = true;

            if (other is EndScreen)
                m_gameOver = true;
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed.
        /// </summary>
        public void Uncovered(IScreen other)
        {
            if (other is PauseScreen || other is IntroScreen)
                m_paused = false;

            if (other is EndScreen)
                m_gameOver = false;
        }
    }
}
