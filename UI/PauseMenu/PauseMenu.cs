#region Директивы
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
#endregion
namespace BloodSim.UI.PauseMenu
{
    class PauseMenu
    {
        #region Поля
        Texture2D TestImage;
        private SpriteFont bold42;
        PauseMenuButton button1 = new PauseMenuButton(275, "Продолжить");
        PauseMenuButton button2 = new PauseMenuButton(330, "Главное меню");
        Rectangle backgroundRectangle;
        Texture2D backgroundTexture;
        Color backgroundColor;
        Vector2 startPosition;
        string title;
        #endregion
        public PauseMenu(string title)
        {
            button1.clicked += Play;

            this.title = title;
            backgroundColor = new Color(10, 10, 10, 255);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(TestImage, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, backgroundColor);
            button1.Draw(spriteBatch);
            button2.Draw(spriteBatch);
            spriteBatch.DrawString(bold42, title, startPosition, Color.White);
        }
        public void LoadContent(ContentManager Content)
        {
            TestImage = Content.Load<Texture2D>("TestImage");
            button1.LoadContent(Content);
            button2.LoadContent(Content);
            backgroundTexture = Content.Load<Texture2D>("background");
            bold42 = Content.Load<SpriteFont>("bold42");
        }
        public void Update(GameTime gameTime)
        {
            backgroundRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
            button1.Update(gameTime);
            button2.Update(gameTime);
            startPosition = new Vector2(Game1.gameWidth / 2 - bold42.MeasureString(title).Length() / 2, Game1.gameHeight / 2 - Game1.gameWidth / 8);

        }
        void Play()
        {
            Game1.gameState = Game1.State.Playing;
        }

    }
}
