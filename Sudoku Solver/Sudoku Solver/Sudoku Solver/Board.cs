using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Sudoku_Solver
{
    public delegate void BoardCompletionEventHandler();
    /// <summary>
    /// Dictates whether or not the particular space is correct, or inconclusive.
    /// This will be used by the Board, PerfMeasure, and Space classes.
    /// </summary>
    public enum SpaceAccuracyStates
    {
        Right,
        Wrong,
        Try,
        IDK
    }

    /// <summary>
    /// A struct to keep track of the locations of each entity based off the 2D Space array.
    /// </summary>
    public struct Location
    {
        public int X, Y;
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Represents one really smart board
    /// </summary>
    public class Board : DrawableGameComponent
    {
        #region Variables
        #region private
        private Game game;
        private SpriteBatch spriteBatch;

        private Color c_num_color;

        /// <summary>
        /// A huge list of Texture2Ds the Board personally uses
        /// </summary>
        private Texture2D t2d_wrong_bg, t2d_right_bg, t2d_try_bg, t2d_idk_bg, t2d_check_mark,
            t2d_grid, t2d_hgl, t2d_vgl, t2d_single_circle, t2d_unique_square, t2d_IDK_mark;

        /// <summary>
        /// Holds all the different types of colors the spaces can be.
        /// The array values are linked to the Hash values of the
        /// SpaceAccuracyStates enumerator
        /// </summary>
        private Texture2D[] ColoredSpaces;

        /// <summary>
        /// A bunch of Vector2s the Board personally uses for
        /// drawing the huge list of Texture2Ds the Board
        /// personally uses.
        /// </summary>
        private Vector2 v2_grid_size, v2_grid_pos, v2_space_offest, v2_space_size, v2_text_spacing, v2_grid_offset,
            v2_square_offset, v2_cumu_sum, v2_gl_offset, v2_max_word_distance, v2_index_distance, v2_letter_distance, v2_word_draw_pos;

        /// <summary>
        /// A small list of SpriteFont the Board personally uses 
        /// in accordance with some of the bunch of Vector2s the 
        /// Board personally uses for drawing the huge list of
        /// Texture2Ds the Board personally uses, but not with
        /// the SpriteFonts.
        /// </summary>
        private SpriteFont sf_24_num_font, sf_10_num_font;

        /// <summary>
        /// For debugging purposes. They're not used much now. ;_;
        /// </summary>
        private Texture2D pixel;
        private Color[] colordata;

        private bool b_complete;

        private bool Complete
        {
            get
            {
                return b_complete;
            }
            set
            {
                b_complete = value;
                if (b_complete)
                    OnCompletion();
            }
        }

        private string str_puzz_filename;
        #endregion
        #region Public
        public Space[,] Spaces;

        public Square[,] Squares;

        public Vector2 BoardPosition;

        public event BoardCompletionEventHandler BoardComplete;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// A collection of Squares
        /// </summary>
        /// <param name="game">The game in which this will be used</param>
        /// <param name="BigFontFileDir">The full path for the  the large-text spritefont file</param>
        /// <param name="SmallFontFileDir">The full path for the small-text spritefont file</param>
        /// <param name="FontColor">The color in which both spritefonts will be drawn</param>
        /// <param name="rightBGColor">The color which will be used for the right numbers</param>
        /// <param name="wrongBGColor">The color which will be used for the wrong numbers</param>
        /// <param name="IDKBGColor">The color which will be used for the inconclusive numbers</param>
        /// <param name="SpaceWidth">The width of the number space</param>
        /// <param name="SpaceHeight">The height of the number space</param>
        private Board(Game game, int SpaceWidth, int SpaceHeight)
            : base(game)
        {
            this.game = game;
            v2_space_size = new Vector2(SpaceWidth, SpaceHeight);

            c_num_color = Color.Black;
            v2_grid_size.X = v2_space_size.X + 6;
            v2_grid_size.Y = v2_space_size.Y + 6;
            v2_grid_pos = Vector2.Zero;
            v2_space_offest = Vector2.Zero;
            v2_text_spacing = Vector2.Zero;
            v2_square_offset = Vector2.Zero;
            v2_grid_offset = Vector2.Zero;
            v2_cumu_sum = Vector2.Zero;
            v2_gl_offset = Vector2.Zero;
            BoardPosition = Vector2.Zero;
            v2_word_draw_pos = Vector2.Zero;

            ///Creation of the board happens in 7 steps
            ///STEP 1/9: Instantiate the boards
            Spaces = new Space[9, 9];
            Squares = new Square[3, 3];
            ColoredSpaces = new Texture2D[4];
        }

        public Board(Game game, int SpaceWidth, int SpaceHeight, string puzzleFileName)
            : this(game, SpaceWidth, SpaceHeight)
        //Since this inherits from the the previous constructor STEP 1 is always done
        {
            str_puzz_filename = puzzleFileName;

            //STEP 2/9: Read the numbers from the file into the board
            PuzzleReader.MakePuzzle(str_puzz_filename, this);
            ///At this point, the array now has all it's given numbers in

            StreamWriter sw = new StreamWriter("Output.txt");
            int lrow = Spaces.GetLength(0);
            int lcol = Spaces.GetLength(1);

            //This rewrites the puzzle in tabular form
            for (int x = 0; x < lcol; x++)
            {
                sw.Write(x == lcol - 1 ? "+-+\n" : "+-");
            }
            for (int y = 0; y < lrow; y++)
            {
                for (int x = 0; x < lcol; x++)
                {
                    sw.Write("|");
                    if (Spaces[x, y].ChosenNumber != "")
                        sw.Write(Spaces[x, y].ChosenNumber);
                    else
                        sw.Write(" ");
                    if (x == lcol - 1)
                        sw.Write("|\n");
                }
                for (int x = 0; x < lcol; x++)
                {
                    sw.Write(x == lcol - 1 ? "+-+\n" : "+-");
                }
            }
            sw.Close();
            //STEP 3/9: Make the Square mapping array
            for (int row = 0; row < Squares.GetLength(1); row++)
            {
                for (int col = 0; col < Squares.GetLength(0); col++)
                {
                    Squares[col, row] = new Square(new Location(col, row));
                }
            }
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            BoardPosition = Vector2.Zero;

            #region Sets up debugging stuff
            pixel = new Texture2D(GraphicsDevice, 2, 2);
            colordata = new Color[4];
            colordata[0] = Color.Red;
            colordata[1] = Color.Red;
            colordata[2] = Color.Red;
            colordata[3] = Color.Red;
            pixel.SetData(colordata);
            #endregion

            ///STEP 4/9: Have the Board class subscribe to when 
            ///a number has been placed. This way the board
            ///automatically updates itself, and the Agent has
            ///to only worry about making the right choices.
            for (int row = 0; row < Spaces.GetLength(1); row++)
            {
                for (int col = 0; col < Spaces.GetLength(0); col++)
                {
                    Spaces[col, row].NumberPlaced += new PlacedNumberEventHandler(UpdatePossibilities);
                }
            }

            ///STEP 5/9: Map the Square table to the Space Table
            for (int SqRow = 0; SqRow < 3; SqRow++)
            {
                for (int SpRow = 0; SpRow < 3; SpRow++)
                {
                    for (int SqCol = 0; SqCol < 3; SqCol++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)
                        {
                            Squares[SqCol, SqRow].Spaces[SpCol, SpRow] = Spaces[(SqCol * 3) + SpCol, (SqRow * 3) + SpRow];
                        }
                    }
                }
            }

            ///Step 6/9: Calculate the starting possibilities
            CalculatePossibilities();

            ///Step 7/9: Register CheckBoard to each space in the board.
            ///This will check to see if there are no more possiblities
            ///anywhere on the board. At that point the board is complete.
            foreach (Space sp in Spaces)
            {
                sp.NumberPlaced += new PlacedNumberEventHandler(CheckBoard);
            }

            ///STEP 8/9 & 9/9: Have each square register to their respective
            ///spaces and find the averages for the Agent to use later
            foreach (Square Sq in Squares)
            {
                Sq.RegistertoSpaces();
                Sq.CalculateAveragePossieSize();
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            sf_24_num_font = game.Content.Load<SpriteFont>("Large Font");
            sf_10_num_font = game.Content.Load<SpriteFont>("Small Font");
            t2d_right_bg = game.Content.Load<Texture2D>("Right Space");
            t2d_wrong_bg = game.Content.Load<Texture2D>("Wrong Space");
            t2d_idk_bg = game.Content.Load<Texture2D>("IDK Space");
            t2d_try_bg = game.Content.Load<Texture2D>("Try Space");
            t2d_grid = game.Content.Load<Texture2D>("Grid");
            t2d_hgl = game.Content.Load<Texture2D>("GridLine H");
            t2d_vgl = game.Content.Load<Texture2D>("GridLine V");
            t2d_single_circle = game.Content.Load<Texture2D>("Single Circle");
            t2d_unique_square = game.Content.Load<Texture2D>("Unique Square");
            t2d_check_mark = game.Content.Load<Texture2D>("Check Mark");
            t2d_IDK_mark = game.Content.Load<Texture2D>("IDK Mark");

            ColoredSpaces[SpaceAccuracyStates.Right.GetHashCode()] = t2d_right_bg;
            ColoredSpaces[SpaceAccuracyStates.Wrong.GetHashCode()] = t2d_wrong_bg;
            ColoredSpaces[SpaceAccuracyStates.Try.GetHashCode()] = t2d_try_bg;
            ColoredSpaces[SpaceAccuracyStates.IDK.GetHashCode()] = t2d_idk_bg;

            v2_max_word_distance = sf_10_num_font.MeasureString("[0, 0] [0, 0]: 1,2,3,4,5,6,7,8,9");
            v2_index_distance.X = sf_10_num_font.MeasureString("[0, 0] [0, 0]: ").X;
            v2_letter_distance.X = sf_10_num_font.MeasureString("0,").X;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();
            Space sp;
            #region Draw the grid
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    v2_grid_pos.X = col * v2_grid_size.X;
                    v2_grid_pos.Y = row * v2_grid_size.Y;

                    spriteBatch.Draw(t2d_grid, BoardPosition + v2_grid_pos, Color.White);
                }
            }
            #endregion

            #region Draw the gridlines
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    //vertical
                    v2_gl_offset.X = ((3 * v2_grid_size.X) * (col % 4)) - 3;
                    v2_gl_offset.Y = v2_grid_size.Y * row;
                    if (row == 0)
                        spriteBatch.Draw(t2d_vgl,
                              new Rectangle((int)(BoardPosition.X + v2_gl_offset.X), (int)(BoardPosition.Y + v2_gl_offset.Y - 3),
                                   t2d_vgl.Width, (int)v2_grid_size.Y + 6),
                              Color.White);
                    else if (row == 8)
                        spriteBatch.Draw(t2d_vgl,
                               new Rectangle((int)(BoardPosition.X + v2_gl_offset.X), (int)(BoardPosition.Y + v2_gl_offset.Y),
                                    t2d_vgl.Width, (int)v2_grid_size.Y + 3),
                               Color.White);
                    else if (row != 9)
                        spriteBatch.Draw(t2d_vgl, BoardPosition + v2_gl_offset, Color.White);

                    //horizontal
                    v2_gl_offset.X = v2_grid_size.X * col;
                    v2_gl_offset.Y -= 3;
                    if (row % 3 == 0)
                    {
                        if (col != 8 && col < 8)
                            spriteBatch.Draw(t2d_hgl, BoardPosition + v2_gl_offset, Color.White);
                        else if (col != 9)
                            spriteBatch.Draw(t2d_hgl,
                                new Rectangle((int)(BoardPosition.X + v2_gl_offset.X), (int)(BoardPosition.Y + v2_gl_offset.Y),
                                    (int)v2_grid_size.X + 3, t2d_hgl.Height),
                                Color.White);
                    }
                }
            }
            #endregion

            #region Draw the spaces

            for (int SpRow = 0; SpRow < Spaces.GetLength(0); SpRow++)
            {
                for (int SpCol = 0; SpCol < Spaces.GetLength(1); SpCol++)
                {
                    #region Old Code
                    /*
                    v2_square_offset.X = (3 * v2_grid_size.X) * SpCol;
                    v2_square_offset.Y = (3 * v2_grid_size.X) * SpRow;
                    Square sq = Spaces[SpCol, SpRow];
                    int SpCol = 0, SpRow = 0;
                    for (int i = 0; i < sq.Spaces.Length; i++)
                    {
                        Space sp = sq.Spaces[i];
                        SpCol = i % 3;
                        SpRow = i / 3;
                        */
                    #endregion
                    //The base space offset without the grid's border considered
                    sp = Spaces[SpCol, SpRow];
                    v2_space_offest.X = v2_space_size.X * SpCol;
                    v2_space_offest.Y = v2_space_size.Y * SpRow;

                    //Calculates the grid's border offset
                    v2_grid_offset.X = 3/*For one-half of the border (left edge)*/ +
                        (6/*for the right/left borders adjacent to each other*/ * SpCol);
                    v2_grid_offset.Y = 3/*For one-half of the border (top edge)*/ +
                        (6/*for the bottom/top borders adjacent to each other*/ * SpRow);
                    v2_cumu_sum = BoardPosition + /*v2_square_offset +*/ v2_space_offest + v2_grid_offset;

                    spriteBatch.Draw(ColoredSpaces[sp.SpaceAccuracy.GetHashCode()], v2_cumu_sum, Color.White);

                    #region Draw the numbers
                    v2_space_offest.X = v2_grid_size.X * SpCol;
                    v2_space_offest.Y = v2_grid_size.Y * SpRow;

                    v2_cumu_sum = BoardPosition + /*v2_square_offset +*/ v2_space_offest;
                    v2_text_spacing = v2_cumu_sum + (v2_grid_size / 2) - (sf_24_num_font.MeasureString(sp.ChosenNumber) / 2);
                    spriteBatch.DrawString(sf_24_num_font, sp.ChosenNumber, v2_text_spacing, c_num_color);
                    #endregion
                    /*spriteBatch.Draw(pixel, v2_text_spacing, Color.White);*/
                    //for debugging
                    //}
                }
            }
            #endregion

            #region Draw the Possibilities
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sp = Spaces[col, row];
                    string possibilities = "[" + (col / 3) + ", " + (row / 3) + "] [" + (col % 3) + ", " + (row % 3) + "]: ";
                    for (int i = 0; i < sp.Possibilities.Count; i++)
                    {
                        possibilities += sp.Possibilities[i].Number + (i < sp.Possibilities.Count - 1 ? "," : "");
                    }
                    v2_word_draw_pos.X = (((int)(col / 3)) * v2_max_word_distance.X) + (((int)(col / 3) * v2_max_word_distance.Y)) + 500;
                    v2_word_draw_pos.Y = ((col % 3) * v2_max_word_distance.Y) + (row * 3 * v2_max_word_distance.Y) +
                            ((row / 3) * v2_max_word_distance.Y);

                    spriteBatch.DrawString(sf_10_num_font, possibilities, v2_word_draw_pos, Color.Black);
                    #region Draw Pics
                    if (sp.IsAbsolute)
                    {
                        spriteBatch.Draw(t2d_check_mark, v2_word_draw_pos + v2_index_distance, Color.White);
                    }
                    else if (sp.Possibilities.Count == 0)
                    {
                        //It's a tried square, so nothing's concrete.
                        spriteBatch.Draw(t2d_IDK_mark, v2_word_draw_pos + v2_index_distance, Color.White);
                    }
                    //Draw the Singles
                    v2_word_draw_pos.X -= 3;
                    if (sp.Possibilities.Count == 1)
                    {
                        spriteBatch.Draw(t2d_single_circle, v2_word_draw_pos + v2_index_distance, Color.White);
                        continue;
                    }
                    //Draw the Uniques
                    v2_word_draw_pos.X += 2;
                    v2_word_draw_pos.Y += 3;
                    for (int p_index = 0; p_index < sp.Possibilities.Count; p_index++)
                    {
                        Possibility SpP = sp.Possibilities[p_index];
                        if (!SpP.IsUnique)
                            v2_word_draw_pos += v2_letter_distance;
                        else if (SpP.IsUnique)
                        {
                            spriteBatch.Draw(t2d_unique_square, v2_word_draw_pos + v2_index_distance, Color.White);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            spriteBatch.End();
        }

        /// <summary>
        /// Checks to see if the board is complete
        /// </summary>
        /// <param name="sp">There's no need to do anything with
        /// it. It's just to use the delegate.</param>
        private void CheckBoard(Space space)
        {
            int AbsoluteCounter = 0;
            foreach (Square sq in Squares)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (sq.Spaces[col, row].IsAbsolute)
                            AbsoluteCounter++;
                        else
                            break;
                    }
                    if (AbsoluteCounter != (row + 1) * 3)
                        break;
                }
                if (AbsoluteCounter == 9)
                    sq.IsComplete = true;
                AbsoluteCounter = 0;
            }

            foreach (Space sp in Spaces)
            {
                if (sp.Possibilities.Count != 0)
                    return;
            }
            Complete = true;
        }

        /// <summary>
        /// Updates the possibilities related the last spaced used. 
        /// Its like CalculatePossibilities, but more focused on 
        /// the Squares/Rows/Columns that really matter.
        /// </summary>
        /// <param name="LastUsedSpace">The last space used...yep.</param>
        public void UpdatePossibilities(Space LastUsedSpace)
        {
            Square Sq = Squares[LastUsedSpace.TableLocation.X / 3, LastUsedSpace.TableLocation.Y / 3];

            ///Update the Square to which space belongs first.
            UpdateSquare(Sq, LastUsedSpace);

            ///To determine which of the squares to update. I personally made the 
            ///column update before the row.
            switch (Sq.TableLocation.X)
            {
                case 0://The next two spaces within the row within the square
                    UpdateSquare(Squares[1, Sq.TableLocation.Y], LastUsedSpace);
                    UpdateSquare(Squares[2, Sq.TableLocation.Y], LastUsedSpace);
                    break;
                case 1://The first last two spaces within the row
                    UpdateSquare(Squares[0, Sq.TableLocation.Y], LastUsedSpace);
                    UpdateSquare(Squares[2, Sq.TableLocation.Y], LastUsedSpace);
                    break;
                case 2://The first two spaces
                    UpdateSquare(Squares[0, Sq.TableLocation.Y], LastUsedSpace);
                    UpdateSquare(Squares[1, Sq.TableLocation.Y], LastUsedSpace);
                    break;
            }
            switch (Sq.TableLocation.Y)
            {
                //Same as before, but with columns
                case 0:
                    UpdateSquare(Squares[Sq.TableLocation.X, 1], LastUsedSpace);
                    UpdateSquare(Squares[Sq.TableLocation.X, 2], LastUsedSpace);
                    break;
                case 1:
                    UpdateSquare(Squares[Sq.TableLocation.X, 0], LastUsedSpace);
                    UpdateSquare(Squares[Sq.TableLocation.X, 2], LastUsedSpace);
                    break;
                case 2:
                    UpdateSquare(Squares[Sq.TableLocation.X, 0], LastUsedSpace);
                    UpdateSquare(Squares[Sq.TableLocation.X, 1], LastUsedSpace);
                    break;
            }

        }

        /// <summary>
        /// A helper method for UpdatePossibilities().
        /// Updates the possibilities for the Square based on the space used
        /// </summary>
        /// <param name="square">The square to Update</param>
        /// <param name="LastUsedSpace">The space on which an action was made</param>
        private void UpdateSquare(Square square, Space LastUsedSpace)
        {
            if (!square.IsComplete)
            {
                //Check where the space is in relation to the Square
                Location loc = new Location(LastUsedSpace.TableLocation.X / 3, LastUsedSpace.TableLocation.Y / 3);
                //If the space's Square location is either to the left or right of this square...
                if (loc.X < square.TableLocation.X || loc.X > square.TableLocation.X)
                {
                    //lock the row and update
                    for (int SpRow = 0; SpRow < 3; SpRow++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)//We run through the columns faster than the rows
                        {
                            Space sp = square.Spaces[SpCol, LastUsedSpace.TableLocation.Y % 3];
                            if (!sp.IsAbsolute)
                            {
                                //Run through all the possibilities for that space backwards (for deletion purposes)
                                for (int p_index = sp.Possibilities.Count - 1; p_index >= 0; p_index--)
                                {
                                    //If that's the number to be deleted...
                                    if (sp.Possibilities[p_index].Number == LastUsedSpace.ChosenNumber)
                                        sp.Possibilities.RemoveAt(p_index);//delete it
                                }
                            }
                        }
                    }
                }
                //else if the spaces Square location is either above or below this square...
                else if (loc.Y < square.TableLocation.Y || loc.Y > square.TableLocation.Y)
                {
                    //lock the column and update
                    for (int SpCol = 0; SpCol < 3; SpCol++)
                    {
                        for (int SpRow = 0; SpRow < 3; SpRow++)//This time we run through the ROWS faster than the columns
                        {
                            Space sp = square.Spaces[LastUsedSpace.TableLocation.X % 3, SpRow];
                            if (!sp.IsAbsolute)
                            {
                                //Run through all the possibilities for that space backwards (for deletion purposes)
                                for (int p_index = sp.Possibilities.Count - 1; p_index >= 0; p_index--)
                                {
                                    //If that's the number to be deleted...
                                    if (sp.Possibilities[p_index].Number == LastUsedSpace.ChosenNumber)
                                        sp.Possibilities.RemoveAt(p_index);//delete it
                                }
                            }
                        }
                    }
                }
                //else the space's location is the square's location
                else
                {
                    //Update the entire square
                    foreach (Space sp in square.Spaces)
                    {
                        if (!sp.IsAbsolute)
                        {
                            for (int p_index = sp.Possibilities.Count - 1; p_index >= 0; p_index--)
                            {
                                //If that's the number to be deleted...
                                if (sp.Possibilities[p_index].Number == LastUsedSpace.ChosenNumber)
                                    sp.Possibilities.RemoveAt(p_index);//delete it
                            }
                        }
                    }
                }
                SinglesUniques(square);
            }
        }

        /// <summary>
        /// Performs the initial setup of possibilities for the 
        /// table using a subtraction method. It removes the
        /// impoossibilities from the possible possibilities
        /// for each space
        /// </summary>
        private void CalculatePossibilities()
        {
            //Numbers that should be removed from the current possibilities
            List<string> TBRNumbers = new List<string>();
            Square CurSquare;
            Space CurSpace;
            Possibility CurPossie;
            //Check the squares

            for (int SqRow = 0; SqRow < 3; SqRow++)
            {
                for (int SqCol = 0; SqCol < 3; SqCol++)
                {
                    #region Collect impossible numbers
                    CurSquare = Squares[SqCol, SqRow];
                    for (int SpRow = 0; SpRow < 3; SpRow++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)
                        {
                            CurSpace = CurSquare.Spaces[SpCol, SpRow];
                            if (CurSpace.ChosenNumber != "")//i.e. If the number was given
                                TBRNumbers.Add(CurSpace.ChosenNumber);
                        }
                    }

                    #endregion
                    #region Remove them from the square
                    //Runs through every space within the current square
                    for (int SpRow = 0; SpRow < 3; SpRow++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)
                        {
                            CurSpace = CurSquare.Spaces[SpCol, SpRow];
                            foreach (string num in TBRNumbers)
                            {
                                for (int i = CurSpace.Possibilities.Count - 1; i >= 0; i--)
                                {
                                    if (CurSpace.Possibilities[i].Number == num)//If the number's a TBR number
                                        CurSpace.Possibilities.RemoveAt(i);//remove the possibility
                                }
                            }
                        }
                    }
                    #endregion
                    TBRNumbers.Clear();
                    #region Go through the square and remove impossibilities using the related rows and columns
                    for (int SpRow = 0; SpRow < 3; SpRow++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)
                        {
                            CurSpace = CurSquare.Spaces[SpCol, SpRow];
                            List<string> Row = GetRow((SqRow * 3) + SpRow);
                            List<string> Column = GetColumn((SqCol * 3) + SpCol);
                            foreach (string num in Row)
                            {
                                for (int i = CurSpace.Possibilities.Count - 1; i >= 0; i--)
                                {
                                    CurPossie = CurSpace.Possibilities[i];
                                    if ( CurPossie.Number == num)
                                        CurSpace.Possibilities.RemoveAt(i);
                                }
                                #region Old Code
                                /*
                                if (sp.Number != "" && CurSpace.Possibilities.Contains(sp.Number))
                                    CurSpace.Possibilities.Remove(sp.Number);
                                 */
                                #endregion
                            }

                            foreach (string num in Column)
                            {
                                for (int i = CurSpace.Possibilities.Count - 1; i >= 0; i--)
                                {
                                    CurPossie = CurSpace.Possibilities[i];
                                    if (CurPossie.Number == num)
                                        CurSpace.Possibilities.RemoveAt(i);
                                }
                                #region Old Code
                                /*
                                if (sp.Number != "" && CurSpace.Possibilities.Contains(sp.Number))
                                    CurSpace.Possibilities.Remove(sp.Number);
                                 */
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
            }

            #region Mark Numbers as Unique and Single
            for (int SqRow = 0; SqRow < 3; SqRow++)
            {
                for (int SqCol = 0; SqCol < 3; SqCol++)
                {
                    CurSquare = Squares[SqCol, SqRow];
                    SinglesUniques(CurSquare);
                }
            }
            #endregion
        }

        public void SinglesUniques(Square sq)
        {
            List<Possibility> SeenNumbers = new List<Possibility>();
            List<Possibility> NewNumbers = new List<Possibility>();
            Space CurSpace;
            Possibility CurPossie;
            for (int SpRow = 0; SpRow < 3; SpRow++)
            {
                for (int SpCol = 0; SpCol < 3; SpCol++)
                {
                    CurSpace = sq.Spaces[SpCol, SpRow];

                    /*
                    if (CurSpace.Possibilities.Count == 1)//if there's only one possibility for this space...
                    {
                        continue;//skip the rest
                    }
                    */

                    if (CurSpace.Possibilities.Count == 0)//If there are no possibilities (i.e. the number was given)
                        continue;
                    else if (SeenNumbers.Count == 0 && NewNumbers.Count == 0)//If nothing's been seen yet
                    {
                        NewNumbers.AddRange(CurSpace.Possibilities);//Then everything's new
                        continue;
                    }
                    //otherwise...
                    for (int p_index = 0; p_index < CurSpace.Possibilities.Count; p_index++)
                    {
                        CurPossie = CurSpace.Possibilities[p_index];
                        bool isNew = false;
                        //First, check to see if it's been seen
                        if (SeenNumbers.Count != 0)
                        {
                            for (int s_index = 0; s_index < SeenNumbers.Count; s_index++)
                            {
                                //if it has been seen...
                                if (CurPossie.Number == SeenNumbers[s_index].Number)
                                    break;
                                //if it hasn't been seen at all...
                                if (CurPossie.Number != SeenNumbers[s_index].Number && s_index == SeenNumbers.Count - 1)
                                {
                                    isNew = true;//This MIGHT be a new number
                                }
                            }
                        }
                        else //i.e. no numbers have already been seen
                        {
                            for (int n_index = NewNumbers.Count - 1; n_index >= 0; n_index--)
                            {
                                if (CurPossie.Number == NewNumbers[n_index].Number)//If it's in the new numbers...
                                {
                                    //It's been seen: it's no longer new
                                    NewNumbers.RemoveAt(n_index);
                                    if (CurSpace.Possibilities.Count == 1)
                                        SeenNumbers.Add(new Possibility(CurSpace.Possibilities[0].Number,CurSpace.Possibilities[0].IsUnique));
                                    else
                                    SeenNumbers.Add(CurPossie);
                                }
                            }
                        }
                        if (isNew)
                        {
                            for (int n_index = NewNumbers.Count - 1; n_index >= 0; n_index--)
                            {
                                if (CurPossie.Number == NewNumbers[n_index].Number)//If it's in the new numbers...
                                {
                                    //It's been seen: it's no longer new
                                    NewNumbers.RemoveAt(n_index);
                                    if (CurSpace.Possibilities.Count == 1)
                                        SeenNumbers.Add(new Possibility(CurSpace.Possibilities[0].Number, CurSpace.Possibilities[0].IsUnique));
                                    SeenNumbers.Add(CurPossie);
                                    break;
                                }
                                //but if even NewNumbers hasn't seen it yet...
                                else if (CurPossie.Number != NewNumbers[n_index].Number && n_index == 0)
                                {
                                    //then it's definitely new
                                    if (CurSpace.Possibilities.Count == 1)
                                        NewNumbers.Add(new Possibility(CurSpace.Possibilities[0].Number, CurSpace.Possibilities[0].IsUnique));
                                    NewNumbers.Add(CurPossie);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < NewNumbers.Count; i++)
            {
                //If they made it this far, then they're unique
                NewNumbers[i].IsUnique = true;
            }
            SeenNumbers.Clear();
            NewNumbers.Clear();
        }

        /// <summary>
        /// Gets the entire row of the specified index
        /// </summary>
        /// <param name="RowNumber">The zero-based row number of the index</param>
        /// <returns>An list of Space objects holding the row requested</returns>
        private List<string> GetRow(int RowNumber)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                Space sp = Spaces[i, RowNumber];
                if (sp.IsAbsolute && sp.ChosenNumber != "")
                    result.Add(sp.ChosenNumber);
            }
            return result;
        }

        /// <summary>
        /// Gets the entire column of the specified index
        /// </summary>
        /// <param name="ColumnNumber">The zero-based column number of the index</param>
        /// <returns>An list of Space objects holding the column requested</returns>
        private List<string> GetColumn(int ColumnNumber)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                Space sp = Spaces[ColumnNumber, i];
                if (sp.ChosenNumber != "")
                    result.Add(sp.ChosenNumber);
            }
            return result;
        }

        //Triggers if board is complete (no possibilities anywhere)
        private void OnCompletion()
        {
            if (BoardComplete != null)
                BoardComplete();
        }

    }
}
