#region Директивы
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
#endregion
namespace BloodSim
{
    class Card
    {
        #region Поля
        public bool hasBeenClicked = false;

        private Texture2D itemTexture;      // Текстура для предмета
        private Texture2D cardTexture;      // Текстура карточки
        private string item;        // Название предмета
        private string name = "null", description = "null";     // Название и описание предмета
        private Vector2 position = new Vector2(0, 0);       // Позиция карточки
        private Rectangle cardRectangle;        // Ректенгл карточки
        private Rectangle itemRectangle;        // Ректенгл предмета
        private Color cardColor, costColor, textColor;        // Цвет карточки
        private SpriteFont fontBold, fontRegular;       // Шрифты
        private SoundEffect clickSound,  errorSound;     // Звук клика
        private int cost;       // Стоимость предмета 
        public Action NotEnoughMoney;
        #region Управление мышью
        MouseState _currentMouseState;
        MouseState _previousMouseState;
        #endregion
        #endregion
        public Card(Vector2 position, string texture, int cost, string name, string description)              
        {
            costColor = Color.Black;
            this.position = position;
            this.item = texture;
            this.name = name;
            this.description = description;
            this.cost = cost;
            #region Управление мышью
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
            #endregion 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cardTexture, cardRectangle, cardColor);        //  Отрисовка карточки
            spriteBatch.Draw(itemTexture, itemRectangle, Color.White);      //  Отрисовка предмета
            spriteBatch.DrawString(fontBold, name, new Vector2(115, position.Y + 5), textColor);       //  Отрисовка названия предмета
            spriteBatch.DrawString(fontRegular, description, new Vector2(115, position.Y + 50), textColor);        // Отрисовка описания предмета
            spriteBatch.DrawString(fontBold, "Магазин", new Vector2(10, 10), Color.Black);        // Надпись магазин
            spriteBatch.DrawString(fontRegular, cost.ToString() + "R", new Vector2(cardRectangle.X + 39, cardRectangle.Y + 20), costColor);     // Отрисовка стоимости предмета
        }       //  Отрисовка

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();    // Считывание текущего состояния мыши
            #region При нажатии на карточку
            if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed && cardRectangle.Intersects(Game1.cursorRectangle))
            {
                if(cost <= Shop.money)
                {
                    Shop.money -= cost;
                    clickSound.Play();
                    Debug.Print("Куплено: " + name);
                }
                else     // Если недостаточно денег
                {
                    //errorSound.Play();
                    clickSound.Play();

                    NotEnoughMoney();
                    Debug.Print("Недостаточно денег на: " + name);
                }
                hasBeenClicked = true;
            }

            #endregion
            itemRectangle = new Rectangle(20, cardRectangle.Y + 48, 80, 80);        // Ректенгл предмета 
            cardRectangle = new Rectangle(2, (int)position.Y, 355, 200);        // Ректенгл карты
            #region Поведение карточки
            if (cardRectangle.Intersects(Game1.cursorRectangle))
            {
                cardColor = new Color(240, 240, 240);
            }
            else if(!cardRectangle.Intersects(Game1.cursorRectangle))
            {
                cardColor = Color.White;
                
            }
            
            else if(cardRectangle.Intersects(Game1.cursorRectangle) && cost > Shop.money)
            {
                cardColor = new Color(244, 67, 54);
            }
            
            if (cost > Shop.money && !cardRectangle.Intersects(Game1.cursorRectangle))
            {
                cardColor = new Color(255,255,255, 180);        // Было new Color(244, 67, 54, 200);
                costColor = new Color(213, 0, 0, 255);
                textColor = new Color(0, 0, 0, 180);   
            }
            else
            {
                textColor = Color.Black;
            }
            #endregion
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }       //  Обновление 

        public void LoadContent(ContentManager Content)
        {
            itemTexture = Content.Load<Texture2D>(item);        //  Загрузка контента для предмета
            cardTexture = Content.Load<Texture2D>("card");      //  Загрузка конента для карточки
            fontRegular = Content.Load<SpriteFont>("regular");      //  Загрузка конента для шрифта
            fontBold = Content.Load<SpriteFont>("bold");        //  Загрузка конента для шрита
            clickSound = Content.Load<SoundEffect>("clickSound");       //  Загрузка конента для звука клика
            errorSound = Content.Load<SoundEffect>("Sounds/error");     //  Звук ошибки

        }       //  Загрузка контента
    }
}