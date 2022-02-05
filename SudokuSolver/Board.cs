using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    /* 
        Board class. 
    */
    class Board
    {
        public Cell[,] sudokuBoard; //Cells matrix of the board.
        public int size; //The size of the board.
        public Dictionary<int, int[]> rows = new Dictionary<int, int[]>(); //Dictionary used to store whether a number is in a row.
        public Dictionary<int, int[]> columns = new Dictionary<int, int[]>(); //Dictionary used to store whether a number is in a column.
        public Dictionary<int, int[]> boxes = new Dictionary<int, int[]>(); //Dictionary used to store whether a number is in a box.
        /* 
           The keys of each dictionary represent the row/column/box.
           The value is an array where each index represents the count number in the row/column/box.
           For example - if the value of the cell (2, 3) is 5 so:
           rows dictionary[2][5 - 1] = 1
           columns dictionary[3][5 - 1] = 1
           boxes dictionary[0][5 - 1] = 1
        */


        /* 
          Constructor of a board. Creates an array of cells and initializes the indexed dictionaries.
          Input: sudokuBoard - int matrix that contains all the values of the board.
          Returns: None.
        */
        public Board(int[,] sudokuBoard)
        {
            size = (int)Math.Sqrt(sudokuBoard.Length);
            this.sudokuBoard = new Cell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.sudokuBoard[i, j] = new Cell(i, j, sudokuBoard[i, j]);
                }
            }
            initDictionaries();
        }

        /* 
         Empty constructor.
         Input: None.
         Returns: None.
       */
        public Board() {}

        /* 
          Initializes the indexes dictionaries. Each dictionary contains "size" times array of zeros.
          Input: None.
          Returns: None.
        */
        public void initDictionaries()
        {
            int[] zeroArray = new int[size];
            Array.Clear(zeroArray, 0, zeroArray.Length);
            for (int i = 0; i < size; i++)
            {
                rows.Add(i, (int[])zeroArray.Clone());
                columns.Add(i, (int[])zeroArray.Clone());
                boxes.Add(i, (int[])zeroArray.Clone());
            }
        }

        /* 
          Addes the right values to the dictionaries. If there is a double value in a row/column or box, returns false.  
          Input: None.
          Returns: bool.
        */
        public bool addValuesToDictionaries()
        {
            int boxNumber;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (sudokuBoard[i, j].Value != 0)
                    {
                        boxNumber = sudokuBoard[i, j].findBox(size);
                        if (rows[i][sudokuBoard[i, j].Value - 1] == 1 || columns[j][sudokuBoard[i, j].Value - 1] == 1 || boxes[boxNumber][sudokuBoard[i, j].Value - 1] == 1)
                            return false;
                        rows[i][sudokuBoard[i, j].Value - 1]++;
                        columns[j][sudokuBoard[i, j].Value - 1]++;
                        boxes[boxNumber][sudokuBoard[i, j].Value - 1]++;
                    }
                }
            }
            return true;
        }

        /* 
          Loops over all the empty cells and creates a list of the possible numbers that can be placed as the value of the cell.
          Input: None.
          Returns: None.
        */
        public void unsolvedCells()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (sudokuBoard[i, j].Value == 0)
                        sudokuBoard[i, j].findPossibleNumbers(this);
                }
            }
        }

        /* 
          Gets a string of a line and expands it to the size of the board.
          Input: Line (string).
          Returns: Expended line (string).
        */
        public string expandLine(string line)
        {
            string s0 = new StringBuilder().Insert(0, line.Substring(1, 4), (int)Math.Sqrt(size) - 1).ToString();
            string s1 = s0 + line.Substring(5, 4);
            string s2 = new StringBuilder().Insert(0, s1, (int)Math.Sqrt(size - 1)).ToString() + s0;
            return line[0] + s2 + line.Substring(9, 4);
        }

        /* 
          Prints the board nicely.
          Input: number (int).
          Returns: True if a given number is a perfect square, otherwise returns false - (bool).
        */
        public void printBoard()
        {
            string line0 = expandLine("╔═══╤═══╦═══╗");
            string line1 = expandLine("╟───┼───╫───╢");
            string line2 = expandLine("╠═══╪═══╬═══╣");
            string line3 = expandLine("╚═══╧═══╩═══╝");

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("       ");
            for (int i = 0; i < size; i++)
            {
                if ((i + 1) < 9)
                    Console.Write(i + 1 + "   ");
                else
                    Console.Write(i + 1 + "  ");
            }
            Console.Write("\n     ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(line0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (i + 1 < 10)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("  " + (i + 1) + "  ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("║");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("  " + (i + 1) + " ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("║");
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;

                    if (sudokuBoard[i, j].Value == 0)
                        Console.Write("   ");
                    else if (sudokuBoard[i, j].Value < 10)
                        Console.Write(" " + sudokuBoard[i, j].Value + " ");
                    else
                        Console.Write(" " +  sudokuBoard[i, j].Value);
                    if (j != size - 1 && (j + 1) % Math.Sqrt(size) != 0)
                        Console.Write("│");
                    else if (j == size - 1)
                        Console.WriteLine("║");
                    else
                        Console.Write("║");
                }

                Console.BackgroundColor = ConsoleColor.Black;
                if (i != size - 1 && i != 0 && (i + 1) % Math.Sqrt(size) == 0)
                {
                    Console.Write("     ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(line2);
                }
                else if (i != size - 1)
                {
                    Console.Write("     ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(line1);
                }
            }
            Console.Write("     ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(line3);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

        }

        /* 
          Returns a string of the board.
          Input: None.
          Return: String of the board.
        */
        public override string ToString()
        {
            string boardString = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    boardString += ((char)(sudokuBoard[i, j].Value + '0')).ToString();
                }
            }
            return boardString;
        }
    }
}
