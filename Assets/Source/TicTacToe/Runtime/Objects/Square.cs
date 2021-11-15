using UnityEngine;

namespace Source.TicTacToe.Runtime.Objects {

    public enum SquareState {
        Empty,
        Nought,
        Cross
    }
    
    public class Square {
        public SquareState State;
        public Vector2Int Position;

        public Square(SquareState state, Vector2Int position) {
            State = state;
            Position = position;
        }
    }
}