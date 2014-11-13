using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace IsoGridWorld
{
    public enum Direction
    {
        Dummy, Up, Down, Left, Right
    }
    public class Tile
    {
        public struct Location
        {
            public int Row, Col;
            public Location(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }

        int i_trod_number;

        public int TrodNumber
        {
            get
            {
                return i_trod_number;
            }
            set
            {
                if (value > i_trod_number)
                {
                    if (DrawingColor.G > 0 && DrawingColor.B > 0)
                    {
                        col_drawing_color.G--;
                        col_drawing_color.B--;
                    }
                    i_trod_number = value;
                }
            }
        }

        private Color col_drawing_color;

        public Color DrawingColor
        {
            get
            {
                return col_drawing_color;
            }
        }

        public Image TileSprite
        {
            get;
            private set;
        }

        public Image CollapsedTileSprite
        {
            get;
            private set;
        }

        public Image SwitchSprite;

        private Texture2D FootprintTexture;

        public Vector2 Footprint
        {
            get;
            private set;
        }
        public Vector2 Position;
        public int thickness
        {
            get;
            private set;
        }
        public bool InGame
        {
            get;
            private set;
        }

        public Rectangle SrcBounds;
        public Vector2 Center;

        public string Name
        {
            get;
            private set;
        }
        public int ID;
        public bool IsOccupied;
        public bool WinningTile;
        public bool IsWall;
        private bool IsAlt;
        public bool AltSet
        {
            get;
            private set;
        }
        public Location MapLoc;
        /// <summary>
        /// Holds the reward gained over time for this tile
        /// </summary>
        public float Reward;

        public DirProb[] DirProbs;
        public Tile Left, Right, Above, Below;

        public Tile()
        {
            thickness = 0;
            IsOccupied = false;
            ID = 0;
            DirProbs = new DirProb[4];
            DirProbs[0] = new DirProb(Direction.Down, 0.0f);
            DirProbs[1] = new DirProb(Direction.Right, 0.0f);
            DirProbs[2] = new DirProb(Direction.Up, 0.0f);
            DirProbs[3] = new DirProb(Direction.Left, 0.0f);
            Reward = 0.0f;
        }

        public Tile(string name, int id, Image tileSprite)
            : this()
        {
            Name = name;
            ID = id;
            SwitchSprite = TileSprite = tileSprite;
            col_drawing_color = new Color(255, 255, 255);
            i_trod_number = 0;
        }
        public Tile(string name, int id, Image tileSprite, Rectangle srcbounds)
            : this(name, id, tileSprite)
        {
            SrcBounds = srcbounds;
        }
        public Tile(string name, int id, Image tileSprite, Rectangle srcbounds, Vector2 footprint, Vector2 center)
            : this(name, id, tileSprite, srcbounds)
        {
            Center = center;
            Footprint = footprint;
        }
        /*
        public Tile(string name, int id, int altID, Image highWall, Image lowWall, Rectangle size, Vector2 footprint, Vector2 center)
            : this(name, id, altID, highWall, lowWall, size, footprint, center)
        {
            FootprintTexture = pic_footprint;
        }
        */

        /// <summary>
        /// Gets the DirProb of the direction chosen
        /// </summary>
        /// <param name="direction">The direction chosen</param>
        /// <returns>The DirProb oject</returns>
        public DirProb GetValue(Direction direction)
        {
            foreach (DirProb DP in DirProbs)
            {
                if (DP.Dir == direction)
                {
                    return DP;
                }
            }
            return null;
        }

        public void AddCollapsedSprite(Image collapsedSprite)
        {
            CollapsedTileSprite = collapsedSprite;
            AltSet = true;
        }

        /// <summary>
        /// Switches the Wall
        /// </summary>
        public void CollapseWall()
        {
            if (CollapsedTileSprite != null)
                SwitchSprite = CollapsedTileSprite;
        }

        public void RaiseWall()
        {
            if (CollapsedTileSprite != null)
                SwitchSprite = TileSprite;
        }


        /// <summary>
        /// Adds a value to the probability and sets the validity of the direction chosen
        /// </summary>
        /// <param name="direction">The direction chosen</param>
        /// <param name="Value">The probability to be added</param>
        /// <param name="Valid">Whether or not this direction is valid</param>
        public void SetValue(Direction direction, float Value = 0.0f, bool Valid = true)
        {
            foreach (DirProb DP in DirProbs)
            {
                if (DP.Dir == direction)
                {
                    DP.Utility = Value;
                    DP.IsValid = Valid;
                    break;
                }
            }
            SortDirProbs();
        }

        /// <summary>
        /// Fetches a Dirprob with the highest probability value
        /// </summary>
        /// <returns>Returns either the best DirProb from 
        /// the list, or one of the best, if there's a tie</returns>
        public List<DirProb> GetValidTiles()
        {
            //First, ensure they're sorted
            SortDirProbs();

            List<DirProb> Valids = new List<DirProb>();

            //Then grab all of the DirProbs that are valid first
            foreach (DirProb DP in DirProbs)
            {
                if (DP.IsValid)
                    Valids.Add(DP);
            }
            return Valids;
        }

        /// <summary>
        /// Sorts the DirProbs array in descending order by the probability values
        /// </summary>
        private void SortDirProbs()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == j)
                        continue;
                    else if (i < j && DirProbs[j].Utility > DirProbs[i].Utility)
                    {
                        DirProb temp = DirProbs[j];
                        DirProbs[j] = DirProbs[i];
                        DirProbs[i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Resets trodding number and tile discoloration
        /// </summary>
        public void ResetTrodding()
        {
            col_drawing_color.B = 255;
            col_drawing_color.G = 255;
            TrodNumber = 0;
        }
    }
}
