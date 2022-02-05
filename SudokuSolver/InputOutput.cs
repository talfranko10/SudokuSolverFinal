using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SudokuSolver
{
    /* 
        InputOutput class. 
    */
    static class InputOutput
    {
        public static string input; //The input from the user.
        public static int[,] board; //The matrix of the input.
        public static int size; //The size of the board.
        public static string path; //The path to the file.

        /* 
          Checks if a given number is a perfect square.
          Input: number (int).
          Returns: True if a given number is a perfect square, otherwise returns false - (bool).
        */
        public static bool isPerferctSquare(int number)
        {
            return Math.Sqrt(number) % 1 == 0;
        }

        /* 
          Checks if a board is valid. Checks whether the board size is a perfect square and 
          all of its numbers are greater than 0 but smaller than the size of the board.
          Input: None.
          Returns: True if the board is valid, otherwise returns false - (bool).
        */
        public static string isValidBoard()
        {
            if (!isPerferctSquare(input.Length))
                return "Invalid size of board.";
            char[] inputArr = input.ToCharArray();
            board = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (inputArr[i * size + j] - '0' > size || inputArr[i * size + j] - '0' < 0)
                        return "Invalid characters.";
                    board[i, j] = inputArr[i * size + j] - '0';
                }
            }
            return "true";
        }

        /* 
          Converts the string input to a matrix.
          Input: None.
          Returns: Matrix of the data - (int[,]).
        */
        public static int[,] stringToMatrix()
        {
            size = (int)Math.Sqrt(input.Length);
            string result = isValidBoard();
            Console.WriteLine(result);
            if (result == "true")
                return board;
            return null;
        }


        /* 
          Gets data from a file and creates a matrix if the data is a vaild board.
          Input: None.
          Returns: Matrix of the data - (int[,]).
        */
        public static int[,] getDataFromFile()
        {
            while (true)
            {
                Console.WriteLine("\nEnter a path: ");
                path = Console.ReadLine();
                try
                {
                    input = File.ReadAllText(path);
                    if (stringToMatrix() != null)
                        return board;
                    //Console.WriteLine("Invalid input in file. Please try again.\n");
                }
                catch
                {
                    Console.WriteLine("Invalid file path. Please try again.\n");
                }
            }
        }

        /* 
          Gets data from the console and creates a matrix if the data is a vaild board.
          Input: None.
          Returns: Matrix of the data - (int[,]).
        */
        public static int[,] getDataFromString()
        {
            while (true)
            {
                Console.WriteLine("\nEnter a string: ");
                byte[] inputBuffer = new byte[4096];
                Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
                Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
                input = Console.ReadLine();
                if (stringToMatrix() != null)
                    return board;
                //Console.WriteLine("Invalid input, try again.\n");
            }
        }

        /* 
          Saves the data to the file.
          Input: board (Board).
          Returns: None.
        */
        public static void saveDataToFile(Board board)
        {
            path = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "Solution.txt";
            if (!File.Exists(path))
                File.Create(path).Dispose();
            File.WriteAllText(path, board.ToString());
            Console.WriteLine("\nThe solution was saved in " + path + "\n");
        }

        /* 
          Prints the data to the console (string format).
          Input: board (Board).
          Returns: None.
        */
        public static void saveDataToString(Board board)
        {
            Console.WriteLine("\nThe solution in string format:\n" + board.ToString() + "\n");
        }
    }
}