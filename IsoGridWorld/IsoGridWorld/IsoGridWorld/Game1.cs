using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.IO;
using System.Text;

namespace IsoGridWorld
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState kbState;
        KeyboardState prevKBState = Keyboard.GetState();

        Vector2 MenuFootprint, SmallFootprint, CursorPosition, Center, Offset;
        Vector2 FontOffset = Vector2.Zero;

        bool ShowInstructions = false;

        int OffsetFactor = 0;

        SpriteFont ScreenFont;

        StringBuilder input;

        Rectangle LeftSrc = new Rectangle(0, 0, 54, 150);
        Rectangle RightSrc = new Rectangle(216, 0, 54, 150);
        Rectangle SwitchSrc;

        /// <summary>
        /// Maps used for running
        /// </summary>
        List<Map> Maps;

        /// <summary>
        /// Maps used for Drawing
        /// </summary>
        List<Map> StaticMaps;

        Texture2D AIAppearance;

        enum GameStates
        {
            PreSetup,
            Main,
            Load,
            Instructions,
            NumSetup,
            LvLSetup,
            Game,
            Exit,
            Dummy
        }

        List<Text> MainMenu;
        List<Text> InstructionsMenu;
        List<Text> NumSetupMenu;
        List<Text> LevelSetupMenu;
        List<Text> GameMenu;
        List<Text> ImportExportMenu;
        List<Text> ExitMenu;

        Color MainColor;
        Color InstructColor;
        Color NumSetupColor;
        Color LevelSetupColor;
        Color GameColor;
        Color ExitColor;
        Color InterimColor;
        Color GoalColor;
        Color FontInterimColor;

        byte RDelta = 0;
        int RDiff = 0;
        byte GDelta = 0;
        int GDiff = 0;
        byte BDelta = 0;
        int BDiff = 0;

        bool ColorTimeout = false;

        TimeSpan ColorTimer;
        TimeSpan TurnOffTimer;

        const int turnoffMilli = 1000;
        const int fps = 30;
        int FrameMillis = turnoffMilli / fps;


        //TODO: Show the trials and display the map with where the AI was at each trial
        //TODO: Take an average of the runs
        //TODO: Setting for random reset
        //TODO: Fix Regular and Hard reset
        //TODO: Dynamic scaling of trial run setting

        GameStates GS;

        int UserChoice = 0;
        #endregion
        string MapXML = @"..\..\..\XML Files\IsoStuffXML.xml";
        string MapTxt = @"..\..\..\Files\";
        string LvLn1 = "Lvl -1.txt";
        string LvL0 = "LvL 0.txt";
        string LvL1 = "LvL 1.txt";
        string LvL2 = "LvL 2.txt";
        string LvL3 = "LvL 3.txt";
        string LvL4 = "LvL 4.txt";
        string LvL5 = "LvL 5.txt";
        string LvL6 = "LvL 6.txt";
        string LvL7 = "LvL 7.txt";
        string LvL8 = "LvL 8.txt";
        string LvL9 = "LvL 9.txt";

        Map LvLSwitch, PrevLvL;

        //TODO: Graph of Trials vs. Steps Taken
        TimeSpan Timer;
        TimeSpan DecayTimer;

        const int StaticDelay = 1000;
        int Delay = 1000;

        int DecayDelay = 500;

        int DelayDecay = -100;


        float EPSILON_OPTIMA = .9f;//It spiked at around .4f
        float EPSILON_DECAY = .001f;
        float GAMMA = .2f;
        float ALPHA = .4f;
        float LAMBDA = .5f;
        float REWARD = 1.0f;
        int DELAY = 1000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            GS = GameStates.Dummy;

            Maps = new List<Map>();
            StaticMaps = new List<Map>();
            this.IsMouseVisible = true;
            MenuFootprint = new Vector2(158, 80);
            SmallFootprint = new Vector2(126, 64);
            Center = new Vector2(LeftSrc.Width / 2, LeftSrc.Height - 28);
            SwitchSrc = RightSrc;
            input = new StringBuilder();
            Offset = new Vector2(-MenuFootprint.X / 4, 0);
        }

        //TODO: (optional) Fix the read-in so that the tall walls have accompanying collapsed walls
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            switch (GS)
            {
                case GameStates.PreSetup:
                    #region Load Scenes
                    MainMenu = new List<Text>();
                    InstructionsMenu = new List<Text>();
                    NumSetupMenu = new List<Text>();
                    LevelSetupMenu = new List<Text>();
                    GameMenu = new List<Text>();
                    ImportExportMenu = new List<Text>();
                    ExitMenu = new List<Text>();

                    MainColor = new Color(75, 62, 255/*Soft Blue*/);
                    InstructColor = new Color(255, 102, 255/*Purple*/);
                    NumSetupColor = new Color(36, 100, 72/*Bluish Green*/);
                    LevelSetupColor = new Color(24, 20, 52/*Dark Blue (Slight Darker)*/);
                    GameColor = new Color(32, 52, 20/*Dark Green*/);
                    ExitColor = new Color(72, 72, 35/*Dark Yellow*/);

                    InterimColor = new Color();
                    FontInterimColor = new Color();
                    GoalColor = MainColor;
                    CalculateColorSlideRate();
                    #endregion
                    #region LoadTextInfo
                    for (int i = 0; i < 3; i++)
                    {
                        MainMenu.Add(new Text(ScreenFont, MenuFootprint * (i + 1), Color.White));
                    }
                    MainMenu[0].Words = "Run dah Rat";
                    MainMenu[1].Words = "Read the Field Manual";
                    MainMenu[2].Words = "Rage Quit";

                    for (int i = 0; i < 6; i++)
                    {
                        NumSetupMenu.Add(new Text(ScreenFont, MenuFootprint * (i + 1), Color.White));
                    }
                    NumSetupMenu[0].Words = "Epsilon Choice";
                    NumSetupMenu[1].Words = "Epsilon Decay";
                    NumSetupMenu[2].Words = "Alpha";
                    NumSetupMenu[3].Words = "Gamma";
                    NumSetupMenu[4].Words = "Lambda";
                    NumSetupMenu[5].Words = "Reward";


                    #endregion
                    GS = GameStates.Main;
                    break;
                case GameStates.Dummy:
                    GS = GameStates.PreSetup;
                    break;
                case GameStates.Load:
                    //These will be used for map selection
                    #region Load Maps
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvLn1));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL5));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL0));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL2));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL8));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL3));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL1));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL4));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL9));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL6));
                    Maps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL7));

                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvLn1));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL5));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL0));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL2));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL8));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL3));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL1));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL4));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL9));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL6));
                    StaticMaps.Add(new Map(this, DELAY, ALPHA, GAMMA, LAMBDA, EPSILON_OPTIMA, EPSILON_DECAY, REWARD, AIAppearance, MapXML, MapTxt + LvL7));
                    foreach (Map m in Maps)
                    {
                        m.SetAISourceBounds(
                            LeftSrc,
                            new Rectangle(108, 0, 54, 150),
                            RightSrc,
                            new Rectangle(324, 0, 54, 150));
                        Components.Add(m);
                    }

                    foreach (Map m in StaticMaps)
                    {
                        Components.Add(m);
                    }

                    LvLSwitch = Maps[0];
                    PrevLvL = LvLSwitch;
                    #endregion
                    break;
            }
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
            switch (GS)
            {
                case GameStates.PreSetup:
                    AIAppearance = Content.Load<Texture2D>("IsomanANIM");
                    ScreenFont = Content.Load<SpriteFont>("Font");
                    Initialize();
                    break;
                case GameStates.Main:
                    CursorPosition = MenuFootprint + Offset;
                    GS = GameStates.Load;
                    Initialize();
                    break;
                case GameStates.Load:
                    GS = GameStates.Main;
                    break;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            Colorslide(gameTime);
            UHandleGameStates(gameTime);
            //HandleKeyInput(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            /*
            spriteBatch.DrawString(ScreenFont, "Interim Values| R:" + InterimColor.R + "G: " + InterimColor.G + "B: " + InterimColor.B + Delay, Vector2.Zero, Color.White);
            spriteBatch.DrawString(ScreenFont, "ColorTimer.TotalMilliseconds: " + ColorTimer.TotalMilliseconds,
                new Vector2(0, ScreenFont.MeasureString(" ").Y), Color.White);
            spriteBatch.DrawString(ScreenFont, "TurOffTimer.TotalMilliseconds: " + TurnOffTimer.TotalMilliseconds,
                new Vector2(0, ScreenFont.MeasureString(" ").Y * 2), Color.White);
            spriteBatch.DrawString(ScreenFont, "ColorTimout: " + ColorTimeout, new Vector2(0, ScreenFont.MeasureString(" ").Y * 3), Color.White);
            */
            DHandleGameStates();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UHandleGameStates(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            switch (GS)
            {
                case GameStates.Main:
                    #region Main
                    if (kbState.IsKeyDown(Keys.Up) && prevKBState.IsKeyUp(Keys.Up))
                    {
                        if (UserChoice != 0)
                        {
                            UserChoice--;
                            SwitchSrc = LeftSrc;
                            CursorPosition -= MenuFootprint;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Down) && prevKBState.IsKeyUp(Keys.Down))
                    {
                        if (UserChoice != 2)
                        {
                            UserChoice++;
                            SwitchSrc = RightSrc;
                            CursorPosition += MenuFootprint;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
                    {
                        switch (UserChoice)
                        {
                            case 0:
                                GS = GameStates.NumSetup;
                                GoalColor = NumSetupColor;
                                CalculateColorSlideRate();
                                break;
                            case 1:
                                GS = GameStates.Instructions;
                                GoalColor = InstructColor;
                                CalculateColorSlideRate();
                                break;
                            case 2:
                                GS = GameStates.Exit;
                                GoalColor = ExitColor;
                                CalculateColorSlideRate();
                                break;
                        }
                        UserChoice = 0;
                        SwitchSrc = RightSrc;
                        CursorPosition = MenuFootprint + Offset;
                    }
                    #endregion
                    break;
                case GameStates.NumSetup:
                    #region NumSetup
                    #region Typing Controls
                    if ((kbState.IsKeyDown(Keys.D1) && prevKBState.IsKeyUp(Keys.D1)) ||
                        (kbState.IsKeyDown(Keys.NumPad1) && prevKBState.IsKeyUp(Keys.NumPad1)))
                    {
                        input.Append("1");
                    }
                    if ((kbState.IsKeyDown(Keys.D2) && prevKBState.IsKeyUp(Keys.D2)) ||
                        (kbState.IsKeyDown(Keys.NumPad2) && prevKBState.IsKeyUp(Keys.NumPad2)))
                    {
                        input.Append("2");
                    }
                    if ((kbState.IsKeyDown(Keys.D3) && prevKBState.IsKeyUp(Keys.D3)) ||
                        (kbState.IsKeyDown(Keys.NumPad3) && prevKBState.IsKeyUp(Keys.NumPad3)))
                    {
                        input.Append("3");
                    }
                    if ((kbState.IsKeyDown(Keys.D4) && prevKBState.IsKeyUp(Keys.D4)) ||
                        (kbState.IsKeyDown(Keys.NumPad4) && prevKBState.IsKeyUp(Keys.NumPad4)))
                    {
                        input.Append("4");
                    }
                    if ((kbState.IsKeyDown(Keys.D5) && prevKBState.IsKeyUp(Keys.D5)) ||
                        (kbState.IsKeyDown(Keys.NumPad5) && prevKBState.IsKeyUp(Keys.NumPad5)))
                    {
                        input.Append("5");
                    }
                    if ((kbState.IsKeyDown(Keys.D6) && prevKBState.IsKeyUp(Keys.D6)) ||
                        (kbState.IsKeyDown(Keys.NumPad6) && prevKBState.IsKeyUp(Keys.NumPad6)))
                    {
                        input.Append("6");
                    }
                    if ((kbState.IsKeyDown(Keys.D7) && prevKBState.IsKeyUp(Keys.D7)) ||
                        (kbState.IsKeyDown(Keys.NumPad7) && prevKBState.IsKeyUp(Keys.NumPad7)))
                    {
                        input.Append("7");
                    }
                    if ((kbState.IsKeyDown(Keys.D8) && prevKBState.IsKeyUp(Keys.D8)) ||
                        (kbState.IsKeyDown(Keys.NumPad8) && prevKBState.IsKeyUp(Keys.NumPad8)))
                    {
                        input.Append("8");
                    }
                    if ((kbState.IsKeyDown(Keys.D9) && prevKBState.IsKeyUp(Keys.D9)) ||
                        (kbState.IsKeyDown(Keys.NumPad9) && prevKBState.IsKeyUp(Keys.NumPad9)))
                    {
                        input.Append("9");
                    }
                    if ((kbState.IsKeyDown(Keys.D0) && prevKBState.IsKeyUp(Keys.D0)) ||
                        (kbState.IsKeyDown(Keys.NumPad0) && prevKBState.IsKeyUp(Keys.NumPad0)))
                    {
                        input.Append("0");
                    }
                    if (((kbState.IsKeyDown(Keys.OemPeriod) && prevKBState.IsKeyUp(Keys.OemPeriod)) ||
                        (kbState.IsKeyDown(Keys.Decimal) && prevKBState.IsKeyUp(Keys.Decimal))) &&
                        !input.ToString().Contains("."))
                    {
                        input.Append(".");
                    }
                    if (kbState.IsKeyDown(Keys.Back) && prevKBState.IsKeyUp(Keys.Back))
                    {
                        if (input.Length > 0)
                            input.Remove(input.Length - 1, 1);
                    }
                    #endregion
                    #region Tab
                    if (kbState.IsKeyDown(Keys.Tab) && prevKBState.IsKeyUp(Keys.Tab))
                    {
                        switch (UserChoice)
                        {
                            case 0://ε Choice
                                if (input.Length != 0)
                                    EPSILON_OPTIMA = float.Parse(input.ToString());
                                break;
                            case 1://ε Decay
                                if (input.Length != 0)
                                    EPSILON_DECAY = float.Parse(input.ToString());
                                break;
                            case 2://α
                                if (input.Length != 0)
                                    ALPHA = float.Parse(input.ToString());
                                break;
                            case 3://Gamma
                                if (input.Length != 0)
                                    GAMMA = float.Parse(input.ToString());
                                break;
                            case 4://Lambda
                                if (input.Length != 0)
                                    LAMBDA = float.Parse(input.ToString());
                                break;
                            case 5://Reward
                                if (input.Length != 0)
                                    REWARD = float.Parse(input.ToString());
                                break;
                        }
                        switch (UserChoice)
                        {
                            case 0://ε Choice
                                input.Append(EPSILON_OPTIMA.ToString());
                                break;
                            case 1://ε Decay
                                input.Append(EPSILON_DECAY.ToString());
                                break;
                            case 2://α
                                input.Append(ALPHA.ToString());
                                break;
                            case 3://Gamma
                                input.Append(GAMMA.ToString());
                                break;
                            case 4://Lambda
                                input.Append(LAMBDA.ToString());
                                break;
                            case 5://Reward
                                input.Append(REWARD.ToString());
                                break;
                        }
                        input.Clear();
                        if (kbState.IsKeyDown(Keys.LeftShift) && UserChoice != 0)
                        {
                            UserChoice--;
                            SwitchSrc = LeftSrc;
                            CursorPosition -= MenuFootprint;
                        }

                        else if (kbState.IsKeyUp(Keys.LeftShift) && UserChoice != 5)
                        {
                            UserChoice++;
                            SwitchSrc = RightSrc;
                            CursorPosition += MenuFootprint;
                        }
                    }
                    #endregion
                    if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
                    {
                        switch (UserChoice)
                        {
                            case 0://ε Choice
                                if (input.Length != 0)
                                    EPSILON_OPTIMA = float.Parse(input.ToString());
                                break;
                            case 1://ε Decay
                                if (input.Length != 0)
                                    EPSILON_DECAY = float.Parse(input.ToString());
                                break;
                            case 2://α
                                if (input.Length != 0)
                                    ALPHA = float.Parse(input.ToString());
                                break;
                            case 3://Gamma
                                if (input.Length != 0)
                                    GAMMA = float.Parse(input.ToString());
                                break;
                            case 4://Lambda
                                if (input.Length != 0)
                                    LAMBDA = float.Parse(input.ToString());
                                break;
                            case 5://Reward
                                if (input.Length != 0)
                                    REWARD = float.Parse(input.ToString());
                                break;
                        }
                        input.Clear();
                        GS = GameStates.LvLSetup;
                        GoalColor = LevelSetupColor;
                        CalculateColorSlideRate();
                        UserChoice = 0;
                    }
                    if (kbState.IsKeyDown(Keys.Escape) && prevKBState.IsKeyUp(Keys.Escape))
                    {
                        GS = GameStates.Main;
                        GoalColor = MainColor;
                        CalculateColorSlideRate();
                        UserChoice = 0;
                        CursorPosition = MenuFootprint + Offset;
                        SwitchSrc = RightSrc;
                    }
                    #endregion
                    break;
                case GameStates.LvLSetup:
                    #region LvLSetup
                    if (kbState.IsKeyDown(Keys.Up) && prevKBState.IsKeyUp(Keys.Up))
                    {
                        UserChoice--;
                        if (UserChoice < 0)
                        {
                            UserChoice = Maps.Count - 1;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Down) && prevKBState.IsKeyUp(Keys.Down))
                    {
                        UserChoice++;
                        if (UserChoice == Maps.Count)
                            UserChoice = 0;
                    }
                    if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
                    {
                        Maps[UserChoice].EditMapValues(EPSILON_OPTIMA, EPSILON_DECAY, GAMMA, ALPHA, LAMBDA, REWARD);

                        LvLSwitch = Maps[UserChoice];
                        PrevLvL = LvLSwitch;

                        LvLSwitch.IsActive = true;
                        UserChoice = 0;
                        GS = GameStates.Game;
                        GoalColor = GameColor;
                        CalculateColorSlideRate();
                    }
                    if (kbState.IsKeyDown(Keys.Escape) && prevKBState.IsKeyUp(Keys.Escape))
                    {
                        UserChoice = 0;
                        SwitchSrc = RightSrc;
                        CursorPosition = MenuFootprint + Offset;
                        GS = GameStates.NumSetup;
                        GoalColor = NumSetupColor;
                        CalculateColorSlideRate();
                    }
                    #endregion
                    break;
                case GameStates.Load:
                    Initialize();
                    break;
                case GameStates.Instructions:
                    if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
                    {
                        GS = GameStates.Main;
                        UserChoice = 0;
                        SwitchSrc = RightSrc;
                        CursorPosition = MenuFootprint + Offset;
                    }
                    break;
                case GameStates.Game:
                    #region Game
                    if (kbState.IsKeyDown(Keys.Escape) && prevKBState.IsKeyUp(Keys.Escape))
                    {
                        GS = GameStates.LvLSetup;
                        GoalColor = LevelSetupColor;
                        CalculateColorSlideRate();
                        UserChoice = 0;
                        LvLSwitch.IsRunning = false;
                        LvLSwitch.IsActive = false;
                        CursorPosition = MenuFootprint + Offset;
                    }

                    //Show keyboard instructions
                    if (kbState.IsKeyDown(Keys.F1) && prevKBState.IsKeyUp(Keys.F1))
                    {
                        ShowInstructions = !ShowInstructions;
                    }


                    #region Arrows/ Export (F2, F3, F4, F5, F6, F7, F12)
                    if (kbState.IsKeyDown(Keys.F2) && prevKBState.IsKeyUp(Keys.F2))
                    {
                        LvLSwitch.ToggleArrowDraw();
                    }

                    if (kbState.IsKeyDown(Keys.F3) && prevKBState.IsKeyUp(Keys.F3))
                    {
                        LvLSwitch.ToggleArrowOffset();
                    }

                    //Import Table
                    if (kbState.IsKeyDown(Keys.F4) && prevKBState.IsKeyUp(Keys.F4))
                    {
                        LvLSwitch.ImportQER();
                    }

                    //Reset Map
                    if (kbState.IsKeyDown(Keys.F5) && prevKBState.IsKeyUp(Keys.F5))
                    {
                        LvLSwitch.Reset();
                    }

                    //Export Table
                    if (kbState.IsKeyDown(Keys.F6) && prevKBState.IsKeyUp(Keys.F6))
                    {
                        LvLSwitch.ExportQER();
                        LvLSwitch.ExportTrials();
                    }

                    //Export Trials
                    if (kbState.IsKeyDown(Keys.F7) && prevKBState.IsKeyUp(Keys.F7))
                    {
                        LvLSwitch.ExportTrials();
                    }

                    //Hard Reset Map
                    if (kbState.IsKeyDown(Keys.F12) && prevKBState.IsKeyUp(Keys.F12))
                    {
                        LvLSwitch.HardReset();
                    }
                    #endregion
                    //Scroll Screen
                    if (kbState.IsKeyDown(Keys.W))
                        LvLSwitch.OffsetMapBy(0, 3);
                    if (kbState.IsKeyDown(Keys.S))
                        LvLSwitch.OffsetMapBy(0, -3);
                    if (kbState.IsKeyDown(Keys.A))
                        LvLSwitch.OffsetMapBy(6, 0);
                    if (kbState.IsKeyDown(Keys.D))
                        LvLSwitch.OffsetMapBy(-6, 0);

                    //----------------------------------------------------------------------------------------------\\

                    //Move AI
                    if (kbState.IsKeyDown(Keys.I) && prevKBState.IsKeyUp(Keys.I))//Up
                    {
                        LvLSwitch.MoveLabrat(Direction.Up);
                    }
                    if (kbState.IsKeyDown(Keys.K) && prevKBState.IsKeyUp(Keys.K))//Down
                    {
                        LvLSwitch.MoveLabrat(Direction.Down);
                    }
                    if (kbState.IsKeyDown(Keys.J) && prevKBState.IsKeyUp(Keys.J))//Left
                    {
                        LvLSwitch.MoveLabrat(Direction.Left);
                    }
                    if (kbState.IsKeyDown(Keys.L) && prevKBState.IsKeyUp(Keys.L))//Right
                    {
                        LvLSwitch.MoveLabrat(Direction.Right);
                    }


                    //----------------------------------------------------------------------------------------------\\

                    //Show metrics
                    if (kbState.IsKeyDown(Keys.Tab) && prevKBState.IsKeyUp(Keys.Tab))
                        LvLSwitch.DisplayMetrics = !LvLSwitch.DisplayMetrics;

                    //----------------------------------------------------------------------------------------------\\
                    //Change Delay
                    if (kbState.IsKeyDown(Keys.Up) && prevKBState.IsKeyUp(Keys.Up))
                        LvLSwitch.ChangeDelay(100);
                    if (kbState.IsKeyDown(Keys.Down) && prevKBState.IsKeyUp(Keys.Down))
                        LvLSwitch.ChangeDelay(-100);

                    //----------------------------------------------------------------------------------------------\\
                    #region Change Trial Number (Left/ Right)
                    if (kbState.IsKeyDown(Keys.Left) && prevKBState.IsKeyUp(Keys.Left))
                    {
                        LvLSwitch.ChangeTrialsAmountBy(-5);
                    }
                    if (kbState.IsKeyDown(Keys.Left))
                    {
                        Timer += gameTime.ElapsedGameTime;
                        DecayTimer += gameTime.ElapsedGameTime;
                        if (Timer.TotalMilliseconds > Delay)
                        {
                            LvLSwitch.ChangeTrialsAmountBy(-5);
                            Timer = TimeSpan.Zero;
                        }
                        if (DecayTimer.TotalMilliseconds > DecayDelay && Delay + DelayDecay >= 0)
                        {
                            Delay += DelayDecay;
                            DecayTimer = TimeSpan.Zero;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Right) && prevKBState.IsKeyUp(Keys.Right))
                    {
                        LvLSwitch.ChangeTrialsAmountBy(5);
                    }
                    if (kbState.IsKeyDown(Keys.Right))
                    {
                        Timer += gameTime.ElapsedGameTime;
                        DecayTimer += gameTime.ElapsedGameTime;
                        if (Timer.TotalMilliseconds > Delay)
                        {
                            LvLSwitch.ChangeTrialsAmountBy(5);
                            Timer = TimeSpan.Zero;
                        }
                        if (DecayTimer.TotalMilliseconds > DecayDelay && Delay + DelayDecay >= 0)
                        {
                            Delay += DelayDecay;
                            DecayTimer = TimeSpan.Zero;
                        }
                    }
                    if (kbState.IsKeyUp(Keys.Left) && kbState.IsKeyUp(Keys.Right))
                    {
                        Delay = StaticDelay;
                        Timer = TimeSpan.Zero;
                        DecayTimer = TimeSpan.Zero;
                    }
                    #endregion
                    //----------------------------------------------------------------------------------------------\\

                    //Zoom map
                    if (kbState.IsKeyDown(Keys.Add) && prevKBState.IsKeyUp(Keys.Add))
                        LvLSwitch.ZoomMapBy(1);
                    if (kbState.IsKeyDown(Keys.Subtract) && prevKBState.IsKeyUp(Keys.Subtract))
                        LvLSwitch.ZoomMapBy(-1);

                    //----------------------------------------------------------------------------------------------\\

                    //Pause/Play AI
                    if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
                    {
                        LvLSwitch.ToggleRun();
                    }

                    //Collapse/ Uncollapse Walls
                    if (kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space))
                        LvLSwitch.ToggleWalls();
                    #endregion
                    break;
                case GameStates.Exit:
                    if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
                    {
                        this.Exit();
                    }
                    break;
            }
            prevKBState = kbState;
        }

        public void DHandleGameStates()
        {
            switch (GS)
            {
                case GameStates.Main:
                    GraphicsDevice.Clear(MainColor);
                    OffsetFactor = 0;
                    foreach (Text t in MainMenu)
                    {
                        spriteBatch.DrawString(t.Font, t.Words, t.Position, t.FontColor);
                    }
                    spriteBatch.Draw(AIAppearance, CursorPosition, SwitchSrc, Color.White, 0, Center, 1, SpriteEffects.None, 0);

                    //Instructions
                    #region Instructions
                    FontOffset.X = graphics.PreferredBackBufferWidth - ScreenFont.MeasureString("+ To move up/down, Press Up/Down Arrow Keys    ").X;
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To move up/down, Press Up/Down Arrow Keys", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To Accept, Press Enter", FontOffset, Color.White);
                    #endregion
                    break;
                case GameStates.NumSetup:
                    GraphicsDevice.Clear(NumSetupColor);
                    OffsetFactor = 0;
                    #region NumSetup
                    for (int i = 0; i < NumSetupMenu.Count; i++)
                    {
                        switch (i)
                        {
                            case 0://ε Choice
                                spriteBatch.DrawString(ScreenFont, NumSetupMenu[i].Words + ": " +
                                    (input.Length == 0 || UserChoice != i ? EPSILON_OPTIMA.ToString() : input.ToString()),
                                    NumSetupMenu[i].Position, NumSetupMenu[i].FontColor);
                                break;
                            case 1://ε Decay
                                spriteBatch.DrawString(ScreenFont, NumSetupMenu[i].Words + ": " +
                                    (input.Length == 0 || UserChoice != i ? EPSILON_DECAY.ToString() : input.ToString()),
                                    NumSetupMenu[i].Position, NumSetupMenu[i].FontColor);
                                break;
                            case 2://α
                                spriteBatch.DrawString(ScreenFont, NumSetupMenu[i].Words + ": " +
                                    (input.Length == 0 || UserChoice != i ? ALPHA.ToString() : input.ToString()),
                                    NumSetupMenu[i].Position, NumSetupMenu[i].FontColor);
                                break;
                            case 3://Gamma
                                spriteBatch.DrawString(ScreenFont, NumSetupMenu[i].Words + ": " +
                                    (input.Length == 0 || UserChoice != i ? GAMMA.ToString() : input.ToString()),
                                    NumSetupMenu[i].Position, NumSetupMenu[i].FontColor);
                                break;
                            case 4://Lambda
                                spriteBatch.DrawString(ScreenFont, NumSetupMenu[i].Words + ": " +
                                    (input.Length == 0 || UserChoice != i ? LAMBDA.ToString() : input.ToString()),
                                    NumSetupMenu[i].Position, NumSetupMenu[i].FontColor);
                                break;
                            case 5://Reward
                                spriteBatch.DrawString(ScreenFont, NumSetupMenu[i].Words + ": " +
                                    (input.Length == 0 || UserChoice != i ? REWARD.ToString() : input.ToString()),
                                    NumSetupMenu[i].Position, NumSetupMenu[i].FontColor);
                                break;
                        }
                    }
                    #endregion
                    spriteBatch.Draw(AIAppearance, CursorPosition, SwitchSrc, Color.White, 0, Center, 1, SpriteEffects.None, 0);
                    //Instructions
                    #region Instructions
                    FontOffset.X = graphics.PreferredBackBufferWidth - ScreenFont.MeasureString("+ Use keyboard numbers or the numpad to enter values    ").X;
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To move down, press Tab", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To move up, press Left Shift + Tab", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ Use keyboard numbers or the numpad to enter values", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To move to the next screen, press Enter", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To move back on screen, press Escape", FontOffset, Color.White);
                    #endregion
                    break;
                case GameStates.LvLSetup:
                    GraphicsDevice.Clear(LevelSetupColor);
                    OffsetFactor = 0;
                    StaticMaps[UserChoice].DrawMap(false, false);
                    spriteBatch.DrawString(ScreenFont, StaticMaps[UserChoice].LvLFilename, Vector2.Zero, Color.White);

                    //Instructions
                    #region Instructions
                    FontOffset.X = graphics.PreferredBackBufferWidth - ScreenFont.MeasureString("+ To cycle through maps, Press Up/Down Arrow Keys    ").X;
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To cycle through maps, Press Up/Down Arrow Keys", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To Accept, Press Enter", FontOffset, Color.White);
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To move back, press Escape", FontOffset, Color.White);
                    #endregion

                    break;
                case GameStates.Instructions:
                    FontOffset.X = (graphics.PreferredBackBufferWidth / 2) - (ScreenFont.MeasureString(
                        "I really can't think of anything to put here. Press Enter to go back...go back now.").X / 2);
                    FontOffset.Y = (graphics.PreferredBackBufferHeight / 2) - (ScreenFont.MeasureString(" ").Y / 2) - 10;
                    spriteBatch.DrawString(ScreenFont, "I really can't think of anything to put here. Press Enter to go back...go back now.", FontOffset, Color.White);
                    break;
                case GameStates.Game:
                    GraphicsDevice.Clear(GameColor);
                    OffsetFactor = 0;

                    FontOffset.X = graphics.PreferredBackBufferWidth - ScreenFont.MeasureString(
                        "+ To reset EVERYTHING, press F12. Caution, and exporting, is advised.    ").X;
                    FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                    spriteBatch.DrawString(ScreenFont, "+ To toggle key function instructions, press F1", FontOffset, Color.White);
                    if (ShowInstructions)
                    {
                        //Instructions
                        #region Instructions
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To toggle the arrows above the map, press F2", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To toggle relative arrow displacement and coloration, press F3", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To import a QER table, press F4", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To gently reset the Labrat, press F5", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To export the QER table, press F6", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To export the Trial Information, press F7", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ To reset EVERYTHING, press F12. Caution, and exporting, is advised.", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Use WASD keys to scroll the map", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ When the Labrat isn't running, use IJKL to move him", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Use Tab to show the map's metrics", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Use Left/Right Arrow Keys to change the number of automatic trials", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Use Add/Subtract on the numberpad to zoom", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Press Enter to start/stop the Labrat", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Press Space to collapse all the walls", FontOffset, Color.White);
                        FontOffset.Y = ScreenFont.MeasureString(" ").Y * OffsetFactor++;
                        spriteBatch.DrawString(ScreenFont, "+ Press Escape to return to the map selection screen", FontOffset, Color.White);
                        #endregion
                    }
                    break;
                case GameStates.Exit:
                    GraphicsDevice.Clear(ExitColor);
                    OffsetFactor = 0;
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                    spriteBatch.Draw(AIAppearance, Vector2.Zero, new Rectangle(270, 75, 54, 75), new Color(255, 255, 255, 175), 0, Vector2.Zero, 22, SpriteEffects.None, 0);
                    spriteBatch.End();
                    spriteBatch.Begin();
                    #region Text!!!
                    FontOffset.X = (graphics.PreferredBackBufferWidth / 2) -
                        (ScreenFont.MeasureString(
                        "Thanks for playing, or messing with, this program. It took a lot of time" +
                        " and Labrat loves you for it. He has eaten a lot of cheese thanks to you.").X / 2);
                    FontOffset.Y = (graphics.PreferredBackBufferHeight / 2) - (ScreenFont.MeasureString(" ").Y / 2) - 10;
                    spriteBatch.DrawString(ScreenFont, "Thanks for playing, or messing with, this program. It took a" +
                        " lot of time and Labrat loves you for it. He has eaten a lot of cheese thanks to you.", FontOffset, Color.White);

                    FontOffset.X = (graphics.PreferredBackBufferWidth / 2) -
                        (ScreenFont.MeasureString("Author: Sage Kelly").X / 2);
                    FontOffset.Y = (graphics.PreferredBackBufferHeight / 2) + (ScreenFont.MeasureString(" ").Y) - 10;
                    spriteBatch.DrawString(ScreenFont, "Author: Sage Kelly", FontOffset, Color.White);
                    FontOffset.X = (graphics.PreferredBackBufferWidth / 2) -
                        (ScreenFont.MeasureString("April - May 2014").X / 2);
                    FontOffset.Y = (graphics.PreferredBackBufferHeight / 2) + (ScreenFont.MeasureString(" ").Y) * 2 - 10;
                    spriteBatch.DrawString(ScreenFont, "April - May 2014", FontOffset, Color.White);
                    FontOffset.X = (graphics.PreferredBackBufferWidth / 2) -
                        (ScreenFont.MeasureString("AI: We Don't Know What It Means!").X / 2);
                    FontOffset.Y = (graphics.PreferredBackBufferHeight / 2) + (ScreenFont.MeasureString(" ").Y) * 3 - 10;
                    spriteBatch.DrawString(ScreenFont, "AI: We Don't Know What It Means!", FontOffset, Color.White);
                    FontOffset.X = (graphics.PreferredBackBufferWidth / 2) -
                        (ScreenFont.MeasureString("Teacher: Dr. M. Franklin").X / 2);
                    FontOffset.Y = (graphics.PreferredBackBufferHeight / 2) + (ScreenFont.MeasureString(" ").Y) * 4 - 10;
                    spriteBatch.DrawString(ScreenFont, "Teacher: Dr. M. Franklin", FontOffset, Color.White);
                    #endregion
                    break;
            }
        }

        public void CalculateColorSlideRate()
        {
            RDiff = GoalColor.R - InterimColor.R;
            GDiff = GoalColor.G - InterimColor.G;
            BDiff = GoalColor.B - InterimColor.B;

            RDelta = (byte)(RDiff / fps);
            if (RDelta == 0)
            {
                RDelta = 1;
            }
            GDelta = (byte)(GDiff / fps);
            if (GDelta == 0)
            {
                GDelta = 1;
            }
            BDelta = (byte)(BDiff / fps);
            if (BDelta == 0)
            {
                BDelta = 1;
            }
            ColorTimeout = false;
        }

        public void Colorslide(GameTime gameTime)
        {
            if (!ColorTimeout)
            {
                ColorTimer += gameTime.ElapsedGameTime;
                TurnOffTimer += gameTime.ElapsedGameTime;

                if (ColorTimer.TotalMilliseconds >= FrameMillis)
                {
                    if (RDiff > 0 && InterimColor.R < GoalColor.R)
                        InterimColor.R += RDelta;
                    else
                        InterimColor.R -= RDelta;

                    if (GDiff > 0 && InterimColor.G < GoalColor.G)
                        InterimColor.G += GDelta;
                    else
                        InterimColor.G -= GDelta;

                    if (BDiff > 0 && InterimColor.B < GoalColor.B)
                        InterimColor.B += BDelta;
                    else
                        InterimColor.B -= BDelta;

                    ColorTimer = TimeSpan.Zero;
                }
                if (TurnOffTimer.TotalMilliseconds >= turnoffMilli)
                {
                    InterimColor = GoalColor;
                    ColorTimer = TimeSpan.Zero;
                    TurnOffTimer = TimeSpan.Zero;
                    ColorTimeout = true;
                }
            }
        }
    }
}