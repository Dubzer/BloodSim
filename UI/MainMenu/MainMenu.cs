#region Директивы
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

#endregion
namespace BloodSim.UI.PauseMenu
{
    class MainMenu
    {
        #region Поля
        private SpriteFont fontBold42;
        PauseMenuButton button1 = new PauseMenuButton(Game1.gameHeight / 2 - 50, "Начать игру");
        PauseMenuButton button2 = new PauseMenuButton(Game1.gameHeight / 2, "Выход");
       
        Rectangle backgroundRectangle;
        Texture2D backgroundTexture;
        Color backgroundColor;
        Vector2 startPosition;
        string title;
        public static Action FirstStart;

        Song theme;
        public static bool isMusicPlayed = false;

        #region Particles

        public Texture2D particlesTexture;
        public Rectangle particleRectangle;
        public Rectangle particleRectangle2;

        #endregion
        #endregion
        public MainMenu(string title)
        {
            button1.OnClick += Play;
            button2.OnClick += Exit;    
            this.title = title;
            backgroundColor = new Color(0, 0, 0, 0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
            button1.Draw(spriteBatch);
            button2.Draw(spriteBatch);
            spriteBatch.DrawString(fontBold42, title, startPosition, Color.White);
            spriteBatch.Draw(particlesTexture, particleRectangle, Color.White);
            spriteBatch.Draw(particlesTexture, particleRectangle2, Color.White);
        }
        public void LoadContent(ContentManager Content)
        {
            button1.LoadContent(Content);
            button2.LoadContent(Content);
            backgroundTexture = Content.Load<Texture2D>("Textures/UI/mainMenuBackground");
            fontBold42 = Content.Load<SpriteFont>("Fonts/bold42");
            particlesTexture = Content.Load<Texture2D>("Textures/particles");
            theme = Content.Load<Song>("Sounds/mainMenuMusic");
        }
        public void Update(GameTime gameTime)
        {
            backgroundRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
            button1.Update(gameTime);
            button2.Update(gameTime);
            startPosition = new Vector2(Game1.gameWidth / 2 - fontBold42.MeasureString(title).Length() / 2, Game1.gameHeight / 4);

            if (isMusicPlayed == false)
            {
                MediaPlayer.Volume = .1f;
                MediaPlayer.Play(theme);
                isMusicPlayed = true;
            }
            #region Particles
            particleRectangle.Y += 1;
            particleRectangle2.Y += 1;

            if (particleRectangle.Y == Game1.gameHeight)
            {
                particleRectangle.Y = -Game1.gameHeight;
            }

            if (particleRectangle2.Y == Game1.gameHeight)
            {
                particleRectangle2.Y = -Game1.gameHeight;
            }


            #endregion
        }
        void Play()
        {
            Game1.gameState = Game1.State.Playing;
            Game1.musicPlayed = false;

            MediaPlayer.Stop();
            
        }
        void Exit()
        {
            Environment.Exit(0);
        }

    }
}
