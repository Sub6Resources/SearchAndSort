using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SearchAndSort
{
    public class Map
    {
        private Game1 game;
        public int screenWidth;
        public int rowWidth;
        public int screenHeight;
        public int columnHeight;
        public Tile[][] map;
        public Texture2D wallTexture;
        public Map(Game1 _game, int _screenWidth, int _screenHeight)
        {
            game = _game;
            screenWidth = _screenWidth;
            screenHeight = _screenHeight;
            rowWidth = screenWidth / 48;
            columnHeight = screenHeight / 48;
            map = new Tile[rowWidth][];
            for(int i=0; i < map.Length; ++i)
            {
                map[i] = new Tile[columnHeight];
            }
            //TEXTURES
            wallTexture = game.Content.Load<Texture2D>("wall");

            //RESET
            Reset();
        }
        public void Reset()
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {
                    map[i][e] = new Tile(Tile.AIR, new Rectangle(e * 48, i * 48, 48, 48), null);
                }
            }
            DrawWallBorder();
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for (int e = 0; e < map[i].Length; ++e)
                {
                    map[i][e].Update(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                for(int e = 0; e < map[i].Length; ++e)
                {
                    map[i][e].Draw(spriteBatch);
                }
            }
        }
        public void DrawWallBorder()
        {
            //First row
            for(int i = 0; i < map.Length; ++i)
            {
                map[i][0] = new Tile(Tile.WALL, new Rectangle(i * 48, 0, 48, 48), wallTexture);
            }
            //Middle rows
            for(int i = 1; i < map[0].Length - 1; ++i)
            {
                map[0][i] = new Tile(Tile.WALL, new Rectangle(0, i * 48, 48, 48), wallTexture);
                map[rowWidth-1][i] = new Tile(Tile.WALL, new Rectangle((rowWidth-1)*48, i*48, 48, 48), wallTexture);
            }
            //Bottom row
            for(int i = 0; i < map.Length; ++i)
            {
                map[i][columnHeight-1] = new Tile(Tile.WALL, new Rectangle(i * 48, (columnHeight-1)*48, 48, 48), wallTexture);
            }
        }
    }
}
