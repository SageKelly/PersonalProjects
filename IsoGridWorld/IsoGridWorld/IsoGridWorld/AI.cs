using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace IsoGridWorld
{
    public class AI
    {
        #region Variables
        #region Private
        /// <summary>
        /// Holds the unchanging version of the epsilon choice
        /// </summary>
        private float StaticOptima;
        #endregion
        #region Public
        public Texture2D Appearance;

        /// <summary>
        /// Denotes the direction the AI will go this turn.
        /// </summary>
        public Direction MovingDirection;

        /// <summary>
        /// Rolls to see which direction is taken
        /// </summary>
        Random Die;

        /// <summary>
        /// Denotes whether or not he found the cheese
        /// </summary>
        public bool CheeseFound
        /**/ {
            get;
            private set;
        }

        /// <summary>
        /// Holds the percentage of which the AI will make a random choice.
        /// </summary>
        public float Optima
        {
            get;
            private set;
        }

        /// <summary>
        /// Holds the epsilon decay value
        /// </summary>
        public float Decay
        {
            get;
            private set;
        }

        public Tile CurTile;

        public Vector2 Position;
        public Vector2 Center;

        public Rectangle CurSourceBounds;

        public Rectangle LeftSrcBounds;
        public Rectangle RightSrcBounds;
        public Rectangle UpSrcBounds;
        public Rectangle DownSrcBounds;
        #endregion
        #endregion

        /// <summary>
        /// A map-running AI
        /// </summary>
        /// <param name="epsilonOptima">What it is</param>
        /// <param name="epsilonDecay">What it is</param>
        /// <param name="appearance">How the AI will look while drawing</param>
        public AI(float epsilonOptima, float epsilonDecay, Texture2D appearance)
        {
            MovingDirection = new Direction();
            Die = new Random();
            CheeseFound = false;
            Appearance = appearance;
            StaticOptima = Optima = epsilonOptima;
            Decay = epsilonDecay;
            CurSourceBounds = appearance.Bounds;
        }

        /// <summary>
        /// Resets the AI, unoccupies the current tile, and decreases the AI's Episolon Optima value by the decay
        /// </summary>
        public void Reset()
        {
            CheeseFound = false;
            Optima -= Decay;
            CurTile.IsOccupied = false;
        }

        /// <summary>
        /// Chooses a move based on the space the AI's on
        /// </summary>
        public void Think()
        {
            if (LookForCheese())
            {
                return;
            }

            ///Then, make a random decision on those moves.
            ///
            List<DirProb> Valids = CurTile.GetValidTiles();

            Random r = new Random();

            if (Valids.Count > 1)
            {
                if (Optima > r.NextDouble())
                {
                    //choose a random direction
                    int choice = r.Next(Valids.Count);
                    MovingDirection = Valids[choice].Dir;

                    //I might have made this decision again or once before
                    Valids[choice].TrodNumber++;
                }
                else
                {
                    //pick the first (i.e. best) one
                    MovingDirection = Valids[0].Dir;
                }
            }
            else
            {
                MovingDirection = Valids[0].Dir;
            }

            //Since you're about to leave this tile...
            CurTile.IsOccupied = false;
        }

        /// <summary>
        /// Checks to see if this is the winning tile
        /// </summary>
        public bool LookForCheese()
        {
            if (CurTile.WinningTile)
            {
                CheeseFound = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Edits the AI's SARSA values
        /// </summary>
        /// <param name="εOptima"></param>
        /// <param name="εDecay"></param>
        public void EditValues(float εOptima, float εDecay)
        {
            StaticOptima = Optima = εOptima;
            Decay = εDecay;
        }
    }
}
