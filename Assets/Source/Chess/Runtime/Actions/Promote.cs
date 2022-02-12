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
        
        /// <summary>
        /// `pawn` converted to `Vector2Int` using `Rules.ToVector2Int()`
        /// </summary>
        public Promote(Player player, string pawn, PieceType promotion) {
            Player = player;
            Pawn = Rules.ToVector2Int(pawn[1], pawn[0]);
            Promotion = promotion;
        }

        public bool Equals(Promote other) {
            return Equals(Player, other.Player)
                   && Pawn == other.Pawn
                   && Promotion == other.Promotion;
        }
    }
}