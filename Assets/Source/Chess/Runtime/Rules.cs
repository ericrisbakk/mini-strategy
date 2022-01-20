using System;
using System.Collections.Generic;
using System.Linq;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using Source.Chess.Runtime.Steps;
using Source.StrategyFramework.Runtime.History;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime {

    #region Constants

    /// <summary>
    /// Encodes the possible values a square on the board can have. The `OutOfBounds` value is used to simplify
    /// checking whether a coordinate is out of bounds.
    /// NB: The values have been assigned because the order is set explicitly, and some methods are using that order.
    /// </summary>
    public enum PieceType {
        Empty = 0,
        OutOfBounds = 1,
        WPawn = 2,
        WKnight = 3,
        WBishop = 4,
        WRook = 5,
        WQueen = 6,
        WKing = 7,
        BPawn = 8,
        BKnight = 9,
        BBishop = 10,
        BRook = 11,
        BQueen = 12,
        BKing = 13
    }

    public enum Color {
        Unassigned = 0,
        White,
        Black,
    }

    #endregion

    // TODO: Rules should probably inherit from something defining base classes, especially a "GetAllAvailableActions" method.
    public static class Rules {

        public const string StandardWhite = "a2,b2,c2,d2,e2,f2,g2,h2,Ra1,Nb1,Bc1,Qd1,Ke1,Bf1,Ng1,Rh1";
        public const string StandardBlack = "a7,b7,c7,d7,e7,f7,g7,h7,Ra8,Nb8,Bc8,Qd8,Ke8,Bf8,Ng8,Rh8";

        public const int whitePawnRow = 3;
        public const int whitePawnDirection = 1;
        public const int blackPawnRow = 8;
        public const int blackPawnDirection = -1;
        

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
            if (action is Move move)
                return new PawnMoveStep(move);

            throw new Exception("Could not get next step, action was not recognized.");
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
            var lastStep = history.LastStep;
            
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
        
        #endregion

        #region Action

        public static List<IAction> GetActions(GameState state, Vector2Int source) {
            var actions = new List<IAction>();
            var moves = new List<IAction>(GetMoves(state, source));
            actions.AddRange(moves);

            throw new NotImplementedException();
        }
        
        #endregion
        
        #region Moves

        public static List<Move> GetMoves(GameState state, Vector2Int source) {
            throw new NotImplementedException();
        }
        
        public static List<Move> GetPawnMoves(GameState state, Vector2Int source) {
            var piece = state.Square(source);
            var player = state.CurrentPlayer;
            var color = ColorOfPiece(piece);
            var start = GetPawnStartRow(color);
            var direction = GetPawnDirection(color);
            var moveList = new List<Move>();

            var posAhead1 = new Vector2Int(source.x + direction, source.y);
            if (state.Square(posAhead1) == PieceType.Empty) {
                moveList.Add(new Move(player, PieceType.WPawn, source, posAhead1));

                var posAhead2 = new Vector2Int(source.x + (2 * direction), source.y);
                if (source.x == start
                    && state.Square(posAhead2) == PieceType.Empty) {
                    moveList.Add(new Move(player, PieceType.WPawn, source, posAhead2));
                }
            }

            var leftCapture = new Vector2Int(source.x + direction, source.y + 1);
            if (OwnsPiece(GetOtherPlayer(state, player), state.Square(leftCapture)))
                moveList.Add(new Move(player, PieceType.WPawn, source, leftCapture));
            
            var rightCapture = new Vector2Int(source.x + direction, source.y - 1);
            if (OwnsPiece(GetOtherPlayer(state, player), state.Square(rightCapture)))
                moveList.Add(new Move(player, PieceType.WPawn, source, rightCapture));

            return moveList;
        }
        
        
        
        #endregion
        
        #region Checks

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

        #endregion
    }
}