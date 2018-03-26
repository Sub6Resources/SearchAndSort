using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class Menu
    {
        private Game1 _game;
        private string[] _levelStrings;

        private SpriteFont spriteFont;
        
        public Menu(Game1 game, string[] levelStrings)
        {
            _game = game;
            _levelStrings = levelStrings;
            Load();
        }

        private void Load()
        {
            spriteFont = _game.Content.Load<SpriteFont>("score");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, "Search and sort Tank Game",
                new Vector2(100, 100), Color.Purple, 0, Vector2.Zero, 4,
                SpriteEffects.None, 1f);
            for (var i = 0; i < _levelStrings.Length; i++)
            {
                spriteBatch.DrawString(spriteFont, (i+1) + " - " + _levelStrings[i],
                    new Vector2(100, 164 + 20 * i), Color.Black);
            }
            
            spriteBatch.DrawString(spriteFont, "Once the tanks are sorted, hold B to do a binary search", new Vector2(300, 175),
                Color.Purple, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(spriteFont, "Click on a tank to target it in the binary search", new Vector2(300, 200),
                Color.Purple, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
        }
    }
}