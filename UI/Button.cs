using System;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BloodSim
{
    class Button
    {
        Texture2D texture;
        Texture2D iconTexture;
        Rectangle rectangle;
        string icon;
        public Action clicked;
        MouseState _currentMouseState;
        MouseState _previousMouseState;
        Vector2 position;
        Color color;
        private SoundEffect clickSound;     // Звук клика

        public Button(string icon, Vector2 position)
        {
            color = Color.White;
            this.icon = icon;
            this.position = position;
            _currentMouseState = Mouse.GetState();
            _previousMouseState = _currentMouseState;
        }
        public void Update(GameTime gameTime)   
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            if (_previousMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed && rectangle.Intersects(Game1.cursorRectangle))
            {
                Debug.Print("Кнопка нажата " + icon);
                Thread.Sleep(120);   //  Убираем баг с двойным действием при одинарном нажатии
                clickSound.Play();
                clicked();
            }
            if (rectangle.Intersects(Game1.cursorRectangle))
            {

                color = new Color(240, 240, 240);
            }
            else
            {
                color = Color.White;
            }

           _previousMouseState = _currentMouseState;
           _currentMouseState = Mouse.GetState();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
            spriteBatch.Draw(iconTexture, new Vector2(position.X, position.Y), Color.White);
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("buttonBlank");
            iconTexture = Content.Load<Texture2D>(icon);
            clickSound = Content.Load<SoundEffect>("clickSound");       //  Загрузка конента для звука клика

        }
    }
}
