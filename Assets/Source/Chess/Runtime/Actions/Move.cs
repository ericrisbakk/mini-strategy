using Source.Chess.Runtime.Objects;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    /// <summary>
    /// Movement of a chess piece, which may cause a capture (No capture is given the "Empty" value).
    /// En-passant is included within the move rule.
    /// </summary>
    public class Move {
        public Player Player;
        public Vector2Int Source { get; }
        public Vector2Int Target { get; }
        public PieceType Piece;
        public PieceType Capture;

        public Move(Player player, PieceType piece, Vector2Int source, Vector2Int target) {
            Player = player;
            Piece = piece;
            Source = source;
            Target = target;
        }
    }
}