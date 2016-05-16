using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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
            content.Load<SpriteFont>("Fonts/game");

            content.Load<Texture2D>("Maps/test");

            foreach (string texture in new[] {
                "bg_gameover",
                "bg_intro_1", "bg_intro_2", "bg_intro_3", "bg_intro_4",
                "bg_pause",
                "enemy_attack", "enemy_walking",
                "player_attack", "player_walking" })
            {
                content.Load<Texture2D>("Textures/Game/" + texture);
            }

            content.Load<Texture2D>("Textures/HUD/test");
        }

        /// <summary>
        ///
        /// </summary>
        public static void PreloadMenuContent(ContentManager content)
        {
            content.Load<SoundEffect>("Audio/background_music");

            foreach (string texture in new[] {
                "bg_splash",
                "btn_back", "btn_credits", "btn_exit", "btn_exit_white", "btn_options", "btn_start",
                "title_credits", "title_menu", "title_options" })
            {
                content.Load<Texture2D>("Textures/Menu/" + texture);
            }

            content.Load<Video>("Video/main_menu");
        }
    }
}
