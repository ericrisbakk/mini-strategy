namespace Source.Chess.Runtime.Objects {
    
    /// <summary>
    /// Pieces are represented using integer values.
    /// </summary>
    public class Board {
        public int[,] Squares { get; }

        public Board() {
            Squares = new int[8, 8];
        }
    }
}