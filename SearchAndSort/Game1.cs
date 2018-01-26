using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SearchAndSort
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public Map map;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D whiteRectangle;
        public List<Tank> playerTanks = new List<Tank>();
		public List<EnemyTank> enemyTanks = new List<EnemyTank>();
        public List<Bullet> bullets = new List<Bullet>();
        public Score scoreManager;
        public List<Landmine> landmines = new List<Landmine>();
        Rectangle debugRect;
        Rectangle tank2DebugRect;
        Texture2D background;
        public Sound sound;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            //UNCOMMENT NEXT THREE COMMENTS FOR FULLSCREEN
            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - GraphicsDevice.DisplayMode.Width % 48; //Makes the window size a divisor of 48 so the tiles fit more cleanly.
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - GraphicsDevice.DisplayMode.Height % 48;
            graphics.PreferredBackBufferWidth = 48 * 20;
            graphics.PreferredBackBufferHeight = 48 * 16;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            map = new Map(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            whiteRectangle.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Stars");
            playerTanks.Add(new Tank(this, "GreenTank", new Vector2(100,100), new Vector2(3, 3), 0, 1, 1f, whiteRectangle, Keys.W, Keys.A, Keys.S, Keys.D, Keys.Tab, Keys.LeftShift, Keys.Space, Keys.Q, Keys.E));
            playerTanks.Add(new Tank(this, "RedTank", new Vector2(map.screenWidth-100, 100), new Vector2(3, 3), MathHelper.Pi, 2, 1f, whiteRectangle, Keys.Up, Keys.Left, Keys.Down, Keys.Right, Keys.Enter, Keys.RightShift, Keys.Enter, Keys.RightAlt, Keys.OemQuestion));
			enemyTanks.Add(new EnemyTank(this, "PinkTank", new Vector2(200, 200), new Vector2(5, 5), 0, 100, 1f, whiteRectangle));
			enemyTanks.Add(new KamikazeTank(this, "YellowTank", new Vector2(400, 400), new Vector2(3, 3), 0, 100, 1f, whiteRectangle));
            enemyTanks.Add(new StaticTank(this, "YellowTank", new Vector2(300, 300), new Vector2(3, 3), 0, 100, 1f, whiteRectangle));
            scoreManager = new Score(this, 100);
            debugRect = new Rectangle();
            tank2DebugRect = new Rectangle();
            sound = new Sound(this);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            map.Update(gameTime);

            KeyboardState state = Keyboard.GetState();

            //Update delays
            foreach(Tank player in playerTanks) {
                player.updateDelays(gameTime);
                player.Update(state, gameTime);
            }
			foreach (EnemyTank et in enemyTanks)
			{
				et.Update(state, gameTime);
			}
            //debugRect = new Rectangle((int)tank1.location.X-(tank1.tankTexture.Width/2), (int)tank1.location.Y-(tank1.tankTexture.Height/2), tank1.tankTexture.Width, tank1.tankTexture.Height);
            //tank2DebugRect = new Rectangle((int)tank2.location.X - (tank2.tankTexture.Width / 2), (int)tank2.location.Y - (tank2.tankTexture.Height / 2), tank2.tankTexture.Width, tank2.tankTexture.Height);
            foreach (Landmine lm in landmines)
            {
                lm.Update();
            }
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null)
                {
                    bullet.Update();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, map.screenWidth, map.screenHeight), Color.White);
            map.Draw(spriteBatch);
            //DEBUG DRAWS (COMMENT OUT TO TURN OFF DEBUG MODE)
            //spriteBatch.Draw(whiteRectangle, debugRect, Color.Pink); //Tank1 DebugRect
            //spriteBatch.Draw(whiteRectangle, tank2DebugRect, Color.Pink); //Tank2 DebugRect
            foreach (Landmine lm in landmines)
            {
                lm.Draw(spriteBatch);
            }
            foreach (Tank player in playerTanks) {
                player.Draw(spriteBatch);
            }
			foreach (EnemyTank et in enemyTanks)
			{
				et.Draw(spriteBatch);
			}
            
            scoreManager.Draw(spriteBatch);
            
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null)
                {
                    bullet.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
    
