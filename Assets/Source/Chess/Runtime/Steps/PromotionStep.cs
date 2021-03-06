using Source.Chess.Runtime.Actions;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine.Assertions;
using static Source.Chess.Runtime.ChessConstants;
using Color = Source.Chess.Runtime.ChessConstants.Color;
using Checks = Source.Chess.Runtime.ChessChecks;

namespace Source.Chess.Runtime.Steps {
    public class PromotionStep : IStep<GameState, LinearHistory> {
        public Promote Promote { get; }
        
        public PromotionStep(Promote promote) {
            Promote = promote;
        }

        public GameState Forward(GameState state, LinearHistory history) {
            var t = Promote.Pawn;
            state.Squares()[t.x, t.y] = Promote.Promotion;
            state.PromotionNeeded = false;
            state.ActionCount += 1;

            return state;
        }

        public GameState Backward(GameState state, LinearHistory history) {
            var t = Promote.Pawn;
            state.Squares()[t.x, t.y] = Promote.Player.Color == Color.White ? PieceType.WPawn : PieceType.BPawn;
            state.PromotionNeeded = true;
            state.ActionCount -= 1;

            return state;
        }

        public GameState ValidateForward(GameState state, LinearHistory history) {
            CommonValidation(state, history);
            var t = Promote.Pawn;
            StepValidation.PositionIsPiece(state.Squares(), t, StepValidation.Pawns);

            return state;
        }

        public GameState ValidateBackward(GameState state, LinearHistory history) {
            CommonValidation(state, history);
            StepValidation.PositionIsNotPiece(state.Squares(), Promote.Pawn, StepValidation.Pawns);
            StepValidation.PositionIsNotPiece(state.Squares(), Promote.Pawn, StepValidation.Kings);
            var lastAction = history.Events[state.ActionCount - 2];
            var step = lastAction.Item2[0];
            StepValidation.StepIs(step, typeof(MoveStep));
            var move = (MoveStep) step;
            Assert.IsTrue(move.Move.Player == Promote.Player,
                "Player of previous move must match that of the promoting player");
            StepValidation.PieceIs(move.Move.Piece, StepValidation.Pawns);

            return state;
        }

        private GameState CommonValidation(GameState state, LinearHistory history) {
            var t = Promote.Pawn;
            var color = Promote.Player.Color;
            var otherColor = Checks.GetOtherColor(color);
            Assert.IsTrue(t.x == Checks.GetBackRow(otherColor),
                "A pawn can only be promoted when it has reached the furthest opposite row.");
            StepValidation.OwnsPiece(Promote. Player, state.Squares()[t.x, t.y]);
            StepValidation.PieceIsNot(Promote.Promotion, StepValidation.Pawns);
            StepValidation.PieceIsNot(Promote.Promotion, StepValidation.Kings);
            return state;
        }
    }
}