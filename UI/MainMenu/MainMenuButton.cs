#region Директивы
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Diagnostics;
#endregion
namespace BloodSim.UI.PauseMenu
{
    class MainMenuButton
    {
        #region Поля
        private Texture2D texture;      // Текстура карточки
        private string name;        // Название пункта меню
        private float x, y, y_cord;
        private Vector2 textPosition, namePosition;
        private Rectangle rectangle;        // Ректенгл карточки
        private Color color, defaultColor, selectedColor;        // Цвет карточки
        private SpriteFont fontRegular;       // Шрифты
        private SoundEffect clickSound;     // Звук клика
        private bool isExpanded;
        public Action clicked;
        #region Управление мышью
        MouseState _currentMouseState;
        MouseState _previousMouseState;
        #endregion

        #endregion
        public MainMenuButton(int y, string name)
        {
            defaultColor = new Color(180, 180, 180, 200);       //  Цвет карточки, на которую не навели курсором
            selectedColor = new Color(255, 255, 255, 255);      //  Цвет карточки, на которую навели курсором
            this.y_cord = y;
            this.name = name;
            #region Управление мышью
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
            #endregion 
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            
            spriteBatch.Draw(texture, rectangle, color);        //  Отрисовка карточки
            spriteBatch.DrawString(fontRegular, name, textPosition, Color.White);
        }
        public void Update(GameTime gameTime)
        {
            y = (rectangle.Y + (rectangle.Height / 2)) - (fontRegular.MeasureString(name).Y / 2) + y_cord;
            textPosition = new Vector2(x + rectangle.Width / 2 - (fontRegular.MeasureString(name).X / 2), y + rectangle.Height / 2 - (fontRegular.MeasureString(name).Y / 2));
            #region При нажатии на кнопку
            if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed && rectangle.Intersects(Game1.cursorRectangle))
            {
                clickSound.Play();
                Thread.Sleep(120);   //  
                clicked?.Invoke();
            }
            #endregion


            #region При наведении на кнопку
            if (rectangle.Intersects(Game1.cursorRectangle))
            {
                color = new Color(240, 240, 240);
            }
            else
            {
                color = defaultColor;
            }
            #endregion
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

        }
        public void LoadContent(ContentManager Content)
        {
            fontRegular = Content.Load<SpriteFont>("regular");        //  Загрузка конента для шрита
            rectangle = new Rectangle((int)x, (int)y, 190, 40);        // Ректенгл карты

            x = 0;
            rectangle = new Rectangle((int)x, (int)y, 190, 40);        // Ректенгл карты

            clickSound = Content.Load<SoundEffect>("clickSound");       //  Загрузка конента для звука клика
            texture = Content.Load<Texture2D>("button");
        }
    }
}
