using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess2
{
    /// <summary>
    /// Represents a Player in the chess game.
    /// Player can be human or AI, which are child classes of the parent class Player.
    /// </summary>
    public abstract class Player
    {
        //Global variables
        protected bool isAI;                                //Indicates if the Player is an AI or a Human Player
        protected List<Piece> pieces;                           //Stores all pieces belonging to the player
        protected List<Move> moves;                         //Stores all operated moves
        protected bool hasPawnAtOtherSide = false;          //Indicates if the player has a pawn on the last for its possible field
        protected bool suggestsDraw = false;                //Indicates if the player currently suggests a draw
        protected bool gaveUp = false;                      //Indicates if the player gave up
        protected Player opponent;                          //Stores the players opponent player
        protected List<Field> possibleFields;               //List of fields that any player's piece could reach
        private static int cnt = 0;                         //Stores the number of created players
        private int id;                                     //Unique identifier for each player
        private string name;                                //Stores the name of the player
        private Colors color;                               //Stores the color enum type (Colors.White or Colors.Black)
        private bool isCheck = false;                       //Indicates if the players king is in danger
        

        //Constructor
        public Player(string name)
        {
            //Set id to the amount of created players, increment the counter 
            this.id = cnt;
            Player.cnt++;
            this.name = name;
            //The first indexed player has always the color white
            if (this.id == 0)
            {
                this.color = Colors.White;
            }
            else {
                this.color = Colors.Black;
            }
            //Creat the players pieces
            createPieces();
            //Create an empty list to store the operated moves
            this.moves = new List<Move>();
            this.possibleFields = new List<Field>();
            
        }


        //Properties

        public string Name {
            get => this.name;            
        }
        
        public int ID {
            get => this.id;            
        }

        public Player Opponent {
            get => this.opponent;
            set => this.opponent = value;
        }

        public Colors Color {
            get => this.color;            
        }
        
        public bool IsAI {
            get => this.isAI;            
        }

        public List<Piece> Pieces {
            get => this.pieces;
        }

        public List<Move> Moves {
            get => this.moves;
        }

        public bool HasPawnOnOtherSide {
            get => this.hasPawnAtOtherSide;
        }

        public bool IsCheck {
            get => this.isCheck;
            set => this.isCheck = value;
        }

        public static int Cnt {
            set => cnt = value;
        }

        public bool SuggestsDraw {
            get => this.suggestsDraw;            
        }

        public bool GaveUp
        {
            get => this.gaveUp;
        }

        public List<Field> PossibleFields {
            get => this.possibleFields;
        }


        //Public Methods

        /// <summary>
        /// Sets all possible fields for each of the player's pieces.
        /// </summary>
        /// <param name="board"></param>
        public void updatePossibleFields(Board board) {
            this.possibleFields.Clear();
            for (int i = 0; i < this.pieces.Count; i++) {
                this.pieces[i].updatePossibleFields(board);
                this.possibleFields.AddRange(this.pieces[i].PossibleFields);
            }
            this.possibleFields.Distinct();
        }

        /// <summary>
        /// Returns true if the players King has no threadless fields to go to.
        /// => player is mate and loses the game.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool isMate(Board board) {
            List<Piece> pieces;
            //Get this players king
            Piece king = this.findFirstPieceByType(PieceType.King);
            List<Field> wayFields;
            //Mate only possible when player is already check
            if (this.isCheck) {
                //If there are no threadless fields
                if (king.getThreadlessFields(board).Count == 0) {
                    //Get a list of pieces that can reach the king's current field
                    pieces = this.opponent.getPiecesThatCanReachField(king.CurrentField, board);
                    //If any of those pieces can not be reached 
                    for (int i = 0; i < pieces.Count; i++) {
                        if (!pieces[i].canPeaceBeReachedByOpponent(board)) {
                            //Get the fields the piece has to go to reach king
                            wayFields = board.getWayFields(pieces[i].CurrentField, king.CurrentField);
                            //Check for each of those fields
                            for (int j = 0; j < wayFields.Count; j++) {
                                //If field can be reached (sacrafice) or if piece is right next to king (count == 0)
                                if (this.possibleFields.Contains(wayFields[j]) || wayFields.Count == 0) {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }                    
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a list of pieces that can reach a certain field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<Piece> getPiecesThatCanReachField(Field field, Board board) {
            List<Piece> pieces = new List<Piece>();

            for (int i = 0; i < this.pieces.Count; i++) {
                if (this.pieces[i].CurrentField != null)
                {
                    if (this.pieces[i].getPossibleFields(board, false, true).Contains(field)) {
                        pieces.Add(this.pieces[i]);
                    }
                }
            }
            return pieces;
        }

        /// <summary>
        /// //Returns the players first piece by its type from the pieces array
        /// </summary>
        /// <param name="pieceType"></param>
        /// <returns></returns>
        protected Piece findFirstPieceByType(PieceType pieceType) {
            for (int i = 0; i < this.pieces.Count; i++) {
                if (this.pieces[i].Type == pieceType) {
                    return this.pieces[i];
                }
            }
            return null;
        }


        //Public methods

        /// <summary>
        /// Makes an automatic move without needing input from the operator.
        /// Abstract since it differs between human and AI player.
        /// Throws a not-implemented exception for human players since it can only be used by AI players.
        /// </summary>
        /// <param name="board"></param>
        public abstract void makeAutomaticMove(Board board);

        /// <summary>
        /// Checks if the move of a players piece from a source to a target field is valid.
        /// Uses the isValidMove method from the Piece class.
        /// Generally checks basic rules for the move first, then checks individual pieces' validation.
        /// Either throws an explicit exception or a bool depending on the throwException-parameter value.
        /// Important since AI uses this Method to check if moves are valid and exceptions should not be thrown.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public bool isValidMove(Piece piece, Field sourceField, Field targetField, Board board, bool throwException = true)
        {            
            string exceptionMessage = "";           
            bool result = true;
            //If moved piece does exist but is not in the game (on the board) anymore
            if (piece != null && piece.CurrentField == null)
            {
                exceptionMessage = "Piece does not exist or is not in game anymore.";
                result = false;

            }
            //If piece is not moved at all
            else if (sourceField == targetField)
            {
                exceptionMessage = "Piece has to be moved.";
                result = false;
            }
            //If piece does not belong to player
            else if (piece.Player != this)
            {
                exceptionMessage = "Piece does not belong to player.";
                result = false;
            }
            //If source or target field does not exist
            else if (!board.doesFieldExist(sourceField) || !board.doesFieldExist(targetField))
            {
                exceptionMessage = "Field does not exist!";
                result = false;
            }
            //If tries to attack own piece
            else if (targetField.CurrentPiece != null && targetField.CurrentPiece.Player == piece.Player)
            {
                exceptionMessage = "Invalid Move! Can't attack own piece.";
                result = false;
            }
            //If target field has a current piece but it's not a valid move even if attacking
            else if (targetField.CurrentPiece != null && !piece.isValidMove(sourceField, targetField, board, true))
            {
                exceptionMessage = "Invalid Move!";
                result = false;
            }
            //If piece simply can not make that move
            else if (!piece.isValidMove(sourceField, targetField, board) || sourceField == targetField)
            {
                exceptionMessage = "Invalid Move!";
                result = false;
            }
            //If exception is to be thrown and the move is invalid, throw exception
            if (throwException && !result)
            {
                throw new Exception(exceptionMessage);                
            }
            //Otherwise just return the result
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Moves a piece from a source field to a target field if move is valid.
        /// Either throws an explicit exception or a bool depending on the throwException-parameter value.
        /// Important since AI uses this Method to check if moves are valid and exceptions should not be thrown.
        /// Takes care of the fields current pieces.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public bool makeMove(Piece piece, Field sourceField, Field targetField, Board board, bool throwException = true)
        {            
            //Check if move is valid
            if (isValidMove(piece, sourceField, targetField, board, throwException))
            {                
                //If move is valid, add the move to the moves list of the player
                moves.Add(new Move(piece, targetField.CurrentPiece, sourceField, targetField));

                //If the moved piece is Pawn
                if (piece.GetType() == typeof(Chess2.Pawn))
                {
                    //Check if move was En-Passant (see isEnPassant in Board class for definition)
                    if (board.isEnPassant((Pawn)piece, sourceField, targetField))
                    {
                        //Replace the passively attacked piece
                        replacePiece(targetField.Xcoord, sourceField.Ycoord, PieceType.None, board);                   
                    }
                    //If the pawn has reached the other side of the board set the player hasPawnAtOtherSide
                    if ((targetField.Ycoord == board.Fields.GetLength(1) - 1 && this.Color == Colors.White) || (targetField.Ycoord == 0 && this.Color == Colors.Black))
                    {
                        this.hasPawnAtOtherSide = true;
                    }
                }
                //Set current field of attacked piece to null
                if (targetField.CurrentPiece != null) {
                    this.opponent.pieces.Remove(targetField.CurrentPiece);
                    if (targetField.CurrentPiece.Type == PieceType.King)
                    {
                        throw new Exception("King removed!");
                    }
                }

                //Remove any possible piece from target field
                targetField.CurrentPiece = null;
                //Increment the move counter for the moved piece
                sourceField.CurrentPiece.MoveCnt++;
                //Remove piece from old field
                sourceField.CurrentPiece = null;
                //Set target field's current piece to the moved piece
                targetField.CurrentPiece = piece;

                this.updatePossibleFields(board);
                this.opponent.updatePossibleFields(board);

                //If any piece can now reach the king, move is invalid and move needs to be reset
                if (this.opponent.canAnyPieceReachOpKing(board)) {
                    //If move was attack place attacked piece back to the target field
                    if (this.moves[this.moves.Count - 1].AttackedPiece != null)
                    {
                        targetField.CurrentPiece = this.moves[this.moves.Count - 1].AttackedPiece;
                        this.opponent.pieces.Add(this.moves[this.moves.Count - 1].AttackedPiece);
                        this.opponent.pieces[this.opponent.pieces.Count - 1].PossibleFields = this.opponent.pieces[this.opponent.pieces.Count - 1].getPossibleFields(board, false, false);
                    }
                    //If no attack targetField is now empty again
                    else {
                        targetField.CurrentPiece = null;
                    }
                    //Place moved piece back to sourceField
                    sourceField.CurrentPiece = piece;
                    //Reduce move counter of moved piece
                    piece.MoveCnt--;
                    //Remove most recent move of player's move list
                    this.moves.Remove(this.moves[this.moves.Count - 1]);

                    this.updatePossibleFields(board);
                    this.opponent.updatePossibleFields(board);

                    //Return wrong since invalid move
                    return false;
                }

                //If any piece can reach the opponents King, set opponent check
                if (this.canAnyPieceReachOpKing(board))
                {
                    this.opponent.IsCheck = true;
                }
                else
                {
                    this.opponent.IsCheck = false;
                }
                return true;                
            }
            //If move was invalid return false (no exception necessary bc it would get thrown before)
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Checks if any of players pieces can reach defined field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool canAnyPieceReachField(Field field, Board board, bool isAttack)
        {
            for (int i = 0; i < this.pieces.Count; i++)
            {
                if (this.pieces[i].CurrentField != null)
                {
                    if (pieces[i].getPossibleFields(board, false, isAttack).Contains(field)) {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Counts how many moves have been made since the last attack.
        /// Implemented to determine if an AI game should end in a draw.
        /// </summary>
        /// <returns></returns>
        public int getMoveSqeuenceCountWithoutAttack()
        {
            int i = this.moves.Count - 1;
            if (i >= 0)
            {
                while (true)
                {
                    if (this.moves[i].AttackedPiece != null || i == 0)
                    {
                        break;
                    }
                    i--;
                }
                return this.moves.Count - i;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Replaces a piece at the specific coordinates with a new piece of the defined enum pieceType.
        /// Needs to use coordinates instead of Field parameter for EnPassant.
        /// </summary>
        /// <param name="xcoord"></param>
        /// <param name="ycoord"></param>
        /// <param name="pieceType"></param>
        /// <param name="board"></param>
        public void replacePiece(int xcoord, int ycoord, PieceType pieceType, Board board)
        {           
            //Store the field's current piece
            Piece piece = board.Fields[xcoord, ycoord].CurrentPiece;
            //Create variable for replacement piece
            Piece replacement = null;
            //Create new piece depending on the determined PieceType
            if (pieceType == PieceType.Pawn)
            {
                replacement = new Pawn(this);
            }
            else if (pieceType == PieceType.Rook)
            {
                replacement = new Rook(this);
            }
            else if (pieceType == PieceType.Bishop)
            {
                replacement = new Bishop(this);
            }
            else if (pieceType == PieceType.Knight)
            {
                replacement = new Knight(this);
            }
            else if (pieceType == PieceType.Queen)
            {
                replacement = new Queen(this);
            }
            else if (pieceType == PieceType.King)
            {
                replacement = new King(this);
            }
            this.pieces.Remove(piece);
            if (replacement != null) {
                board.Fields[xcoord, ycoord].CurrentPiece = replacement;
                this.updatePossibleFields(board);
                this.pieces.Add(replacement);
            }            
        }

        /// <summary>
        /// Creates the player's pieces.
        /// Generally: 8 pawns, 2 Rooks, 2 Bishops, 2 Knights, 1 Queen, 1 King
        /// </summary>
        private void createPieces() {
            this.pieces = new List<Piece>();
            pieces.Add(new Rook(this));
            pieces.Add(new Knight(this));
            pieces.Add(new Bishop(this));
            pieces.Add(new Queen(this));
            pieces.Add(new King(this));
            pieces.Add(new Bishop(this));
            pieces.Add(new Knight(this));
            pieces.Add(new Rook(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
            pieces.Add(new Pawn(this));
        }

        /// <summary>
        /// Checks if any of players pieces can reach the opponent king.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private bool canAnyPieceReachOpKing(Board board)
        {            
            //Find opponents king
            Piece opKing = this.opponent.findFirstPieceByType(PieceType.King);            
            return canAnyPieceReachField(opKing.CurrentField, board, true);
        }
    }

    /// <summary>
    /// Represents a Human Player and is child class of Player.
    /// </summary>
    public class HumanPlayer : Player
    {
        //Constructor
        public HumanPlayer(string name) : base(name)
        {
            this.isAI = false;
        }

        //Public methods

        /// <summary>
        /// See parent definition.
        /// Throws a not implemented exception for human players.
        /// </summary>
        /// <param name="board"></param>
        public override void makeAutomaticMove(Board board)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents an AI Player and is child class of Player.
    /// </summary>
    public class AIPlayer : Player
    {
        //Constructor
        public AIPlayer(string name) : base(name)
        {
            this.isAI = true;
        }

        /// <summary>
        /// See parent definition.
        /// Makes an automatic move only for AI players.
        /// </summary>
        /// <param name="board"></param>
        public override void makeAutomaticMove(Board board)
        {
            Random rnd = new Random();
            int piecesLength;
            bool madeMove = false;
            Piece piece;
            List<Piece> pieces;
            Field targetField;
            List<Field> possibleFields;

            //Create new list with a copy of players pieces to be able to remove them
            pieces = new List<Piece>(this.pieces);
            //Remember length for while loop
            piecesLength = pieces.Count;
            //Mix up to start with a random piece
            pieces.Shuffle();

            //Go through all pieces
            for (int i = 0; i < piecesLength; i++) {
                piece = pieces[i];
                //Get the possible fields for that piece
                possibleFields = piece.PossibleFields;
                //Shuffle those fields to start with random field
                possibleFields.Shuffle();
                //Go through all possible fields
                for (int j = 0; j < possibleFields.Count; j++)
                {
                    targetField = possibleFields[j];
                    //If move to that field is valid
                    if (makeMove(piece, piece.CurrentField, targetField, board, false))
                    {
                        madeMove = true;
                        break;
                    }
                }
                if (madeMove) {
                    break;
                }
            }
            if (!madeMove) {
                Console.Write("");
            }            
        }
    }

public static class Extensions
    {
        private static Random rand = new Random();

        public static void Shuffle<T>(this IList<T> values)
        {
            for (int i = values.Count - 1; i > 0; i--)
            {
                int k = rand.Next(i + 1);
                T value = values[k];
                values[k] = values[i];
                values[i] = value;
            }
        }
    }
}
