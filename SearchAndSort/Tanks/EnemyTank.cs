using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class EnemyTank : Tank {

		public float targetDirection;
		public new bool enemy = true;
		private float delayOfFire = 3;
		private const float FIRE_DELAY = 3;
		
		public EnemyTank() { }
		public EnemyTank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle)
		{
			tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
			location = _location;
			startingLocation = _location;
			speed = _speed;
			rotation = _rotation;
			origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
			game = _game;
			player = _player;
			scale = _scale;
			whiteRectangle = _whiteRectangle;
			alive = true;
			lives = 3;
			respawnParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Gray, 0);
			deathParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Gray, 0);
			tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
			targetDirection = DOWN;
		}
		public override void Update(KeyboardState state, GameTime gameTime)
		{
			float timer = (float) gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
			delayOfFire -= timer;
			if(delayOfFire <= 0)
			{
				game.bullets.Add(Fire());
				delayOfFire = FIRE_DELAY;
			}
			base.Update(state, gameTime);
		}
		public override void Move(KeyboardState state)
		{
			//AI CODE---------------------------------------------THIS IS A BASIC AI TANK--------------------------------------------------------------------------------
			if(colliding)
			{
				switch ((int)targetDirection)
				{
					case (int)UP:
						targetDirection = RIGHT;
						break;
					case (int)RIGHT:
						targetDirection = DOWN;
						break;
					case (int)LEFT:
						targetDirection = UP;
						break;
					case (int)DOWN:
						targetDirection = LEFT;
						break;
					//case (int)UP_RIGHT:
						//targetDirection = RIGHT;
						//break;
					case (int)UP_LEFT:
						targetDirection = LEFT;
						break;
					//case (int)DOWN_RIGHT:
						//targetDirection = RIGHT;
						//break;
					case (int)DOWN_LEFT:
						targetDirection = LEFT;
						break;
					default:
						break;
				} 
			}
            switch ((int)targetDirection)
			{
				case (int)UP:
					MoveUp(false, false);
					Rotate(UP);
					break;
				case (int)RIGHT:
					MoveRight(false, false);
					Rotate(RIGHT);
					break;
				case (int)LEFT:
					MoveLeft(false, false);
					Rotate(LEFT);
					break;
				case (int)DOWN:
					MoveDown(false, false);
					Rotate(DOWN);
					break;
				//case (int)UP_RIGHT:
					//MoveUp(false, false);
					//MoveRight(false, false);
					//Rotate(UP_RIGHT);
					//break;
				case (int)UP_LEFT:
					MoveUp(false, false);
					MoveLeft(false, false);
					Rotate(UP_LEFT);
					break;
				//case (int)DOWN_RIGHT:
					//MoveDown(false, false);
					//MoveRight(false, false);
					//Rotate(DOWN_RIGHT);
					//break;
				case (int)DOWN_LEFT:
					MoveDown(false, false);
					MoveLeft(false, false);
					Rotate(DOWN_LEFT);
					break;
				default:
					break;
			}
        }
	}
}
