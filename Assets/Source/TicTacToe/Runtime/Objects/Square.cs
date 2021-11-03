namespace Source.TicTacToe.Runtime.Objects {

    public enum SquareState {
        Empty,
        Nought,
        Cross
    }
    
    public class Square {
        public SquareState State;
    }
}