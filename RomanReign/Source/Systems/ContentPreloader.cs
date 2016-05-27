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
            content.Load<SpriteFont>("Fonts/menu");
            content.Load<SpriteFont>("Fonts/wave");

            content.Load<Texture2D>("Maps/test");

            foreach (string texture in new[] {
                "bg_gameover",
                "bg_pause",
                "bg_tutorial1",
                "bg_tutorial2",
                "enemy_attack",
                "enemy_walking",
                "player1_attack",
                "player1_walking",
                "player2_attack",
                "player2_walking",
                "player3_attack",
                "player3_walking",
                "player4_attack",
                "player4_walking" })
            {
                content.Load<Texture2D>("Textures/Game/" + texture);
            }

            content.Load<Texture2D>("Textures/HUD/heart");

            foreach (string texture in new[] {
                "bg_splash",
                "bg_credits",
                "btn_1player",
                "btn_2player",
                "btn_3player",
                "btn_4player",
                "btn_arrow_left",
                "btn_arrow_right",
                "btn_back",
                "btn_background",
                "btn_credits",
                "btn_options",
                "btn_options_base",
                "btn_options_music",
                "btn_options_sfx",
                "btn_play",
                "btn_quit",
                "btn_resume" })
            {
                content.Load<Texture2D>("Textures/Menu/" + texture);
            }

            content.Load<Video>("Video/main_menu");
        }
    }
}
