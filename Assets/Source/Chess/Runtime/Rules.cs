using System;
using System.Collections.Generic;
using System.Linq;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using Source.Chess.Runtime.Steps;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;
using Color = Source.Chess.Runtime.ChessConstants.Color;
using static Source.Chess.Runtime.ChessConstants;
using static Source.Chess.Runtime.ChessChecks;

namespace Source.Chess.Runtime {

    // TODO: Rules should probably inherit from something defining base classes, especially a "GetAllAvailableActions" method.
    public static class Rules {

        #region Steps
        
        
        
        /// <summary>
        /// Updates the state and history with the results of applying the given action.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="history"></param>
        /// <param name="action"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public static GameState Apply(GameState state, LinearHistory history, IAction action, bool validate) {
            var stepList = new List<IStep>();
            HandleStep(state, history, stepList, GetNextStep(state, history, action), validate);
            
            while (HasNextStep(state, history, stepList, out var step)) {
                HandleStep(state, history, stepList, step, validate);
            }
            
            history.Add(action, stepList);
            return state;
        }

        private static void HandleStep(GameState state, LinearHistory history, List<IStep> stepList, IStep<GameState, LinearHistory> step, 
            bool validate) {
            if (validate) step.ValidateForward(state, history);
            step.Forward(state, history);
            stepList.Add(step);
        }

        /// <summary>
        /// Gets the next step based on the action given. There is a matching step for each action.
        /// An action must result in a step - otherwise, it should never have been given.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="history"></param>
        /// <param name="action"></param>
        /// <returns>Step corresponding to action.</returns>
        /// <exception cref="Exception"></exception>
        public static IStep<GameState, LinearHistory> GetNextStep(GameState state, LinearHistory history, IAction action) {
            return action switch {
                Move move => PieceStepDict[move.Piece].Invoke(move),
                Promote promote => new PromotionStep(promote),
                EnPassant enPassant => new EnPassantStep(enPassant),
                _ => throw new Exception("Could not get next step, action was not recognized.")
            };
        }

        /// <summary>
        /// Checks whether there is a next step, and if so, sets it as an out argument in `step`.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="history"></param>
        /// <param name="stepList"></param>
        /// <param name="step"></param>
        /// <returns>True if there is a next step, false otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool HasNextStep(GameState state, LinearHistory history, List<IStep> stepList,
            out IStep<GameState, LinearHistory> step) {
            var lastStep = stepList.Last();
            
            if (lastStep is ChangePlayerStep) {
                step = null;
                return false;
            }
            else if (lastStep is MoveStep move && PawnUpForPromotion(state, move.Move.Target)) {
                step = null;
                return false;
            }
            else {
                step = new ChangePlayerStep();
                return true;
            }
        }

        public static GameState Undo(GameState state, LinearHistory history, bool validate) {
            var steps = history.LastAction;
            foreach (var step1 in steps.Item2) {
                var step = (IStep<GameState, LinearHistory>) step1;
                if (validate)
                    step.ValidateBackward(state, history);
                step.Backward(state, history);
            }

            history.Backtrack();
            return state;
        }
        
        #endregion

        #region Action
        
        public static List<IAction> GetActions(GameState state, LinearHistory history, Vector2Int source) {
            var actions = new List<IAction>();
            var piece = state.Square(source);
            if (state.PromotionNeeded) {
                if (state.PromotionTarget != source)
                    return actions;
                actions.AddRange(GetPromoteActions(state));
            }
            else if(OwnsPiece(state.CurrentPlayer, piece))
                actions.AddRange(PieceActionDict[piece].Invoke(state, history, source));
            return actions;
        }

        /// <summary>
        /// Naive implementation for getting all actions.
        /// TODO: Keep track of piece locations.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<IAction> GetAllActions(GameState state, LinearHistory history) {
            var actions = new List<IAction>();
            if (state.PromotionNeeded) {
                actions.AddRange(GetActions(state, history, state.PromotionTarget));
            }
            else {
                for (int i = 0; i < 8; i++) {
                    for (int j = 0; j < 8; j++) {
                        var t = new Vector2Int(2 + i, 2 + j);
                        if(OwnsPiece(state.CurrentPlayer, state.Squares()[t.x, t.y])) {
                            actions.AddRange(GetActions(state, history, t));
                        }
                    }
                }
            }

            return actions;
        }
        
        public static List<IAction> GetPawnActions(GameState state, LinearHistory history, Vector2Int source) {
            var piece = state.Square(source);
            var player = state.CurrentPlayer;
            var color = ColorOfPiece(piece);
            var start = GetPawnStartRow(color);
            var direction = GetPawnDirection(color);
            var actions = new List<IAction>();

            var posAhead1 = new Vector2Int(source.x + direction, source.y);
            if (state.Square(posAhead1) == PieceType.Empty) {
                actions.Add(new Move(player, piece, source, PieceType.Empty, posAhead1));

                var posAhead2 = new Vector2Int(source.x + (2 * direction), source.y);
                if (source.x == start
                    && state.Square(posAhead2) == PieceType.Empty) {
                    actions.Add(new Move(player, piece, source, PieceType.Empty, posAhead2));
                }
            }

            PawnCapture(state, actions, player, piece, source, 
                new Vector2Int(source.x + direction, source.y + 1));

            PawnCapture(state, actions, player, piece, source, 
                new Vector2Int(source.x + direction, source.y - 1));

            if (CanEnPassant(state, history, source, out var enPassantTarget))
                actions.Add(new EnPassant(player, source, enPassantTarget));

            return actions;
        }

        /// <summary>
        /// Adds
        /// </summary>
        private static void PawnCapture(GameState state, List<IAction> actions, Player player, PieceType piece, 
            Vector2Int source, Vector2Int target) {
            var leftCapturedPiece = state.Square(target);
            if (OwnsPiece(GetOtherPlayer(state, player), state.Square(target)))
                actions.Add(new Move(player, piece, source, leftCapturedPiece, target));
        }

        public static List<Promote> GetPromoteActions(GameState state) {
            var actions = new List<Promote>();
            for (int i = 1; i <= 4; i++) {
                var piece = state.CurrentPlayer.Color == Color.White
                    ? PieceType.WPawn + i
                    : PieceType.BPawn + i;
                actions.Add(new Promote(state.CurrentPlayer, state.PromotionTarget, piece));
            }

            return actions;
        }

        public static List<IAction> GetMovementLine(GameState state, Vector2Int target, int dx, int dy, int length) {
            var piece = state.Square(target);
            var l = new List<IAction>();
            for (int i = 1; i <= length; i++) {
                var pos = new Vector2Int(target.x + dx*i, target.y + dy*i);
                var otherPiece = state.Square(pos);
                if (otherPiece == PieceType.Empty) {
                    l.Add(new Move(state.CurrentPlayer, piece, target, otherPiece, pos));
                    continue;
                }
                if (IsPiece(otherPiece) && ColorOfPiece(piece) != ColorOfPiece(otherPiece)) {
                    l.Add(new Move(state.CurrentPlayer, piece, target, otherPiece, pos));
                }

                break;
            }

            return l;
        }

        public static List<IAction> GetKnightActions(GameState state, LinearHistory history, Vector2Int target) {
            var l = new List<IAction>();
            foreach (var delta in KnightDeltas) {
                l.AddRange(GetMovementLine(state, target, delta[0], delta[1], 1));
            }

            return l;
        }

        public static List<IAction> GetRookActions(GameState state, LinearHistory history, Vector2Int target) {
            var l = new List<IAction>();
            foreach (var delta in StraightDeltas) {
                l.AddRange(GetMovementLine(state, target, delta[0], delta[1], 8));
            }
            return l;
        }

        public static List<IAction> GetBishopActions(GameState state, LinearHistory history, Vector2Int target) {
            var l = new List<IAction>();
            foreach (var delta in DiagonalDeltas) {
                l.AddRange(GetMovementLine(state, target, delta[0], delta[1], 8));
            }

            return l;
        }

        public static List<IAction> GetQueenActions(GameState state, LinearHistory history, Vector2Int target) {
            var l = new List<IAction>();
            for (int i = 0; i < StraightDeltas.Length; i++) {
                var straightDelta = StraightDeltas[i];
                var diagonalDelta = DiagonalDeltas[i];
                l.AddRange(GetMovementLine(state, target, straightDelta[0], straightDelta[1], 8));
                l.AddRange(GetMovementLine(state, target, diagonalDelta[0], diagonalDelta[1], 8));
            }

            return l;
        }

        public static List<IAction> GetKingActions(GameState state, LinearHistory history, Vector2Int target) {
            var l = new List<IAction>();
            for (int i = 0; i < StraightDeltas.Length; i++) {
                var straightDelta = StraightDeltas[i];
                var diagonalDelta = DiagonalDeltas[i];
                l.AddRange(GetMovementLine(state, target, straightDelta[0], straightDelta[1], 1));
                l.AddRange(GetMovementLine(state, target, diagonalDelta[0], diagonalDelta[1], 1));
            }
            
            return l;
        }
        
        public static Vector2Int ToVector2Int(char rank, char file) {
            int x = rank - '1' + 2;
            int y = file - 'a' + 2;
            return new Vector2Int(x, y);
        }

        public static Tuple<IAction, List<IStep>> GetActionFromNow(this LinearHistory history, GameState state, int index) {
            return history.Events[state.ActionCount - index];
        }
        
        #endregion
    }
}