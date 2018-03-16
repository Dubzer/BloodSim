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
using BloodSim.UI.EndMenu;
using Shop;
using System.Diagnostics;
using BloodSim.UI.Notification;

namespace BloodSim
{
    public class Game1 : Game
    {
        public enum State       //  State of the game
        {
            Playing,
            PauseMenu,
            Shop,
            MainMenu,
            Victory,
            Defeat
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

        static HUD hud = new HUD();

        static public int oxygenPoints;

        public Eritro er1 = new Eritro(random);
        public Leiko le1 = new Leiko(random);
        public Trombo tr1 = new Trombo(random);
        #region ui
        Shop shop = new Shop();
        PauseMenu pauseMenu = new PauseMenu("Пауза");
        Button shopButton = new Button("shopIcon", new Vector2(25, gameHeight - 75));
        Button closeShopButton = new Button("closeIcon", new Vector2(gameWidth - 75, 25));
        Button pauseButton = new Button("pauseIcon", new Vector2(25, 25));
        public static int gameWidth = 1280;     //  Тут задается ширина игры
        public static int gameHeight = 720;     //  Тут задается высота игры
        public Cursor cursor = new Cursor(); // Курсор
        MainMenu mainMenu = new MainMenu("");

        EndMenu endMenu = new EndMenu();

        static public bool isGamePaused;
        public static Rectangle cursorRectangle;
        public static string cursorTexture;
        #endregion
        #region Guide
        public static bool isNeedGuide = true;
        private static string[] messages = { "Message1", "Message2", "Message3" };     //  Массив с текстом сообщений
        private static int currentMessage;
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

        SoundEffect penetration;
        Song music;
        bool musicPlayed = false;

        public static State gameState = State.MainMenu;
        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = gameWidth;      
            graphics.PreferredBackBufferHeight = gameHeight;
            //graphics.ToggleFullScreen();
            #region UI

            cursorTexture = "cursorNormal";
            shopButton.clicked += OpenShopMenu;
            closeShopButton.clicked += CloseShopMenu;
            pauseButton.clicked += OpenPauseMenu;
            shop.OnClick0 += BuyEritro;
            shop.OnClick1 += BuyLeiko;
            shop.OnClick2 += BuyTrombo;

            #endregion

            for (int i = 0; i < gameHeight / 200; i++)
            {
                wallList.Add(new Wall());
            }
            if (!wallList[wallList.Count - 1].boundingBox.Intersects(new Rectangle((int)wallList[0].position.X, gameHeight, 3, 3)))
            {
                wallList.Add(new Wall());
            }

        }

        protected override void Initialize()
        {
            base.Initialize();

            SpawnEritro(1);
            SpawnLeiko(1);
            SpawnTrombo(1);
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
            penetration = Content.Load<SoundEffect>("Sounds/Penetration");

            endMenu.LoadContent(Content);

            hud.LoadContent(Content);

            InGameNotification.LoadContent(Content);        //  Внутриигровые уведомления (например недостаточно денег)

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Delete))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.End))
                Shop.money += 10;
            switch(isGamePaused)
            {
                case false:     //  Если игра не на паузе, то...

                    switch (gameState)
                    {
                        case State.PauseMenu:
                            pauseMenu.Update(gameTime);
                            MediaPlayer.Pause();
                            musicPlayed = false;
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
                                MediaPlayer.Volume = .05f;

                                musicPlayed = true;
                            }

                            Shop.money = oxygenPoints;

                            hud.Update(gameTime, oxygenPoints);
                            background.Update(gameTime);

                            /*if (oxygenPoints == hud.oxygenBarRectangle.Width)
                            {
                                gameState = State.Victory;
                            }*/

                            if ((oxygenPoints < 100) && (eritroList.Count == 0))
                            {
                                gameState = State.Defeat;
                            }
                            #endregion

                            #region Очистка от уничтоженных объектов
                            ClearAll();
                            #endregion

                            #region Создание бактерий
                            spawnTimer++;
                            if (spawnTimer == 1000)
                            {
                                Debug.Print("Бактерия заспавнилась успешно!");
                                #region Training. Notification appears once. 

                                if (Bacterium.needNotification)
                                {
                                    InGameNotification.Show("Внимание! Появилась бактерия!", true, gameWidth / 2 - 400, gameHeight / 2 - 65);
                                    
                                    Bacterium.needNotification = !Bacterium.needNotification;
                                }
                                #endregion
                                SpawnBacterium(random.Next(0, 2));
                                spawnTimer = 0;

                                SoundEffect.MasterVolume = 0.5f;
                                penetration.Play();

                                wallList[random.Next(2, wallList.Count)].hp = 0;
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
                        case State.Victory:
                            endMenu.Update(gameTime, "      Ваш организм выжил и смог \n    накопить достаточно кислорода \n        для завершения игры.", 1);
                            break;
                        case State.Defeat:
                            endMenu.Update(gameTime, "   Вы не справились с заданием. \n       Ваш организм погиб.", 0);
                            break;
            }

                    break;
                case true:      //  Если игра на паузе, то..
                    InGameNotification.Update(gameTime);
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
            GraphicsDevice.Clear(new Color(123, 17, 17));
            spriteBatch.Begin();
            {

                switch (gameState)
                {
                    case State.PauseMenu:
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
                        mainMenu.Draw(spriteBatch);
                        break;
                    case State.Defeat:
                        endMenu.Draw(spriteBatch);
                        break;
                    case State.Victory:
                        endMenu.Draw(spriteBatch);
                        break;
                }
                InGameNotification.Draw(spriteBatch);       //  Отрисовка уведомлений
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

        public static void ClearAll() // Очистка списков с уже уничтоженными клетками для уменьшения нагрузки
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

        public static void RestartProgress() // Вернуть состояния всех объектов на исходные значения
        {
            for (int i = 0; i < bacteriumList.Count; i++)
            {
                bacteriumList.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < eritroList.Count; i++) 
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
            oxygenPoints -= 20;
        }

        public void BuyLeiko()
        {
            SpawnLeiko(1);
            oxygenPoints -= 30;
        }

        public void BuyTrombo()
        {
            SpawnTrombo(1);
            oxygenPoints -= 40;
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
            gameState = State.PauseMenu;
        }
        void ClosePauseMenu()
        {
            gameState = State.Playing;
        }

        #endregion

        #region GUIDE
        
        #endregion
    }
}
