using System;
using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Steps {
    public abstract class MoveStep : IStep<GameState> {
        public Move Move { get; }
        
        public MoveStep(Move move) {
            this.Move = move;
        }
        
        public virtual GameState Forward(GameState state) {
            var s = Move.Source;
            var t = Move.Target;
            state.Squares()[s.x, s.y] = PieceType.Empty;
            state.Squares()[t.x, t.y] = Move.Piece;
            return state;
        }
        
        public virtual GameState Backward(GameState state) {
            var s = Move.Source;
            var t = Move.Target;
            state.Squares()[s.x, s.y] = Move.Piece;
            state.Squares()[t.x, t.y] = Move.Capture;
            return state;
        }

        public virtual GameState CommonValidation(GameState state) {
            Assert.AreNotEqual(Move.Player.Color, Color.Unassigned, 
                "Player must have a color.");
            Assert.IsTrue(Rules.OwnsPiece(Move.Player, Move.Piece),
                "Player and piece color do not match.");
            Assert.IsTrue(Move.Capture != PieceType.OutOfBounds,
                "Out of bounds cannot be captured!");
            if (Rules.MoveCaptures(Move))
                Assert.IsTrue(Rules.OwnsPiece(Rules.GetOtherPlayer(state, Move.Player), Move.Capture), 
                    "The piece captured must belong to the other player.");
            return state;
        }
        
        public virtual GameState ValidateBackward(GameState state) {
            CommonValidation(state);
            var s = Move.Source;
            var t = Move.Target;
            Assert.IsTrue(state.Squares()[t.x, t.y] == Move.Piece,
                "[Backward] Target piece does not match piece of move.");
            Assert.IsTrue(state.Squares()[s.x, s.y] == PieceType.Empty,
                "[Backward] Source must be empty.");

            return state;
        }

        public virtual GameState ValidateForward(GameState state) {
            CommonValidation(state);
            var s = Move.Source;
            var t = Move.Target;
            Assert.IsTrue(state.Squares()[s.x, s.y] == Move.Piece,
                "Source piece must match the piece value of the move.");
            Assert.IsTrue(state.Squares()[t.x, t.y] != PieceType.OutOfBounds,
                "Move target cannot be out of bounds.");
            Assert.IsTrue(state.Squares()[t.x, t.y] == Move.Capture,
                "The piece on target position must match the capture value of the move.");

            return state;
        }
    }
    
    public class PawnMoveStep : MoveStep {
        public PawnMoveStep(Move move) : base(move) {}

        // TODO: There actually needs to be a check for the En Passant case, but that requires outside information.
        // TODO: Consider a "Promise" test, that must happen immediately after.
        public override GameState CommonValidation(GameState state) {
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

        public override GameState CommonValidation(GameState state) {
            var s = Move.Source;
            var t = Move.Target;
            Assert.IsTrue((Math.Abs(t.x - s.x) == 2 && Math.Abs(t.y - s.y) == 1)
                || (Math.Abs(t.x - s.x) == 1 && Math.Abs(t.y - s.y) == 2),
                "Knight move must be 2-then-1.");
            return state;
        }
    }
}