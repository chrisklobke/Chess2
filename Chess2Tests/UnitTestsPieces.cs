using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess2;

namespace Chess2Tests
{
    [TestClass]
    public class PawnTests
    {
        //Passing Tests
        [TestMethod]
        public void TestPawnMovesOneStepForwardWhite()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player1);
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player1.makeMove(pawn, game.Board.Fields[0, 3], game.Board.Fields[0, 4], game.Board);
        }

        [TestMethod]
        public void TestPawnMovesOneStepForwardBlack()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player2);
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player2.makeMove(pawn, game.Board.Fields[0, 4], game.Board.Fields[0, 3], game.Board);
        }

        [TestMethod]
        public void TestPawnMovesTwStepForwardOnFirstMoveWhite()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player1);
            pawn.MoveCnt = 0;
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player1.makeMove(pawn, game.Board.Fields[0, 3], game.Board.Fields[0, 5], game.Board);
        }

        [TestMethod]
        public void TestPawnMovesTwStepForwardOnFirstMoveBlack()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player2);
            pawn.MoveCnt = 0;
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player2.makeMove(pawn, game.Board.Fields[0, 5], game.Board.Fields[0, 3], game.Board);
        }

        [TestMethod]
        public void TestPawnLegalEnPassantWhite()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn1 = new Pawn(player1);
            Pawn pawn2 = new Pawn(player2);

            game.Board.Fields[4, 4].CurrentPiece = pawn1;
            game.Board.Fields[3, 6].CurrentPiece = pawn2;

            player2.makeMove(pawn2, game.Board.Fields[3, 6], game.Board.Fields[3, 4], game.Board);
            player1.makeMove(pawn1, game.Board.Fields[4, 4], game.Board.Fields[3, 5], game.Board);                    
        }

        //Failing Tests
        /*
        [TestMethod]
        public void TestPawnMovesOneStepBackwardsWhite()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player1);

            Pawn pawn = new Pawn(player2);
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player1.makeMove(pawn, game.Board.Fields[0, 4], game.Board.Fields[0, 3], game.Board);
        }
        
        [TestMethod]
        public void TestPawnMovesOneStepBackwardsBlack()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player2);
            game.Board.Fields[0, 3].CurrentPiece = pawn;
            
            player2.makeMove(pawn, game.Board.Fields[0, 3], game.Board.Fields[0, 4], game.Board);
            
        }
        
        [TestMethod]
        public void TestPawnMovesTwStepForwardOnSecondtMoveWhite()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player1);
            pawn.MoveCnt = 1;
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player1.makeMove(pawn, game.Board.Fields[0, 3], game.Board.Fields[0, 5], game.Board);
        }
        
        [TestMethod]
        public void TestPawnMovesTwStepForwardOnSecondMoveBlack()
        {
            Player player1 = new Player("White");
            Player player2 = new Player("Black");
            Game game = new Game(player1, player2);

            Pawn pawn = new Pawn(player2);
            pawn.MoveCnt = 1;
            game.Board.Fields[0, 3].CurrentPiece = pawn;

            player2.makeMove(pawn, game.Board.Fields[0, 5], game.Board.Fields[0, 3], game.Board);
        }
        */
    }
}
