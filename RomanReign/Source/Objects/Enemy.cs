using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanReign
{
    class Enemy
    {
        static Random Random = new Random();

        public Vector2 Position
        {
            get { return m_physicsBody.Position; }
            set { m_physicsBody.Position = value; }
        }

        public RectangleF Bounds => m_physicsBody.Bounds;

        public bool IsJumping { get; private set; }
        public bool IsDropping { get; private set; }

        public bool OnGround;

        public int Lives;

        public int DisplayLives => Lives + (m_loseLife ? -1 : 0);

        RomanReignGame m_game;
        GameScreen m_screen;

        AnimatedSprite m_walkingAnimation;
        AnimatedSprite m_attackAnimation;

        DynamicBody m_physicsBody;

        RectangleF m_attackRect;

        public Color Color;

        List<InputAction> m_jumpActions = new List<InputAction>();
        List<InputAction> m_dropActions = new List<InputAction>();
        List<InputAction> m_moveLeftActions = new List<InputAction>();
        List<InputAction> m_moveRightActions = new List<InputAction>();
        List<InputAction> m_attackActions = new List<InputAction>();

        const float ATTACK_COOLDOWN = 0.5f;
        float m_timeSinceAttack = ATTACK_COOLDOWN;

        const float DAMAGE_COOLDOWN = 0.5f;
        float m_timeSinceDamage = DAMAGE_COOLDOWN;

        int m_walkingSpeed;

        bool m_triggerJump;
        bool m_triggerDrop;

        bool m_canJump;
        bool m_canDrop;

        bool m_isAttacking;

        bool m_loseLife;

        SoundEffect m_hurtSound1;
        SoundEffect m_hurtSound2;
        SoundEffect m_hurtSound3;

        public Enemy(GameScreen screen, RomanReignGame game, ContentManager content, Property<Vector2> spawnPoint)
        {
            m_game = game;
            m_screen = screen;

            // Set random color.

            Color[] options = { Color.White, Color.LightBlue, Color.Gold };
            int option;

            int rand = Random.Next(100);
            if (rand < 10) option = 2;      // 10% chance of spawning gold
            else if (rand < 40) option = 1; // 30% chance of spawning lightblue
            else option = 0;                // 60% chance of spawning regular

            Color = options[option];

            Lives = option + 1;

            m_walkingSpeed = (3 - option) * 3;

            m_canJump = (Random.Next(100) >= 20);
            m_canDrop = (Random.Next(100) >= 20);

            // Load walking animation.

            m_walkingAnimation = new AnimatedSprite(4, 1, m_walkingSpeed, true, content.Load<Texture2D>("Textures/Game/enemy_walking")) {
                Position = spawnPoint.Value,
                Color = Color
            };
            m_walkingAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Load attack animation.

            m_attackAnimation = new AnimatedSprite(4, 1, 8, false, content.Load<Texture2D>("Textures/Game/enemy_attack")) {
                Position = spawnPoint.Value,
                Color = Color
            };
            m_attackAnimation.SetRelativeOrigin(0.5f, 0.5f);

            // Create physics body.

            m_physicsBody = new DynamicBody {
                Name = spawnPoint.Name,
                Position = spawnPoint.Value,
                Size = m_walkingAnimation.Bounds.Size.ToVector2(),
                LinearDamping = new Vector2(0.2f, 0f),
                UserData = this
            };
            m_physicsBody.SetRelativeOrigin(0.5f, 0.5f);
            m_physicsBody.OnCollision += other => {
                if (other.Name == "wall" && (IsJumping || IsDropping))
                    return CollisionResponse.NoBlock;
                if (other.Name == "ground" && !IsJumping && !IsDropping)
                    OnGround = true;
                return CollisionResponse.Block;
            };

            m_screen.Physics.AddRigidBody(m_physicsBody);

            // AI

            float jumpDistance = 80 * m_walkingSpeed;

            // walking speed can be 3-9
            if (m_walkingSpeed <= 4) jumpDistance = 80 * 4;
            if (m_walkingSpeed >= 7) jumpDistance = 80 * 7;

            m_jumpActions.Add(() =>
                Random.Next(100) < 2 &&
                m_screen.Players.Any(p =>
                    Math.Abs((p.Position - m_physicsBody.Position).Length()) < jumpDistance &&
                    p.Position.Y < m_physicsBody.Position.Y - 50 &&
                    p.OnGround) &&
                Math.Abs(m_physicsBody.Velocity.Y) < 0.001f);

            m_dropActions.Add(() =>
                Random.Next(100) < 2 &&
                m_screen.Players.Any(p =>
                    Math.Abs((p.Position - m_physicsBody.Position).Length()) < jumpDistance &&
                    p.Position.Y > m_physicsBody.Position.Y + 50 &&
                    p.OnGround) &&
                Math.Abs(m_physicsBody.Velocity.Y) < 0.001f);

            m_moveRightActions.Add(() =>
                m_screen.Players.Select(p => p.Position.X - m_physicsBody.Position.X).OrderBy(Math.Abs).First() > Random.Next(50, 200));

            m_moveLeftActions.Add(() =>
                m_screen.Players.Select(p => p.Position.X - m_physicsBody.Position.X).OrderBy(Math.Abs).First() < -Random.Next(50, 200));

            m_attackActions.Add(() =>
                !m_loseLife &&
                Random.Next(100) < 2 &&
                m_timeSinceAttack > ATTACK_COOLDOWN &&
                Math.Abs(m_screen.Players.Select(p => p.Position.X - m_physicsBody.Position.X).OrderBy(Math.Abs).First()) < 60);

            m_hurtSound1 = content.Load<SoundEffect>("Audio/sfx_enemy_grunt1");
            m_hurtSound2 = content.Load<SoundEffect>("Audio/sfx_enemy_grunt2");
            m_hurtSound3 = content.Load<SoundEffect>("Audio/sfx_enemy_grunt3");
        }

        public void Update(GameTime gameTime)
        {
            // Jumping

            if (m_jumpActions.Any(a => a()) && m_canJump)
            {
                if (!IsJumping)
                    m_triggerJump = true;
            }
            if (m_physicsBody.Velocity.Y > 0)
            {
                IsJumping = false;
            }

            if (m_triggerJump)
            {
                m_physicsBody.Velocity.Y -= 870f;
                m_triggerJump = false;
                IsJumping = true;
            }

            // Dropping

            if (m_dropActions.Any(a => a()) && m_canDrop)
            {
                if (!IsDropping)
                    m_triggerDrop = true;
            }
            if (m_physicsBody.Velocity.Y < 0)
            {
                IsDropping = false;
            }

            if (m_triggerDrop)
            {
                m_triggerDrop = false;
                IsDropping = true;
            }

            // Movement

            if (m_moveLeftActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X -= m_walkingSpeed * 10;
                m_walkingAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_attackAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_walkingAnimation.Update(gameTime);
            }

            if (m_moveRightActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X += m_walkingSpeed * 10;
                m_walkingAnimation.Effects = SpriteEffects.None;
                m_attackAnimation.Effects = SpriteEffects.None;
                m_walkingAnimation.Update(gameTime);
            }

            // Attacking

            if (m_attackActions.Any(a => a()))
            {
                m_isAttacking = true;
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

                foreach (var player in m_screen.Players.Where(player => player.Bounds.Intersects(m_attackRect)))
                {
                    if (player.TakeDamage())
                        m_physicsBody.Velocity.X *= -0.5f;
                }

                if (m_attackAnimation.IsFinished())
                {
                    m_attackAnimation.Reset();
                    m_isAttacking = false;
                    m_timeSinceAttack = 0;
                }
            }

            // Damage

            if (m_timeSinceDamage < 0.2f)
            {
                m_walkingAnimation.Color = (Lives == 1 ? new Color(Color.White, 5) : Color);
                m_attackAnimation.Color = (Lives == 1 ? new Color(Color.White, 5) : Color);
            }
            else
            {
                m_walkingAnimation.Color = Color;
                m_attackAnimation.Color = Color;

                if (m_loseLife)
                {
                    Lives--;
                    m_loseLife = false;
                }
            }

            m_timeSinceAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            // Taking damage

            if (m_timeSinceDamage > DAMAGE_COOLDOWN && !m_loseLife)
            {
                m_loseLife = true;

                Vector2 impluse = new Vector2(2000, -200);
                if (m_walkingAnimation.Effects == SpriteEffects.None)
                    impluse.X *= -1;

                m_physicsBody.Velocity += impluse;

                m_timeSinceDamage = 0;

                SoundEffect[] sounds = { m_hurtSound1, m_hurtSound2, m_hurtSound3 };
                int sound = Random.Next(sounds.Length);
                sounds[sound].Play(0.5f * Config.Data.Volume.SfxNormal, 0f, 0f);

                return true;
            }

            return false;
        }
    }
}
