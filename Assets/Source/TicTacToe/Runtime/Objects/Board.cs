using UnityEngine;

namespace Source.TicTacToe.Runtime.Objects {
    public class Board {
        public Square[,] Squares = new Square[3, 3];

        public Board() {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Squares[i, j] = new Square(SquareState.Empty, new Vector2Int(i, j));
                }
            }
        }
    }
}