using Source.TicTacToe.Runtime.Objects;

namespace Source.TicTacToe.Runtime {

    public enum GameResult {
        Undecided,
        Draw,
        Player0Wins,
        PlayerXWins,
    }
    
    public class GameState {
        public Player Player0;
        public Player PlayerX;
        public Board GameBoard;
        public bool Player0Turn;
        public int MoveCounter = 0;
        public GameResult Result;

        public Square[,] GetBoard() {
            return GameBoard.Squares;
        }
    }
}