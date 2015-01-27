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


///Sudoku Solver
///Author: Sage Kelly
///Class: Artificial Intelligence
///Professor: Dr. M. Franklin
///2/7/2014
namespace Sudoku_Solver
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Board GameBoard;

        SpriteFont MouseFont;
        KeyboardState KBState, PrevKBState = Keyboard.GetState();
        bool InteractiveMode;

        string puzzleName = "Puzzle 3";

        string[,] wordarray;
        Vector2 MaxWordDistance;
        Vector2 BoardOffset;

        enum GameStates
        {
            Main,
            Load,
            IGame,
            AIGame,
            EndGame
        }

        GameStates GS;
        /*
         * Screen Resses:
         * 800 X 600
         * 1024 X 768
         * 1280 X 600
         * 1280 X 720
         * 1280 X 768
         * 1280 X 800
         * 1280 X 960
         * 1280 X 1024
         * 1366 X 768
         */
        const int SCRN_WIDTH_800 = 800;
        const int SCRN_WIDTH_1024 = 1024;
        const int SCRN_WIDTH_1280 = 1280;
        const int SCRN_WIDTH_1366 = 1366;

        const int SCRN_HEIGHT_600 = 600;
        const int SCRN_HEIGHT_720 = 720;
        const int SCRN_HEIGHT_768 = 768;
        const int SCRN_HEIGHT_800 = 800;
        const int SCRN_HEIGHT_960 = 960;
        const int SCRN_HEIGHT_1024 = 1024;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = SCRN_WIDTH_1024;
            graphics.PreferredBackBufferHeight = SCRN_HEIGHT_600;
            IsMouseVisible = true;
        }
        #region Game Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameBoard = new Board(this, 44, 44, @"Puzzles\" + puzzleName + ".txt");
            this.Components.Add(GameBoard);

            wordarray = new string[9, 9];
            #region Test Code
            /*
            for (int SqRow = 0; SqRow < 3; SqRow++)
            {
                for (int SqCol = 0; SqCol < 3; SqCol++)
                {
                    for (int SpRow = 0; SpRow < 3; SpRow++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)
                        {
                            wordarray[(SqCol * 3) + SpCol, (SqRow * 3) + SpRow] =
                                "[" + SqCol + ", " + SqRow + "] [" + SpCol + ", " + SpRow + "]: 1,2,3,4,5,6,7,8,9";
                        }
                    }
                }
            }
            */
            #endregion
            base.Initialize();
            GameBoard.Thinker.Delay = 1000;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MouseFont = Content.Load<SpriteFont>("Small Font");

            //MaxWordDistance = new Vector2(MouseFont.MeasureString("[0, 0] [0, 0]: 1,2,3,4,5,6,7,8,9").X, MouseFont.MeasureString("0").Y);
            BoardOffset = new Vector2(500, 0);

            GameBoard.BoardPosition.X = 3;
            GameBoard.BoardPosition.Y = 3;
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
            KBState = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (KBState.IsKeyDown(Keys.Escape) && PrevKBState.IsKeyUp(Keys.Escape))
                this.Exit();
            
            if (KBState.IsKeyDown(Keys.Down) && PrevKBState.IsKeyUp(Keys.Down))
            {
                GameBoard.Thinker.Delay -= 100;
            }

            if (KBState.IsKeyDown(Keys.Up) && PrevKBState.IsKeyUp(Keys.Up))
            {
                GameBoard.Thinker.Delay += 100;
            }

            if (KBState.IsKeyDown(Keys.Space) && PrevKBState.IsKeyUp(Keys.Space))
            {
                GameBoard.Thinker.Running = !GameBoard.Thinker.Running;
            }

            if (KBState.IsKeyDown(Keys.F) && PrevKBState.IsKeyUp(Keys.F))
            {
                GameBoard.Thinker.ForceAct = true;
            }
            GameBoard.Thinker.Update(gameTime);

            base.Update(gameTime);
            PrevKBState = KBState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.DrawString(MouseFont, "AI's Ticker Speed: " + GameBoard.Thinker.Delay, new Vector2(0, 480), Color.Black);
            spriteBatch.DrawString(MouseFont, "AI Running: " + GameBoard.Thinker.Running, new Vector2(0, 500), Color.Black);
            if (GameBoard.Thinker.CurrentState != null)
            {
                spriteBatch.DrawString(MouseFont, "AI's Current State Space: [" + GameBoard.Thinker.CurrentState.UsedSpace.TableLocation.X +
                    "," + GameBoard.Thinker.CurrentState.UsedSpace.TableLocation.Y + "]", new Vector2(0, 520), Color.Black);
            }
            #region Test Code
            /*spriteBatch.DrawString(MouseFont, "(" + Mouse.GetState().X + ", " + Mouse.GetState().Y + ")",
                new Vector2(Mouse.GetState().X + 10, Mouse.GetState().Y), Color.Black);*/

            //Testing drawing logistics
            /*
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    spriteBatch.DrawString(MouseFont, wordarray[col, row],
                        new Vector2((((int)(col / 3)) * MaxWordDistance.X) + (((int)(col / 3) * MaxWordDistance.Y)) + BoardOffset.X,
                            ((col % 3) * MaxWordDistance.Y) + (row * 3 * MaxWordDistance.Y) +
                            ((row / 3) * MaxWordDistance.Y) + BoardOffset.Y),
                            Color.Black);
                }
            }
            */
            #endregion
            spriteBatch.End();
        }
        #endregion
        #region My Methods
        public void HandleGameStates()
        {

        }
        #endregion
    }
}
