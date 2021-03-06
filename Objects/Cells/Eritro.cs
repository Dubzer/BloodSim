﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace BloodSim
{
    public class Eritro : Cell
    {
        private bool oppression = false;

        public Rectangle currentTarget;
        public Random random;
        public bool oxygenMining = true;

        public SoundEffect death_soundEffect;
        bool dead = false;

        public event Action OnDeath;

        public Eritro(Random random)
        {
            texture = null;
            this.random = random;
            currentTarget = new Rectangle(100, Game1.gameHeight + 10000, 2, 2);
            position = new Vector2(random.Next(0, Game1.gameWidth / 4), -100);
            boundingBox = new Rectangle((int)position.X, (int)position.Y, Game1.gameWidth / 15, Game1.gameWidth / 20);

            OnDeath += Die;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/eritro");
            death_soundEffect = content.Load<SoundEffect>("Sounds/kill");
        }

        public void Update(GameTime gameTime, Rectangle currentTarget, List<Wall> list)
        {
            if (hp > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].hp <= 0)
                    {
                        oxygenMining = false;
                        currentTarget = new Rectangle(list[i].boundingBox.X + 50, list[i].boundingBox.Y, list[i].boundingBox.Width, list[i].boundingBox.Height);
                        break;
                    }
                    else
                    {
                        oxygenMining = true;
                        continue;
                    }
                }


                /*if (!oxygenMining)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].hp <= 0)
                        {
                            currentTarget = new Rectangle(list[i].boundingBox.X + 50, list[i].boundingBox.Y, list[i].boundingBox.Width, list[i].boundingBox.Height);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }*/

                if ((position.Y >= Game1.gameHeight) && (oxygenMining))
                {
                    position.Y = -boundingBox.Height;
                    position.X = random.Next(0, Game1.gameWidth / 2);
                    Game1.oxygenPoints += 30;
                }
                
                if (boundingBox.X > list[0].position.X + 30)
                {
                    hp = 0;
                }

                Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                Direction.Normalize();
                position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 200;

                boundingBox = new Rectangle((int)position.X, (int)position.Y, Game1.gameWidth / 15, Game1.gameWidth / 20);
            }
            if (hp <= 0)
            {
                Debug.Print("Df");
                if (!dead)
                {
                    Debug.Print("Eritro умер как бы");

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
    }
}
