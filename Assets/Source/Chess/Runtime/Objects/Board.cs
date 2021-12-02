namespace Source.Chess.Runtime.Objects {
    
    /// <summary>
    /// Pieces are represented using integer values.
    /// The board is actually 10x10, in which the borders are assigned an "OutOfBounds" value.
    /// </summary>
    public class Board {
        public PieceType[,] Squares { get; }

        public Board() {
            Squares = new PieceType[10, 10];
            for (int i = 0; i < 10; i++) {
                Squares[0, i] = PieceType.OutOfBounds;
                Squares[i, 0] = PieceType.OutOfBounds;
                Squares[Squares.Length-1, i] = PieceType.OutOfBounds;
                Squares[i,Squares.Length-1] = PieceType.OutOfBounds;
            }
        }
    }
}