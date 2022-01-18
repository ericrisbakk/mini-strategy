using System;
using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Steps {
    public abstract class MoveStep : IStep<GameState, LinearHistory> {
        public Move Move { get; }
        
        public MoveStep(Move move) {
            this.Move = move;
        }
        
        public virtual GameState Forward(GameState state, LinearHistory history) {
            var s = Move.Source;
            var t = Move.Target;
            state.Squares()[s.x, s.y] = PieceType.Empty;
            state.Squares()[t.x, t.y] = Move.Piece;
            state.ActionCount += 1;
            return state;
        }
        
        public virtual GameState Backward(GameState state, LinearHistory history) {
            var s = Move.Source;
            var t = Move.Target;
            state.Squares()[s.x, s.y] = Move.Piece;
            state.Squares()[t.x, t.y] = Move.Capture;
            state.ActionCount -= 1;
            return state;
        }

        /// <summary>
        /// Validation shared by both backwards and forwards validation.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual GameState CommonValidation(GameState state, LinearHistory history) {
            StepValidation.ActionCountValid(state, history);
            StepValidation.PlayerColorAssigned(Move.Player);
            StepValidation.OwnsPiece(Move.Player, Move.Piece);
            StepValidation.InBounds(Move.Capture, "Capture");
            if (Rules.MoveCaptures(Move))
                StepValidation.OpposingPieces(Move.Piece, Move.Capture);
            return state;
        }
        
        public virtual GameState ValidateBackward(GameState state, LinearHistory history) {
            CommonValidation(state, history);
            var s = Move.Source;
            var t = Move.Target;
            StepValidation.PositionIsPiece(state.Squares(), t, Move.Piece);
            StepValidation.PositionIsPiece(state.Squares(), s, PieceType.Empty);

            return state;
        }

        public virtual GameState ValidateForward(GameState state, LinearHistory history) {
            CommonValidation(state, history);
            var s = Move.Source;
            var t = Move.Target;
            StepValidation.PositionIsPiece(state.Squares(), s, Move.Piece);
            Assert.IsTrue(state.Squares()[t.x, t.y] != PieceType.OutOfBounds,
                "Move target cannot be out of bounds.");
            StepValidation.PositionIsPiece(state.Squares(), t, Move.Capture);

            return state;
        }
    }
    
    public class PawnMoveStep : MoveStep {
        public PawnMoveStep(Move move) : base(move) {}

        // TODO: There actually needs to be a check for the En Passant case, but that requires outside information.
        // TODO: Consider a "Promise" test, that must happen immediately after.
        public override GameState CommonValidation(GameState state, LinearHistory history) {
            var s = Move.Source;
            var t = Move.Target;
            var start = Rules.GetPawnStartRow(Move.Player.Color);
            var direction = Rules.GetPawnDirection(Move.Player.Color);

            Assert.IsTrue(direction == Math.Sign(t.x - s.x),
                "Pawn can only move towards opposite side.");
            
            if (Rules.MoveCaptures(Move))
                Assert.IsTrue(Math.Abs(t.y - s.y) == 1
                              && Math.Abs(t.x - s.x) == 1,
                    "pawn can only ever capture by moving diagonally.");

            if (Math.Abs(t.x - s.x) == 2)
                Assert.IsTrue(s.x == start 
                              && Math.Abs(t.y - s.y) == 0
                              && state.Squares()[s.x + direction, s.y] == PieceType.Empty,
                    "Pawn can only move forward two spaces when at starting position and unblocked.");

            return state;
        }
    }

    public class KnightMoveStep : MoveStep {
        public KnightMoveStep(Move move) : base(move) { }

        public override GameState CommonValidation(GameState state, LinearHistory history) {
            var s = Move.Source;
            var t = Move.Target;
            Assert.IsTrue((Math.Abs(t.x - s.x) == 2 && Math.Abs(t.y - s.y) == 1)
                || (Math.Abs(t.x - s.x) == 1 && Math.Abs(t.y - s.y) == 2),
                "Knight move must be 2-then-1.");
            return state;
        }
    }
}