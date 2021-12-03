using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime {
    public class Steps {

        public abstract class MoveStep : IStep<GameState> {
            public Move move { get; }
            
            public MoveStep(Move move) {
                this.move = move;
            }

            public GameState Forward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                state.Squares()[s.x, s.y] = PieceType.Empty;
                state.Squares()[t.x, t.y] = move.Piece;
                return state;
            }
            public abstract GameState ValidateForward(GameState state);

            public GameState Backward(GameState state) {
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
                Assert.IsTrue(state.Squares()[t.x, t.y] == move.Capture,
                    "The piece on target position must match the capture value of the move.");
                if (Rules.MoveCaptures(move))
                    Assert.IsTrue(Rules.OwnsPiece(Rules.GetOtherPlayer(state, move.Player), move.Capture));
                return state;
            }
        }

        public class PawnMoveStep : MoveStep {
            public PawnMoveStep(Move move) : base(move) {}

            public override GameState ValidateForward(GameState state) {
                var s = move.Source;
                var t = move.Target;
                throw new NotImplementedException();
            }

            public override GameState ValidateBackward(GameState state) {
                throw new NotImplementedException();
            }
        }
    }
}