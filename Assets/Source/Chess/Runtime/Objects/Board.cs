namespace Source.Chess.Runtime.Objects {
    
    /// <summary>
    /// Pieces are represented using integer values.
    /// </summary>
    public class Board {
        public PieceType[,] Squares { get; }

        public Board() {
            Squares = new PieceType[8, 8];
        }
    }
}