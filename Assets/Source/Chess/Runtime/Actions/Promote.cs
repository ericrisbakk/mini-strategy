using Source.Chess.Runtime.Objects;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    public class Promote {
        public Player Player { get; }
        public Vector2Int Pawn { get; }
        public PieceType Promotion { get; }

        public Promote(Player player, Vector2Int pawn, PieceType promotion) {
            Player = player;
            Pawn = pawn;
            Promotion = promotion;
        }
    }
}