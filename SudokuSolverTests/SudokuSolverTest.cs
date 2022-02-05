using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuSolverTest
    {
        [TestMethod]
        public bool buildSudoku(string input)
        {
            InputOutput.input = input;
            if (InputOutput.stringToMatrix() == null)
                return false;
            Board board = new Board(InputOutput.board);
            if(!board.addValuesToDictionaries())
                return false;
            Solver.initHiddenSinglesDictionary(board);
            board.unsolvedCells();
            return Solver.solve(board);
        }

        [TestMethod]
        public void TestValidBoards()
        {
            string board = "1400200441320001"; //Easy 4*4
            Assert.AreEqual(buildSudoku(board), true);

            board = "000006217000240000000003060000084000060907030000600108630001870970000000000090040"; //Easy 9*9
            Assert.AreEqual(buildSudoku(board), true);

            board = "000000250000903100047050003020000700003070008100800304000300410000000009000492800"; //Easy 9*9
            Assert.AreEqual(buildSudoku(board), true);

            board = "000000132010000790970030050706402500040100020005070000301600000000324000000007000"; //Medium 9*9
            Assert.AreEqual(buildSudoku(board), true);

            board = "800000000003600000070090200050007000000045700000100030001000068008500010090000400"; //Hard 9*9
            Assert.AreEqual(buildSudoku(board), true);

            board = "6007:1004>2?800=00@>0760<900004?0;=0008<07000069<00400>0036:072096001000>:0503008<0;9002?@00010004:0>005201;000<10000:00009<?2;0?020007=0109;008300008007=4000000071@<000;0050020@083510000064<00?05000000020=002180;0=70?0000>00060<000900=0000@04:?290;5300080"; //Easy 16*16
            Assert.AreEqual(buildSudoku(board), true);

            board = "00<00010020008000003?=<001:4500000@>;007500=?30020=706800?>0410;000000?>23000000<02;=90@:05>1?07>50000000000003600180002;0009=0000?:00014000@<004;000000000000?8107<240;=0?83:0500000063<:000000@09?0<200;70=5030031500?>0027;0000057>;00<13800000;000@004000900"; //Hard 16*16
            Assert.AreEqual(buildSudoku(board), true);
        }

        [TestMethod]
        public void TestUnsolvableBoards()
        {
            string board = "1200002000010002";
            Assert.AreEqual(buildSudoku(board), false);

            board = "837050000246173985951020000328597460674030100195060000509080073402010000703040009";
            Assert.AreEqual(buildSudoku(board), false);
        }

        [TestMethod]
        public void TestIllegalkeys()
        {
            string board = "124ngl4012003004";
            Assert.AreEqual(buildSudoku(board), false);

            board = "8370500+024617398595102000032pu59746067403010019506000050908007340201000070304011";
            Assert.AreEqual(buildSudoku(board), false);

            board = "6~~~:1004>2?+*0=00@>0760<9qqq04?0;=0008<07000069<0040t>0036:072096001000>:05030z8<0;9002?@00010004:0>005201;000<10000:00009<?2;0?020007=0109;008300008007=4000000071@<000;0050020@083510000064<00?05000000020=002180;0=70?0000>00060<000900=0000@04:?290;5300080";
            Assert.AreEqual(buildSudoku(board), false);
        }

        [TestMethod]
        public void TestIllegalLength()
        {
            string board = "123400140120030043011";
            Assert.AreEqual(buildSudoku(board), false);

            board = "2197645387345981626851324799268713454732568918513497265684279133429135687197683254";
            Assert.AreEqual(buildSudoku(board), false);

            board = "17062<;:3080=00?0000@703=01000<5050@0806<0004000:00;0000000700>0@1030000?>0800;0;8:00000003>70000=;0400009008000701000004000=05>0070000:26000@00000:000004290100<?0000003160009=08<0000000000000174300:00?05600090005>;00000@400000?<020000=0020000@0000007000"; 
            Assert.AreEqual(buildSudoku(board), false);
        }

        [TestMethod]
        public void TestFullBoards()
        {
            string board = "1432321441232341";
            Assert.AreEqual(buildSudoku(board), true);

            board = "1?9732@<=:5>;846;6285:974?1@=><3>:@<4=6;8923?517453=1>8?6<7;2:@9247?>9<=@8;5316:69812;:@34>?7=5<=;5@7?3691<:>4283<:>814527=69@?;9164;<?875:=@23>@7?5:3=2;>9<1684:8>3@5791642<;=?<=;264>1?3@8:975524;961><@?783:=?><6=82:5;31479@731:<@54>=896?;28@=9?7;3:2645<>1"; 
            Assert.AreEqual(buildSudoku(board), true);
        }
    }
}
