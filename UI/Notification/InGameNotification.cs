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
namespace BloodSim.UI.Notification
{
    static class InGameNotification
    {
        static private string text = "SomeText", continueText = "Нажмите, что-бы продолжить", icon;
        static private Vector2 textPosition, continuePosition;
        static private Texture2D texture;
        static public Rectangle rectangle, backgroundRectangle;
        static private Color notificationColor, defaultNotificationColor, backgroundColor;
        static private SpriteFont fontRegular, fontBold;
        static private SoundEffect clickSound;     // Звук клика
        public static event Action Click;
        private static int x = 0, y = 0;
        private static Texture2D backgroundTexture, iconTexture;
        public static bool isVisible;
        private static bool background;
        #region Mouse
        static private MouseState _currentMouseState;
        static private MouseState _previousMouseState;
        #endregion
        #region Constructor
        static InGameNotification()
        {

            defaultNotificationColor = new Color(0, 0, 0, 120);
            notificationColor = defaultNotificationColor;
            backgroundColor = new Color(0, 0, 0, 80);
            #region Mouse
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
            #endregion 

        }
        #endregion
        static public void Draw(SpriteBatch spriteBatch)
        {
            if(isVisible)
            {
                if (background)
                    spriteBatch.Draw(backgroundTexture, backgroundRectangle, backgroundColor);      //  Задний фон(затемнение)
                spriteBatch.Draw(texture, rectangle, notificationColor);        //  Отрисовка уведомления
                spriteBatch.DrawString(fontBold, text, textPosition, Color.White);
                spriteBatch.DrawString(fontRegular, continueText, continuePosition, Color.White);

            }
        }
        static public void Update(GameTime gameTime)
        {
            #region Rectangles
            rectangle = new Rectangle(x, y, 800, 130);      //  Rectangle уведомления
            backgroundRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
            #endregion
            textPosition = new Vector2(rectangle.X + rectangle.Width / 2 - (fontBold.MeasureString(text).X / 2), rectangle.Y + rectangle.Height / 2 - (fontBold.MeasureString(text).Y / 2) - 15);
            continuePosition = new Vector2(rectangle.X + rectangle.Width / 2 - (fontRegular.MeasureString(continueText).X / 2), textPosition.Y + 43);
            #region При нажатии на кнопку
            if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed && rectangle.Intersects(Game1.cursorRectangle))
            {
            }
            #endregion
            #region При наведении на кнопку
            if (rectangle.Intersects(Game1.cursorRectangle))
            {
                notificationColor = new Color(0, 0, 0, 160);
            }
            else
            {
                notificationColor = defaultNotificationColor;
            }
            #endregion
            #region Mouse
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            #endregion
        }
        static public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/UI/notification");     //  Загрузка контента для текстурки уведомления
            fontBold = Content.Load<SpriteFont>("Fonts/bold23");        //  Загрузка конента для шрифта
            fontRegular = Content.Load<SpriteFont>("Fonts/regular");      //  Загрузка контента для еще одно шрифта
            clickSound = Content.Load<SoundEffect>("clickSound");       //  Загрузка конента для звука клика
            backgroundTexture = Content.Load<Texture2D>("Textures/UI/background");        //  Загрузка контента для заднего фона(затемнения)
            //iconTexture = Content.Load<Texture2D>(icon);
        }
        /// <summary>
        /// Показать уведомление
        /// </summary>
        /// <param name="text">Текст</param>
        static public void Show(string text, bool background,int x, int y)
        {
            Game1.isGamePaused = true;      //  Ставит игру на паузу
            InGameNotification.x = x;       //  Передача параметров координаты X
            InGameNotification.y = y;       //  Передача параметров координаты Y
            InGameNotification.text = text;     //  Передача текста
            InGameNotification.background = background;     //  Передача параметра "Показывать ли задний фон(затемнение)?"
            isVisible = true;       //  Показывать ли уведомление? - Да
        }
        /// <summary>
        /// Показать уведомление с иконкой
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="icon">Иконка</param>
        /// <param name="background">Показывать ли задний фон?</param>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public static void Show(string text, string icon, bool background, int x, int y)
        {
            Game1.isGamePaused = true;      //  Ставит игру на паузу
            InGameNotification.x = x;       //  Передача параметров координаты X
            InGameNotification.y = y;       //  Передача параметров координаты Y
            InGameNotification.text = text;     //  Передача текста
            InGameNotification.background = background;     //  Передача параметра "Показывать ли задний фон(затемнение)?"
            isVisible = true;       //  Показывать ли уведомление? - Да
        }
        public static void Hide()       //  Закрывает уведомление
        {
            Game1.isGamePaused = false;     //  Убирает паузу игры
            clickSound.Play();      //  Воспроизводит звук
            isVisible = false;      //  Делает невидимым уведомление
        }
    }
}