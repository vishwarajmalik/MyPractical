using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WordSearch
{
	// word search in 8 direction
    class Program
    {
        static char[,] Grid = new char[,]{
            {'C', 'P', 'K', 'X', 'O', 'I', 'G', 'H', 'S', 'F', 'C', 'H'},
            {'Y', 'G', 'W', 'R', 'I', 'A', 'H', 'C', 'Q', 'R', 'X', 'K'},
            {'M', 'A', 'X', 'I', 'M', 'I', 'Z', 'A', 'T', 'I', 'O', 'N'},
            {'E', 'T', 'W', 'Z', 'N', 'L', 'W', 'G', 'E', 'D', 'Y', 'W'},
            {'M', 'C', 'L', 'E', 'L', 'D', 'N', 'V', 'L', 'G', 'P', 'T'},
            {'O', 'J', 'A', 'A', 'V', 'I', 'O', 'T', 'E', 'E', 'P', 'X'},
            {'C', 'D', 'B', 'P', 'H', 'I', 'A', 'W', 'V', 'X', 'U', 'I'},
            {'L', 'G', 'O', 'S', 'S', 'B', 'R', 'Q', 'I', 'A', 'P', 'K'},
            {'E', 'O', 'I', 'G', 'L', 'P', 'S', 'D', 'S', 'F', 'W', 'P'},
            {'W', 'F', 'K', 'E', 'G', 'O', 'L', 'F', 'I', 'F', 'R', 'S'},
            {'O', 'T', 'R', 'U', 'O', 'C', 'D', 'O', 'O', 'F', 'T', 'P'},
            {'C', 'A', 'R', 'P', 'E', 'T', 'R', 'W', 'N', 'G', 'V', 'Z' }
        };

        static string[] Words = new string[]
        {
            "CARPET",
            "CHAIR",
            "DOG",
            "BALL",
            "DRIVEWAY",
            "FISHING",
            "FOODCOURT",
            "FRIDGE",
            "GOLF",
            "MAXIMIZATION",
            "PUPPY",
            "SPACE",
            "TABLE",
            "TELEVISION",
            "WELCOME",
            "WINDOW"
        };

        static Dictionary<char, List<CharPosition>> dictCharPosition = new Dictionary<char, List<CharPosition>>();
        static void Main(string[] args)
        {
            Console.WriteLine("Word Search");

            List<CharPosition> lstPos;

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    Console.Write(Grid[y, x]);

                    char current = Grid[y, x];

                    // keep all all postions of all characters in Dictionary to that we can get  the 1st letter postion for each word.
                    // so that we dont need tostart from [0,0] for all words.

                    CharPosition pos = new CharPosition
                    {
                        ColNo = x,
                        RowNo = y
                    };

                    if (dictCharPosition.ContainsKey(current))
                    {
                        lstPos = dictCharPosition[current];
                    }
                    else
                    {
                        lstPos = new List<CharPosition>();
                    }
                    lstPos.Add(pos);


                    dictCharPosition[current] = lstPos;


                    Console.Write(' ');
                }
                Console.WriteLine("");

            }

            Console.WriteLine("");
            Console.WriteLine("Found Words");
            Console.WriteLine("------------------------------");

            FindWords();

            Console.WriteLine("------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }

        private static void FindWords()
        {
            //Find each of the words in the grid, outputting the start and end location of
            //each word, e.g.
            //PUPPY found at (10,7) to (10, 3);

            PrintCoordinates("GO1LF");
            //PrintCoordinates("PUPPY");
            //PrintCoordinates("LABO");

            Console.WriteLine("------------------------------");

            foreach (string word in Words)
            {
                PrintCoordinates(word);
            }
        }

        private static void PrintCoordinates(string word)
        {
            bool found = false;
            char[] chars = word.ToCharArray();
            int X, Y;

            int noOfRows = Grid.GetLength(0);
            int noOfCols = Grid.GetLength(1);
            List<CharPosition> firstLetterPos = new List<CharPosition>();
            if (dictCharPosition.ContainsKey(chars[0]))
            {
                firstLetterPos = dictCharPosition[chars[0]];
            }

            // check if all letters of word exists in dictCharPosition
            foreach (var c in chars)
            {
                if (!dictCharPosition.ContainsKey(c))
                {
                    Console.WriteLine($"{word} not found.");
                    return;
                }
            }             

            // Console.WriteLine($"{word} first letter match count is {firstLetterPos.Count}");

            List<string> lstMatchePos = new List<string>();
            for (int j = 0; j < firstLetterPos.Count; j++)
            {
                int colNo = firstLetterPos[j].ColNo;
                int rowNo = firstLetterPos[j].RowNo;

                // check left to right
                if ((noOfCols - colNo) >= chars.Length)
                {
                    lstMatchePos = MatchWord(chars, colNo, rowNo, SearchDirection.LeftToRight);
                }

                // check top to bottom
                if (lstMatchePos.Count == 0 && (noOfRows - rowNo) >= chars.Length)
                {
                    lstMatchePos = MatchWord(chars, colNo, rowNo, SearchDirection.TopToBottom);
                }

                // check right to left
                if (lstMatchePos.Count == 0 && (colNo + 1 >= chars.Length))
                {
                    lstMatchePos = MatchWordReverse(chars, colNo, rowNo, SearchDirection.RightToLeft);
                }

                // check bottom to top
                if (lstMatchePos.Count == 0 && (1 + rowNo >= chars.Length))
                {
                    lstMatchePos = MatchWordReverse(chars, colNo, rowNo, SearchDirection.BottomToTop);
                }

                // check Diagonally 

                // check UpperLeft Diagonally rowno-1, colNo-1
                if (lstMatchePos.Count == 0 && (colNo + 1 >= chars.Length && rowNo + 1 >= chars.Length))
                {
                    lstMatchePos = MatchWordDiagonally(chars, colNo, rowNo, SearchDirection.UpperLeft);
                }

                //// check UpperRight Diagonally
                if (lstMatchePos.Count == 0 && (rowNo + 1 >= chars.Length && (noOfCols - colNo) >= chars.Length))
                {
                    lstMatchePos = MatchWordDiagonally(chars, colNo, rowNo, SearchDirection.UpperRight);
                }

                //// check BottomLeft Diagonally
                if (lstMatchePos.Count == 0 && (colNo + 1 >= chars.Length && (noOfRows - rowNo) >= chars.Length))
                {
                    lstMatchePos = MatchWordDiagonally(chars, colNo, rowNo, SearchDirection.BottomLeft);
                }

                // check BottomRight Diagonally
                if (lstMatchePos.Count == 0 && ((noOfCols - colNo) >= chars.Length) && (noOfRows - rowNo) >= chars.Length)
                {
                    lstMatchePos = MatchWordDiagonally(chars, colNo, rowNo, SearchDirection.BottomRight);
                }

                if (lstMatchePos.Count == chars.Length)
                {
                    Console.WriteLine($"{word} found at {lstMatchePos.First()} to {lstMatchePos.Last()}");
                    break;
                }
            }

            if (lstMatchePos.Count == 0)
            {
                Console.WriteLine($"{word} not found");
            }
        }

        enum SearchDirection
        {
            LeftToRight = 1,
            TopToBottom = 2,
            RightToLeft = 3,
            BottomToTop = 4,
            UpperLeft = 5,
            UpperRight = 6,
            BottomLeft = 7,
            BottomRight = 8
        }

        private static List<string> MatchWord(char[] chars, int colNo, int rowNo, SearchDirection searchDirection)
        {
            bool isMatch = false;
            List<string> matchedPos = new List<string>();
            matchedPos.Add($" ({colNo}, {rowNo})");

            for (int i = 1; i < chars.Length; i++)
            {
                try
                {
                    if (searchDirection == SearchDirection.LeftToRight && chars[i] == Grid[rowNo, colNo + i])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo + i}, {rowNo})");
                    }
                    else if (searchDirection == SearchDirection.TopToBottom && chars[i] == Grid[rowNo + i, colNo])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo}, {rowNo + i})");
                    }
                    else
                    {
                        isMatch = false;
                    }

                    if (!isMatch)
                    {
                        matchedPos.Clear();
                        break;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"MatchWord {ex.Message} and rowno --> {rowNo}, colNo --> {colNo} i --> ${i}");
                }
            }
            return matchedPos;
        }

        private static List<string> MatchWordReverse(char[] chars, int colNo, int rowNo, SearchDirection searchDirection)
        {
            bool isMatch = false;
            List<string> matchedPos = new List<string>();
            matchedPos.Add($"({colNo}, {rowNo})");

            for (int i = 1; i < chars.Length; i++)
            {
                try
                {
                    if (searchDirection == SearchDirection.RightToLeft && chars[i] == Grid[rowNo, colNo - i])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo - i}, {rowNo})");
                    }
                    else if (searchDirection == SearchDirection.BottomToTop && chars[i] == Grid[rowNo - i, colNo])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo}, {rowNo - i})");
                    }
                    else
                    {
                        isMatch = false;
                    }
                    if (!isMatch)
                    {
                        matchedPos.Clear();
                        break;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"MatchWordReverse {ex.Message} and rowno --> {rowNo}, colNo --> {colNo} i --> ${i}");
                }
            }
            return matchedPos;
        }

        private static List<string> MatchWordDiagonally(char[] chars, int colNo, int rowNo, SearchDirection searchDirection)
        {
            bool isMatch = false;
            List<string> matchedPos = new List<string>
            {
                $"({colNo}, {rowNo})"
            };
            
            for (int i = 1; i < chars.Length; i++)
            {
                try
                {
                    if (searchDirection == SearchDirection.UpperLeft && chars[i] == Grid[rowNo - i, colNo - i])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo - i}, {rowNo - i})");
                    }
                    else if (searchDirection == SearchDirection.UpperRight && chars[i] == Grid[rowNo - i, colNo + i])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo + i}, {rowNo - i})");
                    }
                    else if (searchDirection == SearchDirection.BottomRight && chars[i] == Grid[rowNo + i, colNo + i])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo + i}, {rowNo + i})");
                    }
                    else if (searchDirection == SearchDirection.BottomLeft && chars[i] == Grid[rowNo + i, colNo - i])
                    {
                        isMatch = true;
                        matchedPos.Add($"({colNo - i}, {rowNo + i})");
                    }
                    else
                    {
                        isMatch = false;
                    }
                    if (!isMatch)
                    {
                        matchedPos.Clear();
                        break;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"MatchWordDiagonally {ex.Message} and rowno --> {rowNo}, colNo --> {colNo} i --> ${i}");
                }
            }
            return matchedPos;
        }
    }

    public class CharPosition
    {
        public int ColNo { get; set; }
        public int RowNo { get; set; }
    }
}
