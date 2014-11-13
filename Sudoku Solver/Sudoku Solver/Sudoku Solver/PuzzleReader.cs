using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sudoku_Solver
{
    /// <summary>
    /// Reads in the puzzle file and sets up the table. It's only
    /// to be used by the Board class.
    /// </summary>
    public static class PuzzleReader
    {
        private static StreamReader sr_puzzle_reader;
        private static string[] strarr_tokens;
        private static string[,] strarr_puzzle;
        private static string str_file_path;
        private static Board game_board;

        /// <summary>
        /// Reads in a .txt puzzle file and sets up the spaces.
        /// </summary>
        /// <param name="path">The file path for the puzzle</param>
        /// <param name="gameboard">The Board on which the puzzle file will be used</param>
        /// <returns></returns>
        public static void MakePuzzle(string path, Board gameboard)
        {
            str_file_path = path;
            game_board = gameboard;
            strarr_tokens = new string[9];
            strarr_puzzle = new string[9, 9];
            sr_puzzle_reader = new StreamReader(str_file_path);
            ReadPuzzle();

            for (int SpRow = 0; SpRow < 9; SpRow++)
            {
                for (int SpCol = 0; SpCol < 9; SpCol++)
                {
                    game_board.Spaces[SpCol, SpRow] = new Space(new Location(SpCol, SpRow));

                    if (strarr_puzzle[SpRow, SpCol] != "")
                    {
                        ///Since this only changes the local variable and not the 
                        ///property, the Square's subscription to the name change 
                        ///event will not be triggered.
                        gameboard.Spaces[SpCol, SpRow] = 
                            new Space(gameboard.Spaces[SpCol, SpRow].TableLocation,
                                true, strarr_puzzle[SpRow, SpCol]);
                    }
                }
            }
        }


        private static void ReadPuzzle()
        {
            for (int i = 0; i < 9; i++)
            {
                strarr_tokens = sr_puzzle_reader.ReadLine().Split(',');
                for (int j = 0; j < 9; j++)
                {
                    strarr_puzzle[i, j] = strarr_tokens[j];
                }
            }
        }
    }
}
