using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RomanReign
{
    static class ContentPreloader
    {
        /// <summary>
        ///
        /// </summary>
        public static void PreloadAllContent(ContentManager content)
        {
            PreloadMenuContent(content);
            PreloadGameContent(content);
        }

        /// <summary>
        ///
        /// </summary>
        public static void PreloadGameContent(ContentManager content)
        {
            content.Load<Texture2D>("Maps/Test");

            content.Load<Texture2D>("Textures/Game/bg_gameover");
            content.Load<Texture2D>("Textures/Game/bg_intro_1");
            content.Load<Texture2D>("Textures/Game/bg_intro_2");
            content.Load<Texture2D>("Textures/Game/bg_intro_3");
            content.Load<Texture2D>("Textures/Game/bg_intro_4");
            content.Load<Texture2D>("Textures/Game/bg_pause");
            content.Load<Texture2D>("Textures/Game/enemy_attack");
            content.Load<Texture2D>("Textures/Game/enemy_walking");
            content.Load<Texture2D>("Textures/Game/player_attack");
            content.Load<Texture2D>("Textures/Game/player_walking");

            content.Load<Texture2D>("Textures/HUD/test");
        }

        /// <summary>
        ///
        /// </summary>
        public static void PreloadMenuContent(ContentManager content)
        {
            content.Load<Texture2D>("Textures/Menu/bg_menu");
            content.Load<Texture2D>("Textures/Menu/bg_splash");
            content.Load<Texture2D>("Textures/Menu/btn_back");
            content.Load<Texture2D>("Textures/Menu/btn_credits");
            content.Load<Texture2D>("Textures/Menu/btn_exit");
            content.Load<Texture2D>("Textures/Menu/btn_exit_white");
            content.Load<Texture2D>("Textures/Menu/btn_options");
            content.Load<Texture2D>("Textures/Menu/btn_start");
            content.Load<Texture2D>("Textures/Menu/title_credits");
            content.Load<Texture2D>("Textures/Menu/title_menu");
            content.Load<Texture2D>("Textures/Menu/title_options");
        }
    }
}
