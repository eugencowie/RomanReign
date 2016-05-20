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
            content.Load<SoundEffect>("Audio/background_music");

            foreach (var sfx in new[] {
                "enemy_grunt1",
                "enemy_grunt2",
                "enemy_grunt3",
                "menu_select",
                "player_attack1",
                "player_attack2",
                "player_hurt",
                "player_jump1",
                "player_jump2",
                "player_walking_long" })
            {
                content.Load<SoundEffect>("Audio/sfx_" + sfx);
            }

            content.Load<SpriteFont>("Fonts/game");

            content.Load<Texture2D>("Maps/test");

            foreach (string texture in new[] {
                "bg_gameover",
                "bg_pause",
                "enemy_attack",
                "enemy_walking",
                "player_attack",
                "player_walking" })
            {
                content.Load<Texture2D>("Textures/Game/" + texture);
            }

            content.Load<Texture2D>("Textures/HUD/heart");

            foreach (string texture in new[] {
                "bg_splash",
                "btn_back",
                "btn_credits",
                "btn_exit",
                "btn_exit_white",
                "btn_options",
                "btn_start",
                "btn_start1",
                "btn_start2",
                "btn_start3",
                "btn_start4",
                "title_credits",
                "title_menu",
                "title_options" })
            {
                content.Load<Texture2D>("Textures/Menu/" + texture);
            }

            content.Load<Video>("Video/main_menu");
        }
    }
}
