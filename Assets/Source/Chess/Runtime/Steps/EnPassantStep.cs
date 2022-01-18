using Source.Chess.Runtime.Actions;
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
            state.ActionCount += 1;

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
            state.ActionCount -= 1;

            return state;
        }
        
        public GameState ValidateForward(GameState state, LinearHistory history) {
            CommonValidation(state, history, true);
            var squares = state.Squares();
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var c = EnPassant.Capture;

            StepValidation.OwnsPiece(EnPassant.Player, squares[s.x, s.y]);
            StepValidation.PositionIsPiece(squares, s, new [] {PieceType.WPawn, PieceType.BPawn});
            StepValidation.PositionIsPiece(squares, c, new [] {PieceType.WPawn, PieceType.BPawn});
            StepValidation.OpposingPieces(squares[s.x, s.y], squares[c.x, c.y]);
            StepValidation.PositionIsPiece(squares, t, PieceType.Empty);

            return state;
        }

        public GameState ValidateBackward(GameState state, LinearHistory history) {
            CommonValidation(state, history, false);
            var squares = state.Squares();
            var s = EnPassant.Source;
            var t = EnPassant.Target;
            var c = EnPassant.Capture;
            
            StepValidation.OwnsPiece(EnPassant.Player, squares[t.x, t.y]);
            StepValidation.PositionIsPiece(squares, s, PieceType.Empty);
            StepValidation.PositionIsPiece(squares, c, PieceType.Empty);
            StepValidation.PositionIsPiece(squares, t, new [] {PieceType.WPawn, PieceType.BPawn});

            return state;
        }
        
        private GameState CommonValidation(GameState state, LinearHistory history, bool isForward) {
            StepValidation.ActionCountValid(state, history);
            StepValidation.PlayerColorAssigned(EnPassant.Player);
            
            var lastAction = history.Events[state.ActionCount - (isForward ? 1 : 2)];
            var step = lastAction.Item2[0];
            Assert.IsTrue(step is MoveStep, "EnPassant requires last action to be a MoveStep.");
            var move = (MoveStep) step;
            var otherColor = move.Move.Player.Color;
            var otherStart = Rules.GetPawnStartRow(otherColor);
            
            StepValidation.PlayerIsColor(Rules.GetOtherPlayer(state, EnPassant.Player), otherColor);
            Assert.IsTrue(move.Move.Source.x == otherStart,
                "Other pawn must have started from starting row.");
            Assert.IsTrue(move.Move.Target.x == otherStart + 2*Rules.GetPawnDirection(otherColor),
                "Other pawn must have moved ahead two spaces.");

            return state;
        }
    }
}