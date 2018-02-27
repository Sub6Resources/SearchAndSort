using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class EnemyTank : Tank
    {
        public new bool enemy = true;
        public int strength;

        public float targetDirection;

        public EnemyTank()
        {
        }

        public EnemyTank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation,
            int _strength, int _lives) : base(_game, _tankTexture, _location, _speed, _rotation, 100, Color.Red, _lives)
        {
            strength = _strength;
            targetDirection = DOWN;
            deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Gray, 0);
        }

        public override void Update(KeyboardState state, GameTime gameTime)
        {
            base.Update(state, gameTime);
        }

        public override void Move(KeyboardState state)
        {
            //AI CODE---------------------------------------------THIS IS A BASIC AI TANK--------------------------------------------------------------------------------
            if (colliding)
                switch ((int) targetDirection)
                {
                    case (int) UP:
                        targetDirection = RIGHT;
                        break;
                    case (int) RIGHT:
                        targetDirection = DOWN;
                        break;
                    case (int) LEFT:
                        targetDirection = UP;
                        break;
                    case (int) DOWN:
                        targetDirection = LEFT;
                        break;
                    //case (int)UP_RIGHT:
                    //targetDirection = RIGHT;
                    //break;
                    case (int) UP_LEFT:
                        targetDirection = LEFT;
                        break;
                    //case (int)DOWN_RIGHT:
                    //targetDirection = RIGHT;
                    //break;
                    case (int) DOWN_LEFT:
                        targetDirection = LEFT;
                        break;
                    default:
                        break;
                }
            switch ((int) targetDirection)
            {
                case (int) UP:
                    MoveUp(false, false);
                    Rotate(UP);
                    break;
                case (int) RIGHT:
                    MoveRight(false, false);
                    Rotate(RIGHT);
                    break;
                case (int) LEFT:
                    MoveLeft(false, false);
                    Rotate(LEFT);
                    break;
                case (int) DOWN:
                    MoveDown(false, false);
                    Rotate(DOWN);
                    break;
                //case (int)UP_RIGHT:
                //MoveUp(false, false);
                //MoveRight(false, false);
                //Rotate(UP_RIGHT);
                //break;
                case (int) UP_LEFT:
                    MoveUp(false, false);
                    MoveLeft(false, false);
                    Rotate(UP_LEFT);
                    break;
                //case (int)DOWN_RIGHT:
                //MoveDown(false, false);
                //MoveRight(false, false);
                //Rotate(DOWN_RIGHT);
                //break;
                case (int) DOWN_LEFT:
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