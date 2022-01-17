using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Steps {
    public class EnPassantStep : IStep<GameState, LinearHistory> {

        public EnPassant EnPassant { get; }

        public EnPassantStep(EnPassant enPassant) {
            EnPassant = enPassant;
        }
        public GameState Forward(GameState state, LinearHistory history) {
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var c = EnPassant.Capture;
            var color = EnPassant.Player.Color;

            state.Squares()[s.x, s.y] = PieceType.Empty;
            state.Squares()[t.x, t.y] = color == Color.White ? PieceType.WPawn : PieceType.BPawn;
            state.Squares()[c.x, c.y] = PieceType.Empty;

            return state;
        }

        public GameState Backward(GameState state, LinearHistory history) {
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var c = EnPassant.Capture;
            var color = EnPassant.Player.Color;

            state.Squares()[s.x, s.y] = color == Color.White ? PieceType.WPawn : PieceType.BPawn;
            state.Squares()[t.x, t.y] = PieceType.Empty;
            state.Squares()[c.x, c.y] = color == Color.White ? PieceType.BPawn : PieceType.WPawn;

            return state;
        }

        // TODO: Historical validation - this move can only be done if the captured pawn recently moved.
        public GameState ValidateForward(GameState state, LinearHistory history) {
            CommonValidation(state);
            var squares = state.Squares();
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var c = EnPassant.Capture;

            StepValidation.OwnsPiece(EnPassant.Player, state.Squares()[s.x, s.y]);
            StepValidation.PositionIsPiece(state.Squares(), s, new [] {PieceType.WPawn, PieceType.BPawn});
            StepValidation.PositionIsPiece(state.Squares(), c, new [] {PieceType.WPawn, PieceType.BPawn});
            StepValidation.OpposingPieces(squares[s.x, s.y], squares[c.x, c.y]);
            StepValidation.PositionIsPiece(state.Squares(), t, PieceType.Empty);
            throw new System.NotImplementedException();
        }

        public GameState ValidateBackward(GameState state, LinearHistory history) {
            CommonValidation(state);
            throw new System.NotImplementedException();
        }

        private GameState CommonValidation(GameState state) {
            StepValidation.PlayerColorAssigned(EnPassant.Player);

            return state;
        }
    }
}