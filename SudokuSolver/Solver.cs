using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace SudokuSolver
{
    /* 
       Solver class. 
    */
    static class Solver
    {
        public static bool wrongSituation = true; //True if the board is valid, otherwise false.
        public static Board solvedSudoku = null; //Board used to store the solved sudoku.
        public static Dictionary<int, int[]> rowsHiddenSingles = new Dictionary<int, int[]>();  //Dictionary used to store the possibilities of the rows.
        public static Dictionary<int, int[]> columnsHiddenSingles = new Dictionary<int, int[]>(); //Dictionary used to store the possibilities of the columns.
        public static Dictionary<int, int[]> boxesHiddenSingles = new Dictionary<int, int[]>(); //Dictionary used to store the possibilities of the boxes.
        /* 
           The keys of each dictionary represent the row/column/box.
           The value is an array where each index represents the count of the possibilities of a number in the row/column/box.
           For example - if the possibilities of the cell (2, 3) are 1, 5, 15 so:
           rows dictionary[2][1 - 1] = 1
           rows dictionary[2][5 - 1] = 1
           rows dictionary[2][15 - 1] = 1
           columns dictionary[3][1 - 1] = 1
           columns dictionary[3][5 - 1] = 1
           columns dictionary[3][15 - 1] = 1
           boxes dictionary[0][1 - 1] = 1
           boxes dictionary[0][5 - 1] = 1
           boxes dictionary[0][15 - 1] = 1
           Where 1 represents a number and 0 represents empty.
        */

        /* 
          Check if the given number can be placed as the value of the given cell.
          Input: board (Board), cell (Cell) , number (int). 
          Returns: True if the given number can be placed as the value of the given cell, otherwise returns false.
        */
        public static bool isSafe(Board board, Cell cell, int number)
        {
            return board.rows[cell.Row][number - 1] == 0 && board.columns[cell.Col][number - 1] == 0 && board.boxes[cell.findBox(board.size)][number - 1] == 0;
        }

        /* 
          Check if the board is solved (if there aren't empty cells in the board).
          Input: board (Board). 
          Returns: True if the board is solved, otherwise returns false.
        */
        public static bool isSolved(Board board)
        {
            for (int i = 0; i < board.size; i++)
            {
                for (int j = 0; j < board.size; j++)
                {
                    if (board.sudokuBoard[i, j].Value == 0)
                        return false;
                }
            }
            return true;
        }

        /* 
          Initializes the hidden singles dictionaries.
          Input: board (Board). 
          Returns: None.
        */
        public static void initHiddenSinglesDictionary(Board board)
        {
            rowsHiddenSingles.Clear();
            columnsHiddenSingles.Clear();
            boxesHiddenSingles.Clear();
            int[] zeroArray = new int[board.size];
            Array.Clear(zeroArray, 0, zeroArray.Length);
            for (int i = 0; i < board.size; i++)
            {
                rowsHiddenSingles.Add(i, (int[])zeroArray.Clone());
                columnsHiddenSingles.Add(i, (int[])zeroArray.Clone());
                boxesHiddenSingles.Add(i, (int[])zeroArray.Clone());
            }
        }

        /* 
          Solves the board with "Naked single" tactic. 
          Naked Single means that in a specific cell where only one digit remains possible, the digit must go into that cell.
          If the board is invalid, changes the "wrongSituation" variable to false.
          Input: board (Board). 
          Returns: True if there was a change, otherwise returns false.
        */
        public static bool nakedSingle(Board board)
        {
            bool flag = false;
            foreach (Cell cell in board.sudokuBoard)
            {
                if (cell.Value == 0 && cell.possibleNumbersList.Count == 1)
                {
                    int number = cell.possibleNumbersList[0];
                    if (isSafe(board, cell, number))
                    {
                        board.sudokuBoard[cell.Row, cell.Col].Value = number;
                        updateSafeDictionaries(board, cell, number);
                        flag = true;
                    }
                    else
                        wrongSituation = false;

                }
            }
            return flag;
        }

        /* 
          Solves the board with "Hidden single" tactic. 
          A Hidden Single is a single cell remaining for a specific digit in a row, column or box.
          If the board is invalid, changes the "wrongSituation" variable to false.
          Input: board (Board). 
          Returns: True if there was a change, otherwise returns false.
        */
        public static bool hiddenSingle(Board board)
        {
            int boxNumber;
            bool flag = false;
            foreach (Cell cell in board.sudokuBoard)
            {
                if (cell.Value != 0)
                    continue;
                boxNumber = cell.findBox(board.size);
                foreach (int number in cell.possibleNumbersList)
                {
                    if (rowsHiddenSingles[cell.Row][number - 1] == 1 || columnsHiddenSingles[cell.Col][number - 1] == 1 || boxesHiddenSingles[boxNumber][number - 1] == 1)
                    {
                        if (isSafe(board, cell, number))
                        {
                            board.sudokuBoard[cell.Row, cell.Col].Value = number;
                            updateSafeDictionaries(board, cell, number);
                            flag = true;
                        }
                        else
                            wrongSituation = false;
                    }
                }
            }
            return flag;
        }

        /* 
          Solves the board with "Naked single" and "Hidden single" tactics, until they don't have an affect. 
          If the board is invalid, returns false. 
          Input: board (Board). 
          Returns: True if the board is valid, otherwise returns false.
        */
        public static bool preSolve(Board board)
        {
            while (nakedSingle(board) || hiddenSingle(board))
            {
                if (!wrongSituation)
                {
                    wrongSituation = true;
                    return false;
                }
            }
            return true;
        }

        /* 
          Updates the safe dictionaries, increases each value by one.
          Input: board (Board), cell (Cell), number (int).
          Returns: None.
        */
        public static void updateSafeDictionaries(Board board, Cell cell, int number)
        { 
            board.rows[cell.Row][number - 1]++;
            board.columns[cell.Col][number - 1]++;
            board.boxes[cell.findBox(board.size)][number - 1]++;
        }

        /* 
          Gets a cell and updates it's possibilities.
          Input: board (Board), cell (Cell).
          Returns: None.
        */
        public static void updateCellPossibilities(Board board, Cell cell)
        {
            for (int i = 0; i < cell.possibleNumbersList.Count; i++)
            {
                if (!isSafe(board, cell, cell.possibleNumbersList[i]))
                    cell.possibleNumbersList.Remove(cell.possibleNumbersList[i]);
            }
        }

        /* 
          Finds the cell with the minimum possibilities.
          Input: board (Board).
          Returns: Minimum cell (Cell).
        */
        public static Cell minCell(Board board)
        {
            int min = board.size + 1;
            Cell minCell = null;
            for (int i = 0; i < board.size; i++)
            {
                for (int j = 0; j < board.size; j++)
                {
                    if (board.sudokuBoard[i, j].Value == 0)
                    {
                        updateCellPossibilities(board, board.sudokuBoard[i, j]);
                        if (board.sudokuBoard[i, j].possibleNumbersList.Count < min && board.sudokuBoard[i, j].possibleNumbersList.Count != 0)
                        {
                            minCell = board.sudokuBoard[i, j];
                            min = board.sudokuBoard[i, j].possibleNumbersList.Count;
                        }
                    }
                }
            }
            return minCell;
        }

        /* 
          Gets a board and returns a copy of it.
          Input: board (Board).
          Returns: Copy of the board (Board).
        */  
        public static Board copyBoard(Board board)
        {
            Board newBoard = new Board();
            newBoard.size = board.size;
            newBoard.sudokuBoard = new Cell[board.size, board.size];
            for (int i = 0; i < board.size; i++)
            {
                for (int j = 0; j < board.size; j++)
                {
                    newBoard.sudokuBoard[i, j] = new Cell(board.sudokuBoard[i, j]);
                }
                newBoard.rows[i] = (int[])board.rows[i].Clone();
                newBoard.columns[i] = (int[])board.columns[i].Clone();
                newBoard.boxes[i] = (int[])board.boxes[i].Clone();
            }
            return newBoard;
        }

        /* 
          Recursion to solve the board.
          Input: board (Board).
          Returns: True if the board is solved, otherwise returns false (bool).
        */
        public static bool solve(Board board)
        {
            if (isSolved(board))
            {
                solvedSudoku = board;
                return true;
            }

            Cell cellToCheck = minCell(board);

            if (cellToCheck == null)
                return false;

            if (!preSolve(board))
                return false;

            Board copiedBoard = copyBoard(board);

            for (int i = 0; i < cellToCheck.possibleNumbersList.Count; i++)
            {
                int number = cellToCheck.possibleNumbersList[i];
                board.sudokuBoard[cellToCheck.Row, cellToCheck.Col].Value = number;
                board.sudokuBoard[cellToCheck.Row, cellToCheck.Col].possibleNumbersList.Remove(number);
                updateSafeDictionaries(board, cellToCheck, number);
                if (solve(board))
                    return true;
                board = copyBoard(copiedBoard);
                cellToCheck = board.sudokuBoard[cellToCheck.Row, cellToCheck.Col];
            }
            return false;
        }
    }

}
