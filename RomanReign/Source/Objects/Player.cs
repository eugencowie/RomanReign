using Microsoft.Xna.Framework;
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
        public Vector2 Position => m_physicsBody.Position;

        public bool IsJumping  => m_isJumping;
        public bool IsDropping => m_isDropping;

        RomanReignGame m_game;
        GameScreen m_screen;

        AnimatedSprite m_walkingAnimation;
        DynamicBody m_physicsBody;

        List<InputAction> m_jumpActions = new List<InputAction>();
        List<InputAction> m_dropActions = new List<InputAction>();
        List<InputAction> m_moveLeftActions = new List<InputAction>();
        List<InputAction> m_moveRightActions = new List<InputAction>();

        bool m_isJumping;
        bool m_isDropping;

        public Player(GameScreen screen, RomanReignGame game, ContentManager content)
        {
            m_game = game;
            m_screen = screen;

            m_walkingAnimation = new AnimatedSprite(4, 1, 8, content.Load<Texture2D>("Textures/Game/player_walking")) {
                Position = m_screen.Map.Info.PlayerSpawn.Value
            };
            m_walkingAnimation.SetRelativeOrigin(0.5f, 0.5f);

            m_physicsBody = new DynamicBody {
                Name = m_screen.Map.Info.PlayerSpawn.Name,
                Position = m_screen.Map.Info.PlayerSpawn.Value,
                Size = m_walkingAnimation.Bounds.Size.ToVector2(),
                LinearDamping = new Vector2(0.2f, 0f),
                UserData = this
            };
            m_physicsBody.SetRelativeOrigin(0.5f, 0.5f);

            m_game.Physics.AddRigidBody(m_physicsBody);

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
        }

        public void Update(GameTime gameTime)
        {
            if (m_jumpActions.Any(a => a()))
            {
                m_physicsBody.Velocity.Y -= 800f;
                m_isJumping = true;
            }
            if (m_physicsBody.Velocity.Y > 0)
            {
                m_isJumping = false;
            }

            if (m_dropActions.Any(a => a()))
            {
                m_isDropping = true;
            }
            if (m_physicsBody.Velocity.Y < 0)
            {
                m_isDropping = false;
            }

            if (m_moveLeftActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X -= 175f;
                m_walkingAnimation.Effects = SpriteEffects.FlipHorizontally;
                m_walkingAnimation.Update(gameTime);
            }

            if (m_moveRightActions.Any(a => a()))
            {
                m_physicsBody.Velocity.X += 175f;
                m_walkingAnimation.Effects = SpriteEffects.None;
                m_walkingAnimation.Update(gameTime);
            }

            m_walkingAnimation.Position = m_physicsBody.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_walkingAnimation.Draw(spriteBatch);

            m_game.Debug.Draw(m_physicsBody.Bounds.ToRect(), Color.Blue);
        }
    }
}
