using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SearchAndSort.Tanks;

namespace SearchAndSort
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const int MAIN_MENU = 0;
        private const int SEARCH_CIRCLE = 1;
        private const int SORT_LINE = 2;
        private const int SORT_LINE_SELECT = 3;
        private const int SORT_LINE_INSERTION = 4;

        private string[] listOfLevels = {"Linear Search O(n)", "Bubble Sort O(n^2)", "Selection Sort O(n^2)", "Insertion Sort O()"};
        
        private int gameState = MAIN_MENU;
        
        
        //Extra game stuff
        private Texture2D background;
        public List<Bullet> bullets = new List<Bullet>();

        public List<EnemyTank> enemyTanks = new List<EnemyTank>();

        //Graphics Stuff
        private readonly GraphicsDeviceManager graphics;
        public List<Landmine> landmines = new List<Landmine>();
        public Map map;

        //Game stuff
        public List<Tank> playerTanks = new List<Tank>();
        public Score scoreManager;
        public Sound sound;
        private SpriteBatch spriteBatch;
        public Texture2D texture2d;
        public Menu menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        /// This is the global initialize for all levels.
        protected override void Initialize()
        {
            //Initialize generic texture
            texture2d = new Texture2D(GraphicsDevice, 1, 1);
            texture2d.SetData(new[] {Color.White});

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

            //Initialize window background
            background = Content.Load<Texture2D>("Stars");

            
//            enemyTanks.Add(new EnemyTank(this, Content.Load<Texture2D>("PinkTank"), new Vector2(200, 200), new Vector2(5, 5), 0, 100, 1));
//            enemyTanks.Add(new KamikazeTank(this, Content.Load<Texture2D>("YellowTank"), new Vector2(400, 400), new Vector2(3, 3), 0, 3));
//            enemyTanks.Add(new StaticTank(this, Content.Load<Texture2D>("YellowTank"), new Vector2(300, 300), new Vector2(3, 3), 0, 3));
            
            //Initialize scoring system
            scoreManager = new Score(this);

            //Initialize sound system.
            sound = new Sound(this);
            
            menu = new Menu(this, listOfLevels);
            
            Initialize(MAIN_MENU);

            //Initialize base
            base.Initialize();
        }

        /// <summary>
        /// Starts a level.
        /// </summary>
        /// <param name="level">The desired level.</param>
        private void Initialize(int level)
        {
            playerTanks.Clear();
            enemyTanks.Clear();
            scoreManager.Reset();
            //Initialize controls
            var player1Controls = new Controls(Keys.W, Keys.A, keyDown: Keys.S, keyRight: Keys.D, keyBoost: Keys.Tab,
                keyReverse: Keys.LeftShift, keyFire: Keys.Space, keyExplode: Keys.E, keyMine: Keys.Q,
                keySearch: Keys.R);
            var player2Controls = new Controls(Keys.Up, Keys.Left, keyDown: Keys.Down, keyRight: Keys.Right,
                keyBoost: Keys.RightShift, keyReverse: Keys.LeftShift, keyFire: Keys.Enter, keyExplode: Keys.RightAlt,
                keyMine: Keys.OemQuestion, keySearch: Keys.OemComma);

            Random randy = new Random();
            
            switch (level)
            {
                case MAIN_MENU:
                    break;
                case SEARCH_CIRCLE:
                    //Map
                    var searchCircleMap =
                        "11111111111111111111\n" +
                        "10000000000000000001\n" +
                        "10000001111110000001\n" +
                        "10000010000001000001\n" +
                        "10000100000000100001\n" +
                        "10001000000000010001\n" +
                        "10001000000000010001\n" +
                        "10001000000000010001\n" +
                        "10001000000000010001\n" +
                        "10001000000000010001\n" +
                        "10001000000000010001\n" +
                        "10000100000000100001\n" +
                        "10000010000001000001\n" +
                        "10000001111110000001\n" +
                        "10000000000000000001\n" +
                        "11111111111111111111";
                    map.setMap(searchCircleMap);
                    
                    
                    //Initialize tanks
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("GreenTank"), new Vector2(100, 100),
                        new Vector2(3, 3), 0, 1, Color.Green, player1Controls));
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("RedTank"),
                        new Vector2(map.screenWidth - 100, 100), new Vector2(3, 3), MathHelper.Pi, 2, Color.Red, player2Controls));
                    playerTanks.Add(new ConfusedTank(this, Content.Load<Texture2D>("YellowTank"),
                        new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2),
                        Vector2.Zero, 0, 3, Color.Yellow));
                    const int NUM_TANKS_SEARCHCIRCLE = 16;
                    //Generate circle of static tanks
                    for (var i = 0; i < NUM_TANKS_SEARCHCIRCLE; i++)
                    {
                        var centerX = graphics.PreferredBackBufferWidth / 2;
                        var centerY = graphics.PreferredBackBufferHeight / 2;
                        var angle = 360 / NUM_TANKS_SEARCHCIRCLE * i * (Math.PI / 180);
                        var circleRadius = 200;
        
                        var pointX = (int) Math.Round(Math.Cos(angle) * circleRadius + centerX);
                        var pointY = (int) Math.Round(Math.Sin(angle) * circleRadius + centerY);
        
                        enemyTanks.Add(new StaticTank(this, Content.Load<Texture2D>("GreenTank"), new Vector2(pointX, pointY),
                            new Vector2(3, 3), (float) (angle + Math.PI), randy.Next(0, 100), 1));
                    }
                    break;
                case SORT_LINE:
                    //Map
                    var sortLineMap =
                        "11111111111111111111\n" +
                        "10000000000000000001\n" +
                        "10000001111110000001\n" +
                        "10000010000001000001\n" +
                        "10000100000000100001\n" +
                        "10001000000000010001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10001000000000010001\n" +
                        "10000100000000100001\n" +
                        "10000010000001000001\n" +
                        "10000001111110000001\n" +
                        "10000000000000000001\n" +
                        "11111111111111111111";
                    map.setMap(sortLineMap);
                    //Initialize tanks
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("GreenTank"), new Vector2(100, 100),
                        new Vector2(3, 3), 0, 1, Color.Green, player1Controls));
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("RedTank"),
                        new Vector2(map.screenWidth - 100, 100), new Vector2(3, 3), MathHelper.Pi, 2, Color.Red, player2Controls));
                    
                    const int NUM_TANKSLINE = 16;

            
                    //Generate line of static tanks
                    for (var i = 0; i < NUM_TANKSLINE; i++)
                    {
                        var centerX = graphics.PreferredBackBufferWidth / 2;
                        var centerY = graphics.PreferredBackBufferHeight / 2;
                        var margin = 100;
                        var width = graphics.PreferredBackBufferWidth - 100;

                        var pointX = margin + width / NUM_TANKSLINE * i;
                        var pointY = centerY;
                
                        enemyTanks.Add(new StaticTank(this, Content.Load<Texture2D>("GreenTank"), new Vector2(pointX, pointY),
                            new Vector2(3,3), 0f, randy.Next(0, 100), 1));
                    }
                    
                    //Reset variables
                    doingBigMove = false;
                    tankDoingBigMove = 0;
                    doingLittleMove = false;
                    doneWithOneSort = false;
                    tankDoingLittleMove = 0;
                    movingTanksLocation = Vector2.Zero;
                    break;
                case SORT_LINE_SELECT:
                    //Map
                    var sortLineSelectMap =
                        "11111111111111111111\n" +
                        "10000000000000000001\n" +
                        "10000001111110000001\n" +
                        "10000010000001000001\n" +
                        "10000100000000100001\n" +
                        "10001000000000010001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10001000000000010001\n" +
                        "10000100000000100001\n" +
                        "10000010000001000001\n" +
                        "10000001111110000001\n" +
                        "10000000000000000001\n" +
                        "11111111111111111111";
                    map.setMap(sortLineSelectMap);
                    //Initialize tanks
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("GreenTank"), new Vector2(100, 100),
                        new Vector2(3, 3), 0, 1, Color.Green, player1Controls));
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("RedTank"),
                        new Vector2(map.screenWidth - 100, 100), new Vector2(3, 3), MathHelper.Pi, 2, Color.Red, player2Controls));
                    
                    const int NUM_TANKSLINESELECT = 16;

            
                    //Generate line of static tanks
                    for (var i = 0; i < NUM_TANKSLINESELECT; i++)
                    {
                        var centerX = graphics.PreferredBackBufferWidth / 2;
                        var centerY = graphics.PreferredBackBufferHeight / 2;
                        var margin = 100;
                        var width = graphics.PreferredBackBufferWidth - 100;

                        var pointX = margin + width / NUM_TANKSLINESELECT * i;
                        var pointY = centerY;
                
                        enemyTanks.Add(new StaticTank(this, Content.Load<Texture2D>("GreenTank"), new Vector2(pointX, pointY),
                            new Vector2(3,3), 0f, randy.Next(0, 100), 1));
                    }
                    
                    //Reset Variables
                    selectSortCurrentIndex = 0;
                    break;
                case SORT_LINE_INSERTION:
                    //Map
                    var sortLineInsertMap =
                        "11111111111111111111\n" +
                        "10000000000000000001\n" +
                        "10000001111110000001\n" +
                        "10000010000001000001\n" +
                        "10000100000000100001\n" +
                        "10001000000000010001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10000000000000000001\n" +
                        "10001000000000010001\n" +
                        "10000100000000100001\n" +
                        "10000010000001000001\n" +
                        "10000001111110000001\n" +
                        "10000000000000000001\n" +
                        "11111111111111111111";
                    map.setMap(sortLineInsertMap);
                    //Initialize tanks
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("GreenTank"), new Vector2(100, 100),
                        new Vector2(3, 3), 0, 1, Color.Green, player1Controls));
                    playerTanks.Add(new Player(this, Content.Load<Texture2D>("RedTank"),
                        new Vector2(map.screenWidth - 100, 100), new Vector2(3, 3), MathHelper.Pi, 2, Color.Red, player2Controls));
                    
                    const int NUM_TANKSLINEINSERT= 8;

            
                    //Generate line of static tanks
                    for (var i = 0; i < NUM_TANKSLINEINSERT; i++)
                    {
                        var centerX = graphics.PreferredBackBufferWidth / 2;
                        var centerY = graphics.PreferredBackBufferHeight / 2;
                        var margin = 100;
                        var width = graphics.PreferredBackBufferWidth - 100;

                        var pointX = margin + width / NUM_TANKSLINEINSERT * i;
                        var pointY = centerY;
                
                        enemyTanks.Add(new StaticTank(this, Content.Load<Texture2D>("GreenTank"), new Vector2(pointX, pointY),
                            new Vector2(3,3), 0f, randy.Next(0, 100), 1));
                    }

                    //Reset Variables
                    insertionSortCurrentIndex = 0;
                    innerInsertionSortLoopIndex = 0;
                    break;
            }

            gameState = level;
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Eh, whatever. Maybe do this later.
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            
            //Level Logic
            switch (gameState)
            {
                case MAIN_MENU:
                    if (state.IsKeyDown(Keys.Escape) && state.IsKeyDown(Keys.LeftControl))
                        Exit();
                    if(state.IsKeyDown(Keys.D1))
                        Initialize(SEARCH_CIRCLE);
                    if(state.IsKeyDown(Keys.D2))
                        Initialize(SORT_LINE);
                    if(state.IsKeyDown(Keys.D3))
                        Initialize(SORT_LINE_SELECT);
                    if(state.IsKeyDown(Keys.D4))
                        Initialize(SORT_LINE_INSERTION);
                    break;
                case SEARCH_CIRCLE:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                        Initialize(MAIN_MENU);
                    updateGame(gameTime, state);
                    break;
                case SORT_LINE:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                        Initialize(MAIN_MENU);
                    updateGame(gameTime, state);
                    BubbleSort();
                    break;
                case SORT_LINE_SELECT:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                        Initialize(MAIN_MENU);
                    updateGame(gameTime, state);
                    SelectionSort();
                    break;
                case SORT_LINE_INSERTION:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                        Initialize(MAIN_MENU);
                    updateGame(gameTime, state);
                    InsertionSort();
                    break;
            }
            
            //Update base
            base.Update(gameTime);
        }

        private void updateGame(GameTime gameTime, KeyboardState state)
        {
            //Update Map
            map.Update(gameTime);
                    
            //Update Tanks
            foreach (var player in playerTanks) player.Update(state, gameTime);
            foreach (var enemyTank in enemyTanks) enemyTank.Update(state, gameTime);
                    
            //Update Game Objects
            foreach (var landmine in landmines) landmine.Update();
            foreach (var bullet in bullets)
                if (bullet != null)
                    bullet.Update();
        }

        private bool doingBigMove;
        private bool doingLittleMove;
        private bool doneWithOneSort;
        private int tankDoingBigMove;
        private int tankDoingLittleMove;
        private Vector2 movingTanksLocation;
        
        private void BubbleSort()
        {
            //If a tank is currently moving, then wait.
            foreach (var tank in enemyTanks)
            {
                if (((StaticTank) tank).Moving) return;
            }

            
            
            for (var i = tankDoingBigMove; i < enemyTanks.Count; i++)
            {
                var staticTank = (StaticTank) enemyTanks[i];
                try
                {
                    if (staticTank.strength > enemyTanks[i + 1].strength && !doingBigMove)
                    {
                        movingTanksLocation = staticTank.location;
                        staticTank.SetTarget(new Vector2(graphics.PreferredBackBufferWidth / 2 , 500));
                        doingBigMove = true;
                        tankDoingBigMove = i;
                        return;
                    }
                    
                    if (doingBigMove && !doingLittleMove)
                    {
                        if (enemyTanks[tankDoingBigMove].strength > enemyTanks[tankDoingBigMove + 1].strength)
                        {
                            //Move the next tank to the old location.
                            ((StaticTank) enemyTanks[tankDoingBigMove + 1]).SetTarget(movingTanksLocation);
                            movingTanksLocation = enemyTanks[tankDoingBigMove + 1].location;
                            doingLittleMove = true;
                            tankDoingLittleMove = tankDoingBigMove + 1;
                            return;
                        }

                        if (tankDoingBigMove == 0) return;
                        ((StaticTank) enemyTanks[tankDoingBigMove]).SetTarget(movingTanksLocation);
                        doingBigMove = false;
                        doingLittleMove = false;
                        doneWithOneSort = false;
                        return;
                    }

                    if (doingLittleMove)
                    {
                        doingLittleMove = false;
                        int temp = tankDoingBigMove;
                        tankDoingBigMove = tankDoingLittleMove;
                        tankDoingLittleMove = temp;
                        EnemyTank tempTank = enemyTanks[tankDoingBigMove];
                        enemyTanks[tankDoingBigMove] = enemyTanks[tankDoingLittleMove];
                        enemyTanks[tankDoingLittleMove] = tempTank;
                        return;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    for (var j = 0; j < enemyTanks.Count; j++)
                    {
                        var _staticTank = (StaticTank) enemyTanks[j];
                        try
                        {
                            if (!(_staticTank.strength < enemyTanks[j + 1].strength) || _staticTank.Moving)
                            {
                                break;
                            }
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            return;
                        }
                    }
                    if (tankDoingBigMove == 0) return;
                    ((StaticTank) enemyTanks[tankDoingBigMove]).SetTarget(movingTanksLocation);
                    doingBigMove = false;
                    doingLittleMove = false;
                    doneWithOneSort = false;
                    tankDoingBigMove = 0;
                    tankDoingLittleMove = 0;
                    return;
                }
            }
        }

        private int selectSortCurrentIndex;
        
        //This function is called every update loop.
        private void SelectionSort()
        {
            //The outer loop is complete, so the sort is done.
            if (!(selectSortCurrentIndex < enemyTanks.Count)) return;
            
            //If a tank is currently moving, then wait.
            foreach (var tank in enemyTanks)
            {
                if (((StaticTank) tank).Moving) return;
            }

            //Get min and mindex.
            var min = enemyTanks[selectSortCurrentIndex].strength;
            var mindex = selectSortCurrentIndex;
            
            //Find the min and mindex
            for (int i = selectSortCurrentIndex; i < enemyTanks.Count; i++)
            {
                if (enemyTanks[i].strength < min)
                {
                    min = enemyTanks[i].strength;
                    mindex = i;
                }
            }
            
            //Put the min at the front of the unsorted list.
            swapTankPositions(selectSortCurrentIndex, mindex);
            
            //Increment the outer loop.
            selectSortCurrentIndex++;
        }

        private int insertionSortCurrentIndex;
        private int innerInsertionSortLoopIndex;
        
        //This function is called every update loop.
        private void InsertionSort()
        {
            //Outer loop is completed, so the sort is done.
            if (insertionSortCurrentIndex >= enemyTanks.Count) return;
            
            //If a tank is currently moving, then wait.
            foreach (var tank in enemyTanks)
            {
                if (((StaticTank) tank).Moving) return;
            }
            
            //Inner loop
            //Starts where it left off before return (innerInsertionSortLoopIndex)
            for (int start = innerInsertionSortLoopIndex; start > 0; start--)
            {
                //Updates innerInsertionSortLoopIndex to match the current iteration.
                innerInsertionSortLoopIndex = start;
                
                //Compare strengths
                if (enemyTanks[start].strength < enemyTanks[start - 1].strength)
                {
                    //Swap
                    swapTankPositions(start, start - 1);
                    
                    //Return so that the tank can finish moving. (The loop will pick back up where it left off when the tank finishes moving)
                    return;
                }
            }

            //Increment the outer loop
            insertionSortCurrentIndex++;
            //Reset the inner loop
            innerInsertionSortLoopIndex = insertionSortCurrentIndex;
        }

        private void swapTankPositions(int index1, int index2)
        {
            //If the tanks are the same tank, then nothing needs to happen.
            if (index1 == index2) return;
            
            //Create some temporary variables to make it easier to work with StaticTank specific functions.
            var tank1 = (StaticTank) enemyTanks[index1];
            var tank2 = (StaticTank) enemyTanks[index2];
            
            //Switch Tank Locations
            tank1.SetTarget(tank2.location);
            tank2.SetTarget(tank1.location);
            
            //Switch Array Location
            var tempTank = tank1;
            enemyTanks[index1] = enemyTanks[index2];
            enemyTanks[index2] = tempTank;
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            spriteBatch.Begin();

            switch (gameState)
            {
                case MAIN_MENU:
                    menu.Draw(spriteBatch);
                    break;
                case SEARCH_CIRCLE:
                    drawGame(spriteBatch);
                    break;
                case SORT_LINE:
                    drawGame(spriteBatch);
                    break;
                case SORT_LINE_SELECT:
                    drawGame(spriteBatch);
                    break;
                case SORT_LINE_INSERTION:
                    drawGame(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void drawGame(SpriteBatch spriteBatch) {
            //Draw background
            spriteBatch.Draw(background, new Rectangle(0, 0, map.screenWidth, map.screenHeight), Color.White);

            //Draw game objects
            foreach (var lm in landmines) lm.Draw(spriteBatch);
            foreach (var bullet in bullets)
                if (bullet != null)
                    bullet.Draw(spriteBatch);

            //Draw tanks
            foreach (var player in playerTanks) player.Draw(spriteBatch);
            foreach (var et in enemyTanks) et.Draw(spriteBatch);

            //Draw map
            map.Draw(spriteBatch);

            //Draw score
            scoreManager.Draw(spriteBatch);
        }
    }
}