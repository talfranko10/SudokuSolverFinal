using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SudokuSolver
{
    class Program
    {
        public static void Main(string[] args)
        {
            Play p = new Play(); //Start game
            p.startPlay();
        }
    }
}



