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
        PauseMenuButton button0 = new PauseMenuButton(275, "Продолжить");
        PauseMenuButton button1 = new PauseMenuButton(330, "Главное меню");
        Rectangle backgroundRectangle;
        Texture2D backgroundTexture;
        Color backgroundColor;
        Vector2 startPosition;
        string title;
        #endregion
        public PauseMenu(string title)
        {
            button0.clicked += Play;
            button1.clicked += MainMenu;
            this.title = title;
            backgroundColor = new Color(10, 10, 10, 255);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(TestImage, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, backgroundColor);
            button0.Draw(spriteBatch);
            button1.Draw(spriteBatch);
            spriteBatch.DrawString(bold42, title, startPosition, Color.White);
        }
        public void LoadContent(ContentManager Content)
        {
            TestImage = Content.Load<Texture2D>("TestImage");
            button0.LoadContent(Content);
            button1.LoadContent(Content);
            backgroundTexture = Content.Load<Texture2D>("background");
            bold42 = Content.Load<SpriteFont>("bold42");
        }
        public void Update(GameTime gameTime)
        {
            backgroundRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
            button0.Update(gameTime);
            button1.Update(gameTime);
            startPosition = new Vector2(Game1.gameWidth / 2 - bold42.MeasureString(title).Length() / 2, Game1.gameHeight / 2 - Game1.gameWidth / 8);

        }
        void Play()
        {
            Game1.gameState = Game1.State.Playing;
        }
        void MainMenu()
        {
            Game1.gameState = Game1.State.MainMenu;
        }
    }
}
