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
        
        #region Checks

        /// <summary>
        /// Checks whether all squares between source (excluding) and target (excluding), are empty. Assumes that
        /// they exist on a horizontal, vertical, or diagonal (45 degree) line.
        /// </summary>
        /// <returns></returns>
        public static bool EmptyLine(GameState state, Vector2Int source, Vector2Int target) {
            var dx = Math.Sign(target.x - source.x);
            var dy = Math.Sign(target.y - source.y);
            var x = source.x + dx;
            var y = source.y +dy;
            while (x != target.x && y != target.y) {
                if (state.Square(x, y) != PieceType.Empty)
                    return false;
                x += dx;
                y += dy;
            }

            return true;
        }
        
        public static bool IsPiece(PieceType piece) => (int) piece > 1;
        
        /// <summary>
        /// Checks whether the given player owns the given piece by using integer enum values of `PieceType`
        /// </summary>
        /// <param name="player"></param>
        /// <param name="piece"></param>
        /// <returns>True if player colour matches piece enum colour (hardcoded). False otherwise,
        /// including if the `player.Color` is unassigned.</returns>
        public static bool OwnsPiece(Player player, PieceType piece) {
            var pieceVal = (int) piece;
            if (player.Color == Color.White && 2 <= pieceVal && pieceVal <= 7)
                return true;
            if (player.Color == Color.Black && pieceVal > 7)
                return true;
            return false;
        }

        public static Color ColorOfPiece(PieceType piece) {
            var pieceVal = (int) piece;
            if (2 <= pieceVal && pieceVal <= 7)
                return Color.White;
            if ( pieceVal > 7)
                return Color.Black;
            return Color.Unassigned;
        }
        
        /// <summary>
        /// Check whether the PieceType of the move capture is of a piece or not by checking the int value.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static bool MoveCaptures(Move move) => (int) move.Capture >= 2;
        
        public static Player GetOtherPlayer(GameState state, Player player) {
            if (player == state.White) return state.Black;
            if (player == state.Black) return state.White;

            throw new Exception("Player object of state does not match the given player.");
        }

        public static Color GetOtherColor(Color color) {
            if (color == Color.White) return Color.Black;
            if (color == Color.Black) return Color.White;
            
            throw new Exception("Color given should be black or white.");
        }

        public static bool PawnUpForPromotion(GameState state, Vector2Int pos) {
            var target = state.Squares()[pos.x, pos.y];
            var color = ColorOfPiece(target);
            var targetRow = GetPawnStartRow(GetOtherColor(color));
            var targetDirection = GetPawnDirection(color);
            
            if (target == PieceType.WPawn && pos.x == targetRow + targetDirection) return true;
            if (target == PieceType.BPawn && pos.x == targetRow + targetDirection) return true;

            return false;
        }

        public static int GetPawnStartRow(Color color) {
            switch (color) {
                case Color.White:
                    return whitePawnRow;
                case Color.Black:
                    return blackPawnRow;
                default:
                    throw new Exception("Handed unassigned player while trying to determine pawn direction.");
            }
        }

        public static int GetPawnDirection(Color color) {
            switch (color) {
                case Color.White:
                    return whitePawnDirection;
                case Color.Black:
                    return blackPawnDirection;
                default:
                    throw new Exception("Handed unassigned player while trying to determine pawn direction.");
            }
        }
        
        public static int GetBackRow(Color color) {
            switch (color) {
                case Color.White:
                    return whiteBackRow;
                case Color.Black:
                    return blackBackRow;
                default:
                    throw new Exception("Handed unassigned player while trying to determine pawn direction.");
            }
        }

        public static PieceType GetOppositePiece(PieceType piece) {
            if ((int) piece < 2)
                throw new Exception("There is no opposite piece for empty or out of bounds types.");
            if ((int) piece < 8)
                return piece + 6;
            return piece - 6;
        }

        public static bool CanEnPassant(GameState state, LinearHistory history, Vector2Int source, out Vector2Int target) {
            if (history.Events.Count == 0) {
                target = Vector2Int.zero;
                return false;
            }
            
            var piece = state.Squares()[source.x, source.y];
            var start = GetPawnStartRow(ColorOfPiece(piece));
            var direction = GetPawnDirection(ColorOfPiece(piece));

            if (source.x != start + (3 * direction)) {
                target = Vector2Int.zero;
                return false;
            }

            var oppositePiece = GetOppositePiece(piece);
            var action = history.GetActionFromNow(state, 1).Item1;
            if (!(action is Move move)) {
                target = Vector2Int.zero;
                return false;
            }
            if (move.Player == state.CurrentPlayer
                || move.Piece != oppositePiece
                || source.x != move.Target.x
                || Math.Abs(source.y - move.Target.y) != 1) {
                target = Vector2Int.zero;
                return false;
            }

            target = new Vector2Int(move.Target.x + direction, move.Target.y);
            return true;
        }

        /// <summary>
        /// Check whether movement from `source` to `target` uses cardinal directions.
        /// </summary>
        public static bool StraightMovement(Vector2Int source, Vector2Int target) {
            var dx = target.x - source.x;
            var dy = target.y - source.y;
            return (dx == 0 && dy != 0) || (dx != 0 && dy == 0);
        }

        /// <summary>
        /// Check whether movement from `source` to `target` is strictly diagonal (45 degrees).
        /// </summary>
        public static bool DiagonalMovement(Vector2Int source, Vector2Int target) {
            var dx = target.x - source.x;
            var dy = target.y - source.y;
            return Math.Abs(dx) == Math.Abs(dy);
        }

        // TODO: I *could* just keep track of this in a field.
        public static Vector2Int GetKingPosition(GameState state, Color color) {
            var king = color == Color.White ? PieceType.WKing : PieceType.BKing;
            for (var rank = '1'; rank < '9'; rank++) {
                for (var file = 'a'; file < 'i'; file++) {
                    var pos = ToVector2Int(rank, file);
                    if (state.Square(pos) == king)
                        return pos;
                }
            }

            throw new Exception($"Error. Could not find king for {color}");
        }
        
        /// <summary>
        /// Checks whether the current player is in Checkmate.
        /// </summary>
        public static bool CheckMate(GameState state, LinearHistory history, bool validate) {
            var color = state.CurrentPlayer.Color;
            if (!Check(state, color))
                return false;

            var actions = GetAllActions(state, history);
            foreach (var action in actions) {
                Apply(state, history, action, validate);
                if (!Check(state, color))
                    return false;
                Undo(state, history, validate);
            }

            return true;
        }
        
        /// <summary>
        /// Finds the king of the given color, then looks for possible threats to the king,
        /// based on movement of the other pieces.
        /// </summary>
        public static bool Check(GameState state, Color color) {
            var target = GetKingPosition(state, color);
            var straightThreats = StraightThreats(color);
            var diagonalThreats = Rules.DiagonalThreats(color);
            var knightThreat = color == Color.White 
                    ? new PieceType[] { PieceType.BKnight } 
                    : new PieceType[] { PieceType.WKnight }
            ;
            var kingThreat = color == Color.White 
                    ? new PieceType[] { PieceType.BKing } 
                    : new PieceType[] { PieceType.WKing }
                ;

            for (int i = 0; i < StraightDeltas.Length; i++) {
                var straightDelta = StraightDeltas[i];
                var diagonalDelta = DiagonalDeltas[i];
                if (CheckThreatOnLine(state, target, straightDelta[0], straightDelta[1], 8, straightThreats)
                || CheckThreatOnLine(state, target, diagonalDelta[0], diagonalDelta[1], 8, diagonalThreats)
                || CheckThreatOnLine(state, target, straightDelta[0], straightDelta[1], 1, kingThreat)
                || CheckThreatOnLine(state, target, diagonalDelta[0], diagonalDelta[1], 1, kingThreat)
                )
                    return true;
            }

            foreach (var delta in KnightDeltas) {
                if (CheckThreatOnLine(state, target, delta[0], delta[1], 1, knightThreat))
                    return true;
            }

            return false;
        }
        
        /// <summary>
        /// Checks in a line from the given `source`, `dx`, and `dy` until it meets a non-empty `PieceType´.
        /// Returns true if the piece is found in `threats`, false otherwise..
        /// </summary>
        public static bool CheckThreatOnLine(GameState state, Vector2Int source, int dx, int dy, int length,
            PieceType[] threats) {
            for (int i = 1; i <= length; i++) {
                var x = source.x + dx*i;
                var y = source.y + dy*i;
                var piece = state.Square(x, y);
                if (piece != PieceType.Empty)
                    return threats.Contains(piece);
            }
            return false;
        }
        
        public static PieceType[] StraightThreats(Color color) {
            var threats = new  PieceType[] {
                color == Color.White ? PieceType.BQueen : PieceType.WQueen,
                color == Color.White ? PieceType.BRook : PieceType.WRook,
            };
            return threats;
        }

        public static PieceType[] DiagonalThreats(Color color) {
            var threats = new PieceType[] {
                color == Color.White ? PieceType.BQueen : PieceType.WQueen,
                color == Color.White ? PieceType.BBishop : PieceType.WBishop,
            };
            return threats;
        }
        
        #endregion
    }
}