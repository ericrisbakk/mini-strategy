using Source.Chess.Runtime.Objects;
using Source.StrategyFramework.Runtime.Representation;
using UnityEngine;

namespace Source.Chess.Runtime.Actions {
    public class Promote :IAction {
        public Player Player { get; }
        public Vector2Int Pawn { get; }
        public PieceType Promotion { get; }

        public Promote(Player player, Vector2Int pawn, PieceType promotion) {
            Player = player;
            Pawn = pawn;
            Promotion = promotion;
        }

        public bool Equals(Promote other) {
            return Equals(Player, other.Player)
                   && Pawn == other.Pawn
                   && Promotion == other.Promotion;
        }
    }
}