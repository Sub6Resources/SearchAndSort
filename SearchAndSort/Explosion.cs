using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SearchAndSort
{
    public class Explosion
    {
        public static int MAX_PROJECTILES = 60;
        private Color color;
        private Bullet[] shrapnel = new Bullet[MAX_PROJECTILES];
        public Explosion() { }
        public Explosion(Vector2 location, Game1 game, int player, Texture2D whiteRectangle, Color _color)
        {
            color = _color;
            Random randy = new Random();
            for(int i=0; i<MAX_PROJECTILES; ++i)
            {
                Vector2 speed = new Vector2();
                int a = randy.Next(-20, 20);
                int b = randy.Next(-20, 20);
                if (a == 0 || a ==1 || a == 2)
                {
                    a = 3;
                }
                if (b == 0 || b == 1 || b== 2)
                {
                    b = 3;
                }
                speed = new Vector2(a, b);
                int r = randy.Next(0, 456);
                int g = randy.Next(0, 456);
                int c = randy.Next(0, 456);
                color = Color.FromNonPremultiplied(255, r, g, c);
                //Create a new shrapnel bullet
                shrapnel[i] = new Bullet(game, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(randy.Next(1, 20), randy.Next(1, 20))), speed, color, player, randy.Next(-((int)MathHelper.Pi), (int)MathHelper.Pi), whiteRectangle);
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < MAX_PROJECTILES; ++i)
            {
                shrapnel[i].Draw(spriteBatch);
            }
        }
        public void Update()
        {
            for (int i = 0; i < MAX_PROJECTILES; ++i)
            {
                shrapnel[i].Update();
            }
        }
    }
}
