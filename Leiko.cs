using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BloodSim
{
    public class Leiko : Cell
    {

        public Rectangle currentTarget;
        public Random random;

        public static bool needNotification = true;

        public SoundEffect death_soundEffect;
        bool dead = false;

        public SoundEffect bite_soundEffect;
        int biteTimer = 0;

        public event Action OnDeath;

        public Leiko(Random random)
        {
            texture = null;
            this.random = random;
            currentTarget = new Rectangle(100, 100, 2, 2);
            position = new Vector2(Game1.gameWidth / 4, Game1.gameHeight / 2);

            OnDeath += Die;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/leiko");
            death_soundEffect = content.Load<SoundEffect>("Sounds/kill");
            bite_soundEffect = content.Load<SoundEffect>("Sounds/bite");
        }

        public void Update(GameTime gameTime, List<Bacterium> Blist, List<Wall> list)
        {
            if (hp > 0)
            {
                foreach (Bacterium bac in Blist)
                {
                    Vector2 dis = bac.position - position;
                    float length = (float)Math.Sqrt(dis.X + dis.Y);
                    //float length = (float)Math.Sqrt(Math.Sqrt(dis.X + dis.Y) * Math.Sqrt(dis.X + dis.Y));

                    if ((length < 5000) && (bac.position.Y < Game1.gameHeight) && (bac.position.Y > 0) && (bac.position.X < list[1].position.X))
                    {
                        //currentTarget = new Rectangle((int)bac.position.X + bac.boundingBox.Width / 2, (int)bac.position.Y + bac.boundingBox.Height / 2, 3, 3);
                        currentTarget = bac.boundingBox;
                    }
                    else
                    {
                        currentTarget = new Rectangle(random.Next(0, 500), random.Next(0, Game1.gameHeight - boundingBox.Height), 3, 3);
                    }

                    if (boundingBox.Intersects(bac.boundingBox))
                    {
                        biteTimer++;
                        if (biteTimer == 30)
                        {
                            bac.hp -= 10;
                            SoundEffect.MasterVolume = 0.5f;
                            bite_soundEffect.Play();
                            biteTimer = 0;
                        }
                    }

                    /*if (bac.hp <= 0)
                    {
                        currentTarget = new Rectangle(100, 100, 2, 2);
                    }*/
                }

                if (!currentTarget.Intersects(boundingBox))
                {
                    Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                    Direction.Normalize();
                    position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 400;
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
    }
}