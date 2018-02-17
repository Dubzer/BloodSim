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

namespace BloodSim
{
    public class Leiko : Cell
    {
        public Rectangle currentTarget;
        public Random random;

        public SoundEffect soundEffect;
        bool dead = false;

        public event Action OnDeath;

        public Leiko(Random random)
        {
            texture = null;
            this.random = random;
            position = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 4, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2);
            currentTarget = new Rectangle(100, 100, 2, 2);
            Game1.leikoList.Add(this);
            Game1.cellList.Add(this);

            OnDeath += Die;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/leiko");
            soundEffect = content.Load<SoundEffect>("Sounds/kill");
        }

        public void Update(GameTime gameTime, List<Bacterium> Blist, List<Wall> list)
        {
            if (hp > 0)
            {
                foreach (Bacterium bac in Blist)
                {
                    Vector2 dis = new Vector2(bac.position.X, bac.position.Y) - position;
                    float length = (float)Math.Sqrt(dis.X + dis.Y);

                    if ((length < 5000) && (bac.position.Y < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) && (bac.position.Y > 0))
                    {
                        currentTarget = bac.boundingBox;
                    }

                    if (boundingBox.Intersects(bac.boundingBox))
                    {
                        bac.hp -= 1;
                    }

                    if (bac.hp <= 0)
                    {
                        currentTarget = new Rectangle(100, 100, 2, 2);
                    }
                }

                if ((currentTarget.X != boundingBox.X) && (currentTarget.Y != boundingBox.Y))
                {
                    Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                    Direction.Normalize();
                    position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 400;
                }

                boundingBox = new Rectangle((int)position.X, (int)position.Y, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 10, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10);
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
            soundEffect.Play();

            dead = true;
        }
    }
}
