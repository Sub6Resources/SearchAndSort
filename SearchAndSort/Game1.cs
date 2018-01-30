using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace SearchAndSort
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Graphics Stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture2d;

        //Game stuff
        public List<Tank> playerTanks = new List<Tank>();
		public List<EnemyTank> enemyTanks = new List<EnemyTank>();
        public List<Bullet> bullets = new List<Bullet>();
        public List<Landmine> landmines = new List<Landmine>();
        public Map map;
        public Score scoreManager;

        //Extra game stuff
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
            //Initialize generic texture
            texture2d = new Texture2D(GraphicsDevice, 1, 1);
            texture2d.SetData(new[] { Color.White });

            //Initialize window
            //SWITCH COMMENTS TO TOGGLE FULLSCREEN
            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - GraphicsDevice.DisplayMode.Width % 48; //Makes the window size a divisor of 48 so the tiles fit more cleanly.
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - GraphicsDevice.DisplayMode.Height % 48;
            graphics.PreferredBackBufferWidth = 48 * 20;
            graphics.PreferredBackBufferHeight = 48 * 16;
            //graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            //Initialize Map
            map = new Map(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //Map
            string levelMap =
                "11111111111111111111\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "10000000000000000001\n" +
                "11111111111111111111";
            map.setMap(levelMap);

            //Initialize window background
            background = Content.Load<Texture2D>("Stars");

            //Initialize tanks
            playerTanks.Add(new Tank(this, "GreenTank", new Vector2(100,100), new Vector2(3, 3), 0, 1, 1f, texture2d, Keys.W, Keys.A, Keys.S, Keys.D, Keys.Tab, Keys.LeftShift, Keys.Space, Keys.Q, Keys.E));
            playerTanks.Add(new Tank(this, "RedTank", new Vector2(map.screenWidth-100, 100), new Vector2(3, 3), MathHelper.Pi, 2, 1f, texture2d, Keys.Up, Keys.Left, Keys.Down, Keys.Right, Keys.Enter, Keys.RightShift, Keys.Enter, Keys.RightAlt, Keys.OemQuestion));
            enemyTanks.Add(new EnemyTank(this, "PinkTank", new Vector2(200, 200), new Vector2(5, 5), 0, 100, 1f, texture2d));
            enemyTanks.Add(new KamikazeTank(this, "YellowTank", new Vector2(400, 400), new Vector2(3, 3), 0, 100, 1f, texture2d));
            enemyTanks.Add(new StaticTank(this, "YellowTank", new Vector2(300, 300), new Vector2(3, 3), 0, 100, 1f, texture2d));

            int numTanks = 36;

            //Generate circle of static tanks
            for (int i = 0; i < numTanks; i++) {
                int centerX = graphics.PreferredBackBufferWidth / 2;
                int centerY = graphics.PreferredBackBufferHeight / 2;
                double angle = 360/numTanks * i * (Math.PI / 180);
                int circleRadius = 200;

                int pointX = (int) Math.Round(Math.Cos(angle) * circleRadius + centerX);
                int pointY = (int) Math.Round(Math.Sin(angle) * circleRadius + centerY);

                enemyTanks.Add(new StaticTank(this, "GreenTank", new Vector2(pointX, pointY), new Vector2(3, 3), (float) (angle+Math.PI), 100, 1f, texture2d));
            }
            //Initialize scoring system
            scoreManager = new Score(this, 100);

            //Initialize sound system.
            sound = new Sound(this);

            //Initialize base
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
            //Eh, whatever. Maybe do this later.
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            //Update Map
            map.Update(gameTime);

            //Update Tanks
            foreach(Tank player in playerTanks) {
                player.Update(state, gameTime);
            }
			foreach (EnemyTank enemyTank in enemyTanks)
			{
				enemyTank.Update(state, gameTime);
			}

            //Update Game Objects
            foreach (Landmine landmine in landmines)
            {
                landmine.Update();
            }
            foreach (Bullet bullet in bullets)
            {
                if(bullet != null)
                    bullet.Update();
            }

            //Update base
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();

            //Draw background
            spriteBatch.Draw(background, new Rectangle(0, 0, map.screenWidth, map.screenHeight), Color.White);

            //Draw tanks
            foreach (Tank player in playerTanks)
            {
                player.Draw(spriteBatch);
            }
            foreach (EnemyTank et in enemyTanks)
            {
                et.Draw(spriteBatch);
            }

            //Draw map
            map.Draw(spriteBatch);

            //Draw game objects
            foreach (Landmine lm in landmines)
            {
                lm.Draw(spriteBatch);
            }
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null)
                {
                    bullet.Draw(spriteBatch);
                }
            }

            //Draw score
            scoreManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}