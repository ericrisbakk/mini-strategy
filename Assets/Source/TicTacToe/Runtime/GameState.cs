using Source.StrategyFramework.Runtime.Representation;
using Source.TicTacToe.Runtime.Objects;

namespace Source.TicTacToe.Runtime {

    public enum GameResult {
        Undecided,
        Draw,
        Player0Wins,
        PlayerXWins,
    }
    
    public class GameState : IState {
        public Player Player0;
        public Player PlayerX;
        public Board GameBoard;
        public bool Player0Turn;
        public int MoveCounter = 0;
        public GameResult Result;
        public Player GetCurrentPlayer => Player0Turn ? Player0 : PlayerX;

        public GameState() {
            Player0 = new Player(true);
            PlayerX = new Player(false);
            GameBoard = new Board();
            Player0Turn = true;
            Result = GameResult.Undecided;
        }
        
        public Square[,] GetBoard() {
            return GameBoard.Squares;
        }
        
    }
}