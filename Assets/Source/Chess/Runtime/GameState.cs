using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using static Source.Chess.Runtime.ChessConstants;
using Color = Source.Chess.Runtime.ChessConstants.Color;

namespace Source.Chess.Runtime {
    public class GameState : IState {
        public Player White { get; }
        public Player Black { get; }
        public Board Board { get; }
        
        public Player CurrentPlayer { get; set; }
        public int ActionCount { get; set; }
        public bool PromotionNeeded { get; set; }
        public Vector2Int PromotionTarget { get; set; }
        
        public GameState(string white, string black) {
            White = new Player(Color.White);
            Black = new Player(Color.Black);
            Board = new Board(white, black);
            CurrentPlayer = White;
            ActionCount = 0;
        }

        public PieceType[,] Squares() => Board.Squares;
        public PieceType Square(int x, int y) => Board.Squares[x, y];
        public PieceType Square(Vector2Int pos) => Board.Squares[pos.x, pos.y];
    }
}