using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime {
    public class GameState : IState {
        public Player White { get; }
        public Player Black { get; }
        public Board Board { get; }
        
        public Player CurrentPlayer { get; set; }

        public GameState() {
            White = new Player(PlayerType.White);
            Black = new Player(PlayerType.Black);
            Board = new Board();
        }

        public PieceType[,] Squares() => Board.Squares;
        public PieceType Square(int x, int y) => Board.Squares[x, y];
        public PieceType Square(Vector2Int pos) => Board.Squares[pos.x, pos.y];
    }
}