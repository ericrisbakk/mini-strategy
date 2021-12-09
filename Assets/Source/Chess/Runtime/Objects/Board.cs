namespace Source.Chess.Runtime.Objects {
    
    /// <summary>
    /// Pieces are represented using integer values.
    /// The board is actually 12x12, in which the borders are assigned an "OutOfBounds" value.
    /// </summary>
    public class Board {
        public PieceType[,] Squares { get; }

        public Board() {
            Squares = new PieceType[12, 12];
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 2; j++) {
                    Squares[i, j] = PieceType.OutOfBounds;
                    Squares[j, i] = PieceType.OutOfBounds;
                    Squares[Squares.Length-(1+j), i] = PieceType.OutOfBounds;
                    Squares[i,Squares.Length-(1+j)] = PieceType.OutOfBounds;
                }
            }
        }
    }
}