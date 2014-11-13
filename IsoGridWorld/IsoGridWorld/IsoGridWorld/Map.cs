using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

namespace IsoGridWorld
{
    class Map : DrawableGameComponent
    {
        public struct Dimension
        {
            public int Width;
            public int Length;
            public int Depth;

            public Dimension(int width, int length, int depth)
            {
                Width = width;
                Length = length;
                Depth = depth;
            }
        }

        public struct TrialInfo
        {
            public int StepsTaken;
            public Tile.Location StartingTile;

            public TrialInfo(int steps, Tile.Location startingTile)
            {
                StepsTaken = steps;
                StartingTile = startingTile;
            }
        }

        internal class Association
        {
            public int From;
            public int To;

            public Association(int from, int to)
            {
                From = from;
                To = to;
            }
        }

        #region Variables
        #region Graphics Stuff
        /// <summary>
        /// List of all the used tiles in the map
        /// </summary>
        public List<Tile> Tiles;

        public List<Image> Images;


        /// <summary>
        /// Holds a list of all the tiles on which the AI CAN'T walk
        /// </summary>
        private List<Tile> BlockingTiles;

        /// <summary>
        /// Holds a list of all the tiles on which the AI can walk
        /// </summary>
        private List<Tile> WalkingTiles;

        private Vector2 MapFootprint;

        SpriteBatch spriteBatch;

        SoundEffect Beep;
        SpriteFont Font;
        #endregion
        #region I/O Stuff
        /// <summary>
        /// Holds the room width
        /// </summary>
        int arr_width;
        /// <summary>
        /// Holds the room height
        /// </summary>
        int arr_height;
        /// <summary>
        /// Holds the map depth
        /// </summary>
        int arr_depth;

        /// <summary>
        /// Holds the id for a wall
        /// </summary>
        List<int> WallIDs;

        List<Association> WallAsos;

        /// <summary>
        /// Holds the ID for the cheese
        /// </summary>
        int CheeseID;

        StreamReader LevelReader;
        string line;
        string[] tokens;
        char delim = ',';
        Tile[, ,] tiles;

        private string str_pic_path;
        public string LevelPath
        {
            get;
            private set;
        }
        public string LvLFilename
        {
            get;
            private set;
        }
        //TODO: For Paper: Adding more walls make the AI work harder

        public bool DisplayMetrics;

        StreamWriter QERWriter;
        StreamReader QERReader;
        #endregion
        #region Map Stuff
        Game gameLoaded;

        /// <summary>
        /// Keeps track of the length of each trial in steps
        /// </summary>
        private List<TrialInfo> Trials;

        /// <summary>
        /// Keeps track of every move the AI makes.
        /// </summary>
        public List<Direction> Trail;

        /// <summary>
        /// Holds the tile the LabRat previously visited
        /// </summary>
        private Tile PrevTile;

        /// <summary>
        /// Holds the AI's starting tile
        /// </summary>
        private Tile StartTile;

        /// <summary>
        /// Holds the AI's surrounding wall tiles. Used for "collapsing" them
        /// </summary>
        public Tile[] WallTiles;

        /// <summary>
        /// Holds the CheeseTile's surrounding wall tiles. Used for "collapsing" them
        /// </summary>
        public Tile[] CheeseWallTiles;

        /// <summary>
        /// Holds the Cheese Tile
        /// </summary>
        private Tile CheeseTile;

        public Dimension MapSize;

        public Vector2 offset;

        TimeSpan Timer;

        /// <summary>
        /// Denotes whether or not LabRat is running the maze
        /// </summary>
        public bool IsRunning = false;

        /// <summary>
        /// Denotes whether or not all the wall blocks have been flattened.
        /// </summary>
        public bool Flattened = false;

        /// <summary>
        /// Denotes whether or not the directional bounds for the AI has been set
        /// </summary>
        bool BoundsSet = false;

        /// <summary>
        /// Denotes whether or not this componenet is active
        /// </summary>
        public bool IsActive = false;

        public int DELAY
        {
            get;
            private set;
        }

        float AverageTrialLength = 0.0f;

        /// <summary>
        /// Whether or not the AI will reset to a random position
        /// </summary>
        private bool RandomReset = true;

        /// <summary>
        /// Denotes how many times the AI runs before it stops
        /// </summary>
        private int LoopAmount;

        /// <summary>
        /// The unchanging version of RunTime
        /// </summary>
        private int StaticLoopAmount;

        /// <summary>
        /// Acts as a zoom for the screen
        /// </summary>
        int ViewSize = 1;


        public bool DrawArrows = true, DrawArrowOffset = false, FadeArrows = false;

        public float ArrowFadePerc = 0.0f;

        const int i_arrow_offset_factor = 32;

        Texture2D Arrows;

        Vector2 ArrowOffset, IndieArrowOffset = Vector2.Zero;
        Vector2 ArrowCenter = new Vector2(14, 14);

        Rectangle DownArrowBounds = new Rectangle(0, 0, 29, 29);
        Rectangle RightArrowBounds = new Rectangle(29, 0, 29, 29);
        Rectangle UpArrowBounds = new Rectangle(58, 0, 29, 29);
        Rectangle LeftArrowBounds = new Rectangle(87, 0, 29, 29);

        private bool b_outside_movement = false;
        #endregion
        #region AI Stuff
        AI LabRat;

        /// <summary>
        /// The version of the Reward that doesn't change by lambda
        /// </summary>
        float StaticReward;

        /// <summary>
        /// The reward for wining the cheese
        /// </summary>
        float Reward;

        /// <summary>
        /// For QER table-writing purposes
        /// </summary>
        private float EpsilonOptima;

        /// <summary>
        /// For QER table-writing purposes
        /// </summary>
        private float EpsilonDecay;

        /// <summary>
        /// Holds the alpha for SARSA
        /// </summary>
        private float Alpha;

        /// <summary>
        /// Holds the gamma for SARSA
        /// </summary>
        private float Gamma;

        /// <summary>
        /// The reduction rate of the reward as it trickles back to the starting tile.
        /// </summary>
        float Lambda;
        #endregion
        #endregion

        /// <summary>
        /// Creates a smart map that records the AI's movement
        /// </summary>
        /// <param name="game">The game in which this map will be used</param>
        /// <param name="delay">The delay for the thinking/animation timer</param>
        /// <param name="alpha">alpha for SARSA lambda</param>
        /// <param name="gamma">gamma for SARSA lambda</param>
        /// <param name="lambda">lambda for SARSA lambda</param>
        /// <param name="epsilonOptima">The decimal percentage of the AI choosing the best piece</param>
        /// <param name="epsilonDecay">The reduction rate of the reward as the heuristic is made</param>
        ///<param name="reward">The amount of reward given for finding the cheese</param>
        /// <param name="AIPicPath">The path containing the AI's sprites</param>
        /// <param name="TilePath">The path containing the Tiles for the map</param>
        /// <param name="levelPath">The path containing the level .txt file</param>
        public Map(Game game, int delay, float alpha, float gamma, float lambda, float epsilonOptima,
            float epsilonDecay, float reward, string AIPicPath, string TilePath, string levelPath)
            : base(game)
        {
            IsActive = false;
            DisplayMetrics = false;
            gameLoaded = game;
            DELAY = delay;
            Alpha = alpha;
            Gamma = gamma;
            Lambda = lambda;
            EpsilonOptima = epsilonOptima;
            EpsilonDecay = epsilonDecay;
            StaticReward = Reward = reward;

            LabRat = new AI(epsilonOptima, epsilonDecay,
                Texture2D.FromStream(game.GraphicsDevice, new FileStream(AIPicPath, FileMode.Open)));
            str_pic_path = TilePath;
            LevelPath = levelPath;

            Images = ImageGetter.LoadXMLFile(TilePath, game);
            MapFootprint = ImageGetter.GetFootPrint(TilePath);


            LvLFilename = Path.GetFileName(LevelPath);

            WallIDs = new List<int>();
            Tiles = new List<Tile>();
            WalkingTiles = new List<Tile>();
            BlockingTiles = new List<Tile>();
            WallAsos = new List<Association>();
            WallTiles = new Tile[15];
            CheeseWallTiles = new Tile[15];
            ReadInMap();

            offset = Vector2.Zero;
            Trail = new List<Direction>();
            Trials = new List<TrialInfo>();
        }

        public Map(Game game, int delay, float alpha, float gamma, float lambda, float epsilonOptima,
            float epsilonDecay, float reward, Texture2D AIPic, string TilePath, string levelPath)
            : base(game)
        {
            IsActive = false;
            DisplayMetrics = false;
            gameLoaded = game;
            DELAY = delay;
            Alpha = alpha;
            Gamma = gamma;
            Lambda = lambda;
            EpsilonOptima = epsilonOptima;
            EpsilonDecay = epsilonDecay;
            StaticReward = Reward = reward;

            LabRat = new AI(epsilonOptima, epsilonDecay, AIPic);
            str_pic_path = TilePath;
            LevelPath = levelPath;

            Images = ImageGetter.LoadXMLFile(TilePath, game);
            MapFootprint = ImageGetter.GetFootPrint(TilePath);

            LvLFilename = Path.GetFileName(LevelPath);

            WallIDs = new List<int>();
            Tiles = new List<Tile>();
            WalkingTiles = new List<Tile>();
            WallAsos = new List<Association>();
            BlockingTiles = new List<Tile>();
            WallTiles = new Tile[15];
            CheeseWallTiles = new Tile[15];
            ReadInMap();

            offset = Vector2.Zero;
            Trail = new List<Direction>();
            Trials = new List<TrialInfo>();

            StaticLoopAmount = LoopAmount = 1;
        }

        #region GAME METHODS
        public override void Initialize()
        {
            base.Initialize();
            Timer = new TimeSpan();

            //Default offset assignments to get the map in the center of the screen
            offset.X = 630;
            offset.Y = 170;

            spriteBatch = new SpriteBatch(this.GraphicsDevice);


            //Assign random values to each valid tile
            ResetMap();

            //Randomly assigns the LabRat a position on the map
            RandomAssign();

            //Once assigned, his Center never changes
            LabRat.Center = new Vector2((LabRat.CurTile.Footprint.X / 2),
                LabRat.Appearance.Height - (LabRat.CurTile.Footprint.Y / 2));


            ArrowOffset = new Vector2(0, -LabRat.Appearance.Height - CheeseTile.Footprint.Y);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            //redpix = gameLoaded.Content.Load<Texture2D>("Red Pixel");
            Font = gameLoaded.Content.Load<SpriteFont>("Font");
            Arrows = gameLoaded.Content.Load<Texture2D>("Iso Arrow");
            Beep = gameLoaded.Content.Load<SoundEffect>("Square Hit");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleMapAndAI(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                DrawMap(true, DrawArrows);
                DrawHUD();
            }
            base.Draw(gameTime);
        }
        #endregion

        #region MAP METHODS
        #region I/O BASED

        /// <summary>
        /// Reads in the maps from the text file
        /// </summary>
        private void ReadInMap()
        {
            Tile Ttemp;
            ///width
            ///height
            ///depth
            ///wall tile IDs
            ///wall associations
            ///cheesetile ID
            ///map
            LevelReader = new StreamReader(LevelPath);
            //Read in Map specs
            #region Read in Map Specs
            arr_width = int.Parse(LevelReader.ReadLine());
            arr_height = int.Parse(LevelReader.ReadLine());
            arr_depth = int.Parse(LevelReader.ReadLine());

            //Wall tile IDs
            string walls = LevelReader.ReadLine();
            tokens = walls.Split(',');

            for (int i = 0; i < tokens.Length; i++)
            {
                WallIDs.Add(int.Parse(tokens[i]));
            }

            //Wall Associations
            string[] wall_associations = LevelReader.ReadLine().Split(',');
            for (int i = 0; i < wall_associations.Length; i++)
            {
                string[] wall_aso = wall_associations[i].Split(':');
                WallAsos.Add(new Association(int.Parse(wall_aso[0]), int.Parse(wall_aso[1])));
            }

            //Cheese ID
            CheeseID = int.Parse(LevelReader.ReadLine());

            ///At this point, we have the IDs for the wall tiles,
            ///the ID for the Cheese tile, all the images for
            ///the map, and all the wall associtations. We can
            ///use the image to make the tiles, with the IDs
            ///in the images to make the Tiles, but only when
            ///they are the floors or the UNcollapsed wall tiels.

            foreach (Image img in Images)
            {
                Association temp = WallAsos.FirstOrDefault<Association>(x => x.To == img.ID);
                if (temp == null)//i.e. If it's not a collapsed wall sprite...
                {
                    //...make it a tile
                    Tiles.Add(new Tile(img.Name, img.ID, img, img.SourceBounds, MapFootprint, img.Center));

                    foreach (Association aso in WallAsos)
                    {
                        //If it has an association...
                        if (img.ID == aso.From)
                        {
                            //..find the matching sprite, and add it to this tile
                            Tiles[Tiles.Count - 1].AddCollapsedSprite(Images.Find(x => x.ID == aso.To));
                        }
                    }
                }
            }

            MapSize = new Dimension(arr_width, arr_height, arr_depth);
            #endregion

            //Read in the map
            #region Read in the map
            tiles = new Tile[arr_depth, arr_height, arr_width];
            tokens = new string[arr_width];
            for (int depth = 0; depth < arr_depth; depth++)
            {
                for (int row = 0; row < arr_height; row++)
                {
                    line = LevelReader.ReadLine();
                    tokens = line.Split(delim);
                    for (int col = 0; col < arr_width; col++)
                    {
                        if (int.Parse(tokens[col]) != 0)
                        {
                            Ttemp = Tiles.Find(x => x.ID == int.Parse(tokens[col]));
                            tiles[depth, row, col] = new Tile(Ttemp.Name, Ttemp.ID, Ttemp.SwitchSprite, Ttemp.SrcBounds, Ttemp.Footprint, Ttemp.Center);
                            if (Ttemp.AltSet)
                                tiles[depth, row, col].AddCollapsedSprite(Ttemp.CollapsedTileSprite);

                            if (WallIDs.Contains(Ttemp.ID))
                            {
                                tiles[depth, row, col].IsWall = true;
                            }
                        }
                    }
                }
            }
            #endregion
            //Sets the intial positions of the tiles
            #region Sets the intial positions of the tiles
            for (int col = arr_width - 1; col >= 0; col--)
            {
                for (int row = 0; row < arr_height; row++)
                {
                    Ttemp = tiles[0, row, col];
                    if (Ttemp != null)
                    {
                        Ttemp.Position.X = ((MapFootprint.X / 2) + 1) * col;
                        Ttemp.Position.Y = ((MapFootprint.Y / 2)) * col;

                        Ttemp.Position.X -= ((MapFootprint.X / 2) + 1) * row;
                        Ttemp.Position.Y += (MapFootprint.Y / 2) * row;

                        Ttemp.MapLoc = new Tile.Location(row, col);

                        if (Ttemp.ID == CheeseID)
                        {
                            Ttemp.WinningTile = true;
                        }


                        if (!WallIDs.Contains(Ttemp.ID))//If its not a wall
                        {
                            //Add the walking list
                            WalkingTiles.Add(Ttemp);
                        }
                        else
                        {
                            BlockingTiles.Add(Ttemp);
                        }

                        //Amongst the walking tiles, find out which one is the cheese tile
                        if (Ttemp.ID == CheeseID)
                        {
                            CheeseTile = Ttemp;
                        }

                        #region Connects the pieces
                        if (row != 0 && tiles[0, row - 1, col] != null && !WallIDs.Contains(tiles[0, row - 1, col].ID))//Above
                        {
                            Ttemp.Above = tiles[0, row - 1, col];
                        }
                        else
                        {
                            Ttemp.SetValue(Direction.Up, 0.0f, false);
                        }

                        if (row != arr_height - 1 && tiles[0, row + 1, col] != null && !WallIDs.Contains(tiles[0, row + 1, col].ID))//Below
                        {
                            Ttemp.Below = tiles[0, row + 1, col];
                        }
                        else
                        {
                            Ttemp.SetValue(Direction.Down, 0.0f, false);
                        }

                        if (col != 0 && tiles[0, row, col - 1] != null && !WallIDs.Contains(tiles[0, row, col - 1].ID))//Left
                        {
                            Ttemp.Left = tiles[0, row, col - 1];
                        }
                        else
                        {
                            Ttemp.SetValue(Direction.Left, 0.0f, false);
                        }

                        if (col != arr_width - 1 && tiles[0, row, col + 1] != null && !WallIDs.Contains(tiles[0, row, col + 1].ID))//Right
                        {
                            Ttemp.Right = tiles[0, row, col + 1];
                        }
                        else
                        {
                            Ttemp.SetValue(Direction.Right, 0.0f, false);
                        }
                        #endregion
                    }
                }
            }
            #endregion
            LevelReader.Close();
            CheeseTile.Reward = StaticReward;
            CheeseTile.DirProbs[0].Utility = StaticReward;
            CheeseTile.DirProbs[1].Utility = StaticReward;
            CheeseTile.DirProbs[2].Utility = StaticReward;
            CheeseTile.DirProbs[3].Utility = StaticReward;
        }

        /// <summary>
        /// Imports the QER file
        /// </summary>
        public void ImportQER()
        {
            ///Example:
            ///EpsilonOptima, EpsilonDecay, Alpha, Gamma, Lambda, Reward
            ///.6,.01,.04,.02,.5,1.0
            ///3,4
            ///D,3.0004|U,0.0012|L,X|R,X|0.1250
            ///
            int index = LevelPath.LastIndexOf(".");
            string import_dir = LevelPath;
            import_dir = import_dir.Insert(index, " QER Values");
            if (File.Exists(import_dir))
            {
                QERReader = new StreamReader(import_dir);
                //Extra Readline for words
                QERReader.ReadLine();

                //Read in Metric Values
                string[] values = QERReader.ReadLine().Split(',');
                EpsilonOptima = float.Parse(values[0]);
                EpsilonDecay = float.Parse(values[1]);
                Alpha = float.Parse(values[2]);
                Gamma = float.Parse(values[3]);
                Lambda = float.Parse(values[4]);
                StaticReward = float.Parse(values[5]);

                //Extra Readline for the Cheese coordinates
                QERReader.ReadLine();
                //Extra Readline for Trial count
                QERReader.ReadLine();

                for (int j = 0; j < WalkingTiles.Count; j++)
                {
                    //Read in Map Number
                    string[] RowCol = QERReader.ReadLine().Split(',');
                    //Read in Q table and Reward
                    string[] DirProbs = QERReader.ReadLine().Split('|');
                    for (int i = 0; i < DirProbs.Length - 1; i++)
                    {
                        string[] Dir_Prob = DirProbs[i].Split(',');
                        switch (Dir_Prob[0])
                        {
                            case "U":
                                if (Dir_Prob[1] == "X")
                                    WalkingTiles[j].SetValue(Direction.Up, 0f, false);
                                else
                                    WalkingTiles[j].SetValue(Direction.Up, float.Parse(Dir_Prob[1]));
                                WalkingTiles[j].GetValue(Direction.Up).ResetTrodding();
                                break;
                            case "R":
                                if (Dir_Prob[1] == "X")
                                    WalkingTiles[j].SetValue(Direction.Right, 0f, false);
                                else
                                    WalkingTiles[j].SetValue(Direction.Right, float.Parse(Dir_Prob[1]));
                                WalkingTiles[j].GetValue(Direction.Right).ResetTrodding();
                                break;
                            case "D":
                                if (Dir_Prob[1] == "X")
                                    WalkingTiles[j].SetValue(Direction.Down, 0f, false);
                                else
                                    WalkingTiles[j].SetValue(Direction.Down, float.Parse(Dir_Prob[1]));
                                WalkingTiles[j].GetValue(Direction.Down).ResetTrodding();
                                break;
                            case "L":
                                if (Dir_Prob[1] == "X")
                                    WalkingTiles[j].SetValue(Direction.Left, 0f, false);
                                else
                                    WalkingTiles[j].SetValue(Direction.Left, float.Parse(Dir_Prob[1]));
                                WalkingTiles[j].GetValue(Direction.Left).ResetTrodding();
                                break;
                        }
                    }
                    WalkingTiles[j].Reward = float.Parse(DirProbs[DirProbs.Length - 1]);
                    WalkingTiles[j].ResetTrodding();
                }
                QERReader.Close();
                Reset();
            }
        }

        /// <summary>
        /// Writes the tiles' values to a file
        /// </summary>
        public void ExportQER()
        {
            int index = LevelPath.LastIndexOf(".");
            string export_dir = LevelPath;
            export_dir = export_dir.Insert(index, " QER Values");
            QERWriter = new StreamWriter(export_dir);
            #region Q/E/R
            //In this order: Epsilon Optima, Epsilon Decay, Alpha, Gamma, Lambda, Reward
            ///Example:
            ///EpsilonOptima, EpsilonDecay, Alpha, Gamma, Lambda, Reward
            ///.6,.01,.04,.02,.5,1.0
            ///Cheese Coordinates: 4,1
            ///3,4
            ///D,3.0004|U,0.0012|L,X|R,X|0.1250
            ///
            //Write down the order of the specs
            QERWriter.WriteLine("Epsilon Optima, Epsilon Decay, Alpha, Gamma, Lambda, Reward");
            //Write down the specs used for this table
            QERWriter.WriteLine("{0},{1},{2},{3},{4},{5}", EpsilonOptima, EpsilonDecay, Alpha, Gamma, Lambda, Reward);
            //Write down the cheese tile coordinates
            QERWriter.WriteLine("Cheese coordinates: {0},{1}", CheeseTile.MapLoc.Row, CheeseTile.MapLoc.Col);
            //Write down the number of trials taken
            QERWriter.WriteLine("Number of runs: {0}", Trials.Count);
            Tile Ticker;// = StartTile;
            for (int i = 0; i < WalkingTiles.Count; i++)
            {
                Ticker = WalkingTiles[i];
                QERWriter.WriteLine("{0},{1}", Ticker.MapLoc.Row, Ticker.MapLoc.Col);
                for (int j = 0; j < Ticker.DirProbs.Length; j++)
                {
                    QERWriter.Write("{0},{1}|",
                        Ticker.DirProbs[j].Dir.ToString()[0],//The first letter of the direction
                        //The utility of that action is the action is valid, else just an "X".
                        Ticker.DirProbs[j].IsValid ? Ticker.DirProbs[j].Utility.ToString() : "X");
                }
                QERWriter.WriteLine(Ticker.Reward);
                /*
                switch (WalkingTiles[i])
                {
                    case Direction.Left:
                        Ticker = Ticker.Left;
                        break;
                    case Direction.Up:
                        Ticker = Ticker.Above;
                        break;
                    case Direction.Right:
                        Ticker = Ticker.Right;
                        break;
                    case Direction.Down:
                        Ticker = Ticker.Below;
                        break;
                }
                */
            }
            QERWriter.WriteLine();
            QERWriter.Close();
            #endregion
        }

        /// <summary>
        /// Writes the tiles' values to a file
        /// </summary>
        public void ExportTrials()
        {
            int index = LevelPath.LastIndexOf(".");
            string export_dir = LevelPath;
            export_dir = export_dir.Insert(index, " Trial Runs");
            QERWriter = new StreamWriter(export_dir);

            ///Example:
            ///Level: Level 0.txt
            ///Width: 20, Height: 20
            ///Cheese Tile location: 8,6
            ///Number of Trials: 900
            ///Average Step per trial: 345
            ///2,4: 278
            ///...
            ///
            //Write down the name of the level
            QERWriter.WriteLine("Level: {0}", LvLFilename);
            //Write down the maps specs
            QERWriter.WriteLine("Width: {0}, Height {1}", arr_width, arr_height);
            //Write down the Cheese tile location
            QERWriter.WriteLine("Cheese Tile location: {0},{1}", CheeseTile.MapLoc.Row, CheeseTile.MapLoc.Col);
            //Write down the number of trials taken
            QERWriter.WriteLine("Number of runs: {0}", Trials.Count);
            //Write down the Average step per trial
            QERWriter.WriteLine("Average Step per trial: {0}", AverageTrialLength);

            for (int i = 0; i < Trials.Count; i++)
            {
                QERWriter.WriteLine("Tile: {0}, {1}; Length: {2}",
                    Trials[i].StartingTile.Row, Trials[i].StartingTile.Col, Trials[i].StepsTaken);
            }

            QERWriter.WriteLine();
            QERWriter.Close();
        }
        #endregion

        #region INITIALIZATION BASED

        /// <summary>
        /// Resets every tile completely, and randomizes their Q tables.
        /// </summary>
        private void ResetMap()
        {
            foreach (Tile t in WalkingTiles)
            {
                t.ResetTrodding();
                t.IsOccupied = false;
                t.Reward = 0;

                Random r = new Random(126578915);
                Random r1 = new Random(481247254);
                Random r2 = new Random(997213494);
                Random r3 = new Random(764750064);
                float result = 0.0f;
                ///These will be null if they are walls, so I can
                ///use that to update the correct directions.

                for (int i = 0; i < 4; i++)
                {
                    result = (float)(Math.Round(r.NextDouble() % .001f, 4));
                    if (t.Above != null)
                    {
                        t.SetValue(Direction.Up, result);
                    }

                    result = (float)(Math.Round(r1.NextDouble() % .001f, 4));
                    if (t.Right != null)
                    {
                        t.SetValue(Direction.Right, result);
                    }

                    result = (float)(Math.Round(r2.NextDouble() % .001f, 4));
                    if (t.Below != null)
                    {
                        t.SetValue(Direction.Down, result);
                    }

                    result = (float)(Math.Round(r3.NextDouble() % .001f, 4));
                    if (t.Left != null)
                    {
                        t.SetValue(Direction.Left, result);
                    }

                }
            }
            CollectSurroundingWalls(ref CheeseWallTiles, CheeseTile.MapLoc.Row, CheeseTile.MapLoc.Col);
        }

        #region Old Ridiculous Reset
        /*
        /// <summary>
        /// Resets every tile completely, and randomizes their Q tables.
        /// </summary>
        private void ResetMap2()
        {
            foreach (Tile t in WalkingTiles)
            {
                t.ResetTrodding();
                t.IsOccupied = false;
                t.Reward = 0;

                Random r = new Random(126578915);
                Random r1 = new Random(481247254);
                Random r2 = new Random(997213494);
                Random r3 = new Random(764750064);
                float result = 0.0f;
                DirProb[] Dirs = new DirProb[t.DirProbs.Length];
                //First, Randomly pick an order
                for (int index = 0; index < Dirs.Length; index++)
                {
                    Dirs[index] = new DirProb();
                }
                for (int index = 0; index < Dirs.Length; index++)
                {
                    int dir_res = r.Next(2 * Dirs.Length) % Dirs.Length;
                    bool ValidDir = false;
                    while (!ValidDir)
                    {
                        DirProb Result;
                        switch (dir_res)
                        {
                            case 0:
                                Result = Dirs.FirstOrDefault(x => x.Dir == Direction.Down);
                                if (Result == null)
                                {
                                    Dirs[index].Dir = Direction.Down;
                                    if (t.Below == null)
                                        Dirs[index].IsValid = false;
                                    else
                                        Dirs[index].IsValid = true;
                                    ValidDir = true;
                                }
                                else
                                    dir_res = (dir_res + 1) % Dirs.Length;
                                break;
                            case 1:
                                Result = Dirs.FirstOrDefault<DirProb>(x => x.Dir == Direction.Right);
                                if (Result == null)
                                {
                                    Dirs[index].Dir = Direction.Right;
                                    if (t.Right == null)
                                        Dirs[index].IsValid = false;
                                    else
                                        Dirs[index].IsValid = true;
                                    ValidDir = true;
                                }
                                else
                                    dir_res = (dir_res + 1) % Dirs.Length;
                                break;
                            case 2:
                                Result = Dirs.FirstOrDefault(x => x.Dir == Direction.Up);
                                if (Result == null)
                                {
                                    Dirs[index].Dir = Direction.Up;
                                    if (t.Above == null)
                                        Dirs[index].IsValid = false;
                                    else
                                        Dirs[index].IsValid = true;
                                    ValidDir = true;
                                }
                                else
                                    dir_res = (dir_res + 1) % Dirs.Length;
                                break;
                            case 3:
                                Result = Dirs.FirstOrDefault(x => x.Dir == Direction.Left);
                                if (Result == null)
                                {
                                    Dirs[index].Dir = Direction.Left;
                                    if (t.Left == null)
                                        Dirs[index].IsValid = false;
                                    else
                                        Dirs[index].IsValid = true;
                                    ValidDir = true;
                                }
                                else
                                    dir_res = (dir_res + 1) % Dirs.Length;
                                break;
                        }
                    }
                }
                ///These will be null if they are walls, so I can
                ///use that to update the correct directions.

                for (int i = 0; i < 4; i++)
                {
                    result = (float)(Math.Round(r.NextDouble() % .001f, 4));
                    if (Dirs[i].IsValid)
                    {
                        t.SetValue(Dirs[i].Dir, result);
                    }

                    result = (float)(Math.Round(r1.NextDouble() % .001f, 4));
                    if (Dirs[i].IsValid)
                    {
                        t.SetValue(Dirs[i].Dir, result);
                    }

                    result = (float)(Math.Round(r2.NextDouble() % .001f, 4));
                    if (Dirs[i].IsValid)
                    {
                        t.SetValue(Dirs[i].Dir, result);
                    }

                    result = (float)(Math.Round(r3.NextDouble() % .001f, 4));
                    if (Dirs[i].IsValid)
                    {
                        t.SetValue(Dirs[i].Dir, result);
                    }

                }
            }
        }
        */
        #endregion

        /// <summary>
        /// Resets everything...EVERYTHING
        /// </summary>
        public void HardReset()
        {
            Reset(true);
        }
        //TODO: (Optional) make an interface that allows for the design and saving of maps

        /// <summary>
        /// Resets the map and places the AI in a new position
        /// </summary>
        public void Reset(bool Hard = false)
        {
            /*
             * What to Reset:
             * AI's values
             * AI's Position
             * AI Completely
             * Map's Rewards
             * Map Completely
             */
            #region Happens Regardless
            LabRat.CurTile.IsOccupied = true;
            PrevTile = null;
            Trail.Clear();
            if (!Flattened)
            {
                ResetWalls();
            }
            Reward = StaticReward;
            #endregion
            #region Hard Reset-specific
            if (Hard)
            {
                LabRat.EditValues(EpsilonOptima, EpsilonDecay);
                IsRunning = false;
                LoopAmount = StaticLoopAmount = 1;
                DELAY = 1000;
                ResetMap();
                LabRat.Reset();
                if (RandomReset)
                {
                    RandomAssign();
                }
                else
                {
                    LabRat.CurTile = StartTile;
                }
                Trials.Clear();
                AverageTrialLength = 0;
            }
            #endregion
            #region Gentle Reset-specific
            else
            {
                LabRat.Reset();
                if (RandomReset)
                {
                    RandomAssign();
                }
                else
                {
                    LabRat.CurTile = StartTile;
                }
                if (LoopAmount == 1)
                    IsRunning = false;
                else if (StaticLoopAmount > 1 && LoopAmount > 1)
                {
                    LoopAmount--;
                }
            }
            #endregion
            CheeseTile.Reward = StaticReward;
            CheeseTile.DirProbs[0].Utility = StaticReward;
            CheeseTile.DirProbs[1].Utility = StaticReward;
            CheeseTile.DirProbs[2].Utility = StaticReward;
            CheeseTile.DirProbs[3].Utility = StaticReward;

        }

        /// <summary>
        /// Sets the sourceBounds for the AI when turning in the game
        /// </summary>
        /// <param name="LeftSource">For when the AI turns left</param>
        /// <param name="RightSource">For when the AI turns right</param>
        /// <param name="UpSource">For when the AI turns up</param>
        /// <param name="DownSource">For when the AI turns down</param>
        public void SetAISourceBounds(Rectangle LeftSource, Rectangle UpSource, Rectangle RightSource, Rectangle DownSource)
        {
            LabRat.LeftSrcBounds = LeftSource;
            LabRat.RightSrcBounds = RightSource;
            LabRat.UpSrcBounds = UpSource;
            LabRat.DownSrcBounds = DownSource;
            BoundsSet = true;
        }
        #endregion

        #region DRAW BASED
        /// <summary>
        /// Draws everything within the map
        /// </summary>
        /// <param name="drawAI">Dictates whether or not to draw the Labrat within the map</param>
        public void DrawMap(bool drawAI = true, bool drawArrows = true)
        {
            DrawArrows = drawArrows;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
               SamplerState.PointClamp, null, null);
            for (int height = arr_depth - 1; height > -1; height--)
            {
                for (int col = 0; col < arr_width; col++)
                {
                    for (int row = 0; row < arr_height; row++)
                    {
                        //Tile
                        Tile temp = tiles[height, row, col];
                        if (temp != null)
                        {
                            spriteBatch.Draw(temp.SwitchSprite.Avatar, (temp.Position + offset) * ViewSize,
                                 temp.SwitchSprite.SourceBounds, temp.DrawingColor, 0, temp.SwitchSprite.Center, ViewSize,
                                 SpriteEffects.None, 0);

                            //Arrow
                            #region Draw Arrows
                            if (!temp.IsWall && DrawArrows)
                            {
                                DirProb BestDP = temp.GetValidTiles()[0];

                                Color ArrowColor = Color.White;

                                if (temp.ID == CheeseID)
                                    ArrowColor = Color.DarkOrange;
                                else if (temp.Reward > 0)
                                    ArrowColor = Color.CadetBlue;
                                else if ((temp.Below != null && temp.Below.Reward > 0) ||
                                    (temp.Right != null && temp.Right.Reward > 0) ||
                                    (temp.Above != null && temp.Above.Reward > 0) ||
                                    (temp.Left != null && temp.Left.Reward > 0))
                                    ArrowColor = Color.Yellow;

                                IndieArrowOffset.Y = temp.Reward * i_arrow_offset_factor;
                                if (DrawArrowOffset)
                                {
                                    ArrowColor.R = (byte)(ArrowColor.R * temp.Reward);
                                    ArrowColor.G = (byte)(ArrowColor.G * temp.Reward);
                                    ArrowColor.B = (byte)(ArrowColor.B * temp.Reward);
                                }

                                if (b_outside_movement && temp == PrevTile && !IsRunning)
                                    ArrowColor = Color.BlueViolet;

                                if (temp.IsOccupied)
                                {
                                    if (!b_outside_movement)
                                        ArrowColor = Color.BlueViolet;
                                    else
                                        ArrowColor = Color.OrangeRed;
                                }

                                switch (BestDP.Dir)
                                {
                                    case Direction.Down:
                                        spriteBatch.Draw(Arrows, (temp.Position + ArrowOffset + offset +
                                            (DrawArrowOffset ? IndieArrowOffset : Vector2.Zero)) * ViewSize, DownArrowBounds,
                                            ArrowColor, 0, ArrowCenter, ViewSize, SpriteEffects.None, 0);
                                        break;
                                    case Direction.Right:
                                        spriteBatch.Draw(Arrows, (temp.Position + ArrowOffset + offset +
                                            (DrawArrowOffset ? IndieArrowOffset : Vector2.Zero)) * ViewSize, RightArrowBounds,
                                            ArrowColor, 0, ArrowCenter, ViewSize, SpriteEffects.None, 0);
                                        break;
                                    case Direction.Up:
                                        spriteBatch.Draw(Arrows, (temp.Position + ArrowOffset + offset +
                                            (DrawArrowOffset ? IndieArrowOffset : Vector2.Zero)) * ViewSize, UpArrowBounds,
                                            ArrowColor, 0, ArrowCenter, ViewSize, SpriteEffects.None, 0);
                                        break;
                                    case Direction.Left:
                                        spriteBatch.Draw(Arrows, (temp.Position + ArrowOffset + offset +
                                            (DrawArrowOffset ? IndieArrowOffset : Vector2.Zero)) * ViewSize, LeftArrowBounds,
                                            ArrowColor, 0, ArrowCenter, ViewSize, SpriteEffects.None, 0);
                                        break;
                                    default:
                                        spriteBatch.Draw(Arrows, (temp.Position + ArrowOffset + offset +
                                            (DrawArrowOffset ? IndieArrowOffset : Vector2.Zero)) * ViewSize, DownArrowBounds,
                                            ArrowColor, 0, ArrowCenter, ViewSize, SpriteEffects.None, 0);
                                        break;
                                }
                            }
                            #endregion
                            //AI, when available
                            if (temp.IsOccupied && drawAI)
                            {
                                if (BoundsSet)
                                {
                                    switch (LabRat.MovingDirection)
                                    {
                                        case Direction.Right:
                                            spriteBatch.Draw(LabRat.Appearance, (LabRat.Position + offset) * ViewSize,
                                       LabRat.RightSrcBounds, Color.White, 0, LabRat.Center, ViewSize,
                                       SpriteEffects.None, 0);
                                            break;
                                        case Direction.Down:
                                            spriteBatch.Draw(LabRat.Appearance, (LabRat.Position + offset) * ViewSize,
                                       LabRat.DownSrcBounds, Color.White, 0, LabRat.Center, ViewSize,
                                       SpriteEffects.None, 0);
                                            break;
                                        case Direction.Left:
                                            spriteBatch.Draw(LabRat.Appearance, (LabRat.Position + offset) * ViewSize,
                                       LabRat.LeftSrcBounds, Color.White, 0, LabRat.Center, ViewSize,
                                       SpriteEffects.None, 0);
                                            break;
                                        case Direction.Up:
                                            spriteBatch.Draw(LabRat.Appearance, (LabRat.Position + offset) * ViewSize,
                                       LabRat.UpSrcBounds, Color.White, 0, LabRat.Center, ViewSize,
                                       SpriteEffects.None, 0);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Draws all the metrics for the map
        /// </summary>
        public void DrawHUD()
        {
            spriteBatch.Begin();
            if (DisplayMetrics)
            {
                int factor = 0;
                //int End = 1;

                spriteBatch.DrawString(Font, "Level Filename: " + LvLFilename, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);

                spriteBatch.DrawString(Font, "", new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);

                spriteBatch.DrawString(Font, "AI's Epsilon Optima: " + LabRat.Optima, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "AI's Epsilon Decay: " + LabRat.Decay, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Alpha: " + Alpha, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Gamma: " + Gamma, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Lambda: " + Lambda, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Main Reward: " + Reward, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);

                spriteBatch.DrawString(Font, "", new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);

                spriteBatch.DrawString(Font, "AI's delay: " + DELAY, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Steps Taken: " + Trail.Count, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Average Step Size: " + AverageTrialLength.ToString(), new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Trials Run: " + Trials.Count, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "Trials Left: " + LoopAmount, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                /*
                spriteBatch.DrawString(Font, "Tiles's location : " + LabRat.CurTile.MapLoc.Row + ", " + LabRat.CurTile.MapLoc.Col,
                    new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                spriteBatch.DrawString(Font, "AI's Direction: " + LabRat.MovingDirection, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                */
                spriteBatch.DrawString(Font, "Space Reward: " + LabRat.CurTile.Reward, new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);

                /*
                int index = 0;
                while (index != 10 && index != Trials.Count)
                {
                    spriteBatch.DrawString(Font,
                        "Trial " + (index + 1) + ": " + Trials[index].StepsTaken + " Starting tile: " +
                        Trials[index].StartingTile.Row + ", " + Trials[index].StartingTile.Col,
                        new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                    index++;
                }
                
                while (End <= Trail.Count && End != 6)
                {
                    spriteBatch.DrawString(Font, "Trail[ " + (Trail.Count - End) + " ]: " + Trail[Trail.Count - End].ToString(),
                        new Vector2(0, Font.MeasureString(" ").Y * factor++), Color.White);
                    End++;
                }
                */
                for (int i = 0; i < LabRat.CurTile.DirProbs.Length; i++)
                {
                    DirProb temp = LabRat.CurTile.DirProbs[i];

                    if (temp.IsValid)
                    {
                        spriteBatch.DrawString(Font, "Dir: " + temp.Dir + "; Utility: " + temp.Utility + "|Times Trod: " + temp.TrodNumber,
                            new Vector2(0, (Font.MeasureString(" ").Y) * (i + 1) + (Font.MeasureString(" ").Y * factor)), temp.DrawColor);
                    }
                    else
                    {
                        spriteBatch.DrawString(Font, "Dir: " + temp.Dir + "; Utility: " + temp.Utility + "|Times Trod: " + temp.TrodNumber,
                            new Vector2(0, (Font.MeasureString(" ").Y) * (i + 1) + (Font.MeasureString(" ").Y * factor)), Color.Gray);
                    }
                }
            }
            spriteBatch.End();
        }

        #endregion

        #region EDIT/UPDATE BASED
        /// <summary>
        /// Moves the Labrat in the particular direction, if it is a valid move
        /// </summary>
        /// <param name="D">The direction of choice</param>
        public void MoveLabrat(Direction D)
        {
            if (!IsRunning)
            {
                if (!b_outside_movement)
                {
                    PrevTile = LabRat.CurTile;
                }
                switch (D)
                {
                    case Direction.Down:
                        if (LabRat.CurTile.Below != null)
                        {
                            LabRat.CurTile.IsOccupied = false;
                            LabRat.CurTile = LabRat.CurTile.Below;
                            LabRat.CurTile.IsOccupied = true;
                        }
                        break;
                    case Direction.Right:
                        if (LabRat.CurTile.Right != null)
                        {
                            LabRat.CurTile.IsOccupied = false;
                            LabRat.CurTile = LabRat.CurTile.Right;
                            LabRat.CurTile.IsOccupied = true;
                        }
                        break;
                    case Direction.Up:
                        if (LabRat.CurTile.Above != null)
                        {
                            LabRat.CurTile.IsOccupied = false;
                            LabRat.CurTile = LabRat.CurTile.Above;
                            LabRat.CurTile.IsOccupied = true;
                        }
                        break;
                    case Direction.Left:
                        if (LabRat.CurTile.Left != null)
                        {
                            LabRat.CurTile.IsOccupied = false;
                            LabRat.CurTile = LabRat.CurTile.Left;
                            LabRat.CurTile.IsOccupied = true;
                        }
                        break;
                }
            }
            LabRat.Position = LabRat.CurTile.Position;
            b_outside_movement = true;
        }

        /// <summary>
        /// Randomly assigns the Labrat a place in the map
        /// </summary>
        private void RandomAssign()
        {
            Random r = new Random();
            int tile = r.Next(WalkingTiles.Count);

            LabRat.CurTile = WalkingTiles[tile];
            StartTile = LabRat.CurTile;

            LabRat.Position = LabRat.CurTile.Position;

            LabRat.CurTile.IsOccupied = true;
        }

        /// <summary>
        /// Uncollapses the walls around the Labrat
        /// </summary>
        private void ResetWalls()
        {
            for (int i = 0; i < WallTiles.Length; i++)
            {
                if (WallTiles[i] != null)
                {
                    //switches to tall walls
                    WallTiles[i].RaiseWall();
                }
            }

            //Clears WallTiles array
            for (int i = 0; i < WallTiles.Length; i++)
            {
                WallTiles[i] = null;
            }
        }

        /// <summary>
        /// Collapses walls around particular corrdinates
        /// </summary>
        /// <param name="Row">The row</param>
        /// <param name="Col">the column</param>
        private void CollectSurroundingWalls(ref Tile[] CollapsingTiles, int Row, int Col)
        {
            int row = Row;
            int col = Col;
            int index = 0;
            //One col at a time
            //First column: the one the AI's in.
            if (tiles[0, row + 1, col] != null && WallIDs.Contains(tiles[0, row + 1, col].ID))
                CollapsingTiles[index++] = tiles[0, row + 1, col];

            if (row <= arr_height - 3)
            {
                if (tiles[0, row + 2, col] != null && WallIDs.Contains(tiles[0, row + 2, col].ID))
                    CollapsingTiles[index++] = tiles[0, row + 2, col];
            }
            if (row <= arr_height - 4)
            {
                if (tiles[0, row + 3, col] != null && WallIDs.Contains(tiles[0, row + 3, col].ID))
                    CollapsingTiles[index++] = tiles[0, row + 3, col];
            }

            //Second column: the direction next to the AI
            if (tiles[0, row, col + 1] != null && WallIDs.Contains(tiles[0, row, col + 1].ID))
                CollapsingTiles[index++] = tiles[0, row, col + 1];
            if (tiles[0, row + 1, col + 1] != null && WallIDs.Contains(tiles[0, row + 1, col + 1].ID))
                CollapsingTiles[index++] = tiles[0, row + 1, col + 1];

            if (row <= arr_height - 3)
            {
                if (tiles[0, row + 2, col + 1] != null && WallIDs.Contains(tiles[0, row + 2, col + 1].ID))
                    CollapsingTiles[index++] = tiles[0, row + 2, col + 1];
            }

            if (row <= arr_height - 4)
            {
                if (tiles[0, row + 3, col + 1] != null && WallIDs.Contains(tiles[0, row + 3, col + 1].ID))
                    CollapsingTiles[index++] = tiles[0, row + 3, col + 1];
            }
            //Third column
            if (col <= arr_width - 3)
            {
                if (tiles[0, row, col + 2] != null && WallIDs.Contains(tiles[0, row, col + 2].ID))
                    CollapsingTiles[index++] = tiles[0, row, col + 2];
                if (tiles[0, row + 1, col + 2] != null && WallIDs.Contains(tiles[0, row + 1, col + 2].ID))
                    CollapsingTiles[index++] = tiles[0, row + 1, col + 2];

                if (row <= arr_height - 3)
                {
                    if (tiles[0, row + 2, col + 2] != null && WallIDs.Contains(tiles[0, row + 2, col + 2].ID))
                        CollapsingTiles[index++] = tiles[0, row + 2, col + 2];
                }

                if (row <= arr_height - 4)
                {
                    if (tiles[0, row + 3, col + 2] != null && WallIDs.Contains(tiles[0, row + 3, col + 2].ID))
                        CollapsingTiles[index++] = tiles[0, row + 3, col + 2];
                }
            }
            //Fourth column
            if (col <= arr_width - 4)
            {
                if (tiles[0, row, col + 3] != null && WallIDs.Contains(tiles[0, row, col + 3].ID))
                    CollapsingTiles[index++] = tiles[0, row, col + 3];
                if (tiles[0, row + 1, col + 3] != null && WallIDs.Contains(tiles[0, row + 1, col + 3].ID))
                    CollapsingTiles[index++] = tiles[0, row + 1, col + 3];

                if (row <= arr_height - 3)
                {
                    if (tiles[0, row + 2, col + 3] != null && WallIDs.Contains(tiles[0, row + 2, col + 3].ID))
                        CollapsingTiles[index++] = tiles[0, row + 2, col + 3];
                }

                if (row <= arr_height - 4)
                {
                    if (tiles[0, row + 3, col + 3] != null && WallIDs.Contains(tiles[0, row + 3, col + 3].ID))
                        CollapsingTiles[index++] = tiles[0, row + 3, col + 3];
                }
            }
        }

        private void CollapseWalls(ref Tile[] CollapsingTiles)
        {
            //Switch WallTiles to collapsed
            for (int i = 0; i < CollapsingTiles.Length; i++)
            {
                if (CollapsingTiles[i] != null)
                {
                    CollapsingTiles[i].CollapseWall();
                }
            }
        }

        /// <summary>
        /// Edits all of the map SARSA values
        /// </summary>
        /// <param name="εOptima"></param>
        /// <param name="εDecay"></param>
        /// <param name="gamma"></param>
        /// <param name="αAlpha"></param>
        /// <param name="lambda"></param>
        /// <param name="reward"></param>
        public void EditMapValues(float εOptima, float εDecay, float gamma, float αAlpha, float lambda, float reward)
        {
            EpsilonOptima = εOptima;
            EpsilonDecay = εDecay;
            Gamma = gamma;
            Alpha = αAlpha;
            Lambda = lambda;
            Reward = reward;
            LabRat.EditValues(εOptima, εDecay);
            CheeseTile.Reward = StaticReward;
        }

        /// <summary>
        /// Runs the Map
        /// </summary>
        /// <param name="gameTime">For updating the thinking interval</param>
        private void HandleMapAndAI(GameTime gameTime)
        {
            if (IsRunning)
            {
                LabRat.LookForCheese();
                Timer += gameTime.ElapsedGameTime;
                if (Timer.TotalMilliseconds >= DELAY)
                {
                    LabRat.Think();

                    //Switch WallTiles back
                    if (!Flattened)
                    {
                        ResetWalls();
                    }

                    #region Main Math and Movement Code
                    ///By this point, he should have the updated moving direction, 
                    ///but not moved yet.
                    //Get the probability value of the last step
                    if (Trail.Count > 0)
                    {
                        //Trail holds the last-made decision
                        DirProb ThisState = PrevTile.GetValue(Trail[Trail.Count - 1]);//Trail is Q(s,a)
                        DirProb NextState = LabRat.CurTile.GetValue(LabRat.MovingDirection);//Q(s',a')

                        //Update Previous tile's trodnumber
                        ThisState.TrodNumber++;
                        PrevTile.TrodNumber++;

                        //Then do the math
                        ThisState.Utility = (float)(Math.Round(
                            ThisState.Utility + Alpha * (LabRat.CurTile.Reward + (Gamma * NextState.Utility) - ThisState.Utility)
                            , 4));
                    }

                    PrevTile = LabRat.CurTile;
                    if (!LabRat.CheeseFound)
                    {
                        switch (LabRat.MovingDirection)
                        {
                            case Direction.Up:
                                LabRat.CurTile = LabRat.CurTile.Above;
                                break;
                            case Direction.Right:
                                LabRat.CurTile = LabRat.CurTile.Right;
                                break;
                            case Direction.Down:
                                LabRat.CurTile = LabRat.CurTile.Below;
                                break;
                            case Direction.Left:
                                LabRat.CurTile = LabRat.CurTile.Left;
                                break;
                        }


                        Trail.Add(new Direction());
                        Trail[Trail.Count - 1] = LabRat.MovingDirection;

                        //Move the Labrat
                        LabRat.Position = LabRat.CurTile.Position;
                        LabRat.CurTile.IsOccupied = true;
                    }
                    Timer = TimeSpan.Zero;
                    #endregion
                    CollectSurroundingWalls(ref WallTiles, LabRat.CurTile.MapLoc.Row, LabRat.CurTile.MapLoc.Col);
                }

                if (LabRat.CheeseFound)
                {
                    //Add Trial length to the list of trials
                    Trials.Add(new TrialInfo(Trail.Count, new Tile.Location(StartTile.MapLoc.Row, StartTile.MapLoc.Col)));
                    #region Assign Rewards
                    Tile temp = LabRat.CurTile;
                    //CheeseTile.Reward += StaticReward;
                    for (int i = Trail.Count - 1; i >= 0; i--)
                    {
                        PrevTile = LabRat.CurTile;
                        switch (Trail[i])
                        {
                            case Direction.Left:
                                LabRat.CurTile = LabRat.CurTile.Right;
                                break;
                            case Direction.Up:
                                LabRat.CurTile = LabRat.CurTile.Below;
                                break;
                            case Direction.Right:
                                LabRat.CurTile = LabRat.CurTile.Left;
                                break;
                            case Direction.Down:
                                LabRat.CurTile = LabRat.CurTile.Above;
                                break;
                        }
                        LabRat.CurTile.Reward += (float)Math.Round(Lambda * (PrevTile.Reward - LabRat.CurTile.Reward), 4);
                        //LabRat.CurTile.Reward += (float)Math.Round(Reward, 4);
                        Reward *= Lambda * Gamma;
                        //Reward *= Lambda;
                    }
                    LabRat.CurTile = temp;
                    #endregion

                    //Calculate the Average
                    int sum = 0;
                    for (int i = 0; i < Trials.Count; i++)
                    {
                        sum += Trials[i].StepsTaken;
                    }
                    AverageTrialLength = sum / Trials.Count;

                    //Reset the map and Labrat
                    Reset();
                    if (!IsRunning && LoopAmount == 1)
                    {
                        Beep.Play();
                    }
                }
                if (!Flattened)
                {
                    CollapseWalls(ref WallTiles);
                    CollapseWalls(ref CheeseWallTiles);
                }

            }

        }

        /// <summary>
        /// Increases the offset of the map by set amount
        /// </summary>
        /// <param name="x">the x axis</param>
        /// <param name="y">the y axis</param>
        public void OffsetMapBy(float x, float y)
        {
            offset.X += x;
            offset.Y += y;
        }

        /// <summary>
        /// Changes how many times the Labrat will run consecutively
        /// </summary>
        /// <param name="x">The numbers of times</param>
        public void ChangeTrialsAmountBy(int x)
        {
            if (x < 0)
            {
                if (LoopAmount != 1 && LoopAmount + x >= 1)
                    LoopAmount += x;
            }
            else
            {
                LoopAmount += x;
            }
            StaticLoopAmount = LoopAmount;
        }

        /// <summary>
        /// Increases the view size by a given number
        /// </summary>
        /// <param name="Factor">amount of incrementation</param>
        public void ZoomMapBy(int Factor)
        {
            if (Factor < 0 && ViewSize > 1)//Zooming out
            {
                offset *= ViewSize;
                ViewSize += Factor;
            }
            else if (Factor > 0)//Zooming in
            {
                ViewSize += Factor;
                offset /= ViewSize;
            }
        }

        /// <summary>
        /// Toggles whether or not the AI, once reset, moves back to a random placement or the same as the last run.
        /// </summary>
        public void ToggleRandomReset()
        {
            RandomReset = !RandomReset;
        }

        /// <summary>
        /// Flattens or raises all of the walls on the map
        /// </summary>
        public void ToggleWalls()
        {
            foreach (Tile t in BlockingTiles)
            {
                if (Flattened)
                    t.RaiseWall();
                else
                    t.CollapseWall();
            }
            Flattened = !Flattened;
        }

        /// <summary>
        /// Changes the delay between each time the AI thinks by the given amount
        /// </summary>
        /// <param name="amount">The amount of change. Can be negative</param>
        public void ChangeDelay(int amount)
        {
            DELAY += amount;
            if (DELAY < 0)
                DELAY = 0;
        }

        /// <summary>
        /// Turn the AI's thinking and movement on and off
        /// </summary>
        public void ToggleRun()
        {
            IsRunning = !IsRunning;
            if (!IsRunning && Trail.Count != 0)//Which means he was in the middle of a run
            {
                PrevTile = LabRat.CurTile;
            }
            else if (IsRunning && b_outside_movement && Trail.Count != 0)
            {
                LabRat.CurTile.IsOccupied = false;
                LabRat.CurTile = PrevTile;
                LabRat.CurTile.IsOccupied = true;
                LabRat.Position = LabRat.CurTile.Position;
            }
            if (IsRunning)
            {
                b_outside_movement = false;
            }
        }

        /// <summary>
        /// Toggles the directional arrows being drawn above the map
        /// </summary>
        public void ToggleArrowDraw()
        {
            DrawArrows = !DrawArrows;
        }

        /// <summary>
        /// Toggles the directional arrows being drawn being offset in relation to their tile's reward
        /// </summary>
        public void ToggleArrowOffset()
        {
            DrawArrowOffset = !DrawArrowOffset;
        }
        #endregion
        #endregion
    }
}
