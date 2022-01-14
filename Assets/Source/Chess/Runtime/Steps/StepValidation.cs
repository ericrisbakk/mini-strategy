using Source.Chess.Runtime.Actions;
using Source.Chess.Runtime.Objects;
using UnityEngine.Assertions;

namespace Source.Chess.Runtime.Steps {
    public static class StepValidation {
        /// <summary>
        /// Asserts that `player.Color` is not value `Color.Unassigned`.
        /// </summary>
        public static void PlayerColorAssigned(Player player) {
            Assert.AreNotEqual(player.Color, Color.Unassigned, 
                "Player must have a color.");
        }

        /// <summary>
        /// Asserts that `piece` belongs to the `player`.
        /// </summary>
        public static void OwnsPiece(Player player, PieceType piece) {
            Assert.IsTrue(Rules.OwnsPiece(player, piece),
                "Player and piece color do not match.");
        }

        /// <summary>
        /// Asserts that the given `piece` value is not `PieceType.OutOfBounds`.
        /// </summary>
        public static void InBounds(PieceType piece, string target) {
            Assert.IsTrue(piece != PieceType.OutOfBounds,
                $"{target} value should never be `OutOfBounds`");
        }

        /// <summary>
        /// Asserts that `p1` is `Color.White` and  `p2` is `Color.Black`, or the other way around.
        /// </summary>
        public static void OpposingPieces(PieceType p1, PieceType p2) {
            var c1 = Rules.ColorOfPiece(p1);
            var c2 = Rules.GetOtherColor(c1);
            Assert.IsTrue(c1 == Color.White && c2 == Color.Black || c1 == Color.Black && c2 == Color.White,
                $"Pieces {p1} and {p2} should be opposing pieces.");
        }
    }
}