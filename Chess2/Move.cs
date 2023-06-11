using System;
namespace Chess2
{
    /// <summary>
    /// Represents a move in the chess game.
    /// </summary>
    public class Move
    {
        //Global Variables
        private Field sourceField;              //Stores the source field 
        private Field targetField;              //Stores the target field
        private Piece movedPiece;               //Stores the moeved piece
        private Piece attackedPiece;            //Stores the attacked field (might be null)

        //Constructor
        public Move(Piece movedPiece, Piece attackedPiece, Field sourceField, Field targetField)
        {
            this.movedPiece = movedPiece;           
            this.sourceField = sourceField;
            this.targetField = targetField;
            if (attackedPiece != null) {
                switch (attackedPiece.Type)
                {
                    case PieceType.Bishop:
                        this.attackedPiece = new Bishop(movedPiece.Player.Opponent);
                        break;
                    case PieceType.King:
                        this.attackedPiece = new King(movedPiece.Player.Opponent);
                        break;
                    case PieceType.Knight:
                        this.attackedPiece = new Knight(movedPiece.Player.Opponent);
                        break;
                    case PieceType.Pawn:
                        this.attackedPiece = new Pawn(movedPiece.Player.Opponent);
                        break;
                    case PieceType.Queen:
                        this.attackedPiece = new Queen(movedPiece.Player.Opponent);
                        break;
                    case PieceType.Rook:
                        this.attackedPiece = new Rook(movedPiece.Player.Opponent);
                        break;
                }
            }
        }


        //Properties

        public Piece MovedPiece {
            get => this.movedPiece;
        }

        public int getXDistance() {
            return Math.Abs(targetField.Xcoord - sourceField.Xcoord);
        }

        public int getYDistance()
        {
            return Math.Abs(targetField.Ycoord - sourceField.Ycoord);
        }

        public Field SourceField {
            get => this.sourceField;
        }

        public Field TargetField {
            get => this.targetField;
        }

        public Piece AttackedPiece {
            get => this.attackedPiece;
        }
    }
}
