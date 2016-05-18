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

        public Vector2 Position => m_physicsBody.Position;
        public Vector2 Velocity => m_physicsBody.Velocity;
        public RectangleF Bounds => m_physicsBody.Bounds;

        public bool IsJumping  => m_isJumping;
        public bool IsDropping => m_isDropping;

        public bool OnGround
        {
            get { return m_onGround; }
            set { m_onGround = value; }
        }

        public int DisplayLives => Lives + (m_loseLife ? -1 : 0);

        public int Lives = 3;
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

        bool m_isJumping;
        bool m_isDropping;
        bool m_isAttacking;

        bool m_loseLife;

        bool m_onGround;

        SoundEffect Attack1;
        SoundEffect Attack2;
        SoundEffect Hurt;
        SoundEffect Jump1;
        SoundEffect Jump2;

        public Player(GameScreen screen, RomanReignGame game, ContentManager content)
        {
            m_game = game;
            m_screen = screen;

            // Load walking animation.

            m_walkingAnimation = new AnimatedSprite(4, 1, 8, true, content.Load<Texture2D>("Textures/Game/player_walking")) {
                Position = m_screen.Map.Info.PlayerSpawn.Value
            };
            m_walkingAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Load attack animation.

            m_attackAnimation = new AnimatedSprite(5, 1, 20, false, content.Load<Texture2D>("Textures/Game/player_attack")) {
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
                    m_onGround = true;
                if (other.Name == "wall" && (IsJumping || IsDropping))
                    return CollisionResponse.NoBlock;
                return CollisionResponse.Block;
            };

            m_screen.Physics.AddRigidBody(m_physicsBody);

            // Set up input events.

            InputManager i = m_game.Input;

            m_jumpActions.Add(() =>
                i.IsJustPressed(Keys.Z) &&
                i.IsUp(Keys.Down));

            m_jumpActions.Add(() =>
                i.IsJustPressed(Buttons.A) &&
                (i.IsUp(Buttons.DPadDown) && !i.IsStickDown(Thumbsticks.Left, 0.5f)));

            m_dropActions.Add(() =>
                i.IsJustPressed(Keys.Z) &&
                i.IsDown(Keys.Down));

            m_dropActions.Add(() =>
                i.IsJustPressed(Buttons.A) &&
                (i.IsDown(Buttons.DPadDown) || i.IsStickDown(Thumbsticks.Left, 0.5f)));

            m_moveLeftActions.Add(() =>
                i.IsDown(Keys.Left) &&
                i.IsUp(Keys.Right));

            m_moveLeftActions.Add(() =>
                (i.IsDown(Buttons.DPadLeft) || i.IsStickLeft(Thumbsticks.Left)) &&
                (i.IsUp(Buttons.DPadRight) && !i.IsStickRight(Thumbsticks.Left)));

            m_moveRightActions.Add(() =>
                i.IsDown(Keys.Right) &&
                i.IsUp(Keys.Left));

            m_moveRightActions.Add(() =>
                (i.IsDown(Buttons.DPadRight) || i.IsStickRight(Thumbsticks.Left)) &&
                (i.IsUp(Buttons.DPadLeft) && !i.IsStickLeft(Thumbsticks.Left)));

            m_attackActions.Add(() => i.IsJustPressed(Keys.X) && !m_isAttacking);

            m_attackActions.Add(() => i.IsJustPressed(Buttons.X) && !m_isAttacking);

            Attack1 = content.Load<SoundEffect>("Audio/sfx_player_attack1");
            Attack2 = content.Load<SoundEffect>("Audio/sfx_player_attack2");
            Hurt = content.Load<SoundEffect>("Audio/sfx_player_hurt");
            Jump1 = content.Load<SoundEffect>("Audio/sfx_player_jump1");
            Jump2 = content.Load<SoundEffect>("Audio/sfx_player_jump2");
        }

        public void Update(GameTime gameTime)
        {
            if (m_physicsBody.Velocity.Y >= 0)
            {
                m_isJumping = false;
            }
            if (m_jumpActions.Any(a => a()) && m_onGround)
            {
                m_physicsBody.Velocity.Y -= 870f;
                m_isJumping = true;
                int Sound = Random.Next(2);
                new[] { Jump1, Jump2 }[Sound].Play();
            }

            if (m_physicsBody.Velocity.Y <= 0)
            {
                m_isDropping = false;
            }
            if (m_dropActions.Any(a => a()))
            {
                m_isDropping = true;
            }

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

            if (m_attackActions.Any(a => a()))
            {
                m_isAttacking = true;
                int Sound = Random.Next(2);
                new[] { Attack1, Attack2 }[Sound].Play();
            }

            m_walkingAnimation.Position = m_physicsBody.Position;
            m_attackAnimation.Position = m_physicsBody.Position;

            m_walkingAnimation.Visible = !m_isAttacking;
            m_attackAnimation.Visible = m_isAttacking;

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

            if (m_timeSinceDamage > DAMAGE_COOLDOWN && !m_loseLife)
            {
                m_loseLife = true;

                Vector2 impluse = new Vector2(2000, -200);
                if (m_walkingAnimation.Effects == SpriteEffects.None)
                    impluse.X *= -1;

                m_physicsBody.Velocity += impluse;

                m_timeSinceDamage = 0;

                Hurt.Play();
                return true;
            }

            return false;
        }
    }
}
