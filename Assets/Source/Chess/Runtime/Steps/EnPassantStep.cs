using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;

namespace Source.Chess.Runtime.Steps {
    public class EnPassantStep : IStep<GameState> {

        public EnPassant EnPassant { get; }

        public EnPassantStep(EnPassant enPassant) {
            EnPassant = enPassant;
        }
        public GameState Forward(GameState state) {
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var color = EnPassant.Player.Color;
            var direction = Rules.GetPawnDirection(color);

            state.Squares()[s.x, s.y] = PieceType.Empty;
            state.Squares()[t.x, t.y] = color == Color.White ? PieceType.WPawn : PieceType.BPawn;
            state.Squares()[t.x - direction, t.y] = PieceType.Empty;

            return state;
        }

        public GameState Backward(GameState state) {
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var color = EnPassant.Player.Color;
            var direction = Rules.GetPawnDirection(color);

            state.Squares()[s.x, s.y] = color == Color.White ? PieceType.WPawn : PieceType.BPawn;
            state.Squares()[t.x, t.y] = PieceType.Empty;
            state.Squares()[t.x - direction, t.y] = color == Color.White ? PieceType.BPawn : PieceType.WPawn;

            return state;
        }

        public GameState ValidateForward(GameState state) {
            throw new System.NotImplementedException();
        }

        public GameState ValidateBackward(GameState state) {
            throw new System.NotImplementedException();
        }
    }
}