using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class Landmine : Bullet
    {
        public Explosion theExplosion;
        public Landmine() { }
        public Landmine(Game1 _game, Rectangle _bulletRect, Vector2 _speed, Color _color, int _player, float _rotation, Texture2D _rectangleTexture)
        {
            game = _game;
            bulletRect = _bulletRect;
            speed = _speed;
            color = _color;
            player = _player;
            rotation = _rotation;
            rectangleTexture = _rectangleTexture;
            alive = true;
            pointsOnHit = 50;
            pointsOnKill = 200;
        }
        public override void Die()
        {
            base.Die();

            game.scoreManager.addScore(player - 1, 1000);
            theExplosion = new Explosion(new Vector2(this.bulletRect.X, this.bulletRect.Y), this.game, player, this.rectangleTexture, Color.Orange);
        }
        public override void Update()
        {
            base.Update();
            if (!alive)
            {
                theExplosion.Update();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!alive)
            {
                theExplosion.Draw(spriteBatch);
            }

        }
        public override void CheckCollision()
        {
			foreach (EnemyTank et in game.enemyTanks)
			{
				if ((Rectangle.Intersect(bulletRect, new Rectangle((int)et.location.X - (et.tankTexture.Width / 2), (int)et.location.Y - (et.tankTexture.Height / 2), et.tankTexture.Width, et.tankTexture.Height)).Width != 0) && et.alive)
				{
					this.Die();
				}
			}
            foreach (Tank tank in game.playerTanks) {
                if(tank.player != this.player && Rectangle.Intersect(bulletRect, tank.tankRect).Width != 0 && tank.alive) {
                    this.Die();
                }
            }
        }
    }
}
