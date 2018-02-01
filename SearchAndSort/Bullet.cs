using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{



    public class Bullet
    {
        public Game1 game;
        public Rectangle bulletRect;
        public Vector2 speed;
        public Color color { get; set; }
        public int player { get; set; }
        public float rotation { get; set; }
        public Texture2D rectangleTexture;
        public bool alive { get; set; }
        public int pointsOnHit { get; set; }
        public int pointsOnKill { get; set; }
        public Bullet() { }
        public Bullet(Game1 _game, Rectangle _bulletRect, Vector2 _speed, Color _color, int _player, float _rotation, Texture2D _rectangleTexture)
        {
            game = _game;
            bulletRect = _bulletRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            rectangleTexture = _rectangleTexture;
            //rectangleTexture.SetData(new[] { Color.White });
            alive = true;
            pointsOnHit = 50;
            pointsOnKill = 200;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(rectangleTexture, bulletRect, color);
            }
        }
        public virtual void Update()
        {
            if (alive)
            {
                bulletRect.X += (int)speed.X;
                bulletRect.Y += (int)speed.Y;
                CheckCollision();
            }
        }
        public virtual void CheckCollision()
        {
			foreach(EnemyTank et in game.enemyTanks)
			{
				if((Rectangle.Intersect(bulletRect, new Rectangle((int)et.location.X - (et.tankTexture.Width / 2), (int)et.location.Y - (et.tankTexture.Height / 2), et.tankTexture.Width, et.tankTexture.Height)).Width != 0) && et.alive && player != 100)
				{
					et.Hit();
					game.scoreManager.addScore(player-1, pointsOnHit);
					if (!et.alive)
					{
						game.scoreManager.addScore(player-1, pointsOnKill);
					}
					this.Die();
				}
			}
            foreach(Tank tank in game.playerTanks) {
                if (tank.player != this.player && Rectangle.Intersect(bulletRect, tank.tankRect).Width != 0 && tank.alive) {
                    tank.Hit();
                    game.scoreManager.addScore(player - 1, pointsOnHit);
                    if(!tank.alive) {
                        game.scoreManager.addScore(player - 1, pointsOnKill);
                    }
                    this.Die();
                }
                if (player == 100 && Rectangle.Intersect(bulletRect, tank.tankRect).Width != 0 && tank.alive)
                {
                    tank.Hit();
                    game.scoreManager.addScore(0, pointsOnHit);
                    if (!tank.alive)
                    {
                        game.scoreManager.addScore(0, pointsOnKill);
                    }
                    this.Die();
                }
            }
			foreach (Tile[] tiles in game.map.map)
            {
                foreach (Tile tile in tiles)
                {
                    if (Rectangle.Intersect(bulletRect, tile.collisionRect).Width > 0 && tile.type != Tile.AIR)
                    {
                        this.Die();
                    }
                }
            }
        }
        public virtual void Die()
        {
            alive = false;
            speed = Vector2.Zero;
            color = Color.Blue;
        }
    }
}
