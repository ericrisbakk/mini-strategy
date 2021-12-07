using System;
using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;

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

        public static bool MoveCaptures(Move move) => (int) move.Capture >= 2;
        #endregion

        #region Utility

        public static Player GetOtherPlayer(GameState state, Player player) {
            if (player == state.White) return state.Black;
            if (player == state.Black) return state.White;

            throw new Exception("Player object of state does not match the given player.");
        }


        #endregion
    }
}