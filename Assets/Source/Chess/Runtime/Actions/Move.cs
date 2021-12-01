using Source.Chess.Runtime.Objects;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    /// <summary>
    /// Movement of a chess piece, which may cause a capture (though this is not included as a field)
    /// En-passant is included within the move rule.
    /// </summary>
    public class Move {
        public Player Player;
        public Vector2Int Source { get; }
        public Vector2Int Target { get; }
        public int Piece;

        public Move(Player player, int piece, Vector2Int source, Vector2Int target) {
            Player = player;
            Piece = piece;
            Source = source;
            Target = target;
        }
    }
}