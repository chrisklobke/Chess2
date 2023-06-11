using System;
using System.Collections.Generic;
namespace Chess2
{
    /// <summary>
    /// Represents the actual chess game.
    /// Combines all the necessary components by creating the board and players.
    /// Also sets every piece on its initial field.
    /// </summary>
    public class Game
    {
        //Global Variables
        private Board board;                    //Stores the created board globally
        private List<Player> players;           //Stores the created players
        private Player playerInTurn;            //Stores the player that is currently in turn
        private bool gameOver =false;           //Indicates if the current game is over (win or draw)

        //Constructor
        public Game(Player player1, Player player2)
        {
            //Create an array of two players and set those to the given players
            players = new List<Player>();
            this.players.Add(player1);
            this.players.Add(player2);
            //Set each players opponents player
            this.players[0].Opponent = this.players[1];
            this.players[1].Opponent = this.players[0];
            //Create a new board, place all the pieces and set the first indexed player as the playerInTurn
            board = new Board();
            placePieces();
            playerInTurn = players[0];
            this.players[0].updatePossibleFields(board);
            this.players[1].updatePossibleFields(board);
        }


        //Properties

        public Board Board {
            get => this.board;            
        }

        public List<Player> Players {
            get => this.players;
            set => this.players = value;
        }

        public Player PlayerInTurn {
            get => this.playerInTurn;
            set => this.playerInTurn = value;
        }

        public bool GameOver {
            get => this.gameOver;
            set => this.gameOver = value;
        }

        //Public Methods

        /// <summary>
        /// Finishes a move.
        /// Sets player in turn to the opponents player.
        /// </summary>
        /// <param name="player"></param>
        public void finishedMove(Player player)
        {         
            if (player == players[0])
            {
                playerInTurn = players[1];
            }
            else
            {
                playerInTurn = players[0];
            }
        }


        //Private Methods

        /// <summary>
        /// Places all pieces on the initial fields on the board.
        /// </summary>
        private void placePieces() {
            board.Fields[0, 0].CurrentPiece = players[0].Pieces[0];
            board.Fields[1, 0].CurrentPiece = players[0].Pieces[1];
            board.Fields[2, 0].CurrentPiece = players[0].Pieces[2];
            board.Fields[3, 0].CurrentPiece = players[0].Pieces[3];
            board.Fields[4, 0].CurrentPiece = players[0].Pieces[4];
            board.Fields[5, 0].CurrentPiece = players[0].Pieces[5];
            board.Fields[6, 0].CurrentPiece = players[0].Pieces[6];
            board.Fields[7, 0].CurrentPiece = players[0].Pieces[7];
            board.Fields[0, 1].CurrentPiece = players[0].Pieces[8];
            board.Fields[1, 1].CurrentPiece = players[0].Pieces[9];
            board.Fields[2, 1].CurrentPiece = players[0].Pieces[10];
            board.Fields[3, 1].CurrentPiece = players[0].Pieces[11];
            board.Fields[4, 1].CurrentPiece = players[0].Pieces[12];
            board.Fields[5, 1].CurrentPiece = players[0].Pieces[13];
            board.Fields[6, 1].CurrentPiece = players[0].Pieces[14];
            board.Fields[7, 1].CurrentPiece = players[0].Pieces[15];

            board.Fields[0, 7].CurrentPiece = players[1].Pieces[0];
            board.Fields[1, 7].CurrentPiece = players[1].Pieces[1];
            board.Fields[2, 7].CurrentPiece = players[1].Pieces[2];
            board.Fields[3, 7].CurrentPiece = players[1].Pieces[3];
            board.Fields[4, 7].CurrentPiece = players[1].Pieces[4];
            board.Fields[5, 7].CurrentPiece = players[1].Pieces[5];
            board.Fields[6, 7].CurrentPiece = players[1].Pieces[6];
            board.Fields[7, 7].CurrentPiece = players[1].Pieces[7];
            board.Fields[0, 6].CurrentPiece = players[1].Pieces[8];
            board.Fields[1, 6].CurrentPiece = players[1].Pieces[9];
            board.Fields[2, 6].CurrentPiece = players[1].Pieces[10];
            board.Fields[3, 6].CurrentPiece = players[1].Pieces[11];
            board.Fields[4, 6].CurrentPiece = players[1].Pieces[12];
            board.Fields[5, 6].CurrentPiece = players[1].Pieces[13];
            board.Fields[6, 6].CurrentPiece = players[1].Pieces[14];
            board.Fields[7, 6].CurrentPiece = players[1].Pieces[15];
        }

    }
}
