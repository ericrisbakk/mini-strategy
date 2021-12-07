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
            public abstract GameState ValidateForward(GameState state);

            public virtual GameState Backward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                state.Squares()[s.x, s.y] = move.Piece;
                state.Squares()[t.x, t.y] = move.Capture;
                return state;
            }
            public abstract GameState ValidateBackward(GameState state);

            public GameState CommonForwardValidation(GameState state) {
                var s = move.Source;
                var t = move.Target;
                Assert.AreNotEqual(move.Player.Color, PlayerType.Unassigned, 
                    "Player must have a color.");
                Assert.IsTrue(Rules.OwnsPiece(move.Player, move.Piece),
                    "Player and piece color do not match.");
                Assert.IsTrue(state.Squares()[s.x, s.y] == move.Piece,
                    "The piece on source position must match the piece value of the move.");
                Assert.IsTrue(state.Squares()[t.x, t.y] != PieceType.OutOfBounds,
                    "Move target cannot be out of bounds.");
                Assert.IsTrue(move.Capture != PieceType.OutOfBounds,
                    "Out of bounds cannot be captured!");
                Assert.IsTrue(state.Squares()[t.x, t.y] == move.Capture,
                    "The piece on target position must match the capture value of the move.");
                if (Rules.MoveCaptures(move))
                    Assert.IsTrue(Rules.OwnsPiece(Rules.GetOtherPlayer(state, move.Player), move.Capture), 
                        "The piece captured must belong to the other player.");
                return state;
            }
        }
        
        // TODO: En passant must become its own step, because its behaviour is different from other captures.
        public class PawnMoveStep : MoveStep {
            public PawnMoveStep(Move move) : base(move) {}

            public override GameState Forward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                state.Squares()[s.x, t.y] = PieceType.Empty;
                return base.Forward(state);
            }

            public override GameState Backward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                // TODO: OOOF! En Passant will be incorrect.
                // The base `Backward` sets target to capture, but `En passant` is not captured.
                // ALSO: En passant can only be done immediately after another player
                // moves their pawn two spaces forward
                return base.Backward(state);
            }

            public override GameState ValidateForward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                var start = move.Player.Color == PlayerType.White ? 7 : 2;
                var direction = move.Player.Color == PlayerType.White ? -1 : 1;

                Assert.IsTrue(direction == Math.Sign(t.x - s.x),
                    "Pawn can only move towards opposite side.");
                
                if (Rules.MoveCaptures(move)) {
                    Assert.IsTrue(Math.Abs(t.y - s.y) == 1
                                  && Math.Abs(t.x - s.x) == 1,
                        "pawn can only ever capture by moving diagonally.");
                    
                    if (state.Squares()[t.x, t.y] == PieceType.Empty) {
                        Assert.IsTrue(state.Squares()[s.x, t.y] == move.Capture,
                            "En passant detected, but the captured piece did not match.");
                    }
                }
                
                if (Math.Abs(t.x - s.x) == 2)
                    Assert.IsTrue(s.x == start 
                                  && Math.Abs(t.y - s.y) == 0,
                    "Pawn can only move forward two spaces when at starting position");


                return state;
            }

            public override GameState ValidateBackward(GameState state) {
                throw new NotImplementedException();
            }
        }
    }
}