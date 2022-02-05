using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Play
    {
        private int[,] matrix; //Matrix of the board data.

        /* 
          The main function of the game. 
          Input: None.
          Returns: None.
        */
        public void startPlay()
        {
            string startText = @"   _____           _       _             _____       _                
  / ____|         | |     | |           / ____|     | |               
 | (___  _   _  __| | ___ | | ___   _  | (___   ___ | |_   _____ _ __ 
  \___ \| | | |/ _` |/ _ \| |/ / | | |  \___ \ / _ \| \ \ / / _ \ '__|
  ____) | |_| | (_| | (_) |   <| |_| |  ____) | (_) | |\ V /  __/ |   
 |_____/ \__,_|\__,_|\___/|_|\_\\__,_| |_____/ \___/|_| \_/ \___|_|   
                                                                      
                                                                      ";
           
            Console.WriteLine(startText);
            Console.WriteLine("Welcome!");
            char tav;
            Board board = null;
            while (true)
            {
                while (true)
                {
                    Console.WriteLine("\nPlease enter F for file or S for string.");
                    try
                    {
                        tav = char.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        tav = 'A';
                    }
                    if (tav == 'F')
                        matrix = InputOutput.getDataFromFile();
                    else if (tav == 'S')
                        matrix = InputOutput.getDataFromString();
                    if (tav == 'F' || tav == 'S')
                    {
                        board = new Board(matrix);
                        if (board.addValuesToDictionaries()) //Addes the right values to the dictionaries.
                            break;
                        Console.WriteLine("Double values. Illegal board.");
                    }
                    else
                        Console.WriteLine("\nWrong input. Please try again.");
                }
                board.printBoard();
                Solver.initHiddenSinglesDictionary(board); //Initializes the hidden singles dictionaries.
                board.unsolvedCells(); //Creates a list of the possible numbers.
                DateTime t1 = DateTime.Now;
                if (Solver.solve(board)) //Solves the sudoku.
                {
                    board = Solver.solvedSudoku; //The sulotion of the sudoku.
                    TimeSpan RunTime = DateTime.Now - t1;
                    Console.WriteLine("\n\nRun Time: " + RunTime + "\n\n");
                    board.printBoard();
                    if (tav == 'F')
                    {
                        while (true)
                        {
                            Console.WriteLine("Do you want to save the file?\nEnter Y for yes or N for no.");
                            try
                            {
                                tav = char.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                tav = 'A';
                            }
                            if (tav == 'N')
                                break;
                            else if (tav == 'Y')
                            {
                                InputOutput.saveDataToFile(board);
                                break;
                            }
                            else
                                Console.WriteLine("\nWrong input. Please try again.");
                        }
                    }
                    else if (tav == 'S')
                        InputOutput.saveDataToString(board);
                }
                else
                {
                    Console.WriteLine("Failed");
                }
                while (true)
                {
                    Console.WriteLine("\nEnter C to continue or E to exit.");
                    try
                    {
                        tav = char.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        tav = 'A';
                    }
                    if (tav != 'E' && tav != 'C')
                        Console.WriteLine("\nWrong input. Please try again.");
                    else
                        break;
                }
                if (tav == 'E')
                {
                    break;
                }
            }
            Console.WriteLine("\nGoodbye!\n");
        }
    }
}
