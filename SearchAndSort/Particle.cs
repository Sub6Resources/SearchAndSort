using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    internal class Particle
    {
        private Game1 game;
        public Rectangle particleRect;
        public Texture2D particleTexture;
        public Vector2 speed;

        public Particle()
        {
        }

        public Particle(Game1 _game, Rectangle _particleRect, Vector2 _speed, Color _color, int _player,
            float _rotation, Texture2D _particleTexture)
        {
            game = _game;
            particleRect = _particleRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            particleTexture = _particleTexture;
            alive = true;
            decay = false;
            decayTime = float.MinValue;
            initDecayTime = decayTime;
            fade = false;
        }

        public Particle(Game1 _game, Rectangle _particleRect, Vector2 _speed, Color _color, int _player,
            float _rotation, Texture2D _particleTexture, float _decayTime, bool _fade)
        {
            game = _game;
            particleRect = _particleRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            particleTexture = _particleTexture;
            alive = true;
            decay = true;
            decayTime = 0;
            initDecayTime = _decayTime;
            fade = _fade;
        }

        public Color color { get; set; }
        public int player { get; set; }
        public float rotation { get; set; }
        public bool alive { get; set; }
        public bool decay { get; set; }
        public bool fade { get; set; }
        public float decayTime { get; set; }
        public float initDecayTime { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive) spriteBatch.Draw(particleTexture, particleRect, color);
        }

        public void Update(GameTime gameTime)
        {
            if (alive)
            {
                particleRect.X += (int) speed.X;
                particleRect.Y += (int) speed.Y;
                if (decay)
                {
                    decayTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                    if (fade) color = Color.Lerp(color, Color.Transparent, decayTime / initDecayTime);
                    if (decayTime >= initDecayTime) Die();
                }
            }
        }

        public void Die()
        {
            alive = false;
            speed = Vector2.Zero;
            color = Color.Transparent;
        }
    }
}