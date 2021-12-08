using System;
using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime {
    public class Steps {

        public abstract class MoveStep : IStep<GameState> {
            public Move move { get; }
            
            public MoveStep(Move move) {
                this.move = move;
            }
            
            public virtual GameState Forward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                state.Squares()[s.x, s.y] = PieceType.Empty;
                state.Squares()[t.x, t.y] = move.Piece;
                return state;
            }
            
            public virtual GameState Backward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                state.Squares()[s.x, s.y] = move.Piece;
                state.Squares()[t.x, t.y] = move.Capture;
                return state;
            }

            private GameState CommonValidation(GameState state) {
                Assert.AreNotEqual(move.Player.Color, PlayerType.Unassigned, 
                    "Player must have a color.");
                Assert.IsTrue(Rules.OwnsPiece(move.Player, move.Piece),
                    "Player and piece color do not match.");
                Assert.IsTrue(move.Capture != PieceType.OutOfBounds,
                    "Out of bounds cannot be captured!");
                if (Rules.MoveCaptures(move))
                    Assert.IsTrue(Rules.OwnsPiece(Rules.GetOtherPlayer(state, move.Player), move.Capture), 
                        "The piece captured must belong to the other player.");
                return state;
            }
            
            public virtual GameState ValidateBackward(GameState state) {
                CommonValidation(state);
                var s = move.Source;
                var t = move.Target;
                Assert.IsTrue(state.Squares()[t.x, t.y] == move.Piece,
                    "[Backward] Target piece does not match piece of move.");
                Assert.IsTrue(state.Squares()[s.x, s.y] == PieceType.Empty,
                    "[Backward] Source must be empty.");

                return state;
            }

            public virtual GameState ValidateForward(GameState state) {
                CommonValidation(state);
                var s = move.Source;
                var t = move.Target;
                Assert.IsTrue(state.Squares()[s.x, s.y] == move.Piece,
                    "Source piece must match the piece value of the move.");
                Assert.IsTrue(state.Squares()[t.x, t.y] != PieceType.OutOfBounds,
                    "Move target cannot be out of bounds.");
                Assert.IsTrue(state.Squares()[t.x, t.y] == move.Capture,
                    "The piece on target position must match the capture value of the move.");

                return state;
            }
        }
        
        // TODO: En passant must become its own step, because its behaviour is different from other captures.
        public class PawnMoveStep : MoveStep {
            public PawnMoveStep(Move move) : base(move) {}

            public override GameState ValidateForward(GameState state) {
                base.ValidateForward(state);
                var s = move.Source;
                var t = move.Target;
                var start = move.Player.Color == PlayerType.White ? 7 : 2;
                var direction = move.Player.Color == PlayerType.White ? -1 : 1;

                Assert.IsTrue(direction == Math.Sign(t.x - s.x),
                    "Pawn can only move towards opposite side.");
                
                if (Rules.MoveCaptures(move))
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

            public override GameState ValidateBackward(GameState state) {
                throw new NotImplementedException();
            }
        }
    }
}