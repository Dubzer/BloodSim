using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BloodSim
{
    public class Bacterium : Unit
    {
        Random random;

        public event Action OnPenetration;
        public bool hasPenetrated = false;
        public bool holeFound = false;
        public static bool needNotification = true;
        public event Action OnDeath;
        bool dead = false;

        public SoundEffect death_soundEffect;

        public SoundEffect bite_soundEffect;
        int biteTimer = 0;


        public Rectangle currentTarget;

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/bacterium");
            death_soundEffect = content.Load<SoundEffect>("Sounds/kill");
            bite_soundEffect = content.Load<SoundEffect>("Sounds/bite");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public Bacterium(Random random)
        {
            OnDeath += Die;
            OnPenetration += Penetrate;
            position = new Vector2(Game1.gameWidth + boundingBox.Width, random.Next(0, Game1.gameHeight));

            this.random = random;
        }

        public void Update(GameTime gameTime, List<Cell> list, List<Wall> wallList)
        {
            if (hp > 0) 
            {

                if (hasPenetrated)
                {
                    foreach (Cell cell in list)
                    {
                        if (cell.hp > 0)
                        {
                            Vector2 dis = new Vector2(cell.position.X, cell.position.Y) - position;
                            float length = (float)Math.Sqrt(dis.X + dis.Y);

                            if ((length < 5000) && (cell.position.Y < Game1.gameHeight) && (cell.position.Y > 0) && (cell.hp > 0))
                            {
                                currentTarget = cell.boundingBox;
                            }
                            else
                            {
                                currentTarget = new Rectangle(200, Game1.gameHeight / 2, 3, 3);
                            }

                            if (boundingBox.Intersects(cell.boundingBox))
                            {
                                biteTimer++;
                                if (biteTimer == 50)
                                {
                                    cell.hp -= 10;
                                    SoundEffect.MasterVolume = 0.5f;
                                    bite_soundEffect.Play();
                                    biteTimer = 0;
                                }
                            }
                        }

                    }
                }
                else
                {
                        for (int i = 1; i < wallList.Count; i++)
                        {
                            if (wallList[i].hp <= 0)
                            {
                                currentTarget = new Rectangle(wallList[i].boundingBox.X + 50, wallList[i].boundingBox.Y, 3, 3);
                                holeFound = true;
                                break;
                            }
                            else
                            {
                                currentTarget = new Rectangle(random.Next(Game1.gameWidth - Game1.gameWidth / 4, Game1.gameWidth), random.Next(0, Game1.gameHeight), 3, 3);
                                holeFound = false;
                                continue;
                            }
                        }
                }

                /*if (new Vector2(boundingBox.X, boundingBox.Y) != new Vector2(currentTarget.X, currentTarget.Y))
                {
                    Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                    Direction.Normalize();
                    position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 400;
                }*/

                if (!boundingBox.Intersects(currentTarget))
                {
                    Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                    Direction.Normalize();
                    position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 400;
                }
                else if (holeFound)
                {
                    OnPenetration();
                }

                if (position.Y >= Game1.gameHeight - boundingBox.Height)
                {
                    position.Y = Game1.gameHeight - boundingBox.Height;
                }
                if (position.Y <= 0)
                {
                    position.Y = 0;
                }

                boundingBox = new Rectangle((int)position.X, (int)position.Y, Game1.gameWidth / 10, Game1.gameHeight / 10);
            }
            else
            {
                if (!dead)
                {
                    position = new Vector2(-1000, -1000);
                    OnDeath();
                }
            }

        }

        public void Die()
        {
            SoundEffect.MasterVolume = 0.5f;
            death_soundEffect.Play();

            dead = true;
        }

        public void Penetrate()
        {
            hasPenetrated = true;
        }
    }
}
