using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;

namespace Source.Chess.Runtime {
    public class GameState : IState {
        public Player White { get; }
        public Player Black { get; }
        public Board Board { get; }

        public GameState() {
            White = new Player(PlayerType.White);
            Black = new Player(PlayerType.Black);
            Board = new Board();
        }

        public PieceType[,] Squares() => Board.Squares;
    }
}