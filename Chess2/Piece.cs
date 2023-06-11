using System;
using System.Collections.Generic;
using System.Linq;
namespace Chess2
{
    /// <summary>
    /// A Class that represents a chess piece.
    /// All implemented pieces are child classes of the abstract parent Piece class.    
    /// </summary>
    public abstract class Piece
    {
        //Global variables
        protected Player player;                        //Stores the player the piece belongs to
        protected int moveCnt = 0;                      //Stores the moves the piece has made
        protected PieceType pieceType;                  //Stores the custom enum piece type (Pawn, Bishop, Rook, Knight, Queen, King)
        protected List<Field> possibleFields;           
        private static int cnt = 0;                     //Counts the instances of Piece
        private int id;                                 //Unique Identifier for each piece
        private Field currentField;                     //Sotres the current field the piece is on
                                                                

        //Constructor
        public Piece(Player player)
        {
            //Set the ID to the current amount of created pieces and increment            
            this.id = cnt;
            Piece.cnt++;
            this.player = player;
        }

        //Properties

        public int ID {
            get => this.id;
        }

        public int MoveCnt {
            get => this.moveCnt;
            set => this.moveCnt = value;
        }

        public Player Player {
            get => this.player;
        }

        public PieceType Type {
            get => this.pieceType;
        }

        public Field CurrentField {
            get => this.currentField;
            set => this.currentField = value;
        }

        public List<Field> PossibleFields {
            get => this.possibleFields;
            set => this.possibleFields = value;
        }

        //Abstract Properties, each differs from individual piece to piece

        public abstract string Name { get; }            //Generally returns the color of the owner player plus the classes name            
        public abstract string ShortName { get; }       //Generally returns the first char of the owner player's color plus the first char of the class name    


        //Abstract Methods

        /// <summary>
        /// Checks if the move of its piece from one field to another is valid.
        /// Only checks invalid moves and returns true if none of them are the case.
        /// Makes decision based off if the move is an attack.
        /// Returns a bool.
        /// Differs widely from piece to piece, hence abstract.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public abstract bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false);

        /// <summary>
        /// Uses getPossibleFields to set all possible fields for this piece.
        /// </summary>
        /// <param name="board"></param>
        public void updatePossibleFields(Board board) {
            this.possibleFields = this.getPossibleFields(board, false, false);
        }


        //Public Methods

        /// <summary>
        /// Returns a List of fields that are possible for the piece to move to.
        /// Can ignore taken fields in order to make them possible fields as well, even if taken by own player.
        /// That is important to calculate the safe fields since same-player-piece next to it can be taken and then be taken by this piece again.
        /// Example: King is surrounded only by same-player-pieces, opponent Queen attacks one of them, now King can attack queen.
        /// Hence: those fields can be possible as well, depending on the moves the individual piece can make.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="ignoreTakenField"></param>
        /// <returns></returns>
        public List<Field> getPossibleFields(Board board, bool ignoreTakenField, bool isAttack) {            
            //Create a fields list for the output
            List<Field> fields = new List<Field>();            
            //Look at each field on the board (two dimensional array)
            for (int i = 0; i < board.Fields.GetLength(0); i++) {
                for (int j = 0; j < board.Fields.GetLength(1); j++) {
                    //Only if move to i,j field is valid and if the field is not equal to the field the piece is currently on                    
                    if (isValidMove(this.currentField, board.Fields[i, j], board, isAttack) && this.currentField != board.Fields[i, j]) {
                        //If that field has a current piece on it                        
                        if (board.Fields[i, j].CurrentPiece != null)
                        {                            
                            //Check if the current player of that piece is either opponents piece or if ignoreTakenField is set
                            //If it's an opponent players piece, it can be taken and is possible to move to
                            //If ignoreTakenField is set, any taken field should be considered as possible to move to
                            if (this.Player != board.Fields[i, j].CurrentPiece.Player || ignoreTakenField)
                            {
                                fields.Add(board.Fields[i, j]);
                            }
                        }
                        //If field has no current piece, it's possible to move to
                        else {
                            fields.Add(board.Fields[i, j]);
                        }                        
                    }
                }                
            }
            return fields;
        }

        /// <summary>
        /// Chekcks if the piece can be reached by any opponent's piece.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool canPeaceBeReachedByOpponent(Board board) {
            return this.player.Opponent.canAnyPieceReachField(this.currentField, board, true);
        }

        /// <summary>
        /// Returns a list of fields the piece can go to without being threaten.
        /// Important function in order to determine a mate situation.        
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<Field> getThreadlessFields(Board board)
        {           
            //Get all possible fields of this piece            
            List<Field> possibleFields = this.getPossibleFields(board, true, true);

            //Remove King from its current position so other places can "jump" over that field
            this.currentField.CurrentPiece = null;

            //If all possible fields have own player pieces, they are all threadless
            if (possibleFields.All(x => x.CurrentPiece != null))
            {
                if (possibleFields.All(x => x.CurrentPiece.player == this.player))
                {
                    return possibleFields;
                }
            }
            else {
                possibleFields = this.possibleFields;           
            }

            //Create list for possible thread fields
            List<Field> threadFields = new List<Field>();

            //If possible fields exist
            if (possibleFields.Count > 0) {
                //For every opponents piece 
                for (int i = 0; i < this.player.Opponent.Pieces.Count; i++) {
                    if (this.player.Opponent.Pieces[i].currentField != null) {
                        //Add each pieces possible fields by ignoring own pieces since those would not exist if opponents piece attacks that one
                        threadFields.AddRange(this.player.Opponent.Pieces[i].getPossibleFields(board, true, true));
                    }                    
                }
            }


            //Put King on its current Field again
            this.currentField.CurrentPiece = this;

            //Return the empty set of those two lists 
            return possibleFields.FindAll((x => !threadFields.Contains(x)));            
        }
    }

    /// <summary>
    /// Pawn: Can generally only move one step into the Y-direction.
    /// Exceptions:
    /// - First step: either one or two steps into the Y-direction
    /// - Attack: ONLY one step diagonal into the Y-direction
    /// - En-Passant: see isEnPassant in Board-class
    /// </summary>
    public class Pawn : Piece
    {
        //Constructor
        public Pawn(Player player) : base(player) { this.player = player; this.pieceType = PieceType.Pawn; }

        //Properties

        public override string Name {
            get => this.player.Color + " Pawn";
        }

        public override string ShortName {
            get => this.player.Color.ToString().Substring(0,1) + "P";
        }


        //Public Methods

        /// <summary>
        /// See parent definition.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public override bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false)
        {            
            //If tries to move diagonal without attacking, immediately not a possible move
            if (sourceField.Xcoord == targetField.Xcoord && isAttack)
            {
                return false;
            }
            //If tries to move more than two steps into Y or more than one into X direction
            if (Math.Abs(sourceField.Ycoord - targetField.Ycoord) > 2 || Math.Abs(sourceField.Xcoord - targetField.Xcoord) > 1)
            {
                return false;
            }
            //If tries to move horizontally
            if (sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord == targetField.Ycoord)
            {
                return false;
            }
            //If tries to move two steps other than at the very first piece move
            if (Math.Abs(sourceField.Ycoord - targetField.Ycoord) == 2 && this.moveCnt > 0)
            {
                return false;
            }
            //If tries to move two steps into Y direction and any step into X direction
            if (Math.Abs(sourceField.Ycoord - targetField.Ycoord) == 2 && sourceField.Xcoord != targetField.Xcoord)
            {
                return false;
            }
            //If tries to move two steps but jump
            if (Math.Abs(sourceField.Ycoord - targetField.Ycoord) == 2 && !board.isWayFree(sourceField, targetField))
            {
                return false;
            }
            //If tries to go backwards
            if (this.player.Color == Colors.White && (targetField.Ycoord - sourceField.Ycoord) < 0)
            {
                return false;
            }
            if (this.player.Color == Colors.Black && (targetField.Ycoord - sourceField.Ycoord) > 0)
            {
                return false;
            }
            //If tries to move diagonal wihtout attacking
            if (sourceField.Xcoord != targetField.Xcoord && !isAttack)
            {
                //Is En Passant?
                if (!board.isEnPassant(this, sourceField, targetField))
                {
                    return false;
                }
            }
            return true;
        }
    }


    /// <summary>
    /// Rook: Can only move straight either into the X- or the Y-direction.
    /// No step limitation.
    /// Can move forward and backwards.
    /// </summary>
    public class Rook : Piece
    {
        //Constructor
        public Rook(Player player) : base(player) { this.player = player; this.pieceType = PieceType.Rook; }

        //Properties
        public override string Name
        {
            get => this.player.Color + " Rook";
        }

        public override string ShortName
        {
            get => this.player.Color.ToString().Substring(0, 1) + "R";
        }


        //Public Methods

        /// <summary>
        /// See parent definition.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public override bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false)
        {            
            //If tries to move into more than one dimension
            if (sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord != targetField.Ycoord) {
                return false;
            }
            //If tries to jump
            if (!board.isWayFree(sourceField, targetField)) {
                return false;
            }
            return true;            
        }
    }

    /// <summary>
    /// Knight:
    /// - Can move jump over any other pieces
    /// - Always has to either:
    ///     - Move two steps into the X- and one into the Y-direction
    ///     - Or move one step into the X- and two into the Y-direction
    /// - Can go backwards and forward
    /// </summary>
    public class Knight : Piece
    {
        //Constructor
        public Knight(Player player) : base(player) { this.player = player; this.pieceType = PieceType.Knight; }

        //Properties

        public override string Name
        {
            get => this.player.Color + " Knight";
        }

        public override string ShortName
        {
            get => this.player.Color.ToString().Substring(0, 1) + "N";
        }


        //Public Methods

        /// <summary>
        /// See parent definition.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public override bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false)
        {           
            //If X diff is not exactly 2 while Y diff is exactly 1 or if X diff is not 1 while Y diff is 2
            if (!((Math.Abs(sourceField.Xcoord - targetField.Xcoord) == 2 && Math.Abs(sourceField.Ycoord - targetField.Ycoord) == 1) || (Math.Abs(sourceField.Xcoord - targetField.Xcoord) == 1 && Math.Abs(sourceField.Ycoord - targetField.Ycoord) == 2)))
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Bishop: Can move into any one direction diagonally with however many steps
    /// </summary>
    public class Bishop : Piece
    {
        //Constructor
        public Bishop(Player player) : base(player) { this.player = player; this.pieceType = PieceType.Bishop; }


        //Properties

        public override string Name
        {
            get => this.player.Color + " Bishop";
        }

        public override string ShortName
        {
            get => this.player.Color.ToString().Substring(0, 1) + "B";
        }


        //Public Methods

        /// <summary>
        /// See parent definition.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public override bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false)
        {            
            //If not moving diagonal (X-Diff does not equal Y-Diff)
            if (Math.Abs(sourceField.Xcoord - targetField.Xcoord) != Math.Abs(sourceField.Ycoord - targetField.Ycoord)) {
                return false;
            }
            //If trying to jump
            if (!board.isWayFree(sourceField, targetField)) {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// King: Can only move one step but into any direction.
    /// </summary>
    public class King : Piece
    {
        //Constructor
        public King(Player player) : base(player) { this.player = player; this.pieceType = PieceType.King; }


        //Properties

        public override string Name
        {
            get => this.player.Color + " King";
        }

        public override string ShortName
        {
            get => this.player.Color.ToString().Substring(0, 1) + "K";
        }


        //Public Methods

        /// <summary>
        /// See parent definition.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public override bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false)
        {            
            //If tries to move into any direction with more than one step
            if (Math.Abs(sourceField.Xcoord - targetField.Xcoord) > 1 || Math.Abs(sourceField.Ycoord - targetField.Ycoord) > 1) {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Queen: Can move horizontally, vertically or diagonally without any step limitations.
    /// </summary>
    public class Queen : Piece
    {        
        //Constructor
        public Queen(Player player) : base(player) { this.player = player; this.pieceType = PieceType.Queen; }


        //Properties

        public override string Name
        {
            get => this.player.Color + " Queen";
        }

        public override string ShortName
        {
            get => this.player.Color.ToString().Substring(0, 1) + "Q";
        }


        //Public Methods

        /// <summary>
        /// See parent definition.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <param name="board"></param>
        /// <param name="isAttack"></param>
        /// <returns></returns>
        public override bool isValidMove(Field sourceField, Field targetField, Board board, bool isAttack = false)
        {            
            //If X and Y change, difference has to be the same (diagonal)
            if ((sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord != targetField.Ycoord) && (Math.Abs(sourceField.Xcoord - targetField.Xcoord) != Math.Abs(sourceField.Ycoord - targetField.Ycoord)))
            {
                return false;
            }
            //If trying to jump
            if (!board.isWayFree(sourceField, targetField)) {
                return false;
            }
            return true;
        }
    }
}
