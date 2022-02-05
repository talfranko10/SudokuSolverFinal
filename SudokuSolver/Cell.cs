using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    /* 
        Cell class. 
    */
    class Cell
    {
        private int row; //The row of the cell.
        private int col; //The col of the cell.
        private int value; //The value of the cell.
        public List<int> possibleNumbersList = new List<int>(); //List of the all possible numbers that can be placed as the value of the cell.

        /* 
          Constructor of a cell. Creates a new cell with the given values of row, column and the value of the cell.
          Input: Row, col and value (int).
          Returns: None.
        */
        public Cell(int row, int col, int value)
        {
            this.row = row;
            this.col = col;
            this.value = value;
        }

        /* 
          Copy constructor of a cell. Creates a copy of the given cell.
          Input: cell (Cell).
          Returns: None.
        */
        public Cell(Cell cell)
        {
            row = cell.Row;
            col = cell.col;
            value = cell.value;
            foreach (int number in cell.possibleNumbersList)
            {
                possibleNumbersList.Add(number);
            }
        }

        /* 
          Get and Set of "row".
          Input: None for get function, int for set function.
          Returns: Int for get function, none for set function.
        */
        public int Col
        {
            get => col;
            set => col = value;
        }

        /* 
          Get and Set of "col".
          Input: None for get function, int for set function.
          Returns: Int for get function, none for set function.
        */
        public int Row
        {
            get => row;
            set => row = value;
        }

        /* 
          Get and Set of "value".
          Input: None for get function, int for set function.
          Returns: Int for get function, none for set function.
        */
        public int Value
        {
            get => value;
            set => this.value = value;
        }

        /* 
          Calcultes the box number of the cell.
          Input: Size (int) - the size of the board.
          Returns: The box number of the cell.
        */
        public int findBox(int size)
        {
            size = (int)Math.Sqrt(size);
            int index = row * size + col / size;
            int rows = index / (size * size);
            int columns = index % size;
            return columns + rows * size;
        }

        /* 
          Finds all the possible numbers that can be placed as the value of the cell 
          and updates the HiddenSingles dictionaries.
          Input: board (Board).
          Returns: None.
        */
        public void findPossibleNumbers(Board board)
        {
            int boxNumber = findBox(board.size);
            for (int number = 1; number <= board.size; number++)
            {
                if (Solver.isSafe(board, this, number))
                {
                    possibleNumbersList.Add(number);
                    Solver.rowsHiddenSingles[row][number - 1]++;
                    Solver.columnsHiddenSingles[col][number - 1]++;
                    Solver.boxesHiddenSingles[boxNumber][number - 1]++;
                }
            }
        }
    }
}