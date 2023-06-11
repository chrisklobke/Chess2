using System;
using System.Collections.Generic;
using System.Threading;
namespace Chess2
{
    class Program
    {
        static void Main(string[] args)
        {
            int xcoordSource;
            int ycoordSource;
            int xcoordTarget;
            int ycoordTarget;
            List<String> messagesP1 = new List<string>();
            List<String> messagesP2 = new List<string>();
            Piece piece;
            string input;

            while (true)
            {
                Player.Cnt = 0;
                Player player1 = new AIPlayer("Player 1");
                Player player2 = new AIPlayer("Player 2");
                Game game = new Game(player1, player2);

                Console.WriteLine("Player1: " + game.Players[0].ID.ToString() + " " + game.Players[0].Color);
                Console.WriteLine("Player2: " + game.Players[1].ID.ToString() + " " + game.Players[1].Color);

                messagesP1.Clear();
                messagesP2.Clear();

                drawBoard(game);
                while (!game.GameOver)
                {
                    //try
                    //{
                        if (game.PlayerInTurn.IsAI)
                        {
                            //Console.ReadLine();
                            game.PlayerInTurn.makeAutomaticMove(game.Board);
                            Move move = game.PlayerInTurn.Moves[game.PlayerInTurn.Moves.Count - 1];
                        if (game.PlayerInTurn == game.Players[0])
                        {
                            messagesP1.Add(move.MovedPiece.Name + " from " + move.SourceField.Coord + " to " + move.TargetField.Coord);
                        }
                        else {
                            messagesP2.Add(move.MovedPiece.Name + " from " + move.SourceField.Coord + " to " + move.TargetField.Coord);
                        }
                        if (messagesP1.Count > 2 && messagesP2.Count > 2) {
                            if (messagesP1[messagesP1.Count - 1] == messagesP1[messagesP1.Count - 2]) {
                                Console.Write("");
                            }
                            if (messagesP2[messagesP2.Count - 1] == messagesP2[messagesP2.Count - 2])
                            {
                                Console.Write("");
                            }
                        }
                            
                            Console.WriteLine(move.MovedPiece.Name + " from " + move.SourceField.Coord + " to " + move.TargetField.Coord);

                            drawBoard(game);
                        }
                        else
                        {
                            input = Console.ReadLine();
                            xcoordSource = Field.getIntCoord((Char)input.Substring(0, 1).ToCharArray()[0]);
                            ycoordSource = Int32.Parse(input.Substring(1, input.Length - 1)) - 1;

                            input = Console.ReadLine();
                            xcoordTarget = Field.getIntCoord((Char)input.Substring(0, 1).ToCharArray()[0]);
                            ycoordTarget = Int32.Parse(input.Substring(1, input.Length - 1)) - 1;
                        
                            piece = game.Board.Fields[xcoordSource, ycoordSource].CurrentPiece;
                            
                            game.PlayerInTurn.makeMove(piece, game.Board.Fields[xcoordSource, ycoordSource], game.Board.Fields[xcoordTarget, ycoordTarget], game.Board);
                            Console.WriteLine(piece.Name + " from " + game.PlayerInTurn.Moves[game.PlayerInTurn.Moves.Count - 1].SourceField.Coord + " to " + game.PlayerInTurn.Moves[game.PlayerInTurn.Moves.Count - 1].TargetField.Coord);


                            drawBoard(game);

                            if (game.PlayerInTurn.HasPawnOnOtherSide)
                            {
                                while (true)
                                {
                                    Console.WriteLine("Choose piece to substitude: R, B, N, Q");
                                    input = Console.ReadLine();
                                    if (input == "R")
                                    {
                                        game.PlayerInTurn.replacePiece(xcoordTarget, ycoordTarget, PieceType.Rook, game.Board);
                                        drawBoard(game);
                                        break;
                                    }
                                    else if (input == "B")
                                    {
                                        game.PlayerInTurn.replacePiece(xcoordTarget, ycoordTarget, PieceType.Bishop, game.Board);
                                        drawBoard(game);
                                        break;
                                    }
                                    else if (input == "N")
                                    {
                                        game.PlayerInTurn.replacePiece(xcoordTarget, ycoordTarget, PieceType.Knight, game.Board);
                                        drawBoard(game);
                                        break;
                                    }
                                    else if (input == "Q")
                                    {
                                        game.PlayerInTurn.replacePiece(xcoordTarget, ycoordTarget, PieceType.Queen, game.Board);
                                        drawBoard(game);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Input.");
                                    }
                                }
                            }
                        }
                        if (game.PlayerInTurn.Opponent.IsCheck) {
                            Console.WriteLine(game.PlayerInTurn.Opponent.Name + " is check!");
                        }                        
                        game.finishedMove(game.PlayerInTurn);

                    if (game.PlayerInTurn.GaveUp) {
                        Console.WriteLine(game.PlayerInTurn.Name + " gives up.");
                        game.GameOver = true;
                    }
                    else if (game.PlayerInTurn.getMoveSqeuenceCountWithoutAttack() > 20 && game.PlayerInTurn.Opponent.getMoveSqeuenceCountWithoutAttack() > 20)
                    {
                        Console.WriteLine("Both players agree on a draw.");
                        game.GameOver = true;
                    }
                    else if (game.PlayerInTurn.SuggestsDraw && game.PlayerInTurn.Opponent.SuggestsDraw)
                    {
                        Console.WriteLine("Both players agree on a draw.");
                        game.GameOver = true;
                    }
                    else if (game.PlayerInTurn.isMate(game.Board)) {
                        Console.WriteLine(game.PlayerInTurn.Name + " is mate.");
                        game.GameOver = true;
                    }
                    else if (game.PlayerInTurn.Opponent.isMate(game.Board))
                    {
                        Console.WriteLine(game.PlayerInTurn.Name + " is mate.");
                        game.GameOver = true;
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
                game.Players.Clear();                
                Console.WriteLine("Press any key for a rematch.");
                Console.ReadLine();
            }
        }

        static void drawBoard(Game game)
        {
            for (int i = -1; i < game.Board.Fields.GetLength(0) + 1; i++)
            {
                for (int j = -1; j < game.Board.Fields.GetLength(1) + 1; j++)
                {
                    if (i == -1 || i == game.Board.Fields.GetLength(0))
                    {
                        if (j == -1 || j == game.Board.Fields.GetLength(1))
                        {
                            Console.Write("   ");
                        }
                        else if (j < game.Board.Fields.GetLength(1))
                        {
                            Console.Write((j + 1).ToString() + "   ");
                        }
                    }

                    if (j == -1 || j == game.Board.Fields.GetLength(1))
                    {
                        if (i >= 0 && i < game.Board.Fields.GetLength(0))
                        {
                            Console.Write(Field.getCharCoord(i) + "  ");
                        }
                    }
                    if (i >= 0 && j >= 0 && i < game.Board.Fields.GetLength(0) && j < game.Board.Fields.GetLength(0))
                    {
                        if (game.Board.Fields[i, j].CurrentPiece != null)
                        {
                            Console.Write("|" + game.Board.Fields[i, j].CurrentPiece.ShortName + "|");
                        }
                        else
                        {
                            Console.Write("|__|");
                        }
                    }
                }
                Console.WriteLine(" ");
            }
        }

    }
}
