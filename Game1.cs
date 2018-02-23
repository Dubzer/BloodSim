using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using BloodSim.UI.PauseMenu;
using Shop;
using System.Diagnostics;

namespace BloodSim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum State
        {
            Playing,
            Pause,
            Shop,
            MainMenu
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static public Random random = new Random(unchecked((int)(DateTime.Now.Ticks)));
        static public List<Cell> cellList = new List<Cell>();
        static public List<Bacterium> bacteriumList = new List<Bacterium>();
        static public List<Eritro> eritroList = new List<Eritro>();
        static public List<Leiko> leikoList = new List<Leiko>();
        static public List<Trombo> tromboList = new List<Trombo>();
        static public List<Wall> wallList = new List<Wall>();

        //static public Spawner spawner = new Spawner();
        int spawnTimer = 0;

        HUD hud = new HUD();

        static public int oxygenPoints;

        public Eritro er1 = new Eritro(random);
        public Leiko le1 = new Leiko(random);
        public Trombo tr1 = new Trombo(random);
        #region ui
        Shop shop = new Shop();
        PauseMenu pauseMenu = new PauseMenu("Пауза");
        Button shopButton = new Button("shopIcon", new Vector2(25, gameHeight - 75));
        Button closeShopButton = new Button("closeIcon", new Vector2(25, gameHeight - 75));
        Button pauseButton = new Button("pauseIcon", new Vector2(25, 25));
        public static int gameWidth = 1920;
        public static int gameHeight = 1080;
        public Cursor cursor = new Cursor(); // Курсор
        MainMenu mainMenu = new MainMenu("Главное меню");

        public static Rectangle cursorRectangle;
        public static string cursorTexture;
        private bool isShopMenuVisible = false;
        private bool isShowPauseMenuVisible = true;

        #endregion
		
		#region Стандартный контент
        Texture2D eritroTexture;
        Texture2D tromboTexture;
        Texture2D leikoTexture;
        Texture2D bacteriumTexture;
        SoundEffect _bite_soundEffect;
        SoundEffect _death_soundEffect;
        #endregion
		
        public Background background = new Background();

        Song music;

        bool musicPlayed = false;

        public static State gameState = State.MainMenu;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            #region 
            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            Debug.Print("инициалировано");
            cursorTexture = "cursorNormal";
            shopButton.clicked += OpenShopMenu;
            closeShopButton.clicked += CloseShopMenu;
            pauseButton.clicked += OpenPauseMenu;

            shop.OnClick0 += BuyEritro;
            shop.OnClick1 += BuyLeiko;
            #endregion

            for (int i = 0; i < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 200 + 1; i++)
            {
                wallList.Add(new Wall());
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            SpawnEritro(1);
            SpawnLeiko(1);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            #region
            pauseMenu.LoadContent(Content);
            cursor.Texture = Content.Load<Texture2D>(cursorTexture); // Загрузка контента для курсора
            pauseButton.LoadContent(Content);
            shopButton.LoadContent(Content);
            closeShopButton.LoadContent(Content);
            shop.LoadContent(Content);
            mainMenu.LoadContent(Content);
            #endregion
            for (int i = 0; i < wallList.Count; i++)
            {
                wallList[i].LoadContent(Content);
            }

            eritroTexture = Content.Load<Texture2D>("Textures/eritro");
            leikoTexture = Content.Load<Texture2D>("Textures/leiko");
            bacteriumTexture = Content.Load<Texture2D>("Textures/bacterium");
            tromboTexture = Content.Load<Texture2D>("Textures/trombo");
            _bite_soundEffect = Content.Load<SoundEffect>("Sounds/bite");
            _death_soundEffect = Content.Load<SoundEffect>("Sounds/kill");

            background.LoadContent(Content);
            music = Content.Load<Song>("Sounds/music1");

            hud.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case State.Pause:
                    pauseMenu.Update(gameTime);
                    break;
                case State.Playing:
                    pauseButton.Update(gameTime);
                    shopButton.Update(gameTime);
                    #region Обновление игровых объектов
                    for (int i = 0; i < eritroList.Count; i++)
                    {
                        eritroList[i].Update(gameTime, new Rectangle(100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + eritroList[i].boundingBox.Height /*- eritroList[i].boundingBox.Height*/, 2, 2), wallList);
                    }

                    for (int i = 0; i < bacteriumList.Count; i++)
                    {
                        bacteriumList[i].Update(gameTime, cellList, wallList);
                    }

                    for (int i = 0; i < leikoList.Count; i++)
                    {
                        leikoList[i].Update(gameTime, bacteriumList, wallList);
                    }

                    for (int i = 0; i < tromboList.Count; i++)
                    {
                        tromboList[i].Update(gameTime, wallList);
                    }

                    for (int i = 0; i < wallList.Count; i++)
                    {
                        wallList[i].Update(gameTime);
                    }

                    if (musicPlayed == false)
                    {
                        MediaPlayer.Play(music);
                        MediaPlayer.Volume = .5f;

                        musicPlayed = true;
                    }

                    hud.Update(gameTime, oxygenPoints);
                    background.Update(gameTime);
                    #endregion

                    #region Очистка от уничтоженных объектов
                    ClearAll();
                    #endregion

                    #region Создание бактерий
                    spawnTimer++;
                    if (spawnTimer == 500)
                    {
                        SpawnBacterium(1);
                        spawnTimer = 0;

                        wallList[random.Next(0, wallList.Count)].hp = 0;
                    }
                    #endregion

                    break;
                case State.Shop:
                    closeShopButton.Update(gameTime);
                    shop.Update(gameTime);
                    break;
                case State.MainMenu:
                    mainMenu.Update(gameTime);
                    break;

            }
            #region UI
            MouseState CurrentMouseState = Mouse.GetState(); // Считывание текущего состояния мыши
            cursor.Position = new Vector2(CurrentMouseState.X, CurrentMouseState.Y); // Привязка позиции внутриигрового курсора к десктопному 
            cursorRectangle = new Rectangle((int)cursor.Position.X, (int)cursor.Position.Y, 5, 5);

            #endregion
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(14, 9, 32));
            spriteBatch.Begin();
            {
                switch (gameState)
                {
                    case State.Pause:
                        background.Draw(spriteBatch);
                        for (int i = 0; i < cellList.Count; i++)
                        {
                            cellList[i].Draw(spriteBatch);
                        }

                        for (int i = 0; i < bacteriumList.Count; i++)
                        {
                            bacteriumList[i].Draw(spriteBatch);
                        }

                        for (int i = 0; i < leikoList.Count; i++)
                        {
                            leikoList[i].Draw(spriteBatch);
                        }
                        for (int i = 0; i < wallList.Count; i++)
                        {
                            wallList[i].Draw(spriteBatch);
                        }
                        for (int i = 0; i < tromboList.Count; i++)
                        {
                            tromboList[i].Draw(spriteBatch);
                        }

                        pauseMenu.Draw(spriteBatch);
                        break;
                    case State.Playing:
                        background.Draw(spriteBatch);
                        for (int i = 0; i < cellList.Count; i++)
                        {
                            cellList[i].Draw(spriteBatch);
                        }

                        for (int i = 0; i < bacteriumList.Count; i++)
                        {
                            bacteriumList[i].Draw(spriteBatch);
                        }

                        for (int i = 0; i < leikoList.Count; i++)
                        {
                            leikoList[i].Draw(spriteBatch);
                        }
                        for (int i = 0; i < wallList.Count; i++)
                        {
                            wallList[i].Draw(spriteBatch);
                        }
                        for (int i = 0; i < tromboList.Count; i++)
                        {
                            tromboList[i].Draw(spriteBatch);
                        }

                        hud.Draw(spriteBatch);
                        shopButton.Draw(spriteBatch);
                        pauseButton.Draw(spriteBatch);
                        break;
                    case State.Shop:
                        background.Draw(spriteBatch);
                        for (int i = 0; i < cellList.Count; i++)
                        {
                            cellList[i].Draw(spriteBatch);
                        }

                        for (int i = 0; i < bacteriumList.Count; i++)
                        {
                            bacteriumList[i].Draw(spriteBatch);
                        }

                        for (int i = 0; i < leikoList.Count; i++)
                        {
                            leikoList[i].Draw(spriteBatch);
                        }
                        for (int i = 0; i < wallList.Count; i++)
                        {
                            wallList[i].Draw(spriteBatch);
                        }
                        for (int i = 0; i < tromboList.Count; i++)
                        {
                            tromboList[i].Draw(spriteBatch);
                        }

                        shop.Draw(spriteBatch);
                        closeShopButton.Draw(spriteBatch);
                        break;
                    case State.MainMenu:
                        background.Draw(spriteBatch);
                        mainMenu.Draw(spriteBatch);
                        break;

                }
                cursor.Draw(spriteBatch); // Отрисовка курсора 
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region УПРАВЛЕНИЕ ИГРОВЫМ ПРОЦЕССОМ
         protected void SpawnBacterium(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Bacterium bacteriumExample = new Bacterium(random)
                {
                    texture = bacteriumTexture,
                    death_soundEffect = _death_soundEffect,
                    bite_soundEffect = _bite_soundEffect
                };
                bacteriumList.Add(bacteriumExample);
            }
        }

        protected void SpawnEritro(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Eritro eritroExample = new Eritro(random)
                {
                    texture = eritroTexture,
                    death_soundEffect = _death_soundEffect
                };
                eritroList.Add(eritroExample);
                cellList.Add(eritroExample);
            }
        }

        protected void SpawnLeiko(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Leiko leikoExample = new Leiko(random)
                {
                    texture = leikoTexture,
                    death_soundEffect = _death_soundEffect,
                    bite_soundEffect = _bite_soundEffect
                };
                leikoList.Add(leikoExample);
                cellList.Add(leikoExample);
            }
        }

        protected void SpawnTrombo(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Trombo tromboExample = new Trombo(random)
                {
                    texture = tromboTexture,
                    death_soundEffect = _death_soundEffect
                };
                tromboList.Add(tromboExample);
                cellList.Add(tromboExample);
            }
        }

        protected void ClearAll() // Очистка списков с уже уничтоженными клетками для уменьшения нагрузки
        {
            for (int i = 0; i < bacteriumList.Count; i++)
            {
                if (bacteriumList[i].hp <= 0)
                {
                    bacteriumList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < eritroList.Count; i++)
            {
                if (eritroList[i].hp <= 0)
                {
                    eritroList.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < leikoList.Count; i++)
            {
                if (leikoList[i].hp <= 0)
                {
                    leikoList.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void RestartProgress() // Вернуть состояния всех объектов на исходные значения
        {
            for (int i = 0; i < bacteriumList.Count; i++)
            {
                bacteriumList.RemoveAt(i);
                i--;
            }

            for (int i = 1; i < eritroList.Count; i++) // Ведём счёт с единицы, чтобы, как минимум, один эритроцит продолжил своё мирное существование
            {
                eritroList.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < wallList.Count; i++)
            {
                wallList[i].hp = 100;
            }

            for (int i = 0; i < leikoList.Count; i++)
            {
                leikoList.RemoveAt(i);
                i--;
            }
        }
        #endregion

        #region МАГАЗИН
        public void BuyEritro()
        {
            SpawnEritro(1);
        }

        public void BuyLeiko()
        {
            SpawnLeiko(1);
        }

        public void BuyTrombo()
        {
            SpawnTrombo(1);
        }
        #endregion

        #region НУЖНО БОЛЬШЕ КОСТЫЛЕЙ
        void OpenShopMenu()
        {
            gameState = State.Shop;
        }
        void CloseShopMenu()
        {
            gameState = State.Playing;
        }
        void OpenPauseMenu()
        {
            gameState = State.Pause;
        }
        void ClosePauseMenu()
        {
            gameState = State.Playing;
        }

        #endregion
    }
}
