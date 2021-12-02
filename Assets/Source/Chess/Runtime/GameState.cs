using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;

namespace Source.Chess.Runtime {
    public class GameState : IState {
        public Player White { get; }
        public Player Black { get; }
        public Board Board { get; }

        public GameState() {
            White = new Player(true);
            Black = new Player(false);
            Board = new Board();
        }
    }
}