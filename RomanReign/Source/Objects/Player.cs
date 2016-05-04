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
        RigidBody m_physicsBody;

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
                Position = m_screen.Map.Info.PlayerSpawn.Value,
                Origin = new Vector2(0.5f, 0.5f)
            };

            m_physicsBody = new RigidBody {
                Name = m_screen.Map.Info.PlayerSpawn.Name,
                Position = m_screen.Map.Info.PlayerSpawn.Value,
                Size = m_walkingAnimation.Bounds.Size.ToVector2(),
                Origin = new Vector2(0.5f, 0.5f),
                LinearDamping = new Vector2(0.2f, 0f),
                UserData = this
            };

            m_game.Physics.AddRigidBody(m_physicsBody);

            // Set up input events.

            InputManager i = m_game.Input;

            m_jumpActions.Add(() =>
                i.IsKeyJustPressed(Keys.Z) &&
                i.IsKeyUp(Keys.Down));

            m_jumpActions.Add(() =>
                i.IsButtonJustPressed(Buttons.A) &&
                (i.IsButtonUp(Buttons.DPadDown) && !i.IsLeftStickDown(0.5f)));

            m_dropActions.Add(() =>
                i.IsKeyJustPressed(Keys.Z) &&
                i.IsKeyDown(Keys.Down));

            m_dropActions.Add(() =>
                i.IsButtonJustPressed(Buttons.A) &&
                (i.IsButtonDown(Buttons.DPadDown) || i.IsLeftStickDown(0.5f)));

            m_moveLeftActions.Add(() =>
                i.IsKeyDown(Keys.Left) &&
                i.IsKeyUp(Keys.Right));

            m_moveLeftActions.Add(() =>
                (i.IsButtonDown(Buttons.DPadLeft) || i.IsLeftStickLeft()) &&
                (i.IsButtonUp(Buttons.DPadRight) && !i.IsLeftStickRight()));

            m_moveRightActions.Add(() =>
                i.IsKeyDown(Keys.Right) &&
                i.IsKeyUp(Keys.Left));

            m_moveRightActions.Add(() =>
                (i.IsButtonDown(Buttons.DPadRight) || i.IsLeftStickRight()) &&
                (i.IsButtonUp(Buttons.DPadLeft) && !i.IsLeftStickLeft()));
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
