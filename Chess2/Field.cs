using System;
namespace Chess2
{
    /// <summary>
    /// Represents a field on the chess board.
    /// </summary>
    public class Field
    {
        //Global Variables
        private static int cnt;                     //Stores the amount of Field instances created
        private int id;                             //Unique identifier for each field
        private int xcoord;                         //X-Coordinate on the chess board
        private int ycoord;                         //Y-Coordinate on the chess board
        private string coord;                       //String that represents the actual coordinates (e.g. A4)
        private Piece currentPiece;                 //Stores the current piece that is on the field        

        //Constructor
        public Field(int xcoord, int ycoord)
        {
            //Set ID to the amount of created fields, increment field and set the String coordinate
            this.id = cnt;
            Field.cnt++;
            this.xcoord = xcoord;
            this.ycoord = ycoord;
            this.coord = getCharCoord(xcoord) + (ycoord + 1).ToString();
        }


        //Properties

        public int ID {
            get => this.id;
        }

        public int Xcoord {
            get => this.xcoord;
        }

        public int Ycoord {
            get => this.ycoord;
        }

        public string Coord {
            get => this.coord;
        }

        public Piece CurrentPiece
        {
            get => this.currentPiece;
            set
            {
                this.currentPiece = value;
                //Set piece's field to this field
                if (this.currentPiece != null && value != null)
                {
                    this.currentPiece.CurrentField = this;
                }
            }
        }


        //Public Methods

        /// <summary>
        /// Returns a char based on the index between 0 and 25.
        /// E.g. A -> 0
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static char getCharCoord(int x) {            
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return s[x];
        }

        /// <summary>
        /// Returns an index depending on the input string.
        /// E.g. 0 -> A
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int getIntCoord(char c) {
            int i = -1;
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (s.Contains(c))
            {
                i = s.IndexOf(c);
            }
            else {
                throw new Exception("Invalid Coordinates Input");
            }
            return i;
        }
    }
}
