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
    public class Bacterium : Unit
    {
        public event Action OnDeath;
        bool dead = false;

        public SoundEffect soundEffect;

        public Rectangle currentTarget;

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/bacterium");
            soundEffect = content.Load<SoundEffect>("Sounds/kill");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public Bacterium()
        {
            position = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            //Game1.bacteriumList.Add(this);

            OnDeath += Die;
        }

        public void Update(GameTime gameTime, List<Cell> list)
        {
            if (hp > 0)
            {
                foreach (Cell cell in list)
                {
                    if (cell.hp > 0)
                    {
                        Vector2 dis = new Vector2(cell.position.X, cell.position.Y) - position;
                        float length = (float)Math.Sqrt(dis.X + dis.Y);

                        if ((length < 5000) && (cell.position.Y < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) && (cell.position.Y > 0) && cell.hp > 0)
                        {
                            currentTarget = cell.boundingBox;
                        }
                        else
                        {
                            currentTarget = new Rectangle(200, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2, 100, 100);
                        }

                        if (boundingBox.Intersects(cell.boundingBox))
                        {
                            cell.hp -= 1;
                        }
                    }
                   
                }

                if (new Vector2(boundingBox.X, boundingBox.Y) != new Vector2(currentTarget.X, currentTarget.Y))
                {
                    Vector2 Direction = new Vector2(currentTarget.X, currentTarget.Y) - position;
                    Direction.Normalize();
                    position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 400;
                }

                if (position.Y >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - boundingBox.Height)
                {
                    position.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - boundingBox.Height;
                }
                if (position.Y <= 0)
                {
                    position.Y = 0;
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
