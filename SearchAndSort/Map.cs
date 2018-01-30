using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SearchAndSort
{
    public class Map
    {
        private Game1 game;
        public int screenWidth;
        public int screenHeight;

        public int rowWidth;
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

            //Initialize map containers
            map = new Tile[rowWidth][];
            for(int x=0; x < map.Length; ++x)
            {
                map[x] = new Tile[columnHeight];
            }

            //TEXTURES
            wallTexture = game.Content.Load<Texture2D>("wall");

            //RESET
            Reset();
        }
        public void Reset()
        {
            for (int x = 0; x < map.Length; ++x)
            {
                for (int y = 0; y < map[x].Length; ++y)
                {
                    map[x][y] = new Tile(Tile.AIR, new Rectangle(x * 48, y * 48, 48, 48), null);
                }
            }
            setMap("");
        }
        public void Update(GameTime gameTime)
        {
            for (int x = 0; x < map.Length; ++x)
            {
                for (int y = 0; y < map[x].Length; ++y)
                {
                    map[x][y].Update(gameTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < map.Length; ++x)
            {
                for(int y = 0; y < map[x].Length; ++y)
                {
                    map[x][y].Draw(spriteBatch);
                }
            }
        }

        public void setMap(string stringMap) {
            string[] splitMap = stringMap.Split('\n');
            for (int y = 0; y < splitMap.Length; ++y) {
                for (int x = 0; x < splitMap[y].Length; ++x) {
                    map[x][y] = new Tile(int.Parse(splitMap[y][x].ToString()), new Rectangle(x * 48, y * 48, 48, 48), wallTexture);
                }
            }
        }
    }
}
