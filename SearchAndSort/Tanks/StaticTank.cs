using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SearchAndSort
{
    public class StaticTank : EnemyTank
    {
        public new bool enemy = true;

        public StaticTank() { }
        public StaticTank(Game1 _game, Texture2D _tankTexture, Vector2 _location, Vector2 _speed, float _rotation, int _strength): base(_game, _tankTexture, _location, _speed, _rotation, _strength)
        {
            deathParticles = new ParticleSpray(location, game, player, tankTexture, Color.Green, 0);
            tankRect = new Rectangle((int)location.X - (tankTexture.Width / 2), (int)location.Y - (tankTexture.Height / 2), tankTexture.Width, tankTexture.Height);
            targetDirection = DOWN;
        }

        public override void Update(KeyboardState state, GameTime gameTime) {
            SlowlyRotate(AimAt(game.playerTanks[0].location), gameTime);
            base.Update(state, gameTime);
        }

        public override void Move(KeyboardState state)
        {
            //AI CODE--This Tank does not move.
        }
    }
}
