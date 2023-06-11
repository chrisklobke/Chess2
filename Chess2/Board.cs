using System;
using System.Collections.Generic;

namespace Chess2
{
    /// <summary>
    /// A Class that represents the game board of the size given to the Constructor.
    /// Default size: 8x8.
    /// </summary>
    public class Board
    {
        //Global variables
        private Field[,] fields;                            //Two dimensional array to store the fields for the board
        private int size;                                   //Determins the size of the board (default 8x8 in constructor)

        //Constructor
        public Board(int size = 8)                      
        {
            //Set size, create an array for the fields and create the board
            this.size = size;
            fields = new Field[size, size];
            createBoard();
        }

        //Properties

        public Field[,] Fields { get => this.fields; }


        //Public Methods        

        /// <summary>
        /// Checks if a field does exist based on the coordinates.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool doesFieldExist(Field field) {
            if (field == null) {
                return false;
            }
            else if (field.Xcoord < 0 || field.Xcoord > this.fields.GetLength(0))
            {
                return false;
            }
            else if (field.Ycoord < 0 || field.Ycoord > this.fields.GetLength(1)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a list of fields that are between two fields for a proper move.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <returns></returns>
        public List<Field> getWayFields(Field sourceField, Field targetField) {
            List<Field> fields = new List<Field>();
            //Vertical
            if (sourceField.Xcoord == targetField.Xcoord && sourceField.Ycoord != targetField.Ycoord)
            {
                //If move to the right
                if ((targetField.Ycoord - sourceField.Ycoord) > 0)
                {
                    //Add fields to output list
                    for (int i = sourceField.Ycoord + 1; i < targetField.Ycoord; i++)
                    {
                        fields.Add(this.fields[sourceField.Xcoord, i]);
                    }
                }
                //If move to the left
                else
                {
                    //Add fields to output list
                    for (int i = sourceField.Ycoord - 1; i > targetField.Ycoord; i--)
                    {
                        fields.Add(this.fields[sourceField.Xcoord, i]);
                    }
                }                
            }
            //Horizontal
            else if (sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord == targetField.Ycoord)
            {
                //If upwards
                if ((targetField.Xcoord - sourceField.Xcoord) > 0)
                {
                    //Add fields to output list
                    for (int i = sourceField.Xcoord + 1; i < targetField.Xcoord; i++)
                    {
                        fields.Add(this.fields[i, sourceField.Ycoord]);
                    }
                }
                //If downwards
                else
                {
                    //Add fields to output list
                    for (int i = sourceField.Xcoord - 1; i > targetField.Xcoord; i--)
                    {
                        fields.Add(this.fields[i, sourceField.Ycoord]);
                    }
                }
            }
            //Diagonal
            else if (sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord != targetField.Ycoord)
            {
                //If move to upper right
                if ((targetField.Xcoord - sourceField.Xcoord) > 0 && (targetField.Ycoord - sourceField.Ycoord) > 0)
                {
                    //Add fields to output list
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        fields.Add(this.fields[i, i]);
                    }
                }
                //If move to upper left
                else if ((targetField.Xcoord - sourceField.Xcoord) < 0 && (targetField.Ycoord - sourceField.Ycoord) > 0)
                {
                    //Add fields to output list
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        fields.Add(this.fields[i, i]);
                    }
                }
                //If move to lower right
                else if ((targetField.Xcoord - sourceField.Xcoord) > 0 && (targetField.Ycoord - sourceField.Ycoord) < 0)
                {
                    //Add fields to output list
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        fields.Add(this.fields[i, i]);
                    }
                }
                //If move to lower left
                else if ((targetField.Xcoord - sourceField.Xcoord) < 0 && (targetField.Ycoord - sourceField.Ycoord) < 0)
                {
                    //Add fields to output list
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        fields.Add(this.fields[i, i]);
                    }
                }                
            }
            return fields;            
        }

        /// <summary>
        /// Checks if the way between two fields is free (no piece is on fields between them).
        /// Calculations depend on the way of moving: vertical, horizontal, diagonal.
        /// Other movements are ignored since the piece "knight" can jump.
        /// </summary>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <returns></returns>
        public bool isWayFree(Field sourceField, Field targetField) {            
            //Vertical
            if (sourceField.Xcoord == targetField.Xcoord && sourceField.Ycoord != targetField.Ycoord)
            {
                //If move to the right
                if ((targetField.Ycoord - sourceField.Ycoord) > 0)
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = sourceField.Ycoord + 1; i < targetField.Ycoord; i++)
                    {                        
                        if (this.fields[sourceField.Xcoord, i].CurrentPiece != null) {
                            return false;
                        }
                    }
                }
                //If move to the left
                else {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = sourceField.Ycoord - 1; i > targetField.Ycoord; i--)
                    {
                        if (this.fields[sourceField.Xcoord, i].CurrentPiece != null)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            //Horizontal
            else if (sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord == targetField.Ycoord)
            {
                //If upwards
                if ((targetField.Xcoord - sourceField.Xcoord) > 0)
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = sourceField.Xcoord + 1; i < targetField.Xcoord; i++)
                    {
                        if (this.fields[i, sourceField.Ycoord].CurrentPiece != null)
                        {
                            return false;
                        }
                    }
                }
                //If downwards
                else
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = sourceField.Xcoord - 1; i > targetField.Xcoord; i--)
                    {
                        if (this.fields[i, sourceField.Ycoord].CurrentPiece != null)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            //Diagonal
            else if (sourceField.Xcoord != targetField.Xcoord && sourceField.Ycoord != targetField.Ycoord)
            {
                //If move to upper right
                if ((targetField.Xcoord - sourceField.Xcoord) > 0 && (targetField.Ycoord - sourceField.Ycoord) > 0)
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++) {
                        if (this.fields[sourceField.Xcoord + i, sourceField.Ycoord + i].CurrentPiece != null) {
                            return false;
                        }
                    }
                }
                //If move to upper left
                else if ((targetField.Xcoord - sourceField.Xcoord) < 0 && (targetField.Ycoord - sourceField.Ycoord) > 0)
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        if (this.fields[sourceField.Xcoord - i, sourceField.Ycoord + i].CurrentPiece != null)
                        {
                            return false;
                        }
                    }
                }
                //If move to lower right
                else if ((targetField.Xcoord - sourceField.Xcoord) > 0 && (targetField.Ycoord - sourceField.Ycoord) < 0)
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        if (this.fields[sourceField.Xcoord + i, sourceField.Ycoord - i].CurrentPiece != null)
                        {
                            return false;
                        }
                    }
                }
                //If move to lower left
                else if ((targetField.Xcoord - sourceField.Xcoord) < 0 && (targetField.Ycoord - sourceField.Ycoord) < 0)
                {
                    //Check betwen source and target field but not source or target field, return false if any field is not empty
                    for (int i = 1; i < Math.Abs(sourceField.Xcoord - targetField.Xcoord); i++)
                    {
                        if (this.fields[sourceField.Xcoord - i, sourceField.Ycoord - i].CurrentPiece != null)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the move of a pawn between two fields is an EnPassant Move
        /// EnPassant is a move where the attacking pawn is beside an opponent pawn
        /// The attacking pawn is allowed to move diagonal to the empty field behind the opponent pawn
        /// If and only if:
        /// - Attacking pawn has advanced exactly three ranks
        /// - Opponent pawns last move was two steps forward from its initial position
        /// - The attack happens immediately after the attacked opponent's pawn moved two steps
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="sourceField"></param>
        /// <param name="targetField"></param>
        /// <returns></returns>
        public bool isEnPassant(Pawn pawn, Field sourceField, Field targetField) {            
            Piece opPiece;
            if (sourceField.Ycoord != 4 && pawn.Player.Color == Colors.White) {
                return false;
            }
            if (sourceField.Ycoord != 3 && pawn.Player.Color == Colors.Black) {
                return false;
            }
            if (targetField.CurrentPiece != null) {
                return false;
            }

            //Store Opponent Piece
            opPiece = this.fields[targetField.Xcoord, sourceField.Ycoord].CurrentPiece;
            //If opPiece even made one move yet
            if (opPiece == null) {
                return false;
            }
            else if (opPiece.Player.Moves.Count > 0)
            {
                //If attacked piece is pawn
                if (opPiece.Player != pawn.Player && opPiece.GetType() == pawn.GetType())
                {
                    //If attacked piece is last moved piece of opponent
                    if (opPiece.Player.Moves[opPiece.Player.Moves.Count - 1].MovedPiece == opPiece)
                    {
                        //If last move of pawn was exactly two steps
                        if (opPiece.Player.Moves[opPiece.Player.Moves.Count - 1].getYDistance() == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        //Private Methods

        /// <summary>
        /// Creates a two dimensional size x size array that represents the game board.
        /// </summary>
        private void createBoard()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    fields[i, j] = new Field(i, j);
                }
            }
        }
    }
}
