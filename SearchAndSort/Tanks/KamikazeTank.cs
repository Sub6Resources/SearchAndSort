using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
	public class KamikazeTank : EnemyTank
	{
		private const int AI_TOLERANCE = 3;
        Vector2 initSpeed = new Vector2();
        public bool kamiCharge;
        private bool chargeSoundPlayed = false;

		public KamikazeTank() { }
		public KamikazeTank(Game1 _game, string _tankSpriteName, Vector2 _location, Vector2 _speed, float _rotation, int _player, float _scale, Texture2D _whiteRectangle)
		{
			tankTexture = _game.Content.Load<Texture2D>(_tankSpriteName);
			location = _location;
			startingLocation = _location;
			speed = _speed;
            initSpeed = speed;
			rotation = _rotation;
			origin = new Vector2(this.tankTexture.Width / 2f, this.tankTexture.Height / 2f);
			game = _game;
			player = _player;
			scale = _scale;
			whiteRectangle = _whiteRectangle;
			alive = true;
			lives = 4;
			respawnParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Gray, 0);
			deathParticles = new ParticleSpray(location, game, player, whiteRectangle, Color.Gray, 0);
			tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
			targetDirection = DOWN;
            kamiCharge = false;
		}
		public override void Move(KeyboardState state)
		{
            if (colliding && !kamiCharge)
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
            } else if (colliding && kamiCharge)
            {
                Explode();
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
            speed = initSpeed;
            //If very close to enemy tank, explode
            foreach (Tank tank in game.playerTanks) {
                if ((location.X >= tank.location.X - AI_TOLERANCE && location.X <= tank.location.X + AI_TOLERANCE) && (location.Y >= tank.location.Y - AI_TOLERANCE && location.Y <= tank.location.Y + AI_TOLERANCE))
                {
                    Explode();
                }
                if ((location.X >= tank.location.X - AI_TOLERANCE && location.X <= tank.location.X + AI_TOLERANCE) && location.Y > tank.location.Y)
                {
                    if (lives <= 2 && !kamiCharge)
                    {
                        kamiCharge = true;
                        targetDirection = UP;
                    }
                    if (!kamiCharge)
                    {
                        targetDirection = UP;
                    }
                }
                //If X = X of enemy tank and Y < Y of enemy tank, go down.
                if ((location.X >= tank.location.X - AI_TOLERANCE && location.X <= tank.location.X + AI_TOLERANCE) && location.Y < tank.location.Y)
                {
                    if (lives <= 2 && !kamiCharge)
                    {
                        kamiCharge = true;
                        targetDirection = DOWN;
                    }
                    if (!kamiCharge)
                    {
                        targetDirection = DOWN;
                    }
                }
                //If Y = Y of enemy tank and X > X of enemy tank, go left.
                if ((location.Y >= tank.location.Y - AI_TOLERANCE && location.Y <= tank.location.Y + AI_TOLERANCE) && location.X > tank.location.X)
                {
                    if (lives <= 2 && !kamiCharge)
                    {
                        kamiCharge = true;
                        targetDirection = LEFT;
                    }
                    if (!kamiCharge)
                    {
                        targetDirection = LEFT;
                    }

                }
                //If Y = Y of enemy tank and X < X of enemy tank, go right.
                if ((location.Y >= tank.location.Y - AI_TOLERANCE && location.Y <= tank.location.Y + AI_TOLERANCE) && location.X < tank.location.X)
                {
                    if (lives <= 2 && !kamiCharge)
                    {
                        kamiCharge = true;
                        targetDirection = RIGHT;
                    }
                    if (!kamiCharge)
                    {
                        targetDirection = RIGHT;
                    }
                }
            }

			//If X = X of enemy tank and Y > Y of enemy tank, go up.
			
            if (kamiCharge)
            {
                if (!chargeSoundPlayed)
                {
                    chargeSoundPlayed = true;
                    game.sound.PlaySound(Sound.Sounds.KAMICHARGE);
                }
                speed += new Vector2(3, 3);
            }
		}
        public override void Die()
        {
            base.Die();
            game.sound.PlaySound(Sound.Sounds.KAMIDEATH);
        }
        public override void Hit()
        {
            base.Hit();
            game.sound.PlaySound(Sound.Sounds.KAMIHURT);
        }
    }
}
