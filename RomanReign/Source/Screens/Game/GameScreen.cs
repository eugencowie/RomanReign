using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public List<Player> Players = new List<Player>();
        public List<Player> DeadPlayers = new List<Player>();
        public List<Enemy> Enemies = new List<Enemy>();
        public List<Pickup> Pickups = new List<Pickup>();

        public string PlayerNames = string.Empty;

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
        bool m_hideHud;

        // This boolean is used to toggle the 'roman rain' mode.

        bool m_romanRain;

        // The number of players.

        public int NumberOfPlayers;
        bool m_suppressIntro;

        // Score.

        public int Score;

        /// <summary>
        /// This constructor is run when the game screen object is created.
        /// </summary>
        public GameScreen(RomanReignGame game, int numberOfPlayers, bool suppressIntro=false)
        {
            m_game = game;
            NumberOfPlayers = numberOfPlayers;
            m_suppressIntro = suppressIntro;
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

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                Players.Add(new Player(this, m_game, content, (PlayerIndex)i));
            }

            WaveEnemies = 1;
            WaveEnemiesKilled = 1;

            m_game.Audio.BackgroundMusic.OnLoop += OnBackgroundMusicLoop;

            if (!m_suppressIntro)
            {
                // Load the intro cutscene AFTER the game content has been loaded, so that when the
                // intro is finished the game can start immediately without needing to load anything.
                m_game.Screens.Push(new IntroScreen(m_game));
            }
        }

        /// <summary>
        /// This function is called when the screen is removed from the screen manager, or
        /// if the game exits while the screen is active.
        /// </summary>
        public void UnloadContent()
        {
            m_game.Audio.BackgroundMusic.OnLoop -= OnBackgroundMusicLoop;
            m_game.Audio.BackgroundMusic.Pitch = -0.15f;
            m_game.Audio.BackgroundMusic.TargetPitch = -0.15f;
        }

        /// <summary>
        /// This function is called every frame while the screen is active.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (Players.Count <= 0 && m_romanRain)
            {
                if (m_game.Input.IsJustReleased(Buttons.Start))
                    m_game.Screens.Push(new EndScreen(this, m_game));
            }

#if DEBUG
            // advance to next wave
            if (m_game.Input.IsJustReleased(Keys.Q))
                WaveEnemiesKilled = WaveEnemies;

            // add 60 kills
            if (m_game.Input.IsJustReleased(Keys.W))
            {
                WaveEnemiesKilled += 60;
                WaveEnemiesSpawned += 60;
            }
#endif

            if (m_game.Input.IsJustReleased(Keys.F5))
                m_game.Debug.Enabled = !m_game.Debug.Enabled;

            if (m_game.Input.IsJustReleased(Keys.F7))
                m_hideHud = !m_hideHud;

            if (m_game.Input.IsJustReleased(Keys.F8))
            {
                m_romanRain = !m_romanRain;
                foreach (var player in Players)
                    player.Invincible = true;
            }

            Camera.Update(gameTime);

            if (!m_paused)
            {
                if (m_game.Input.IsJustReleased(Keys.NumPad8))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Property<Vector2> spawnPoint = new Vector2(Random.Next(Map.Bounds.Right), 0);
                        spawnPoint.Name = "enemy";
                        Enemies.Add(new Enemy(this, m_game, m_game.Content, spawnPoint));
                        WaveEnemiesSpawned++;
                    }
                }

                if (m_game.Input.IsJustReleased(Keys.NumPad2))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 spawnPoint = new Vector2(Random.Next(Map.Bounds.Right), -100);
                        Pickups.Add(new Pickup(this, m_game, m_game.Content, spawnPoint));
                    }
                }

                if (m_romanRain)
                {
                    if (Enemies.Count > 300)
                    {
                        Enemies.RemoveAt(0);
                        WaveEnemiesSpawned--;
                    }

                    Property<Vector2> spawnPoint = new Vector2(Random.Next(Map.Bounds.Right), -100);
                    spawnPoint.Name = "enemy";
                    Enemies.Add(new Enemy(this, m_game, m_game.Content, spawnPoint));
                    WaveEnemiesSpawned++;

                    Pickups.Clear();
                }
                else
                {
                    if (WaveEnemiesKilled >= WaveEnemies)
                    {
                        Wave++;
                        WaveEnemies = (int)Math.Ceiling(WaveEnemies * 1.2f);
                        WaveEnemiesSpawned = Enemies.Count;
                        WaveEnemiesKilled = 0;

                        TimeSinceWaveStarted = 0;

                        foreach (var player in DeadPlayers)
                            player.Lives = 1;

                        Players.AddRange(DeadPlayers);
                        DeadPlayers.Clear();
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

                        Property<Vector2> spawnPoint = new Vector2(posX, posY);
                        spawnPoint.Name = "enemy";
                        Enemies.Add(new Enemy(this, m_game, m_game.Content, spawnPoint));
                        WaveEnemiesSpawned++;

                        // The game gets pretty boring from this point on, so enable 'roman reign'
                        // mode if anyone gets this far.

                        if (Config.Data.Internal.WaveLimit > 1 && Wave >= Config.Data.Internal.WaveLimit)
                        {
                            m_romanRain = true;
                        }
                    }
                }

                TimeSinceWaveStarted += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Physics.Update(1 / 60f);

                if (m_game.Input.IsJustReleased(Keys.Escape) || m_game.Input.IsJustReleased(Buttons.Start))
                {
                    m_game.Screens.Push(new PauseScreen(m_game));
                }

                foreach (var player in Players)
                {
                    player.Update(gameTime);

                    if (!player.Bounds.Intersects(Map.Bounds))
                        player.Position += new Vector2(0, 610);
                }

                foreach (var enemy in Enemies)
                {
                    enemy.Update(gameTime);

                    Rectangle bounds = Map.Bounds;
                    bounds.Inflate(200, 200);
                    if (!bounds.Contains(enemy.Position))
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

                        enemy.Position = new Vector2(posX, posY);
                    }

                    if (enemy.Lives <= 0)
                    {
                        WaveEnemiesKilled++;
                    }
                }

                Score += Enemies.Count(e => e.Lives <= 0);
                Enemies.RemoveAll(e => e.Lives <= 0);

                DeadPlayers.AddRange(Players.Where(p => p.Lives <= 0));
                Players.RemoveAll(p => p.Lives <= 0);

                if (Players.Count <= 0 && !m_romanRain)
                {
                    m_game.Screens.Push(new EndScreen(this, m_game));
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

            foreach (var player in Players)
                player.Draw(spriteBatch);

            foreach (var enemy in Enemies)
                enemy.Draw(spriteBatch);

            foreach (var pickup in Pickups)
                pickup.Draw(spriteBatch);

            spriteBatch.End();

            if (!m_paused && !m_hideHud)
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
            if (other is PauseScreen || other is IntroScreen || other is EndScreen)
                m_paused = true;
        }

        /// <summary>
        /// This function is called when the screen on top of this one is removed.
        /// </summary>
        public void Uncovered(IScreen other)
        {
            if (other is PauseScreen || other is IntroScreen || other is EndScreen)
                m_paused = false;
        }

        private void OnBackgroundMusicLoop()
        {
            float pitch;

            if (Wave < 5) pitch = -0.15f;
            else if (Wave < 8) pitch = 0.0f;
            else if (Wave < 10) pitch = 0.15f;
            else if (Wave < 15) pitch = 0.25f;
            else if (Wave < 20) pitch = 0.35f;
            else if (Wave < 22) pitch = 0.5f;
            else pitch = 0.6f;

            m_game.Audio.BackgroundMusic.Pitch = pitch;
            m_game.Audio.BackgroundMusic.TargetPitch = pitch;
        }
    }
}
