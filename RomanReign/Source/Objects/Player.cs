using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    class Player
    {
        static Random Random = new Random();

        public Vector2 Position
        {
            get { return m_physicsBody.Position; }
            set { m_physicsBody.Position = value; }
        }

        public Vector2 Velocity => m_physicsBody.Velocity;
        public RectangleF Bounds => m_physicsBody.Bounds;

        public bool IsJumping { get; private set; }
        public bool IsDropping { get; private set; }

        public bool OnGround;

        public int DisplayLives => Lives + (m_loseLife ? -1 : 0);

        public int Lives = 4;
        public bool Invincible;

        RomanReignGame m_game;
        GameScreen m_screen;

        AnimatedSprite m_walkingAnimation;
        AnimatedSprite m_attackAnimation;

        DynamicBody m_physicsBody;

        RectangleF m_attackRect;

        List<InputAction> m_jumpActions = new List<InputAction>();
        List<InputAction> m_dropActions = new List<InputAction>();
        List<InputAction> m_moveLeftActions = new List<InputAction>();
        List<InputAction> m_moveRightActions = new List<InputAction>();
        List<InputAction> m_attackActions = new List<InputAction>();

        const float DAMAGE_COOLDOWN = 1f;
        float m_timeSinceDamage = DAMAGE_COOLDOWN;

        bool m_isAttacking;

        bool m_loseLife;

        SoundEffect m_attackSound1;
        SoundEffect m_attackSound2;
        SoundEffect m_hurtSound;
        SoundEffect m_jumpSound1;
        SoundEffect m_jumpSound2;
        SoundEffect m_pickupSound;

        public Player(GameScreen screen, RomanReignGame game, ContentManager content, PlayerIndex? playerIndex=null)
        {
            m_game = game;
            m_screen = screen;

            // Load walking animation.

            string texture = "player1";
            if (playerIndex != null)
            {
                texture = "player" + ((int)playerIndex + 1);
            }

            m_walkingAnimation = new AnimatedSprite(4, 1, 8, true, content.Load<Texture2D>("Textures/Game/" + texture  + "_walking")) {
                Position = m_screen.Map.Info.PlayerSpawn.Value
            };
            m_walkingAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Load attack animation.

            m_attackAnimation = new AnimatedSprite(5, 1, 20, false, content.Load<Texture2D>("Textures/Game/" + texture + "_attack")) {
                Position = m_screen.Map.Info.PlayerSpawn.Value
            };
            m_attackAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Create physics body.

            m_physicsBody = new DynamicBody {
                Name = m_screen.Map.Info.PlayerSpawn.Name,
                Position = m_screen.Map.Info.PlayerSpawn.Value,
                Size = m_walkingAnimation.Bounds.Size.ToVector2(),
                LinearDamping = new Vector2(0.2f, 0f),
                UserData = this
            };
            m_physicsBody.SetRelativeOrigin(0.5f, 0.5f);
            m_physicsBody.OnCollision += other => {
                if ((other.Name == "wall" || other.Name == "ground") && !IsJumping && !IsDropping)
                    OnGround = true;
                if (other.Name == "wall" && (IsJumping || IsDropping))
                    return CollisionResponse.NoBlock;
                return CollisionResponse.Block;
            };

            m_screen.Physics.AddDynamicBody(m_physicsBody);

            // Set up input events.

            InputManager i = m_game.Input;

            if (playerIndex.HasValue)
            {
                m_jumpActions.Add(() =>
                    i.IsJustPressed(Buttons.A, playerIndex.Value) &&
                    i.IsUp(Buttons.DPadDown, playerIndex.Value) && !i.IsStickDown(Thumbsticks.Left, playerIndex.Value, 0.5f));

                m_dropActions.Add(() =>
                    //i.IsJustPressed(Buttons.A, (int)playerIndex.Value) &&
                    (i.IsDown(Buttons.DPadDown, playerIndex.Value) || i.IsStickDown(Thumbsticks.Left, playerIndex.Value, 0.5f)));

                m_moveLeftActions.Add(() =>
                    (i.IsDown(Buttons.DPadLeft, playerIndex.Value) || i.IsStickLeft(Thumbsticks.Left, playerIndex.Value)) &&
                    (i.IsUp(Buttons.DPadRight, playerIndex.Value) && !i.IsStickRight(Thumbsticks.Left, playerIndex.Value)));

                m_moveRightActions.Add(() =>
                    (i.IsDown(Buttons.DPadRight, playerIndex.Value) || i.IsStickRight(Thumbsticks.Left, playerIndex.Value)) &&
                    (i.IsUp(Buttons.DPadLeft, playerIndex.Value) && !i.IsStickLeft(Thumbsticks.Left, playerIndex.Value)));

                m_attackActions.Add(() =>
                    i.IsJustPressed(Buttons.X, playerIndex.Value) && !m_isAttacking);
            }
            else
            {
                m_jumpActions.Add(() =>
                    i.IsJustPressed(Keys.Z) &&
                    i.IsUp(Keys.Down));

                m_dropActions.Add(() =>
                    //i.IsJustPressed(Keys.Z) &&
                    i.IsDown(Keys.Down));

                m_moveLeftActions.Add(() =>
                    i.IsDown(Keys.Left) &&
                    i.IsUp(Keys.Right));

                m_moveRightActions.Add(() =>
                    i.IsDown(Keys.Right) &&
                    i.IsUp(Keys.Left));

                m_attackActions.Add(() =>
                    i.IsJustPressed(Keys.X) && !m_isAttacking);
            }

            m_attackSound1 = content.Load<SoundEffect>("Audio/sfx_player_attack1");
            m_attackSound2 = content.Load<SoundEffect>("Audio/sfx_player_attack2");
            m_hurtSound = content.Load<SoundEffect>("Audio/sfx_player_hurt");
            m_jumpSound1 = content.Load<SoundEffect>("Audio/sfx_player_jump1");
            m_jumpSound2 = content.Load<SoundEffect>("Audio/sfx_player_jump2");
            m_pickupSound = content.Load<SoundEffect>("Audio/sfx_menu_select");
        }

        public void Update(GameTime gameTime)
        {
            // Jumping

            if (m_physicsBody.Velocity.Y >= 0)
            {
                IsJumping = false;
            }
            if (m_jumpActions.Any(a => a()) && OnGround)
            {
                m_physicsBody.Velocity.Y -= 870f;
                IsJumping = true;

                SoundEffect[] sounds = { m_jumpSound1, m_jumpSound2 };
                int sound = Random.Next(sounds.Length);
                sounds[sound].Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
            }

            // Dropping

            if (m_physicsBody.Velocity.Y <= 0)
            {
                IsDropping = false;
            }
            if (m_dropActions.Any(a => a()))
            {
                IsDropping = true;
            }

            // Movement

            if (m_moveLeftActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X -= 175f;
                m_walkingAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_attackAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_walkingAnimation.Update(gameTime);
            }

            if (m_moveRightActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X += 175f;
                m_walkingAnimation.Effects = SpriteEffects.None;
                m_attackAnimation.Effects = SpriteEffects.None;
                m_walkingAnimation.Update(gameTime);
            }

            // Attacking

            if (m_attackActions.Any(a => a()))
            {
                m_isAttacking = true;

                SoundEffect[] sounds = { m_attackSound1, m_attackSound2 };
                int sound = Random.Next(sounds.Length);
                sounds[sound].Play(0.5f * Config.Data.Volume.SfxNormal, 0f, 0f);
            }

            // Animation

            m_walkingAnimation.Position = m_physicsBody.Position;
            m_attackAnimation.Position = m_physicsBody.Position;

            m_walkingAnimation.Visible = !m_isAttacking;
            m_attackAnimation.Visible = m_isAttacking;

            // Attacking

            m_attackRect = m_physicsBody.Bounds;
            m_attackRect.Size.X += 20;
            if (m_walkingAnimation.Effects == SpriteEffects.FlipHorizontally)
                m_attackRect.Location.X -= 20;

            if (m_isAttacking)
            {
                m_attackAnimation.Update(gameTime);

                foreach (Enemy enemy in m_screen.Enemies.Where(e => e.Bounds.Intersects(m_attackRect)))
                {
                    if (enemy.TakeDamage())
                        m_physicsBody.Velocity.X *= -0.5f;
                }

                if (m_attackAnimation.IsFinished())
                {
                    m_attackAnimation.Reset();
                    m_isAttacking = false;
                }
            }

            // Damage

            if (m_timeSinceDamage < DAMAGE_COOLDOWN)
            {
                m_walkingAnimation.Color = Color.Red;
                m_attackAnimation.Color = Color.Red;
            }
            else
            {
                m_walkingAnimation.Color = Color.White;
                m_attackAnimation.Color = Color.White;

                if (m_loseLife)
                {
                    Lives--;
                    m_loseLife = false;
                }
            }

            m_timeSinceDamage += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Pickups

            if (Lives < 4)
            {
                foreach (var pickup in m_screen.Pickups.Where(p => p.Bounds.Intersects(Bounds)).ToList())
                {
                    m_pickupSound.Play(0.25f * Config.Data.Volume.SfxNormal, 0f, 0f);
                    m_screen.Pickups.Remove(pickup);
                    Lives++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_walkingAnimation.Draw(spriteBatch);
            m_attackAnimation.Draw(spriteBatch);

            m_game.Debug.Draw(m_physicsBody.Bounds.ToRect(), Color.Blue);
            m_game.Debug.Draw(m_attackRect.ToRect(), Color.Green);
        }

        public bool TakeDamage()
        {
            if (Invincible)
                return false;

            // Taking damage

            if (m_timeSinceDamage > DAMAGE_COOLDOWN && !m_loseLife)
            {
                m_loseLife = true;

                Vector2 impluse = new Vector2(2000, -200);
                if (m_walkingAnimation.Effects == SpriteEffects.None)
                    impluse.X *= -1;

                m_physicsBody.Velocity += impluse;

                m_timeSinceDamage = 0;

                m_hurtSound.Play(0.5f * Config.Data.Volume.SfxNormal, 0f, 0f);
                return true;
            }

            return false;
        }
    }
}
