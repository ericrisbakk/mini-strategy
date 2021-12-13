using System;
using System.Collections.Generic;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using UnityEngine;

namespace Source.Chess.Runtime {
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

    public enum PlayerType {
        Unassigned,
        White,
        Black,
    }

    // TODO: Rules should probably inherit from something defining base classes, especially a "GetAllAvailableActions" method.
    public class Rules {
        #region Moves

        List<Steps.PawnMoveStep> PawnMoves(GameState state, Vector2Int source) {
            var piece = state.Square(source);
            var color = PlayerOfPiece(piece);
            var start = GetPawnStartRow(color);
            var direction = GetPawnDirection(color);
            var moveList = new List<Steps.PawnMoveStep>();

            var posAhead1 = new Vector2Int(source.x + direction, source.y);
            if (state.Square(posAhead1) == PieceType.Empty) {
                Add(moveList, new Move(state.CurrentPlayer, PieceType.WPawn, source, posAhead1));

                var posAhead2 = new Vector2Int(source.x + (2 * direction), source.y);
                if (source.x == start
                    && state.Square(posAhead2) == PieceType.Empty) {
                    Add(moveList, new Move(state.CurrentPlayer, PieceType.WPawn, source, posAhead2));
                }
            }

            var leftCapture = new Vector2Int(source.x + direction, source.y + 1);
            var rightCapture = new Vector2Int(source.x + direction, source.y - 1);

            // TODO left and right captures.

            throw new NotImplementedException();
        }

        private void Add(List<Steps.PawnMoveStep> stepList, Move move) {
            stepList.Add(new Steps.PawnMoveStep(move));
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
            if (player.Color == PlayerType.White && 2 <= pieceVal && pieceVal <= 7)
                return true;
            if (player.Color == PlayerType.Black && pieceVal > 7)
                return true;
            return false;
        }

        public static PlayerType PlayerOfPiece(PieceType piece) {
            var pieceVal = (int) piece;
            if (2 <= pieceVal && pieceVal <= 7)
                return PlayerType.White;
            if ( pieceVal > 7)
                return PlayerType.Black;
            return PlayerType.Unassigned;
        }

        public static bool MoveCaptures(Move move) => (int) move.Capture >= 2;
        
        public static Player GetOtherPlayer(GameState state, Player player) {
            if (player == state.White) return state.Black;
            if (player == state.Black) return state.White;

            throw new Exception("Player object of state does not match the given player.");
        }

        public static int GetPawnStartRow(PlayerType player) {
            switch (player) {
                case PlayerType.White:
                    return 8;
                case PlayerType.Black:
                    return 3;
                default:
                    throw new Exception("Handed unassigned player while trying to determine pawn direction.");
            }
        }

        public static int GetPawnDirection(PlayerType player) {
            switch (player) {
                case PlayerType.White:
                    return -1;
                case PlayerType.Black:
                    return 1;
                default:
                    throw new Exception("Handed unassigned player while trying to determine pawn direction.");
            }
        }

        #endregion
    }
}