using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class StaticTank : EnemyTank
    {
        public new bool enemy = true;
        private SpriteFont spriteFont;
        public bool Moving = false;
        private Vector2 target;
        private Vector2 tempTarget;

        public StaticTank()
        {
            spriteFont = game.Content.Load<SpriteFont>("score");
        }

        public StaticTank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation,
            int _strength, int _lives) : base(_game, _tankTexture, _location, _speed, _rotation, _strength, _lives)
        {
            deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Green, 0);
            tankRect = new Rectangle((int) location.X - tankTexture.Width / 2,
                (int) location.Y - tankTexture.Height / 2, tankTexture.Width, tankTexture.Height);
            targetDirection = DOWN;
            spriteFont = game.Content.Load<SpriteFont>("score");
        }

        public override void Update(KeyboardState state, GameTime gameTime)
        {
            if (Moving)
            {
                if (colliding)
                {
                    tempTarget = target + new Vector2(0, target.X - location.X);
                    SlowlyRotate(AimAt(tempTarget), gameTime);
                    SlowlyMoveTo(tempTarget, gameTime);
                }
                else
                {
                    Rotate(AimAt(target));
                    if (SlowlyMoveTo(target, gameTime))
                    {
                        //Called if tank is done moving
                        Moving = false;
                    }
                }
                
            }
            else {
                SlowlyRotate(AimAt(game.playerTanks[0].location), gameTime);
            }
            base.Update(state, gameTime);
        }

        public override void Move(KeyboardState state)
        {
            //AI CODE--This Tank does not move.
        }

        public void SetTarget(Vector2 location)
        {
            Moving = true;
            target = location;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(
                spriteFont,
                ""+strength,
                location+new Vector2(-10, 15),
                Color.GhostWhite);
        }
    }
}