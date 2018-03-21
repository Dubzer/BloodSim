using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using BloodSim.UI.PauseMenu;
using System.Threading;

namespace BloodSim.UI.EndMenu
{
    public class EndMenu
    {
        private SpriteFont fontBold42;
        public Vector2 textPosition;

        public string endText = "";
        public int musicType;

        public Song deathTheme;
        public Song victoryTheme;

        PauseMenuButton mmButton = new PauseMenuButton(1, "");
        PauseMenuButton quitButton = new PauseMenuButton(1, "");

        public bool isMusicPlayed = false;



        public void Draw(SpriteBatch spriteBatch)
        {
            mmButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);

            spriteBatch.DrawString(fontBold42, endText, new Vector2(Game1.gameWidth / 2 - fontBold42.MeasureString(endText).Length() / 2, Game1.gameHeight / 4), Color.White);
        }

        public void Update(GameTime gameTime, string endText, int musicType)
        {
            mmButton.OnClick += ReturnToMainMenu;
            quitButton.OnClick += Quit;

            this.endText = endText;
            this.musicType = musicType;

            mmButton.Update(gameTime);
            quitButton.Update(gameTime);

            if (isMusicPlayed == false)
            {
                MediaPlayer.Stop();
                switch (musicType)
                {
                    case 0:
                        MediaPlayer.Volume = .05f;
                        MediaPlayer.Play(deathTheme);
                        break;

                    case 1:
                        MediaPlayer.Volume = .05f;
                        MediaPlayer.Play(victoryTheme);
                        break;
                }

                isMusicPlayed = true;
            }
        }

        public void LoadContent(ContentManager content)
        {
            fontBold42 = content.Load<SpriteFont>("Fonts/bold42");

            mmButton = new PauseMenuButton(Game1.gameHeight / 2, "Главное меню");
            quitButton = new PauseMenuButton(Game1.gameHeight / 2 + 50, "Выйти из игры");

            deathTheme = content.Load <Song> ("Sounds/deathTheme");
            victoryTheme = content.Load<Song>("Sounds/victoryTheme");

            mmButton.LoadContent(content);
            quitButton.LoadContent(content);
    }

        public void ReturnToMainMenu()
        {
            Game1.gameState = Game1.State.MainMenu;

            MediaPlayer.Stop();
            MainMenu.isMusicPlayed = false;
            Game1.ClearAll();
            Game1.RestartProgress();
        }

        public void Quit()
        {
            Environment.Exit(0);
        }
    }
}
